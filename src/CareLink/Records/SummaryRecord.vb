' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class SummaryRecord
    Private ReadOnly _isDate As Boolean

    Private ReadOnly _listOfTimeItems As New List(Of Integer) From {
                        ItemIndexs.lastSensorTS,
                        ItemIndexs.lastConduitTime,
                        ItemIndexs.medicalDeviceTime,
                        ItemIndexs.lastSensorTime}

    Protected Friend Sub New(index As ItemIndexs, entry As KeyValuePair(Of String, String))
        Me.RecordNumber = index
        Me.Key = entry.Key
        Me.Value = entry.Value?.ToString(CurrentUICulture)
        _isDate = _listOfTimeItems.Contains(index)
    End Sub

    Public Property RecordNumber As Integer
    Public Property Key As String
    Public Property Value As String

    Public Shared Function GetCellStyle(columnName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(RecordNumber)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(Key)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(Value)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case Else
                Throw UnreachableException(memberName, sourceLineNumber)
        End Select
        Return cellStyle
    End Function

End Class
