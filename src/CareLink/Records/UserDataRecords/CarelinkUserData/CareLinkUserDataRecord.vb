' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

''' <summary>
'''  Represents a user data record for CareLink, encapsulating user credentials and settings.
'''  Supports editing operations and notifies the parent list of changes.
''' </summary>
Partial Public Class CareLinkUserDataRecord
    Implements IEditableObject

    Private _backupData As CareLinkUserData
    Private _inTxn As Boolean = False
    Private _userData As CareLinkUserData

    ''' <summary>
    '''  Initializes a new instance of the <see cref="CareLinkUserDataRecord"/>
    '''  class with the specified parent list. Populates user data from application settings.
    ''' </summary>
    ''' <param name="parent">The parent <see cref="CareLinkUserDataList"/>.</param>
    Public Sub New(parent As CareLinkUserDataList)
        Me.Parent = parent
        _userData = New CareLinkUserData With {
            ._iD = parent.Count,
            ._careLinkUserName = If(My.Settings.CareLinkUserName, ""),
            ._careLinkPassword = If(My.Settings.CareLinkPassword, ""),
            ._countryCode = If(My.Settings.CountryCode, ""),
             ._useLocalTimeZone = My.Settings.UseLocalTimeZone,
            ._autoLogin = My.Settings.AutoLogin,
            ._careLinkPartner = My.Settings.CareLinkPartner,
            ._careLinkPatientUserID =
                If(._careLinkPartner,
                   My.Settings.CareLinkPatientUserID,
                   "")}
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="CareLinkUserDataRecord"/> class
    '''  from a CSV row.
    ''' </summary>
    ''' <param name="parent">The parent <see cref="CareLinkUserDataList"/>.</param>
    ''' <param name="headerRow">The header row of the CSV file.</param>
    ''' <param name="currentRow">The current data row from the CSV file.</param>
    Public Sub New(
        parent As CareLinkUserDataList,
        headerRow As String(),
        currentRow As String())

        _userData = New CareLinkUserData With {
            ._iD = parent.Count
        }
        For Each e As IndexClass(Of String) In currentRow.WithIndex
            Dim value As String = If(e.Value, "")
            Me.UpdateValue(headerRow(e.Index), value)
        Next
    End Sub

#If True Then

    <DisplayName("Id")>
    <Column(Order:=0, TypeName:=NameOf([Int32]))>
    Public ReadOnly Property ID() As Integer
        Get
            Return _userData._iD
        End Get
    End Property

    <DisplayName("CareLink™ UserName")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property CareLinkUserName As String
        Get
            Return _userData._careLinkUserName
        End Get
        Set
            _userData._careLinkUserName = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("CareLink™ Password")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property CareLinkPassword As String
        Get
            Return _userData._careLinkPassword
        End Get
        Set
            _userData._careLinkPassword = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Country Code")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property CountryCode As String
        Get
            Return _userData._countryCode
        End Get
        Set
            _userData._countryCode = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Use Local Time Zone")>
    <Column(Order:=4, TypeName:=NameOf([Boolean]))>
    Public Property UseLocalTimeZone As Boolean
        Get
            Return _userData._useLocalTimeZone
        End Get
        Set
            _userData._useLocalTimeZone = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("Auto Login")>
    <Column(Order:=5, TypeName:=NameOf([Boolean]))>
    Public Property AutoLogin As Boolean
        Get
            Return _userData._autoLogin
        End Get
        Set
            _userData._autoLogin = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("CareLink™ Partner")>
    <Column(Order:=6, TypeName:=NameOf([Boolean]))>
    Public Property CareLinkPartner As Boolean
        Get
            Return _userData._careLinkPartner
        End Get
        Set
            _userData._careLinkPartner = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

    <DisplayName("CareLink™ Patient UserID")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    Public Property CareLinkPatientUserID As String
        Get
            Return _userData._careLinkPatientUserID
        End Get
        Set
            _userData._careLinkPatientUserID = Value
            Me.OnCareLinkUserChanged()
        End Set
    End Property

