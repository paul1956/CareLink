' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Http
Imports System.Threading

Friend Class SimpleResponseHandler
    Inherits HttpMessageHandler

    Public Property ResponseFactory As Func(Of HttpRequestMessage, HttpResponseMessage)

    Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Return If(Me.ResponseFactory IsNot Nothing,
                  Task.FromResult(Me.ResponseFactory.Invoke(arg:=request)),
                  Task.FromResult(New HttpResponseMessage(statusCode:=HttpStatusCode.NotFound)))
    End Function

End Class
