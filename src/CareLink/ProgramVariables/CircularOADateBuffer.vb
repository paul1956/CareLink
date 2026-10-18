' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
''' A circular (ring) _buffer for OADateValueRecord values.
''' Supports dynamic resizing with oldest-entry removal or padding.
''' </summary>
Public Class CircularOADateBuffer
    Private ReadOnly _newOaDate As New OADate(oADateAsDouble:=0)
    Private _buffer() As OADateValueRecord
    Private _count As Integer ' Number of elements currently stored
    Private _head As Integer ' Index of the oldest element

    Public Sub New(initialCapacity As Integer)
        If initialCapacity <= 0 Then
            Throw New ArgumentOutOfRangeException(
            paramName:=NameOf(initialCapacity),
            message:="Capacity must be positive.")
        End If

        ReDim _buffer(initialCapacity - 1)

        For i As Integer = 0 To initialCapacity - 1
            _buffer(i) = New OADateValueRecord(oaDate:=_newOaDate, sg:=0)
        Next

        _head = 0
        _count = initialCapacity ' Buffer is logically full of empty records
    End Sub

    ''' <summary>
    ''' Current capacity of the _buffer.
    ''' </summary>
    Public ReadOnly Property Capacity As Integer
        Get
            Return _buffer.Length
        End Get
    End Property

    ''' <summary>
    ''' Current number of stored entries.
    ''' </summary>
    Public ReadOnly Property Count As Integer
        Get
            Return _count
        End Get
    End Property

    ''' <summary>
    ''' True when the buffer contains no non-empty records.
    ''' </summary>
    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return Not Me.IsNotEmpty
        End Get
    End Property

    ''' <summary>
    ''' True when the buffer contains at least one item
    ''' whose OaDate and Value are both non-zero.
    ''' </summary>
    Public ReadOnly Property IsNotEmpty As Boolean
        Get
            For i As Integer = 0 To _count - 1
                If _buffer((_head + i) Mod Me.Capacity).IsNotEmpty Then
                    Return True
                End If
            Next

            Return False
        End Get
    End Property

    ''' <summary>
    ''' Adds a new item to the buffer unless it is identical to the newest entry.
    ''' Overwrites oldest entry if full.
    ''' </summary>
    Public Sub Add(record As OADateValueRecord)
        If Me.Capacity = 0 Then Return

        ' Check if same as newest entry
        If Me.Count > 0 Then
            Dim lastIndex As Integer = (_head + _count - 1) Mod Me.Capacity
            Dim lastRec As OADateValueRecord = _buffer(lastIndex)
            If lastRec.OaDate = record.OaDate AndAlso lastRec.Value = record.Value Then
                ' Skip adding duplicate
                Return
            End If
        End If

        Dim tail As Integer = (_head + _count) Mod Me.Capacity
        If _count = Me.Capacity Then
            ' Buffer full — overwrite oldest
            _buffer(tail) = record
            _head = (_head + 1) Mod Me.Capacity
        Else
            _buffer(tail) = record
            _count += 1
        End If
    End Sub

    ''' <summary>
    ''' Resets every slot to an empty record.
    ''' Capacity and logical count remain unchanged.
    ''' </summary>
    Public Sub Clear()
        For i As Integer = 0 To _buffer.Length - 1
            _buffer(i) = New OADateValueRecord(oaDate:=_newOaDate, sg:=0)
        Next

        _head = 0
        _count = Me.Capacity
    End Sub

    ''' <summary>
    ''' Resizes the buffer.
    ''' If smaller, the newest entries are retained and oldest entries are dropped.
    ''' If larger, empty records are added at the oldest end.
    ''' </summary>
    Public Sub Resize(newCapacity As Integer)
        If newCapacity <= 0 Then
            Throw New ArgumentOutOfRangeException(
            paramName:=NameOf(newCapacity),
            message:="Capacity must be positive.")
        End If

        Dim newBuffer(newCapacity - 1) As OADateValueRecord

        If newCapacity < _count Then
            ' Keep only the newest newCapacity records.
            Dim startIndex As Integer = _count - newCapacity

            For i As Integer = 0 To newCapacity - 1
                newBuffer(i) = _buffer((_head + startIndex + i) Mod Me.Capacity)
            Next

            _head = 0
            _count = newCapacity
        Else
            ' Copy all existing records, oldest to newest.
            For i As Integer = 0 To _count - 1
                newBuffer(i) = _buffer((_head + i) Mod Me.Capacity)
            Next

            ' Pad the remaining oldest slots with empty records.
            Dim emptyCount As Integer = newCapacity - _count

            For i As Integer = 0 To emptyCount - 1
                newBuffer(i) = New OADateValueRecord(oaDate:=_newOaDate, sg:=0)
            Next

            ' Shift existing records after the empty padding.
            For i As Integer = 0 To _count - 1
                newBuffer(emptyCount + i) = newBuffer(i)
            Next

            _head = 0
            _count = newCapacity
        End If

        _buffer = newBuffer
    End Sub

    ''' <summary>
    ''' Gets all elements sorted by date from oldest to newest,
    ''' including empty records.
    ''' </summary>
    Public Function ToKeyValuePairList() As List(Of KeyValuePair(Of OADate, Single))
        Dim result As New List(Of KeyValuePair(Of OADate, Single))(capacity:=_count)
        For index As Integer = 0 To _count - 1
            Dim record As OADateValueRecord =
                _buffer((_head + index) Mod Me.Capacity)

            Dim key As OADate = record.OaDate
            If key = _newOaDate Then
                key = s_sgRecords(index).GetOaDateTime()
            End If
            Dim item As New KeyValuePair(Of OADate, Single)(
                key,
                record.Value)
            result.Add(item)
        Next

        Return result
    End Function

End Class
