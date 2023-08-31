' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PersonalRemindersRecord

    Public Sub New()
    End Sub

    Public Sub New(r As StringTable.Row, key As String)
        Me.Time = r.Columns(0).Replace(key, "").CleanSpaces
    End Sub

    Public Property Time As String = "Off"
End Class
