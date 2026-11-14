' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text.Json
Imports System.Threading
Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class Client2TransientErrorsTests

    <Theory>
    <InlineData(True)>
    <InlineData(False)>
    Public Async Function GetRecentDataAsync_DoesNotCrash_OnTransientHttpErrors(retryTransientErrors As Boolean) As Task
        Dim handler As New SequenceHandler()
        handler.EnqueueException(ex:=New HttpRequestException(message:="transient 1"))
        handler.EnqueueException(ex:=New HttpRequestException(message:="transient 2"))

        Dim validContentPath As String = Path.Combine(AppContext.BaseDirectory, "TestData", "patient_valid_content.json")
        Dim validContent As String = File.ReadAllText(path:=validContentPath)
        Dim responseFactory As Func(Of HttpResponseMessage) =
            Function()
                Return New HttpResponseMessage(statusCode:=HttpStatusCode.OK) With
                {.Content = New StringContent(content:=validContent)}
            End Function

        handler.EnqueueResponse(responseFactory)

        Using httpClient As New HttpClient(handler)
            Dim client As New Client2(serverRegion:=ServerLocation.US, httpClient) With {
                .Config = New ConfigRecord With {
                    .BaseUrlCumulus = "https://example.com"},
                .PatientPersonalData = New PatientPersonalInfo With {.Role = "patient"},
                .AccessTokenPayload = New Dictionary(Of String, JsonElement) From {
                    {"exp", JsonElement.Parse(json:="10000000000")}}}

            Dim tokenPath As String =
                Path.Combine(AppContext.BaseDirectory, "TestData", "token_with_mag.json")
            Dim tokenJson As String = File.ReadAllText(path:=tokenPath)
            Dim tokenElement As JsonElement =
                JsonSerializer.Deserialize(Of JsonElement)(json:=tokenJson)
            client.SetTokenDataElementForTests(token:=tokenElement)

            Dim result As String = Await client.GetRecentDataAsync(retryTransientErrors).ConfigureAwaitFalse()

            If retryTransientErrors Then
                handler.RequestBodies.Should().HaveCount(expected:=3)
                handler.RequestBodies(index:=1).Should().Be(handler.RequestBodies(0))
                handler.RequestBodies(index:=2).Should().Be(handler.RequestBodies(0))
                result.Should().Be(expected:="No nameValueCollection received from server")
            Else
                handler.RequestBodies.Should().HaveCount(expected:=1)
                result.Should().Be(expected:="Network/server error: transient 1")
            End If
        End Using
    End Function

    Private Class SequenceHandler
        Inherits HttpMessageHandler

        Private ReadOnly _exceptions As Queue(Of Exception)
        Private ReadOnly _responses As Queue(Of Func(Of HttpResponseMessage))

        Public Sub New()
            _responses = New Queue(Of Func(Of HttpResponseMessage))()
            _exceptions = New Queue(Of Exception)()
        End Sub

        Public ReadOnly Property RequestBodies As New List(Of String)

        Protected Overrides Async Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
            Me.RequestBodies.Add(item:=Await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwaitFalse())
            If _exceptions.Count > 0 Then
                Throw _exceptions.Dequeue()
            End If

            If _responses.Count > 0 Then
                Dim factory As Func(Of HttpResponseMessage) = _responses.Dequeue()
                Return factory.Invoke()
            End If

            Return New HttpResponseMessage(statusCode:=HttpStatusCode.NotFound)
        End Function

        Public Sub EnqueueException(ex As Exception)
            _exceptions.Enqueue(item:=ex)
        End Sub

        Public Sub EnqueueResponse(responseFactory As Func(Of HttpResponseMessage))
            _responses.Enqueue(item:=responseFactory)
        End Sub

    End Class
End Class
