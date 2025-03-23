' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class SummaryRecord
    Implements IComparable

    ''' <summary>
    ''' Used where message needs to be translated
    ''' </summary>
    ''' <param name="recordNumber"></param>
    ''' <param name="row"></param>
    ''' <param name="messages"></param>
    ''' <param name="messageTableName"></param>
    Protected Friend Sub New(recordNumber As Single, row As KeyValuePair(Of String, String), messages As Dictionary(Of String, String), messageTableName As String)
        Me.New(recordNumber, row)
        Dim message As String = ""
        If Not String.IsNullOrWhiteSpace(row.Value) Then
            If Not messages.TryGetValue(row.Value, message) Then
                If Debugger.IsAttached Then
                    Stop
                    MsgBox($"{row.Value} is unknown message for {messageTableName}!", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
                End If
                message = row.Value.ToTitle
            End If
        End If

        Me.Message = message
    End Sub

    ''' <summary>
    ''' Create new summary record form KVP
    ''' </summary>
    ''' <param name="recordNumber"></param>
    ''' <param name="row"></param>
    ''' <param name="message"></param>
    Protected Friend Sub New(recordNumber As Single, row As KeyValuePair(Of String, String), Optional message As String = "")
        Me.New(recordNumber, row.Key, row.Value, message)
    End Sub

    ''' <summary>
    ''' Summary record where record number is key
    ''' </summary>
    ''' <param name="recordNumber"></param>
    ''' <param name="key"></param>
    ''' <param name="Value"></param>
    Protected Friend Sub New(recordNumber As Single, key As ServerDataIndexes, Value As String)
        Me.New(recordNumber, key.ToString, Value, "")
    End Sub

    ''' <summary>
    ''' Summary record where record number is key
    ''' </summary>
    ''' <param name="recordNumber"></param>
    ''' <param name="Key"></param>
    ''' <param name="Value"></param>
    Protected Friend Sub New(recordNumber As Single, key As String)
        Me.New(recordNumber, key, ClickToShowDetails, "")
    End Sub

    ''' <summary>
    ''' Summary record where record number is key and we have a message
    ''' </summary>
    ''' <param name="recordNumber"></param>
    ''' <param name="value"></param>
    ''' <param name="message"></param>
    Protected Friend Sub New(recordNumber As Single, value As String, message As String)
        Me.New(recordNumber, CType(recordNumber, ServerDataIndexes).ToString, value, message)
    End Sub

    ''' <summary>
    ''' Handles No Message Case
    ''' </summary>
    ''' <param name="recordNumber"></param>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    Protected Friend Sub New(recordNumber As Single, key As String, value As String, message As String)
        Me.RecordNumber = recordNumber + 1
        Me.Key = key
        Me.Value = value
        Me.Message = message
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public ReadOnly Property RecordNumber As Single

    <DisplayName("Key")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public ReadOnly Property Key As String

    <DisplayName("Value")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public ReadOnly Property Value As String

    <DisplayName("Message")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public ReadOnly Property Message As String = ""

    ''' <summary>
    ''' Do not delete this function. It is used to implement IComparable
    ''' </summary>
    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim bom As SummaryRecord = CType(obj, SummaryRecord)

        If bom IsNot Nothing Then
            Return Me.RecordNumber.CompareTo(bom.RecordNumber)
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function

End Class
