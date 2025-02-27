' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class Basal
    Implements IEquatable(Of Basal)

    Private _oaDateTime As OADate

    <DisplayName("Active Basal Pattern")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    <JsonPropertyName("activeBasalPattern")>
    Public Property ActiveBasalPattern As String

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    <JsonPropertyName("basalRate")>
    Public Property BasalRate As Single

    <DisplayName("Temp Basal Rate")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    <JsonPropertyName("tempBasalRate")>
    Public Property TempBasalRate As Single

    <DisplayName("Temp Basal Percentage")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("tempBasalPercentage")>
    Public Property tempBasalPercentage As Integer

    <DisplayName("Temp Basal Type")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    <JsonPropertyName("tempBasalType")>
    Public Property tempBasalType As String

    <DisplayName("Preset Temp Name")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    <JsonPropertyName("presetTempName")>
    Public Property presetTempName As String

    <DisplayName("Temp Basal Duration Remaining")>
    <Column(Order:=6, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("tempBasalDurationRemaining")>
    Public Property tempBasalDurationRemaining As Integer

    Friend Function ToDictionary() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"kind", "Marker"},
            {"type", "MANUAL_BASAL_DELIVERY"},
            {"activationType", "MANUAL"},
            {"bolusAmount", Me.GetBasal.ToString},
            {"dateTime", Date.FromOADate(Me.GetOaGetTime).ToString(CurrentDateCulture)},
            {"oaDateTime", _oaDateTime.ToString}
        }
    End Function

    Public Shared Operator <>(left As Basal, right As Basal) As Boolean
        Return Not left = right
    End Operator

    Public Shared Operator =(left As Basal, right As Basal) As Boolean
        Return EqualityComparer(Of Basal).Default.Equals(left, right)
    End Operator

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, Basal))
    End Function

    Public Function GetBasal() As Single
        Return Me.GetBasalPerHour / 12
    End Function

    Public Function GetBasalPerHour() As Single
        If Me.ActiveBasalPattern Is Nothing Then
            Return Single.NaN
        End If
        Select Case Me.ActiveBasalPattern
            Case "BASAL1", "BASAL2", "BASAL3", "BASAL4", "BASAL5", "WORKDAY", "DAYOFF", "SICKDAY"
                Return Me.BasalRate
            Case Else
                Return If(Me.tempBasalPercentage > 0,
                          Math.Max(Me.BasalRate, Me.TempBasalRate),
                          Math.Min(Me.BasalRate, Me.TempBasalRate)
                         )
        End Select
    End Function

    Public Function GetBasalType() As String
        Select Case True
            Case Not String.IsNullOrWhiteSpace(Me.ActiveBasalPattern)
                Return Me.ActiveBasalPattern
            Case Not String.IsNullOrWhiteSpace(Me.presetTempName)
                Return Me.presetTempName
            Case Not String.IsNullOrWhiteSpace(Me.presetTempName)
                Return Me.tempBasalType
            Case Else
                Return "MANUAL_BASAL_DELIVERY"
        End Select
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.ActiveBasalPattern, Me.BasalRate, Me.presetTempName, Me.tempBasalDurationRemaining, Me.tempBasalPercentage, Me.TempBasalRate, Me.tempBasalType)
    End Function

    Public Function GetOaGetTime() As OADate
        Return _oaDateTime
    End Function

    Public Sub OaDateTime(d As Date)
        _oaDateTime = New OADate(d)
    End Sub

    Public Overloads Function Equals(other As Basal) As Boolean Implements IEquatable(Of Basal).Equals
        Return other IsNot Nothing AndAlso
               Me.ActiveBasalPattern = other.ActiveBasalPattern AndAlso
               Me.BasalRate = other.BasalRate AndAlso
               Me.presetTempName = other.presetTempName AndAlso
               Me.tempBasalDurationRemaining = other.tempBasalDurationRemaining AndAlso
               Me.tempBasalPercentage = other.tempBasalPercentage AndAlso
               Me.TempBasalRate = other.TempBasalRate AndAlso
               Me.tempBasalType = other.tempBasalType
    End Function

End Class
