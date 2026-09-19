' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Reflection
Imports System.Text.Json
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class Client2RefreshTests

    <Fact>
    Public Async Function DoRefreshAsync_WithClientSecretInTokenData_Succeeds() As Task
        ' Arrange
        Dim handler As New SimpleResponseHandler()
        Dim tokenResponsePath As String =
            GetTestDataFile(fileName:="tokenResponse_new.json")
        Dim tokenResponseJson As String =
            File.ReadAllText(path:=tokenResponsePath)
        handler.ResponseFactory =
            Function(req)
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                    .Content = New StringContent(content:=tokenResponseJson)}
            End Function
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=ServerLocation.US, httpClient) With {
            .Config = New ConfigRecord With {.TokenUrl = "https://example.com/token"}}

        ' Prepare tokenElement JSON with client_secret present
        Dim tokenPath As String = GetTestDataFile("token_with_client_secret.json")
        Dim tokenJson As String = File.ReadAllText(path:=tokenPath)
        Dim tokenElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
        ' Use friend test helper to set token data element; avoids reflection and is explicit
        client.SetTokenDataElementForTests(token:=tokenElement)

        ' Act
        Dim result As JsonElement =
            Await client.DoRefreshAsync(client.Config, tokenElement, httpClient)

        ' Assert
        result.IsEmpty.Should().BeFalse()
        Dim dict As Dictionary(Of String, JsonElement) = Nothing
        Try
            dict = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(result)
        Catch ex As Exception
            Assert.True(condition:=False,
                        userMessage:="Deserialization of token response failed: " & ex.Message)
        End Try
        dict.Should().ContainKey(expected:="access_token")
        dict(key:="access_token").GetString().Should().Be(expected:="new.access.token")
    End Function

    <Fact>
    Public Async Function DoRefreshAsync_PreservesRefreshToken_WhenResponseContainsOnlyAccessToken() As Task
        ' Arrange
        Dim value As Func(Of HttpRequestMessage, HttpResponseMessage) =
            Function(req)
                Dim onlyAccessResponse As String =
                File.ReadAllText(path:=GetTestDataFile(fileName:="token_only_access.json"))
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                           .Content = New StringContent(content:=onlyAccessResponse)}
            End Function

        Dim handler As New SimpleResponseHandler With {
            .ResponseFactory = value}
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=ServerLocation.US, httpClient) With {
            .Config = New ConfigRecord With {
                .TokenUrl = "https://example.com/token"}}

        ' Prepare a token element that has an existing refresh_token
        Dim originalTokenPath As String =
            GetTestDataFile(fileName:="token_with_client_secret.json")
        Dim originalTokenJson As String =
            File.ReadAllText(path:=originalTokenPath)
        Dim originalTokenElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json:=originalTokenJson)
        ' Use friend test helper to set token data element; avoids reflection and is explicit
        client.SetTokenDataElementForTests(token:=originalTokenElement)

        ' Act
        Dim result As JsonElement =
            Await client.DoRefreshAsync(client.Config,
                                        tokenElement:=originalTokenElement,
                                        httpClient)

        ' Assert
        Dim resultDict As Dictionary(Of String, JsonElement) = Nothing
        Try
            resultDict =
                JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(element:=result)
        Catch ex As Exception
            Assert.True(condition:=False,
                        userMessage:=$"Deserialization of refresh result failed: {ex.Message}")
        End Try
        resultDict.Should().ContainKey(expected:="access_token")
        resultDict(key:="access_token").GetString().Should().Be(expected:="only.access.token")
        resultDict.Should().ContainKey(expected:="refresh_token")
        resultDict(key:="refresh_token").GetString().Should().Be(expected:="r")
    End Function

    <Fact>
    Public Async Function DoRefreshAsync_ResolverFallbackProvidesClientSecret_Succeeds() As Task
        ' Arrange
        Dim handler As New SimpleResponseHandler()
        Dim tokenResponsePath2 As String =
            GetTestDataFile(fileName:="tokenResponse_fromresolver.json")
        Dim tokenResponseJson As String =
            File.ReadAllText(path:=tokenResponsePath2)
        handler.ResponseFactory =
            Function(req)
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                    .Content = New StringContent(content:=tokenResponseJson)}
            End Function
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=ServerLocation.US, httpClient) With {
            .Config = New ConfigRecord With {.TokenUrl = "https://example.com/token"}
            }

        ' Prepare tokenElement JSON without client_secret
        Dim tokenOldPath As String = GetTestDataFile(fileName:="token_old.json")
        Dim tokenJson As String = File.ReadAllText(path:=tokenOldPath)
        Dim tokenElement As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
        Dim bindingAttr As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance
        Dim tokenProp As PropertyInfo = client.GetType().GetProperty(NameOf(Client2.TokenDataElement), bindingAttr)
        If tokenProp IsNot Nothing Then
            tokenProp.SetValue(obj:=client, value:=tokenElement)
        Else
            Dim tokenField As FieldInfo = client.GetType().GetField(name:="_tokenDataElement", bindingAttr)
            If tokenField IsNot Nothing Then
                tokenField.SetValue(obj:=client, value:=tokenElement)
            Else
                Assert.True(condition:=False, userMessage:="Could not locate TokenDataElement property or backing field on Client2")
            End If
        End If

        ' Create fake endpoint resolver that returns SsoJson containing client_secret
        Dim endpointResolver As Func(Of ConfigRecord, Task(Of EndpointConfig)) =
            Function(cfg)
                Dim ssoJsonPath As String = GetTestDataFile(fileName:="sso_client_secret.json")
                Dim ssoJson As String = File.ReadAllText(path:=ssoJsonPath)
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
        Try
            dict = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(element:=result)
            Assert.True(condition:=True)
        Catch ex As Exception
            Assert.True(condition:=False,
                        userMessage:=$"Deserialization of token response failed: {ex.Message}")
        End Try
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
                Dim tokenResponsePath3 As String = GetTestDataFile("tokenResponse_tok.json")
                Dim tokenResponseJson As String = File.ReadAllText(path:=tokenResponsePath3)
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With {
                    .Content = New StringContent(content:=tokenResponseJson)}
            End Function

        Dim handler As New CapturingHandler(factory)
        Dim httpClient As New HttpClient(handler)

        Dim client As New Client2(serverRegion:=ServerLocation.US, httpClient) With {
            .Config = New ConfigRecord With {.TokenUrl = "https://example.com/token"}}

        ' token element with mag-identifier and client_secret
        Dim tokenWithMagPath As String =
            GetTestDataFile(fileName:="token_with_mag_and_secret.json")
        Dim tokenJson As String =
            File.ReadAllText(path:=tokenWithMagPath)
        Dim tokenElement As JsonElement =
            JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
        ' Set the friend property directly (tests have InternalsVisibleTo access)
        client.TokenDataElement = tokenElement

        ' Act
        Dim result As JsonElement =
            Await client.DoRefreshAsync(client.Config,
                                        tokenElement,
                                        httpClient)

        ' Assert
        capturedRequest.Should().NotBeNull()
        capturedRequest.Headers.Contains(name:="mag-identifier").Should().BeTrue()
        capturedRequest.Headers.GetValues(name:="mag-identifier").Should().Contain(expected:="mag123")
    End Function

    ' Handlers moved to TestHelpers for reuse across tests.
End Class
