' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LimitsRecord

    <DisplayName(NameOf(highLimit))>
    <Column(Order:=2)>
    Public Property highLimit As Integer

    <DisplayName(NameOf(index))>
    <Column(Order:=1)>
    Public Property index As Integer

    <DisplayName(NameOf(kind))>
    <Column(Order:=4)>
    Public Property kind As String

    <DisplayName(NameOf(lowLimit))>
    <Column(Order:=3)>
    Public Property lowLimit As Integer
    <DisplayName("Record Number")>
    <Column(Order:=0)>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(version))>
    <Column(Order:=5)>
    Public Property version As Integer
    Private Shared ReadOnly columnsToHide As New List(Of String) From {
             NameOf(LimitsRecord.kind),
             NameOf(LimitsRecord.version)
        }

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(LimitsRecord.kind)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(LimitsRecord.RecordNumber)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(LimitsRecord.index),
                 NameOf(LimitsRecord.version),
                 NameOf(LimitsRecord.highLimit),
                 NameOf(LimitsRecord.lowLimit)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function

End Class
