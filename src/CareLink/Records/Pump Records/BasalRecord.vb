' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BasalRecord

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
                If Me.tempBasalPercentage > 0 Then
                    Return Math.Max(Me.basalRate, Me.tempBasalRate)
                Else
                    Return Math.Min(Me.basalRate, Me.tempBasalRate)
                End If
        End Select
    End Function

    Public Function GetOaGetTime() As OADate
        Return _oaDateTime
    End Function

    Public Sub OaDateTime(d As Date)
        _oaDateTime = New OADate(d)
    End Sub

End Class
