' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class AlertRecord

    Public Sub New(onOff As String, timeLeft As String)
        Me.OnOff = onOff
        Me.TimeLeft = timeLeft
    End Sub

    Public Property OnOff As String
    Public Property TimeLeft As String
End Class
