' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class AutoSuspendRecord

    Public Sub New(lines As StringTable)
        Me.Alarm = lines.GetSingleLineValue(Of String)("Auto Suspend ")
        Me.Time = lines.GetSingleLineValue(Of TimeSpan)("Time ")

    End Sub

    Public Property Alarm As String
    Public Property Time As TimeSpan
End Class
