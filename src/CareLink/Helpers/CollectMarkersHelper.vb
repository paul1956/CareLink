' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module CollectMarkersHelper

    <Extension>
    Private Function ScaleMarker(innerDictionary As Dictionary(Of String, String)) As Dictionary(Of String, String)
        Dim newMarker As New Dictionary(Of String, String)
        For Each kvp As KeyValuePair(Of String, String) In innerDictionary
            Select Case kvp.Key
                Case "value"
                    newMarker.Add(kvp.Key, kvp.ScaleSgToString())
                Case Else
                    newMarker.Add(kvp.Key, kvp.Value)
            End Select
        Next
        Return newMarker
    End Function

    ''' <summary>
    ''' Collect up markers
    ''' </summary>
    ''' <param name="jsonRow">JSON Marker Row</param>
    ''' <returns>Max Basal/Hr</returns>
    Friend Function CollectMarkers(jsonRow As String) As String
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfAutoModeStatusMarkers.Clear()
        s_listOfBgReadingMarkers.Clear()
        s_listOfCalibrationMarkers.Clear()
        s_listOfInsulinMarkers.Clear()
        s_listOfLowGlucoseSuspendedMarkers.Clear()
        s_listOfMealMarkers.Clear()
        s_listOfTimeChangeMarkers.Clear()
        s_markers.Clear()

        Dim basalDictionary As New Dictionary(Of OADate, Single)
        MaxBasalPerHour = 0
        MaxBasalPerDose = 0

        Dim markers As List(Of Dictionary(Of String, String)) = LoadList(jsonRow)
        For Each markerEntry As Dictionary(Of String, String) In markers
            Select Case markerEntry("type")
                Case "AUTO_BASAL_DELIVERY"
                    s_markers.Add(markerEntry)
                    Dim item As AutoBasalDeliveryRecord = DictionaryToClass(Of AutoBasalDeliveryRecord)(markerEntry, s_listOfAutoBasalDeliveryMarkers.Count + 1)
                    s_listOfAutoBasalDeliveryMarkers.Add(item)
                    basalDictionary.Add(item.OAdateTime, item.bolusAmount)
                Case "AUTO_MODE_STATUS"
                    s_listOfAutoModeStatusMarkers.Add(DictionaryToClass(Of AutoModeStatusRecord)(markerEntry, s_listOfAutoModeStatusMarkers.Count + 1))
                Case "BG_READING"
                    s_markers.Add(markerEntry.ScaleMarker)
                    s_listOfBgReadingMarkers.Add(DictionaryToClass(Of BGReadingRecord)(markerEntry.ScaleMarker(), s_listOfBgReadingMarkers.Count + 1))
                Case "CALIBRATION"
                    s_markers.Add(markerEntry.ScaleMarker)
                    s_listOfCalibrationMarkers.Add(DictionaryToClass(Of CalibrationRecord)(markerEntry.ScaleMarker(), s_listOfCalibrationMarkers.Count + 1))
                Case "INSULIN"
                    s_markers.Add(markerEntry)
                    Dim lastInsulinRecord As InsulinRecord = DictionaryToClass(Of InsulinRecord)(markerEntry, s_listOfInsulinMarkers.Count + 1)
                    s_listOfInsulinMarkers.Add(lastInsulinRecord)
                    Select Case markerEntry(NameOf(InsulinRecord.activationType))
                        Case "AUTOCORRECTION"
                            basalDictionary.Add(lastInsulinRecord.OAdateTime, lastInsulinRecord.deliveredFastAmount)
                        Case "MANUAL"
                            Stop
                        Case "UNDETERMINED"
                        Case "RECOMMENDED"
                        Case Else
                            Stop
                            Throw UnreachableException(NameOf(CollectMarkers))
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    s_listOfLowGlucoseSuspendedMarkers.Add(DictionaryToClass(Of LowGlucoseSuspendRecord)(markerEntry, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                Case "MEAL"
                    s_listOfMealMarkers.Add(DictionaryToClass(Of MealRecord)(markerEntry, s_listOfMealMarkers.Count + 1))
                    s_markers.Add(markerEntry)
                Case "TIME_CHANGE"
                    s_markers.Add(markerEntry)
                    s_listOfTimeChangeMarkers.Add(New TimeChangeRecord(markerEntry))
                Case Else
                    Stop
                    Throw UnreachableException(NameOf(CollectMarkers))
            End Select
        Next

        For Each e As IndexClass(Of BasalRecord) In s_listOfManualBasal.ToList.WithIndex
            Dim r As BasalRecord = e.Value
            basalDictionary.Add(r.GetOaGetTime, r.GetBasal)
            If r.basalRate > 0 Then
                s_listOfAutoBasalDeliveryMarkers.Add(New AutoBasalDeliveryRecord(r, basalDictionary.Count, 288 - e.Index))
                s_markers.Add(r.ToDictionary)
            End If
        Next

        Dim endOADate As OADate
        If basalDictionary.Count = 0 Then
            endOADate = New OADate(s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2DateTime)
        Else
            endOADate = basalDictionary.Last.Key
        End If

        Dim i As Integer = 0
        While i < basalDictionary.Count AndAlso basalDictionary.Keys(i) <= endOADate
            Dim sum As Single = 0
            Dim j As Integer = i
            Dim startOADate As OADate = basalDictionary.Keys(j)
            While j < basalDictionary.Count AndAlso basalDictionary.Keys(j) <= startOADate + s_hourAsOADate
                sum += basalDictionary.Values(j)
                j += 1
            End While
            MaxBasalPerHour = Math.Max(MaxBasalPerHour, sum)
            MaxBasalPerDose = Math.Max(MaxBasalPerDose, basalDictionary.Values(i))
            MaxBasalPerDose = Math.Min(MaxBasalPerDose, 25)
            i += 1
        End While
        Return $"Max Basal/Hr ~ {MaxBasalPerHour.RoundTo025} U"
    End Function

End Module
