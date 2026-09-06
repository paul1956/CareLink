Imports System.Net
Imports System.Net.Http
Imports System.Reflection
Imports System.Text.Json
Imports System.Threading
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class Client2RefreshTests

    <Fact>
    Public Async Function DoRefreshAsync_WithClientSecretInTokenData_Succeeds() As Task
        ' Arrange
        Dim handler As New SimpleResponseHandler()
        Dim tokenResponseJson As String = "{""access_token"":""new.access.token"",""refresh_token"":""new.refresh""}"
        handler.ResponseFactory =
            Function(req)
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                    .Content = New StringContent(content:=tokenResponseJson)}
            End Function
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=Region.NorthAmerica, httpClient) With {
            .Config = New ConfigRecord With {.TokenUrl = "https://example.com/token"}}

        ' Prepare tokenElement JSON with client_secret present
        Dim tokenJson As String =
                "{""access_token"":""aaa.bbb.ccc"",""refresh_token"":""r"",""client_id"":""cid"",""client_secret"":""secret""}"
        Dim tokenElement As JsonElement =
            JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
        Dim bindingAttr As BindingFlags =
            BindingFlags.NonPublic Or BindingFlags.Instance
        Dim tokenField As FieldInfo =
            client.GetType().GetField(name:="_tokenDataElement", bindingAttr)
        tokenField.SetValue(obj:=client, value:=tokenElement)

        ' Act
        Dim result As JsonElement =
            Await client.DoRefreshAsync(client.Config, tokenElement, httpClient)

        ' Assert
        result.IsEmpty.Should().BeFalse()
        Dim dict As Dictionary(Of String, JsonElement) = Nothing
        result.TryFromJson(result:=dict).Should().BeTrue()
        dict.Should().ContainKey(expected:="access_token")
        dict(key:="access_token").GetString().Should().Be(expected:="new.access.token")
    End Function

    <Fact>
    Public Async Function DoRefreshAsync_ResolverFallbackProvidesClientSecret_Succeeds() As Task
        ' Arrange
        Dim handler As New SimpleResponseHandler()
        Dim tokenResponseJson As String = "{""access_token"":""fromresolver.token"",""refresh_token"":""refreshed""}"
        handler.ResponseFactory =
            Function(req)
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                    .Content = New StringContent(content:=tokenResponseJson)}
            End Function
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=Region.NorthAmerica, httpClient) With {
            .Config = New ConfigRecord With {.TokenUrl = "https://example.com/token"}
            }

        ' Prepare tokenElement JSON without client_secret
        Dim tokenJson As String = "{""access_token"":""old"",""refresh_token"":""r"",""client_id"":""cid""}"
        Dim tokenElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
        Dim bindingAttr As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance
        Dim tokenField As FieldInfo = client.GetType().GetField(name:="_tokenDataElement", bindingAttr)
        tokenField.SetValue(obj:=client, value:=tokenElement)

        ' Create fake endpoint resolver that returns SsoJson containing client_secret
        Dim endpointResolver As Func(Of ConfigRecord, Task(Of EndpointConfig)) =
            Function(cfg)
                Dim ssoJson As String = "{""client_secret"":{""client_secret"":""resolverSecret""}}"
                Dim ec As New EndpointConfig With {.SsoJson = ssoJson, .ApiBaseUrl = "https://api"}
                Return Task.FromResult(ec)
            End Function

        ' Act
        Dim result As JsonElement =
            Await client.DoRefreshAsync(client.Config,
                                        tokenElement,
                                        httpClient,
                                        endpointResolver)

        ' Assert
        result.IsEmpty.Should().BeFalse()
        Dim dict As Dictionary(Of String, JsonElement) = Nothing
        result.TryFromJson(result:=dict).Should().BeTrue()
        dict.Should().ContainKey(expected:="access_token")
        dict(key:="access_token").GetString().Should().Be(expected:="fromresolver.token")
    End Function

    <Fact>
    Public Async Function DoRefreshAsync_IncludesMagIdentifierHeader_WhenPresent() As Task
        ' Arrange
        Dim capturedRequest As HttpRequestMessage = Nothing
        Dim factory As Func(Of HttpRequestMessage, HttpResponseMessage) =
            Function(req)
                capturedRequest = req
                Dim tokenResponseJson As String = "{""access_token"":""tok"",""refresh_token"":""r""}"
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                    .Content = New StringContent(content:=tokenResponseJson)}
            End Function

        Dim handler As New CapturingHandler(factory)
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=Region.NorthAmerica, httpClient) With {
            .Config = New ConfigRecord With {.TokenUrl = "https://example.com/token"}}

        ' token element with mag-identifier and client_secret
        Dim tokenJson As String =
            "{""access_token"":""old"",""refresh_token"":""r"",""client_id"":""cid"",""client_secret"":""secret"",""mag-identifier"":""mag123""}"
        Dim tokenElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
        Dim bindingAttr As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance
        Dim tokenField As FieldInfo = client.GetType().GetField(name:="_tokenDataElement", bindingAttr)
        tokenField.SetValue(obj:=client, value:=tokenElement)

        ' Act
        Dim result As JsonElement = Await client.DoRefreshAsync(client.Config, tokenElement, httpClient)

        ' Assert
        capturedRequest.Should().NotBeNull()
        capturedRequest.Headers.Contains(name:="mag-identifier").Should().BeTrue()
        capturedRequest.Headers.GetValues(name:="mag-identifier").Should().Contain(expected:="mag123")
    End Function

    Private Class SimpleResponseHandler
        Inherits HttpMessageHandler

        Public Property ResponseFactory As Func(Of HttpRequestMessage, HttpResponseMessage)

        Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
            Return If(Me.ResponseFactory IsNot Nothing,
                      Task.FromResult(Me.ResponseFactory.Invoke(request)),
                      Task.FromResult(New HttpResponseMessage(statusCode:=HttpStatusCode.NotFound)))
        End Function

    End Class

    Private Class CapturingHandler
        Inherits HttpMessageHandler

        Private ReadOnly _factory As Func(Of HttpRequestMessage, HttpResponseMessage)

        Public Sub New(factory As Func(Of HttpRequestMessage, HttpResponseMessage))
            _factory = factory
        End Sub

        Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
            Return Task.FromResult(_factory.Invoke(request))
        End Function

    End Class
End Class
