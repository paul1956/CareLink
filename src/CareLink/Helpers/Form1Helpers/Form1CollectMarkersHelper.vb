' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module Form1CollectMarkersHelper

    <Extension>
    Private Function ScaleMarker(marker As Marker) As Marker
        Dim newMarker As Marker = marker
#If False Then ' TODO
        For Each kvp As KeyValuePair(Of String, String) In innerDictionary
            Select Case kvp.Key
                Case "value"
                    newMarker.Add(kvp.Key, kvp.ScaleSgToString())
                Case Else
                    newMarker.Add(kvp.Key, kvp.Value)
            End Select
        Next
#End If
        Return newMarker
    End Function

    ''' <summary>
    ''' Collect up markers
    ''' </summary>
    ''' <param name="jsonRow">JSON Marker Row</param>
    ''' <returns>Max Basal/Hr</returns>
    Friend Function CollectMarkers() As String
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfAutoModeStatusMarkers.Clear()
        s_listOfSgReadingMarkers.Clear()
        s_listOfCalibrationMarkers.Clear()
        s_listOfInsulinMarkers.Clear()
        s_listOfLowGlucoseSuspendedMarkers.Clear()
        s_listOfMealMarkers.Clear()
        s_listOfTimeChangeMarkers.Clear()
        s_markers.Clear()

        Dim basalDictionary As New SortedDictionary(Of OADate, Single)
        MaxBasalPerDose = 0

        Dim markers As List(Of Marker) = PatientData.Markers
        For Each e As IndexClass(Of Marker) In markers.WithIndex
            Dim markerEntry As Marker = e.Value
            Select Case markerEntry.Type
                Case "AUTO_BASAL_DELIVERY"
                    s_markers.Add(markerEntry)
                    Dim item As New AutoBasalDelivery(markerEntry, s_listOfAutoBasalDeliveryMarkers.Count + 1)
                    s_listOfAutoBasalDeliveryMarkers.Add(item)
                    If Not basalDictionary.TryAdd(item.OAdateTime, item.bolusAmount) Then
                        basalDictionary(item.OAdateTime) += item.bolusAmount
                    End If
                Case "AUTO_MODE_STATUS"
                    s_listOfAutoModeStatusMarkers.Add(New AutoModeStatus(markerEntry, s_listOfAutoModeStatusMarkers.Count + 1))
                Case "BG_READING"
                    s_markers.Add(markerEntry.ScaleMarker)
                    s_listOfSgReadingMarkers.Add(New SgReadingRecord(markerEntry.ScaleMarker(), s_listOfSgReadingMarkers.Count + 1))
                Case "CALIBRATION"
                    s_markers.Add(markerEntry.ScaleMarker)
                    s_listOfCalibrationMarkers.Add(New CalibrationRecord(markerEntry.ScaleMarker(), s_listOfCalibrationMarkers.Count + 1))
                Case "INSULIN"
                    s_markers.Add(markerEntry)
                    Dim lastInsulinRecord As New InsulinRecord(markerEntry, s_listOfInsulinMarkers.Count + 1)
                    s_listOfInsulinMarkers.Add(lastInsulinRecord)
                    Select Case markerEntry.Data.DataValues(NameOf(InsulinRecord.activationType)).ToString
                        Case "AUTOCORRECTION"
                            If Not basalDictionary.TryAdd(lastInsulinRecord.OAdateTime, lastInsulinRecord.deliveredFastAmount) Then
                                basalDictionary(lastInsulinRecord.OAdateTime) += lastInsulinRecord.deliveredFastAmount
                            End If
                        Case "MANUAL"
                        Case "UNDETERMINED"
                        Case "RECOMMENDED"
                        Case Else
                            Stop
                            Throw UnreachableException(markerEntry.Type)
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    s_listOfLowGlucoseSuspendedMarkers.Add(New LowGlucoseSuspendRecord(markerEntry, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                    s_markers.Add(markerEntry)
                Case "MEAL"
                    s_listOfMealMarkers.Add(New MealRecord(markerEntry, s_listOfMealMarkers.Count + 1))
                    s_markers.Add(markerEntry)
                Case "TIME_CHANGE"
                    s_markers.Add(markerEntry)
                    s_listOfTimeChangeMarkers.Add(New TimeChangeRecord(markerEntry))
                Case Else
                    Stop
                    Throw UnreachableException(markerEntry.Type)
            End Select
        Next

        For Each e As IndexClass(Of Basal) In s_listOfManualBasal.ToList.WithIndex
            Dim r As Basal = e.Value
            If Single.IsNaN(r.GetBasal) Then
                Continue For
            End If
            basalDictionary.Add(r.GetOaGetTime, r.GetBasal)
            s_listOfAutoBasalDeliveryMarkers.Add(New AutoBasalDelivery(r, basalDictionary.Count, 288 - e.Index))
            Dim basalMarker As New Marker
            s_markers.Add(Marker.Convert(r))
        Next

        Dim endOADate As OADate = If(basalDictionary.Count = 0,
                                     New OADate(s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime),
                                     basalDictionary.Last.Key
                                    )

        Dim i As Integer = 0
        Dim maxBasalPerHour As Single = 0

        If basalDictionary.Count > 2 Then
            While i < basalDictionary.Count ' AndAlso basalDictionary.Keys(i) <= endOADate
                Dim sum As Single = 0
                Dim j As Integer = i
                Dim startOADate As OADate = basalDictionary.Keys(i)
                While j < basalDictionary.Count AndAlso basalDictionary.Keys(j) <= startOADate + s_1HourAsOADate
                    sum += basalDictionary.Values(j)
                    j += 1
                End While
                maxBasalPerHour = Math.Max(maxBasalPerHour, sum)
                MaxBasalPerDose = Math.Max(MaxBasalPerDose, basalDictionary.Values(i))
                MaxBasalPerDose = Math.Min(MaxBasalPerDose, 25)
                i += 1
            End While
        Else
            If CurrentPdf?.IsValid Then
                Dim basalRateRecords As List(Of BasalRateRecord) = GetActiveBasalRateRecords()
                For Each basalRate As BasalRateRecord In basalRateRecords
                    maxBasalPerHour = Math.Max(maxBasalPerHour, basalRate.UnitsPerHr)
                    MaxBasalPerDose = Math.Max(MaxBasalPerDose, basalRate.UnitsPerHr / 12)
                    MaxBasalPerDose = Math.Min(MaxBasalPerDose, 10.0!)
                    MaxBasalPerDose = Math.Max(MaxBasalPerDose, 0.25!)
                Next
            End If
            If maxBasalPerHour = 0 Then
                MaxBasalPerDose = 1
                maxBasalPerHour = 10
            End If
        End If
        Return $"Max Basal/Hr ~{maxBasalPerHour.RoundTo025}U"
    End Function

End Module
