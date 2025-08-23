' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Text

''' <summary>
'''  Represents a collection of <see cref="CareLinkUserDataRecord"/> objects
'''  with support for data binding.
''' </summary>
''' <remarks>
'''  <para>
'''   This class implements <see cref="IBindingList"/> to provide change notification
'''   and editing support for user data records in the CareLink™ application.
'''   Sorting and searching are not supported.
'''  </para>
''' </remarks>
Public Class CareLinkUserDataList
    Inherits CollectionBase
    Implements IBindingList

    Private ReadOnly _onListChanged1 As ListChangedEventHandler
    Private ReadOnly _resetEvent As _
        New ListChangedEventArgs(listChangedType:=ListChangedType.Reset, newIndex:=-1)

    ''' <summary>
    '''  Occurs when the list changes or an item in the list changes.
    ''' </summary>
    Public Event ListChanged As ListChangedEventHandler Implements IBindingList.ListChanged

    ''' <summary>
    '''  Gets a value indicating whether items in the list can be edited.
    ''' </summary>
    Public ReadOnly Property AllowEdit() As Boolean Implements IBindingList.AllowEdit
        Get
            Return True
        End Get
    End Property

    ''' <summary>
    '''  Gets a value indicating whether new items can be added to the list.
    ''' </summary>
    Public ReadOnly Property AllowNew() As Boolean Implements IBindingList.AllowNew
        Get
            Return True
        End Get
    End Property

    ''' <summary>
    '''  Gets a value indicating whether items can be removed from the list.
    ''' </summary>
    Public ReadOnly Property AllowRemove() As Boolean Implements IBindingList.AllowRemove
        Get
            Return True
        End Get
    End Property

    ''' <summary>
    '''  Gets a value indicating whether the list supports change notification.
    ''' </summary>
    Public ReadOnly Property SupportsChangeNotification() As Boolean _
        Implements IBindingList.SupportsChangeNotification
        Get
            Return True
        End Get
    End Property

    ''' <summary>
    '''  Gets a value indicating whether the list supports searching.
    ''' </summary>
    Public ReadOnly Property SupportsSearching() As Boolean _
        Implements IBindingList.SupportsSearching

        Get
            Return False
        End Get
    End Property

    ''' <summary>
    '''  Gets a value indicating whether the list supports sorting.
    ''' </summary>
    Public ReadOnly Property SupportsSorting() As Boolean _
        Implements IBindingList.SupportsSorting

        Get
            Return False
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets the <see cref="CareLinkUserDataRecord"/> at the specified index.
    ''' </summary>
    ''' <param name="index">The zero-based index of the element to get or set.</param>
    Default Public Property Item(index As Integer) As CareLinkUserDataRecord
        Get
            Return CType(Me.List(index), CareLinkUserDataRecord)
        End Get
        Set(Value As CareLinkUserDataRecord)
            Me.List(index) = Value
        End Set
    End Property

    ''' <summary>
    '''  Gets or sets the <see cref="CareLinkUserDataRecord"/> with the specified user name.
    ''' </summary>
    ''' <param name="itemName">The user name to locate in the list.</param>
    ''' <exception cref="KeyNotFoundException">
    '''  Thrown if the specified user name is not found in the list.
    ''' </exception>
    Default Public Property Item(itemName As String) As CareLinkUserDataRecord
        Get
            Dim message As String
            If String.IsNullOrWhiteSpace(value:=itemName) Then
                message = "Key may not be Nothing, in CareLinkUserDataList.Item"
                Throw New KeyNotFoundException(message)
            End If
            Try
                For index As Integer = 0 To Me.List.Count - 1
                    Dim entry As CareLinkUserDataRecord =
                        CType(Me.List(index), CareLinkUserDataRecord)
                    If entry?.CareLinkUserName.EqualsNoCase(itemName) Then
                        Return CType(Me.List(index), CareLinkUserDataRecord)
                    End If
                Next
            Catch ex As Exception
                Return New CareLinkUserDataRecord(parent:=Me)
            End Try
            message =
                $"Key '{itemName}' Not Present in Dictionary, in CareLinkUserDataList.Item"
            Throw New KeyNotFoundException(message)
        End Get
        Set(Value As CareLinkUserDataRecord)
            For index As Integer = 0 To Me.List.Count - 1
                Dim entry As CareLinkUserDataRecord =
                    CType(Me.List(index), CareLinkUserDataRecord)
                If entry.CareLinkUserName.EqualsNoCase(itemName) Then
                    Me.List(index) = Value
                    Exit Property
                End If
            Next
            Dim message As String = $"Key '{itemName}' Not Present in Dictionary"
            Throw New KeyNotFoundException(message)
        End Set
    End Property

    ''' <summary>
    '''  Clears the parent reference for all items in the list.
    ''' </summary>
    Protected Overrides Sub OnClear()
        Dim c As CareLinkUserDataRecord
        For Each c In Me.List
            c.Parent = Nothing
        Next c
    End Sub

    ''' <summary>
    '''  Raises the <see cref="ListChanged"/> event after the list is cleared.
    ''' </summary>
    Protected Overrides Sub OnClearComplete()
        Me.OnListChanged(_resetEvent)
    End Sub

    ''' <summary>
    '''  Sets the parent reference and raises the <see cref="ListChanged"/>
    '''  event after an item is inserted.
    ''' </summary>
    ''' <param name="newIndex">The index at which the item was inserted.</param>
    ''' <param name="value">The inserted item.</param>
    Protected Overrides Sub OnInsertComplete(newIndex As Integer, value As Object)
        Dim c As CareLinkUserDataRecord = CType(value, CareLinkUserDataRecord)
        c.Parent = Me
        Dim e As New ListChangedEventArgs(listChangedType:=ListChangedType.ItemAdded, newIndex)
        Me.OnListChanged(e)
    End Sub

    ''' <summary>
    '''  Raises the <see cref="ListChanged"/> event.
    ''' </summary>
    ''' <param name="e">The event arguments.</param>
    Protected Overridable Sub OnListChanged(e As ListChangedEventArgs)
        _onListChanged1?(sender:=Me, e)
    End Sub

    ''' <summary>
    '''  Sets the parent reference and raises the <see cref="ListChanged"/> event
    '''  after an item is removed.
    ''' </summary>
    ''' <param name="newIndex">The index of the removed item.</param>
    ''' <param name="value">The removed item.</param>
    Protected Overrides Sub OnRemoveComplete(newIndex As Integer, value As Object)
        Dim c As CareLinkUserDataRecord = CType(value, CareLinkUserDataRecord)
        c.Parent = Me
        Dim e As New ListChangedEventArgs(
            listChangedType:=ListChangedType.ItemDeleted,
            newIndex)
        Me.OnListChanged(e)
    End Sub

    ''' <summary>
    '''  Updates parent references and raises the <see cref="ListChanged"/> event
    '''  after an item is set.
    ''' </summary>
    ''' <param name="newIndex">The index of the item.</param>
    ''' <param name="oldValue">The old value.</param>
    ''' <param name="newValue">The new value.</param>
    Protected Overrides Sub OnSetComplete(
        newIndex As Integer,
        oldValue As Object,
        newValue As Object)

        If oldValue Is newValue Then
            Dim oldUser As CareLinkUserDataRecord = CType(oldValue, CareLinkUserDataRecord)
            Dim newUser As CareLinkUserDataRecord = CType(newValue, CareLinkUserDataRecord)
            oldUser.Parent = Nothing
            newUser.Parent = Me
            Dim e As New ListChangedEventArgs(
                listChangedType:=ListChangedType.ItemAdded,
                newIndex)
            Me.OnListChanged(e)
        End If
    End Sub

    ''' <summary>
    '''  Notifies the list that a user record has changed.
    ''' </summary>
    ''' <param name="value">The changed user record.</param>
    Friend Sub CareLinkUserChanged(value As CareLinkUserDataRecord)
        Dim newIndex As Integer = Me.List.IndexOf(value)
        Dim e As New ListChangedEventArgs(
            listChangedType:=ListChangedType.ItemChanged,
            newIndex)
        Me.OnListChanged(e)
    End Sub

    ''' <summary>
    '''  Determines whether the list contains a user with the specified key.
    ''' </summary>
    ''' <param name="key">The user name to locate.</param>
    ''' <returns>
    '''  <see langword="True"/> if the user exists;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function ContainsKey(key As String) As Boolean
        If String.IsNullOrWhiteSpace(value:=key) Then
            Return False
        End If

        If Me.List Is Nothing Then Return False
        For Each entry As CareLinkUserDataRecord In Me
            If EqualsNoCase(a:=entry?.CareLinkUserName, b:=key) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    '''  Gets a list of all user names in the collection.
    ''' </summary>
    ''' <returns>
    '''  A <see cref="List(Of String)"/> containing all user names.
    ''' </returns>
    Friend Function Keys() As List(Of String)
        Dim result As New List(Of String)
        For Each entry As CareLinkUserDataRecord In Me
            result.Add(item:=entry.CareLinkUserName)
        Next
        Return result
    End Function

    ''' <summary>
    '''  Loads user records from a CSV file.
    ''' </summary>
    Friend Sub LoadUserRecords()
        Dim l As IList = Me
        Using myReader As New FileIO.TextFieldParser(path:=GetAllUsersCsvPath())
            myReader.TextFieldType = FileIO.FieldType.Delimited
            myReader.Delimiters = New String() {","}
            Dim currentRow As String()
            ' Loop through all of the fields in the file.
            ' If any lines are corrupt, report an error and continue parsing.
            Dim rowIndex As Integer = 0
            Dim headerRow As String() = Nothing
            While Not myReader.EndOfData
                Try
                    currentRow = myReader.ReadFields()
                    If rowIndex = 0 Then
                        headerRow = currentRow
                    Else
                        Dim value As New CareLinkUserDataRecord(
                            parent:=Me,
                            headerRow,
                            currentRow)
                        l.Add(value)
                    End If
                    rowIndex += 1
                Catch ex As FileIO.MalformedLineException
                    MsgBox(
                      heading:="Malformed Line Exception",
                      prompt:=$"Line {ex.Message} is invalid.  Skipping",
                      buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                      title:="Load User Records")
                End Try
            End While
        End Using

        Me.OnListChanged(_resetEvent)
    End Sub

    ''' <summary>
    '''  Saves all user records, updating the specified key and value for the logged-on user.
    ''' </summary>
    ''' <param name="loggedOnUser">The user to update.</param>
    ''' <param name="key">The key to update.</param>
    ''' <param name="value">The value to set.</param>
    Friend Sub SaveAllUserRecords(
        loggedOnUser As CareLinkUserDataRecord,
        key As String,
        value As String)

        If Not key.EqualsNoCase(NameOf(My.Settings.CareLinkUserName)) Then
            ' We are changing something other than the user name
            ' Update logged on user and the saved file
            loggedOnUser.UpdateValue(key, value)
            If Not Me.TryAdd(loggedOnUser) Then
                Me(itemName:=loggedOnUser.CareLinkUserName) = loggedOnUser
            End If
        Else
            ' We are changing the user name, first try to load it
            If Me.ContainsKey(key:=value) Then
                loggedOnUser = Me(itemName:=value)
            Else
                ' We have a new user
                Me.Add(value:=loggedOnUser)
            End If
        End If

        Me.SaveAllUserRecords()
    End Sub

    ''' <summary>
    '''  Attempts to add a user record if it does not already exist.
    ''' </summary>
    ''' <param name="loggedOnUser">The user record to add.</param>
    ''' <returns>
    '''  <see langword="True"/> if the user was added;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function TryAdd(loggedOnUser As CareLinkUserDataRecord) As Boolean
        If Me.ContainsKey(key:=loggedOnUser.CareLinkUserName) Then
            Return False
        End If
        Me.Add(value:=loggedOnUser)
        Return True
    End Function

    ''' <summary>
    '''  Attempts to get a user record by key.
    ''' </summary>
    ''' <param name="key">The user name to locate.</param>
    ''' <param name="userRecord">
    '''  When this method returns, contains the user record if found;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the user was found;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Friend Function TryGetValue(
        key As String,
        ByRef userRecord As CareLinkUserDataRecord) As Boolean

        For Each entry As CareLinkUserDataRecord In Me
            If entry.CareLinkUserName = key Then
                userRecord = entry
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    '''  Adds a <see cref="CareLinkUserDataRecord"/> to the list.
    ''' </summary>
    ''' <param name="value">The user record to add.</param>
    ''' <returns>
    '''  The index at which the value has been added.
    ''' </returns>
    Public Function Add(value As CareLinkUserDataRecord) As Integer
        Return Me.List.Add(value)
    End Function

    ''' <summary>
    '''  Adds a new <see cref="CareLinkUserDataRecord"/> to the list and returns it.
    ''' </summary>
    ''' <returns>
    '''  The newly added <see cref="CareLinkUserDataRecord"/>.
    ''' </returns>
    Public Function AddNew() As Object Implements IBindingList.AddNew
        Dim value As New CareLinkUserDataRecord(parent:=Me)
        Me.List.Add(value)
        Return value
    End Function

    ''' <summary>
    '''  Adds a new <see cref="CareLinkUserDataRecord"/> to the list and
    '''  returns it as a strongly typed object.
    ''' </summary>
    ''' <returns>
    '''  The newly added <see cref="CareLinkUserDataRecord"/>.
    ''' </returns>
    Public Function AddNew2() As CareLinkUserDataRecord
        Return CType(CType(Me, IBindingList).AddNew(), CareLinkUserDataRecord)
    End Function

    ''' <summary>
    '''  Removes the specified <see cref="CareLinkUserDataRecord"/> from the list.
    ''' </summary>
    ''' <param name="value">The user record to remove.</param>
    Public Sub Remove(value As CareLinkUserDataRecord)
        Me.List.Remove(value)
    End Sub

    ''' <summary>
    '''  Saves all user records to the persistent storage.
    ''' </summary>
    Public Sub SaveAllUserRecords()
        Dim sb As New StringBuilder
        sb.AppendLine(value:=String.Join(separator:=",", values:=s_headerColumns))
        For Each r As CareLinkUserDataRecord In Me
            sb.AppendLine(value:=r.ToCsvString)
        Next
        Try
            My.Computer.FileSystem.WriteAllText(
                file:=GetAllUsersCsvPath(),
                text:=sb.ToString,
                append:=False)
        Catch ex As Exception
            ' Handle exceptions as needed
        End Try
    End Sub

