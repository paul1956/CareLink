' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class SummaryRecord
    Private ReadOnly _isDate As Boolean

    Protected Friend Sub New(index As ItemIndexs, entry As KeyValuePair(Of String, String))
        Me.RecordNumber = index
        Me.Key = entry.Key
        Me.Value = entry.Value.ToString(CurrentUICulture)
        _isDate = s_ListOfTimeItems.Contains(index)
    End Sub

    Public Property RecordNumber As Integer
    Public Property Key As String
    Public Property Value As String

    Public Shared Function GetCellStyle(memberName As String, <CallerMemberName> Optional functionName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case memberName
            Case NameOf(RecordNumber)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(Key)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                cellStyle.Padding = New Padding(10, 0, 0, 0)
            Case NameOf(Value)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                cellStyle.Padding = New Padding(0, 0, 3, 0)
            Case Else
                Throw New Exception($"Line {sourceLineNumber} in {functionName} thought to be unreachable for '{memberName}'")
        End Select
        Return cellStyle
    End Function

End Class
