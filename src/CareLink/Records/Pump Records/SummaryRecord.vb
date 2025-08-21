' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

''' <summary>
'''   Represents a summary record for pump data, containing a record number, key, value, and message.
'''   Provides multiple constructors for different initialization scenarios and
'''   implements <see cref="IComparable"/> for sorting.
''' </summary>
Public Class SummaryRecord
    Implements IComparable

    ''' <summary>
    '''  Initializes a new instance of the <see cref="SummaryRecord"/> class using a key-value pair
    '''  and a message lookup table. Used where the message needs to be translated.
    ''' </summary>
    ''' <param name="recordNumber">The record number associated with this summary record.</param>
    ''' <param name="kvp">A key-value pair representing the data row.</param>
    ''' <param name="messages">A dictionary containing message translations.</param>
    ''' <param name="messageTableName">The name of the message table for error reporting.</param>
    ''' <remarks>Handles messages that are not in the message table.</remarks>
    Protected Friend Sub New(
            recordNumber As Single,
            kvp As KeyValuePair(Of String, String),
            messages As Dictionary(Of String, String),
            messageTableName As String)
        Me.New(recordNumber, kvp)
        Dim message As String = ""
        If Not String.IsNullOrWhiteSpace(kvp.Value) Then
            If Not messages.TryGetValue(key:=kvp.Value, value:=message) Then
                If Debugger.IsAttached Then
                    Stop
                    MsgBox(
                        heading:=$"{kvp.Value} is unknown message for {messageTableName}!",
                        prompt:="",
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                        title:=GetTitleFromStack(New StackFrame(skipFrames:=0, needFileInfo:=True)))
                End If
                message = kvp.Value.ToTitle
            End If
        End If

        Me.Message = message.Replace(vbCrLf, " ")
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="SummaryRecord"/> class
    '''  from a key-value pair
    '''  and an optional message.
    ''' </summary>
    ''' <param name="recordNumber">
    '''  The record number associated with this summary record.
    ''' </param>
    ''' <param name="kvp">A key-value pair representing the data row.</param>
    ''' <param name="message">An optional message for the record.</param>
    Protected Friend Sub New(
            recordNumber As Single,
            kvp As KeyValuePair(Of String, String),
            Optional message As String = "")

        Me.New(recordNumber, kvp.Key, kvp.Value, message)
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="SummaryRecord"/> class
    '''  using a <see cref="ServerDataIndexes"/> key and value.
    ''' </summary>
    ''' <param name="recordNumber">
    '''  The record number associated with this summary record.
    ''' </param>
    ''' <param name="key">The key as a <see cref="ServerDataIndexes"/> enum value.</param>
    ''' <param name="value">The value associated with the key.</param>
    Protected Friend Sub New(recordNumber As Single, key As ServerDataIndexes, value As String)
        Me.New(recordNumber, key.ToString, value, "")
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="SummaryRecord"/>
    '''  class using a key and a default message.
    '''  Used where we will provide a Button to click for details on another
    '''  <see cref="TabPage"/>.
    ''' </summary>
    ''' <param name="recordNumber">
    '''  The record number associated with this summary record.
    ''' </param>
    ''' <param name="key">The key for the record.</param>
    ''' <remarks>
    '''  Used where we will provide a Button to click for details
    '''  on another <see cref="TabPage"/>.
    ''' </remarks>
    Protected Friend Sub New(recordNumber As Single, key As String)
        Me.New(recordNumber, key, value:=ClickToShowDetails, message:="")
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="SummaryRecord"/> class
    '''  using a record number, key, value, and message.
    ''' </summary>
    ''' <param name="recordNumber">
    '''  The record number associated with this summary record.
    ''' </param>
    ''' <param name="value">The value associated with the key.</param>
    ''' <param name="message">The message for the record.</param>
    Protected Friend Sub New(recordNumber As Single, value As String, message As String)
        Me.New(
            recordNumber,
            key:=CType(recordNumber, ServerDataIndexes).ToString,
            value,
            message)
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="SummaryRecord"/> class
    ''' using a record number, key, value, and message.
    ''' </summary>
    ''' <param name="recordNumber">
    '''  The record number associated with this summary record.
    ''' </param>
    ''' <param name="key">The key for the record.</param>
    ''' <param name="value">The value associated with the key.</param>
    ''' <param name="message">The message for the record.</param>
    Protected Friend Sub New(
        recordNumber As Single,
        key As String,
        value As String,
        message As String)

        Me.RecordNumber = recordNumber + 1
        Me.Key = key
        Me.Value = value
        Me.Message = message
    End Sub

    <JsonIgnore>
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
    '''  Compares this instance with a specified <see cref="SummaryRecord"/> and
    '''  indicates whether this instance
    '''  precedes, follows, or appears in the same position in the sort order
    '''  as the specified object.
    ''' </summary>
    ''' <param name="obj">The object to compare with this instance.</param>
    ''' <returns>
    '''  A value that indicates the relative order of the objects being compared.
    ''' </returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown when the object is not a <see cref="SummaryRecord"/>.
    ''' </exception>
    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim bom As SummaryRecord = CType(obj, SummaryRecord)

        If bom IsNot Nothing Then
            Return Me.RecordNumber.CompareTo(bom.RecordNumber)
        Else
            Throw New ArgumentException("Object is not a BomItem")
        End If
    End Function

End Class
