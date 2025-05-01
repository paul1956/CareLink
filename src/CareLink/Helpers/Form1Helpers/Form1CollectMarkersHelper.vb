' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module Form1CollectMarkersHelper

    <Extension>
    Private Function ScaleMarker(marker As Marker) As Marker
        Dim newMarker As Marker = marker
        Dim value As Object = Nothing
        If marker.Data.DataValues.TryGetValue("unitValue", value) Then
            Select Case True
                Case TypeOf value Is JsonElement
                    marker.Data.DataValues("unitValue") = CType(value, JsonElement).ScaleSgToString
                Case TypeOf value Is String
                    marker.Data.DataValues("unitValue") = CType(value, String).ScaleSgToString
                Case Else
                    Stop
            End Select
        End If
        Return newMarker
    End Function

    Private Sub SortAndFilterListOfLowGlucoseSuspendedMarkers()
        s_listOfLowGlucoseSuspendedMarkers.Sort(Function(x, y) x.DisplayTime.CompareTo(y.DisplayTime))
        Dim tmpList As New List(Of LowGlucoseSuspended)
        For Each r As IndexClass(Of LowGlucoseSuspended) In s_listOfLowGlucoseSuspendedMarkers.WithIndex
            Dim entry As LowGlucoseSuspended = r.Value
            entry.RecordNumber = tmpList.Count + 1
            If r.IsFirst Then
                tmpList.Add(entry)
                Continue For
            End If
            If tmpList.Last.deliverySuspended OrElse entry.deliverySuspended Then
                tmpList.Add(entry)
            End If
        Next
        s_listOfLowGlucoseSuspendedMarkers = tmpList
    End Sub

    ''' <summary>
    ''' Collect up markers
    ''' </summary>
    ''' <param name="jsonRow">JSON Marker Row</param>
    ''' <returns>Max Basal/Hr</returns>
    Friend Function CollectMarkers() As String
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfAutoModeStatusMarkers.Clear()
        s_listOfBasalPerHour.Clear()
        For index As Integer = 0 To 11
            s_listOfBasalPerHour.Add(New BasalPerHour(index * 2))
        Next
        s_listOfBgReadingMarkers.Clear()
        s_listOfCalibrationMarkers.Clear()
        s_listOfInsulinMarkers.Clear()
        s_listOfLimitRecords.Clear()
        s_listOfLowGlucoseSuspendedMarkers.Clear()
        s_listOfMealMarkers.Clear()
        s_listOfTimeChangeMarkers.Clear()
        s_markers.Clear()

        MaxBasalPerDose = 0

        Dim markers As List(Of Marker) = PatientData.Markers

        Dim basalDictionary As New SortedDictionary(Of OADate, Double)
        For Each e As IndexClass(Of Marker) In markers.WithIndex
            Dim markerEntry As Marker = e.Value
            Select Case markerEntry.Type
                Case "AUTO_BASAL_DELIVERY"
                    s_markers.Add(markerEntry)
                    Dim item As New AutoBasalDelivery(markerEntry, recordNumber:=s_listOfAutoBasalDeliveryMarkers.Count + 1)
                    s_listOfAutoBasalDeliveryMarkers.Add(item)
                    Dim index As Integer = item.DisplayTime.Hour
                    If (index And 1) = 0 Then
                        s_listOfBasalPerHour(index \ 2).BasalRate += item.bolusAmount
                    Else
                        s_listOfBasalPerHour(index \ 2).BasalRate2 += item.bolusAmount
                    End If
                    If Not basalDictionary.TryAdd(item.OAdateTime, item.bolusAmount) Then
                        basalDictionary(item.OAdateTime) += item.bolusAmount
                    End If
                    s_listOfLowGlucoseSuspendedMarkers.Add(New LowGlucoseSuspended(markerEntry, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                Case "AUTO_MODE_STATUS"
                    s_listOfAutoModeStatusMarkers.Add(New AutoModeStatus(markerEntry, s_listOfAutoModeStatusMarkers.Count + 1))
                    s_listOfLowGlucoseSuspendedMarkers.Add(New LowGlucoseSuspended(markerEntry, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                Case "BG_READING"
                    s_markers.Add(markerEntry)
                    s_listOfBgReadingMarkers.Add(New BgReading(markerEntry, s_listOfBgReadingMarkers.Count + 1))
                Case "CALIBRATION"
                    s_markers.Add(markerEntry.ScaleMarker)
                    s_listOfCalibrationMarkers.Add(New Calibration(markerEntry.ScaleMarker(), s_listOfCalibrationMarkers.Count + 1))
                Case "INSULIN"
                    s_markers.Add(markerEntry)
                    Dim lastInsulinRecord As New Insulin(markerEntry, s_listOfInsulinMarkers.Count + 1)
                    s_listOfInsulinMarkers.Add(lastInsulinRecord)
                    s_listOfLowGlucoseSuspendedMarkers.Add(New LowGlucoseSuspended(markerEntry, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                    Select Case markerEntry.GetStringValueFromJson(NameOf(Insulin.ActivationType))
                        Case "AUTOCORRECTION"
                            If Not basalDictionary.TryAdd(lastInsulinRecord.OAdateTime, lastInsulinRecord.DeliveredFastAmount) Then
                                basalDictionary(lastInsulinRecord.OAdateTime) += lastInsulinRecord.DeliveredFastAmount
                            End If
                        Case "MANUAL"
                            Stop
                        Case "UNDETERMINED"
                            Stop
                        Case "RECOMMENDED"
                            ' handled elsewhere
                        Case Else
                            Stop
                            Throw UnreachableException(markerEntry.Type)
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    If Not InAutoMode Then
                        s_listOfLowGlucoseSuspendedMarkers.Add(New LowGlucoseSuspended(markerEntry, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                    End If
                    s_markers.Add(markerEntry)
                Case "MEAL"
                    s_listOfMealMarkers.Add(New Meal(markerEntry, s_listOfMealMarkers.Count + 1))
                    s_markers.Add(markerEntry)
                Case "TIME_CHANGE"
                    s_markers.Add(markerEntry)
                    s_listOfTimeChangeMarkers.Add(New TimeChange(markerEntry, s_listOfTimeChangeMarkers.Count + 1))
                Case Else
                    Stop
                    Throw UnreachableException(markerEntry.Type)
            End Select
        Next

        SortAndFilterListOfLowGlucoseSuspendedMarkers()
        Dim endOADate As OADate = If(basalDictionary.Count = 0,
                                     New OADate(PatientData.LastConduitUpdateServerDateTime.Epoch2PumpDateTime),
                                     basalDictionary.Last.Key
                                    )

        Dim i As Integer = 0
        Dim maxBasalPerHour As Double = 0

        If basalDictionary.Count > 2 Then
            While i < basalDictionary.Count AndAlso basalDictionary.Keys(i) <= endOADate
                Dim sum As Double = 0
                Dim j As Integer = i
                Dim startOADate As OADate = basalDictionary.Keys(i)
                While j < basalDictionary.Count AndAlso basalDictionary.Keys(j) <= startOADate + OneHourAsOADate
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
