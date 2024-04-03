' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MealStartEndRecord

    Public Sub New()
    End Sub

    Public Sub New(r As StringTable.Row, key As String)
        Me.Start = r.Columns(0).Replace(key, "").CleanSpaces
        Me.End = r.Columns(1)
    End Sub

    Public Property [End] As String = "Off"
    Public Property Start As String = "Off"
End Class
