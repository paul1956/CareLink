' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Text

Public Class CareLinkUserDataList
    Inherits CollectionBase
    Implements IBindingList

    Private ReadOnly _onListChanged1 As ListChangedEventHandler
    Private ReadOnly _resetEvent As New ListChangedEventArgs(ListChangedType.Reset, -1)

    ' Events.
    Public Event ListChanged As ListChangedEventHandler Implements IBindingList.ListChanged

    ' Implements IBindingList.
    Public ReadOnly Property AllowEdit() As Boolean Implements IBindingList.AllowEdit
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property AllowNew() As Boolean Implements IBindingList.AllowNew
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property AllowRemove() As Boolean Implements IBindingList.AllowRemove
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property SupportsChangeNotification() As Boolean Implements IBindingList.SupportsChangeNotification
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property SupportsSearching() As Boolean Implements IBindingList.SupportsSearching
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property SupportsSorting() As Boolean Implements IBindingList.SupportsSorting
        Get
            Return False
        End Get
    End Property

    Default Public Property Item(index As Integer) As CareLinkUserDataRecord
        Get
            Return CType(Me.List(index), CareLinkUserDataRecord)
        End Get
        Set(Value As CareLinkUserDataRecord)
            Me.List(index) = Value
        End Set
    End Property

    Default Public Property Item(itemName As String) As CareLinkUserDataRecord
        Get
            If String.IsNullOrWhiteSpace(itemName) Then
                Throw New KeyNotFoundException($"Key may not be Nothing, in CareLinkUserDataList.Item")
            End If
            Try
                For i As Integer = 0 To Me.List.Count - 1
                    Dim entry As CareLinkUserDataRecord = CType(Me.List(i), CareLinkUserDataRecord)
                    If entry?.CareLinkUserName.Equals(itemName, StringComparison.OrdinalIgnoreCase) Then
                        Return CType(Me.List(i), CareLinkUserDataRecord)
                    End If
                Next
            Catch ex As Exception
                Return New CareLinkUserDataRecord(Me)
            End Try
            Throw New KeyNotFoundException($"Key '{itemName}' Not Present in Dictionary, in CareLinkUserDataList.Item")
        End Get
        Set(Value As CareLinkUserDataRecord)
            For i As Integer = 0 To Me.List.Count - 1
                Dim entry As CareLinkUserDataRecord = CType(Me.List(i), CareLinkUserDataRecord)
                If entry.CareLinkUserName.Equals(itemName, StringComparison.Ordinal) Then
                    Me.List(i) = Value
                    Exit Property
                End If
            Next
            Throw New KeyNotFoundException($"Key '{itemName}' Not Present in Dictionary")
        End Set
    End Property

    Protected Overrides Sub OnClear()
        Dim c As CareLinkUserDataRecord
        For Each c In Me.List
            c.Parent = Nothing
        Next c
    End Sub

    Protected Overrides Sub OnClearComplete()
        Me.OnListChanged(_resetEvent)
    End Sub

    Protected Overrides Sub OnInsertComplete(index As Integer, value As Object)
        Dim c As CareLinkUserDataRecord = CType(value, CareLinkUserDataRecord)
        c.Parent = Me
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.ItemAdded, index))
    End Sub

    Protected Overridable Sub OnListChanged(ev As ListChangedEventArgs)
        _onListChanged1?(Me, ev)
    End Sub

    Protected Overrides Sub OnRemoveComplete(index As Integer, value As Object)
        Dim c As CareLinkUserDataRecord = CType(value, CareLinkUserDataRecord)
        c.Parent = Me
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.ItemDeleted, index))
    End Sub

    Protected Overrides Sub OnSetComplete(index As Integer, oldValue As Object, newValue As Object)
        If oldValue Is newValue Then

            Dim oldUser As CareLinkUserDataRecord = CType(oldValue, CareLinkUserDataRecord)
            Dim newUser As CareLinkUserDataRecord = CType(newValue, CareLinkUserDataRecord)

            oldUser.Parent = Nothing
            newUser.Parent = Me

            Me.OnListChanged(New ListChangedEventArgs(ListChangedType.ItemAdded, index))
        End If
    End Sub

    ' Called by CareLinkUserDataRecord when it changes.
    Friend Sub CareLinkUserChanged(user As CareLinkUserDataRecord)
        Dim index As Integer = Me.List.IndexOf(user)
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.ItemChanged, index))
    End Sub

    Friend Function ContainsKey(key As String) As Boolean
        If String.IsNullOrWhiteSpace(key) Then
            Return False
        End If

        If Me.List Is Nothing Then Return False
        For Each entry As CareLinkUserDataRecord In Me
            If entry?.CareLinkUserName?.Equals(key, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    Friend Function Keys() As List(Of String)
        Dim result As New List(Of String)
        For Each entry As CareLinkUserDataRecord In Me
            result.Add(entry.CareLinkUserName)
        Next
        Return result
    End Function

    Friend Sub LoadUserRecords(userSettingsCsvFileWithPath As String)

        Dim l As IList = Me
        Using myReader As New FileIO.TextFieldParser(userSettingsCsvFileWithPath)
            myReader.TextFieldType = FileIO.FieldType.Delimited
            myReader.Delimiters = New String() {","}
            Dim currentRow As String()
            'Loop through all of the fields in the file.
            'If any lines are corrupt, report an error and continue parsing.
            Dim rowIndex As Integer = 0
            Dim headerRow As String() = Nothing
            While Not myReader.EndOfData
                Try
                    currentRow = myReader.ReadFields()
                    ' Include code here to handle the row.
                    If rowIndex = 0 Then
                        headerRow = currentRow
                    Else
                        l.Add(New CareLinkUserDataRecord(Me, headerRow, currentRow))
                    End If
                    rowIndex += 1
                Catch ex As FileIO.MalformedLineException
                    MsgBox("Malformed Line Exception", $"Line {ex.Message} is invalid.  Skipping", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Load User Records")
                End Try
            End While
        End Using

        Me.OnListChanged(_resetEvent)
    End Sub

    Friend Sub SaveAllUserRecords(loggedOnUser As CareLinkUserDataRecord, Key As String, Value As String)
        If Not Key.Equals(NameOf(My.Settings.CareLinkUserName).ToString, StringComparison.OrdinalIgnoreCase) Then
            ' We are changing something other than the user name
            ' Update logged on user and the saved file
            loggedOnUser.UpdateValue(Key, Value)
            If Not Me.TryAdd(loggedOnUser) Then
                Me(loggedOnUser.CareLinkUserName) = loggedOnUser
            End If
        Else
            ' We are changing the user name, first try to load it
            If Me.ContainsKey(Value) Then
                loggedOnUser = Me(Value)
            Else
                ' We have a new user
                Me.Add(loggedOnUser)
            End If
        End If

        Me.SaveAllUserRecords()
    End Sub

    Friend Function TryAdd(loggedOnUser As CareLinkUserDataRecord) As Boolean
        If Me.ContainsKey(loggedOnUser.CareLinkUserName) Then
            Return False
        End If
        Me.Add(loggedOnUser)
        Return True
    End Function

    Friend Function TryGetValue(key As String, ByRef userRecord As CareLinkUserDataRecord) As Boolean
        For Each entry As CareLinkUserDataRecord In Me
            If entry.CareLinkUserName = key Then
                userRecord = entry
                Return True
            End If
        Next
        Return False
    End Function

    Public Function Add(value As CareLinkUserDataRecord) As Integer
        Return Me.List.Add(value)
    End Function

    Public Function AddNew() As Object Implements IBindingList.AddNew
        Dim c As New CareLinkUserDataRecord(Me)
        Me.List.Add(c)
        Return c
    End Function

    Public Function AddNew2() As CareLinkUserDataRecord
        Return CType(CType(Me, IBindingList).AddNew(), CareLinkUserDataRecord)
    End Function

    Public Sub Remove(value As CareLinkUserDataRecord)
        Me.List.Remove(value)
    End Sub

    Public Sub SaveAllUserRecords()
        Dim sb As New StringBuilder
        sb.AppendLine(String.Join(",", s_headerColumns))
        For Each r As CareLinkUserDataRecord In Me
            sb.AppendLine(r.ToCsvString)
        Next
        My.Computer.FileSystem.WriteAllText(GetPathToAllUserLoginInfo(True), sb.ToString, False)
    End Sub

#Region "Unsupported Methods"

    Public Sub AddIndex(prop As PropertyDescriptor) Implements IBindingList.AddIndex
        Throw New NotSupportedException()
    End Sub

    Public Sub ApplySort(prop As PropertyDescriptor, direction As ListSortDirection) Implements IBindingList.ApplySort
        Throw New NotSupportedException()
    End Sub

    Public Function Find(prop As PropertyDescriptor, key As Object) As Integer Implements IBindingList.Find
        Throw New NotSupportedException()
    End Function

    Public Sub RemoveIndex(prop As PropertyDescriptor) Implements IBindingList.RemoveIndex
        Throw New NotSupportedException()
    End Sub

    Public Sub RemoveSort() Implements IBindingList.RemoveSort
        Throw New NotSupportedException()
    End Sub

#End Region' Unsupported Methods

#Region "Unsupported Properties"

    Public ReadOnly Property SortProperty() As PropertyDescriptor Implements IBindingList.SortProperty
        Get
            Throw New NotSupportedException()
        End Get
    End Property

    Public ReadOnly Property IsSorted() As Boolean Implements IBindingList.IsSorted
        Get
            Throw New NotSupportedException()
        End Get
    End Property

    Public ReadOnly Property SortDirection() As ListSortDirection Implements IBindingList.SortDirection
        Get
            Throw New NotSupportedException()
        End Get
    End Property

#End Region ' Unsupported Properties

End Class
