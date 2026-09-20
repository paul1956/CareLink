' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net.Http
Imports System.Threading

' Lightweight handler types exposed at Friend scope for tests that don't import the TestUtilities namespace.
Friend Class CapturingHandler
    Inherits HttpMessageHandler

    Private ReadOnly _factory As Func(Of HttpRequestMessage, HttpResponseMessage)

    Public Sub New(factory As Func(Of HttpRequestMessage, HttpResponseMessage))
        _factory = factory
    End Sub

    Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Return Task.FromResult(_factory.Invoke(arg:=request))
    End Function

End Class
