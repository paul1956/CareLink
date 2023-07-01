' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BasalRecord
    Implements IEquatable(Of BasalRecord)

    Private _oaDateTime As OADate

    <DisplayName("Active Basal Pattern")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property activeBasalPattern As String

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    Public Property basalRate As Single

    <DisplayName("Preset Temp Name")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    Public Property presetTempName As String

    <DisplayName("Temp Basal Duration Remaining")>
    <Column(Order:=6, TypeName:=NameOf([Int32]))>
    Public Property tempBasalDurationRemaining As Integer

    <DisplayName("Temp Basal Percentage")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property tempBasalPercentage As Integer

    <DisplayName("Temp Basal Rate")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property tempBasalRate As Single

    <DisplayName("Temp Basal Type")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    Public Property tempBasalType As String

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

    Public Function GetBasal() As Single
        Return (Me.GetBasalPerHour / 12).RoundTo025
    End Function

    Public Function GetBasalPerHour() As Single
        Select Case Me.activeBasalPattern
            Case "BASAL1", "BASAL2"
                Return Me.basalRate
            Case Else
                Return If(Me.tempBasalPercentage > 0,
                          Math.Max(Me.basalRate, Me.tempBasalRate),
                          Math.Min(Me.basalRate, Me.tempBasalRate)
                         )
        End Select
    End Function

    Public Function GetOaGetTime() As OADate
        Return _oaDateTime
    End Function

    Public Sub OaDateTime(d As Date)
        _oaDateTime = New OADate(d)
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.Equals(TryCast(obj, BasalRecord))
    End Function

    Public Overloads Function Equals(other As BasalRecord) As Boolean Implements IEquatable(Of BasalRecord).Equals
        Return other IsNot Nothing AndAlso
               Me.activeBasalPattern = other.activeBasalPattern AndAlso
               Me.basalRate = other.basalRate AndAlso
               Me.presetTempName = other.presetTempName AndAlso
               Me.tempBasalDurationRemaining = other.tempBasalDurationRemaining AndAlso
               Me.tempBasalPercentage = other.tempBasalPercentage AndAlso
               Me.tempBasalRate = other.tempBasalRate AndAlso
               Me.tempBasalType = other.tempBasalType
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(Me.activeBasalPattern, Me.basalRate, Me.presetTempName, Me.tempBasalDurationRemaining, Me.tempBasalPercentage, Me.tempBasalRate, Me.tempBasalType)
    End Function

    Public Shared Operator =(left As BasalRecord, right As BasalRecord) As Boolean
        Return EqualityComparer(Of BasalRecord).Default.Equals(left, right)
    End Operator

    Public Shared Operator <>(left As BasalRecord, right As BasalRecord) As Boolean
        Return Not left = right
    End Operator

End Class
