' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertsRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable, listOfAllTextLines As List(Of String))
        Me.InitializeFromStringTable(sTable, listOfAllTextLines)
    End Sub

    Public Property HighAlert As New List(Of HighAlertRecord)

End Class