#End If

    Public Property Parent As CareLinkUserDataList

    ''' <summary>
    '''  Notifies the parent list that the user data has changed, unless in a transaction.
    ''' </summary>
    Private Sub OnCareLinkUserChanged()
        If Not _inTxn And (Me.Parent IsNot Nothing) Then
            Me.Parent.CareLinkUserChanged(value:=Me)
        End If
    End Sub

    ''' <summary>
    '''  Returns a CSV string representation of the user data record.
    ''' </summary>
    ''' <returns>A CSV string of the user data fields.</returns>
    Friend Function ToCsvString() As String
        Dim values As New List(Of String) From {
            Me.CareLinkUserName,
            Me.CareLinkPassword,
            Me.CountryCode,
            Me.UseLocalTimeZone.ToString,
            Me.AutoLogin.ToString,
            Me.CareLinkPartner.ToString,
            Me.CareLinkPatientUserID}
        Return String.Join(separator:=",", values)
    End Function

    ''' <summary>
    '''  Updates the application settings with the current user data values.
    ''' </summary>
    Friend Sub UpdateSettings()
        My.Settings.CareLinkUserName = _userData._careLinkUserName
        My.Settings.CareLinkPassword = _userData._careLinkPassword
        My.Settings.CountryCode = _userData._countryCode
        My.Settings.UseLocalTimeZone = _userData._useLocalTimeZone
        My.Settings.AutoLogin = _userData._autoLogin
        My.Settings.CareLinkPartner = _userData._careLinkPartner
        My.Settings.CareLinkPatientUserID = _userData._careLinkPatientUserID
    End Sub

    ''' <summary>
    '''  Replace the value of entryName with Value.
    ''' </summary>
    ''' <param name="key">The property name to update.</param>
    ''' <param name="value">The new value as a string.</param>
    Friend Sub UpdateValue(key As String, value As String)
        Select Case key
            Case NameOf(CareLinkUserName)
                Me.CareLinkUserName = value
            Case NameOf(CareLinkPassword)
                Me.CareLinkPassword = value
            Case NameOf(CountryCode)
                Me.CountryCode = value
            Case NameOf(UseLocalTimeZone)
                Me.UseLocalTimeZone = CBool(value)
            Case NameOf(AutoLogin)
                Me.AutoLogin = CBool(value)
            Case NameOf(CareLinkPartner)
                Me.CareLinkPartner = CBool(value)
            Case NameOf(CareLinkPatientUserID)
                Me.CareLinkPatientUserID = value
            Case NameOf(My.Settings.TiTrLowThreshold),
                 NameOf(My.Settings.TiTrTreatmentTargetPercent)
                ' Ignore as these are not user based settings
            Case Else
                Stop
        End Select
    End Sub

#Region "Implements IEditableObject"

    Public Sub BeginEdit() Implements IEditableObject.BeginEdit
        Debug.WriteLine(message:=$"Start EndEdit{_userData._iD}{_userData._careLinkUserName}")
        If Not _inTxn Then
            _backupData = _userData
            _inTxn = True

            Dim message As String =
                $"BeginEdit  - {_userData._iD}{_userData._careLinkUserName}"
            Debug.WriteLine(message)
        End If
    End Sub

    Public Sub CancelEdit() Implements IEditableObject.CancelEdit
        Debug.WriteLine(message:="Start CancelEdit")
        If _inTxn Then
            _userData = _backupData
            Dim message As String =
                $"CancelEdit - {_userData._iD}{_userData._careLinkUserName}"
            Debug.WriteLine(message)
        End If
        Debug.WriteLine(message:="End CancelEdit")
    End Sub

    Public Sub EndEdit() Implements IEditableObject.EndEdit
        Debug.WriteLine(message:=$"Start EndEdit{_userData._iD}{_userData._careLinkUserName}")
        If _inTxn Then
            _backupData = New CareLinkUserData()
            _inTxn = False
            Dim message As String =
                $"Done EndEdit - {_userData._iD}{_userData._careLinkUserName}"
            Debug.WriteLine(message)
        End If
        Debug.WriteLine(message:="End EndEdit")
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim record As CareLinkUserDataRecord = TryCast(obj, CareLinkUserDataRecord)
        Return record IsNot Nothing AndAlso
               Me.ID = record.ID AndAlso
               Me.AutoLogin = record.AutoLogin AndAlso
               Me.CareLinkPassword = record.CareLinkPassword AndAlso
               Me.CareLinkUserName = record.CareLinkUserName AndAlso
               Me.CountryCode = record.CountryCode AndAlso
               Me.UseLocalTimeZone = record.UseLocalTimeZone AndAlso
               Me.CareLinkPartner = record.CareLinkPartner AndAlso
               Me.CareLinkPatientUserID = record.CareLinkPatientUserID
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New NotImplementedException()
    End Function

#End Region ' Implements IEditableObject

End Class
