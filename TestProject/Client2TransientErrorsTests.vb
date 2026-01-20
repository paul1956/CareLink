Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Reflection
Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class Client2TransientErrorsTests

    <Fact>
    Public Async Function GetRecentDataAsync_DoesNotCrash_OnTransientHttpErrors() As Task
        Dim handler As New SequenceHandler()
        handler.EnqueueException(New HttpRequestException("transient 1"))
        handler.EnqueueException(New HttpRequestException("transient 2"))

        Dim validContent As String = "{""meta"":{},""patientData"":[] }"
        Dim responseFactory As Func(Of HttpResponseMessage) =
            Function()
                Return New HttpResponseMessage(HttpStatusCode.OK) With {
                                                     .Content = New StringContent(validContent)}
            End Function

        handler.EnqueueResponse(responseFactory)

        Dim httpClient As New HttpClient(handler)
        Dim client As New Client2(httpClient:=httpClient) With {
            .Config = New Dictionary(Of String, String) From {{"baseUrlCumulus", "https://example.com"}}}
        client.GetType().GetProperty("UserElementDictionary").SetValue(client, New Dictionary(Of String, Object) From {{"role", "patient"}})

        Dim tokenJson As String = "{""access_token"":""aaa.bbb.ccc"",""refresh_token"":""r"",""client_id"":""cid"",""mag-identifier"":""m""}"
        Dim tokenElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(tokenJson)
        Dim tokenField As FieldInfo = client.GetType().GetField("_tokenDataElement", BindingFlags.NonPublic Or BindingFlags.Instance)
        tokenField.SetValue(client, tokenElement)

        Dim accessPayload As New Dictionary(Of String, Object) From {{"exp", JsonDocument.Parse("10000000000").RootElement}}
        Dim accessField As FieldInfo = client.GetType().GetField("_accessTokenPayload", BindingFlags.NonPublic Or BindingFlags.Instance)
        accessField.SetValue(client, CType(accessPayload, Object))

        Dim result As String = Nothing
        Dim ex As Exception = Nothing
        Try
            result = Await client.GetRecentDataAsync().ConfigureAwait(False)
        Catch e As Exception
            ex = e
        End Try

        ex.Should().BeNull()
        result.Should().NotBeNull()
    End Function

    Private Class SequenceHandler
        Inherits HttpMessageHandler

        Private ReadOnly _exceptions As Queue(Of Exception)
        Private ReadOnly _responses As Queue(Of Func(Of HttpResponseMessage))

        Public Sub New()
            _responses = New Queue(Of Func(Of HttpResponseMessage))()
            _exceptions = New Queue(Of Exception)()
        End Sub

        Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
            If _exceptions.Count > 0 Then
                Dim ex As Exception = _exceptions.Dequeue()
                Dim tcs As New TaskCompletionSource(Of HttpResponseMessage)()
                tcs.SetException(ex)
                Return tcs.Task
            End If

            If _responses.Count > 0 Then
                Dim factory As Func(Of HttpResponseMessage) = _responses.Dequeue()
                Return Task.FromResult(factory.Invoke())
            End If

            Return Task.FromResult(New HttpResponseMessage(HttpStatusCode.NotFound))
        End Function

        Public Sub EnqueueException(ex As Exception)
            _exceptions.Enqueue(ex)
        End Sub

        Public Sub EnqueueResponse(responseFactory As Func(Of HttpResponseMessage))
            _responses.Enqueue(responseFactory)
        End Sub

    End Class
End Class
