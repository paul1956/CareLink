' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class SupportedReportRecord

    Public Sub New(Values As Dictionary(Of String, String), recordNumber As Integer)
        If Values.Count <> 3 Then
            Dim innerException As New ApplicationException(
                message:="Invalid supported report record structure.")
            Throw New ApplicationException(
                message:=$"{NameOf(SupportedReportRecord)}({Values}) contains " &
                         $"{Values.Count} entries, 3 expected.",
                innerException)
        End If
        Me.recordNumber = recordNumber
        Me.report = Values(key:=NameOf(report))
        Dim forList As List(Of Dictionary(Of String, String)) =
            JsonToDictionaryList(json:=Values(key:=NameOf(onlyFor)))
        Me.onlyFor =
            kvpToString(forList).ToString.TrimStart(trimChar:=" "c).TrimEnd(trimChar:=","c)
        forList = JsonToDictionaryList(json:=Values(key:=NameOf(notFor)))
        Me.notFor =
            kvpToString(forList).ToString.TrimStart(trimChar:=" "c).TrimEnd(trimChar:=","c)

    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(recordNumber))>
    Public Property recordNumber As Integer

    <DisplayName("Report")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property report As String

    <DisplayName("Only For")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property onlyFor As String

    <DisplayName("Not For")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property notFor As String

    Private Shared Function kvpToString(forList As List(Of Dictionary(Of String, String))) As StringBuilder
        Dim sb As New StringBuilder
        For Each dic As Dictionary(Of String, String) In forList
            For Each kvp As KeyValuePair(Of String, String) In dic
                sb.Append($" {kvp.Key}={kvp.Value},")
            Next
        Next

        Return sb
    End Function

    Private Function GetDebuggerDisplay() As String
        Return Me.report.ToString()
    End Function

End Class