#Region "IBindingList Minimal Implementations"

    ''' <summary>
    '''  Not supported. Always returns <see langword="False"/>.
    ''' </summary>
    Public ReadOnly Property IsSorted As Boolean Implements IBindingList.IsSorted
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    '''  Not supported. Always returns <see cref="ListSortDirection.Ascending"/>.
    ''' </summary>
    Public ReadOnly Property SortDirection As ListSortDirection _
        Implements IBindingList.SortDirection
        Get
            Return ListSortDirection.Ascending
        End Get
    End Property

    ''' <summary>
    '''  Not supported. Always returns <see langword="Nothing"/>.
    ''' </summary>
    Public ReadOnly Property SortProperty As PropertyDescriptor _
        Implements IBindingList.SortProperty
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    '''  Not supported. Required by <see cref="IBindingList"/>.
    ''' </summary>
    ''' <param name="propertyDescriptor">The property descriptor.</param>
    Public Sub AddIndex(propertyDescriptor As PropertyDescriptor) _
        Implements IBindingList.AddIndex
        ' No index support
    End Sub

    ''' <summary>
    '''  Not supported. Required by <see cref="IBindingList"/>.
    ''' </summary>
    ''' <param name="propertyDescriptor">The property descriptor.</param>
    ''' <param name="direction">The sort direction.</param>
    Public Sub ApplySort(
            propertyDescriptor As PropertyDescriptor,
            direction As ListSortDirection) Implements IBindingList.ApplySort

        ' No sorting support
    End Sub

    ''' <summary>
    '''  Not supported. Required by <see cref="IBindingList"/>.
    ''' </summary>
    ''' <param name="propertyDescriptor">The property descriptor.</param>
    ''' <param name="key">The key to search for.</param>
    ''' <returns>
    '''  Always returns -1.
    ''' </returns>
    Public Function Find(
        propertyDescriptor As PropertyDescriptor,
        key As Object) As Integer Implements IBindingList.Find

        ' No searching support
        Return -1
    End Function

    ''' <summary>
    '''  Not supported. Required by <see cref="IBindingList"/>.
    ''' </summary>
    ''' <param name="propertyDescriptor">The property descriptor.</param>
    Public Sub RemoveIndex(propertyDescriptor As PropertyDescriptor) _
        Implements IBindingList.RemoveIndex
        ' No index support
    End Sub

    ''' <summary>
    '''  Not supported. Required by <see cref="IBindingList"/>.
    ''' </summary>
    Public Sub RemoveSort() Implements IBindingList.RemoveSort
        ' No sorting support
    End Sub

#End Region ' IBindingList Minimal Implementations

End Class
