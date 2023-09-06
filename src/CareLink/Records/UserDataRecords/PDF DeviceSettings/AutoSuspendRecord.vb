' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class AutoSuspendRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable)
        Me.Alarm = sTable.GetSingleLineValue(Of String)("Auto Suspend ")
        If Me.Alarm <> "Off" Then
            Me.Time = sTable.GetSingleLineValue(Of TimeSpan)("Time ")
        End If

    End Sub

    Public Property Alarm As String = "Unknown"
    Public Property Time As New TimeSpan
End Class
