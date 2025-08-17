' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module Form1CollectMarkersHelper

    ''' <summary>
    '''  Scales the "unitValue" in the marker's data, converting it to a string representation if necessary.
    ''' </summary>
    ''' <param name="item">The marker to scale.</param>
    ''' <returns>A new <cref name="Marker"/> with the scaled "unitValue".</returns>
    <Extension>
    Private Function ScaleMarker(item As Marker) As Marker
        Const key As String = "unitValue"
        Dim newMarker As Marker = item
        Dim value As Object = Nothing
        If item.Data.DataValues.TryGetValue(key, value) Then
            Select Case True
                Case TypeOf value Is JsonElement
                    item.Data.DataValues(key) = CType(value, JsonElement).ScaleSgToString
                Case TypeOf value Is String
                    item.Data.DataValues(key) = CType(value, String).ScaleSgToString
                Case Else
                    Stop
            End Select
        End If
        Return newMarker
    End Function

    ''' <summary>
    '''  Sorts and filters the list of low glucose suspended markers.
    '''  Ensures correct ordering and updates record numbers.
    ''' </summary>
    Private Sub SortAndFilterListOfLowGlucoseSuspendedMarkers()
        Dim comparison As Comparison(Of LowGlucoseSuspended) =
            Function(x As LowGlucoseSuspended, y As LowGlucoseSuspended) As Integer
                Return x.DisplayTime.CompareTo(value:=y.DisplayTime)
            End Function
        s_suspendedMarkers.Sort(comparison)

        Dim tmpList As New List(Of LowGlucoseSuspended)
        For Each r As IndexClass(Of LowGlucoseSuspended) In s_suspendedMarkers.WithIndex
            Dim item As LowGlucoseSuspended = r.Value
            item.RecordNumber = tmpList.Count + 1
            If r.IsFirst Then
                tmpList.Add(item)
                Continue For
            End If
            If tmpList.Last.deliverySuspended OrElse item.deliverySuspended Then
                tmpList.Add(item)
            End If
        Next
        s_suspendedMarkers = tmpList
    End Sub

    ''' <summary>
    '''  Collect up markers
    ''' </summary>
    ''' <param name="jsonRow">JSON Marker Row</param>
    ''' <returns>Max Basal/Hr</returns>
    Friend Function CollectMarkers() As String
        s_autoBasalDeliveryMarkers.Clear()
        s_autoModeStatusMarkers.Clear()
        s_basalPerHour.Clear()
        For index As Integer = 0 To 11
            s_basalPerHour.Add(item:=New BasalPerHour(hour:=index * 2))
        Next
        s_bgReadingMarkers.Clear()
        s_calibrationMarkers.Clear()
        s_insulinMarkers.Clear()
        s_suspendedMarkers.Clear()
        s_mealMarkers.Clear()
        s_timeChangeMarkers.Clear()
        s_markers.Clear()

        MaxBasalPerDose = 0

        Dim markers As List(Of Marker) = PatientData.Markers

        Dim basalDictionary As New SortedDictionary(Of OADate, Double)
        For Each e As IndexClass(Of Marker) In markers.WithIndex
            Dim item As Marker = e.Value
            Select Case item.Type
                Case "AUTO_BASAL_DELIVERY"
                    s_markers.Add(item)
                    Dim basalDeliveryMarker As New AutoBasalDelivery(
                        item,
                        recordNumber:=s_autoBasalDeliveryMarkers.Count + 1)
                    InsulinPerHour.AddBasalAmountToInsulinPerHour(basalDeliveryMarker)
                    s_autoBasalDeliveryMarkers.Add(item:=basalDeliveryMarker)
                    If Not basalDictionary.TryAdd(
                        key:=basalDeliveryMarker.OAdateTime,
                        value:=basalDeliveryMarker.BolusAmount) Then

                        basalDictionary(key:=basalDeliveryMarker.OAdateTime) +=
                            basalDeliveryMarker.BolusAmount
                    End If
                    s_suspendedMarkers.Add(item:=New LowGlucoseSuspended(
                       item,
                       recordNumber:=s_suspendedMarkers.Count + 1))
                Case "AUTO_MODE_STATUS"
                    s_autoModeStatusMarkers.Add(item:=New AutoModeStatus(
                        item,
                        recordNumber:=s_autoModeStatusMarkers.Count + 1))
                    s_suspendedMarkers.Add(item:=New LowGlucoseSuspended(
                        item,
                        recordNumber:=s_suspendedMarkers.Count + 1))
                Case "BG_READING"
                    s_markers.Add(item)
                    s_bgReadingMarkers.Add(item:=New BgReading(
                        item,
                        recordNumber:=s_bgReadingMarkers.Count + 1))
                Case "CALIBRATION"
                    s_markers.Add(item:=item.ScaleMarker)
                    s_calibrationMarkers.Add(item:=New Calibration(
                        item:=item.ScaleMarker(),
                        recordNumber:=s_calibrationMarkers.Count + 1))
                Case "INSULIN"
                    s_markers.Add(item)
                    Dim lastInsulinRecord As New Insulin(
                        item,
                        recordNumber:=s_insulinMarkers.Count + 1)
                    s_insulinMarkers.Add(item:=lastInsulinRecord)
                    s_suspendedMarkers.Add(item:=New LowGlucoseSuspended(
                        item,
                        recordNumber:=s_suspendedMarkers.Count + 1))
                    Select Case item.GetStringFromJson(key:=NameOf(Insulin.ActivationType))
                        Case "AUTOCORRECTION"
                            Dim key As OADate = lastInsulinRecord.OAdateTime
                            Dim value As Single = lastInsulinRecord.DeliveredFastAmount
                            If Not basalDictionary.TryAdd(key, value) Then
                                basalDictionary(key) += value
                            End If
                        Case "MANUAL"
                            Stop
                        Case "UNDETERMINED"
                            Stop
                        Case "RECOMMENDED"
                            ' handled elsewhere
                        Case Else
                            Stop
                            Throw UnreachableException(paramName:=item.Type)
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    If Not InAutoMode Then
                        s_suspendedMarkers.Add(item:=New LowGlucoseSuspended(
                            item,
                            recordNumber:=s_suspendedMarkers.Count + 1))
                    End If
                    s_markers.Add(item)
                Case "MEAL"
                    s_mealMarkers.Add(item:=New Meal(
                        item,
                        recordNumber:=s_mealMarkers.Count + 1))
                    s_markers.Add(item)
                Case "TIME_CHANGE"
                    s_markers.Add(item)
                    s_timeChangeMarkers.Add(item:=New TimeChange(
                        item,
                        recordNumber:=s_timeChangeMarkers.Count + 1))
                Case Else
                    Stop
                    Throw UnreachableException(paramName:=item.Type)
            End Select
        Next

        SortAndFilterListOfLowGlucoseSuspendedMarkers()
        Dim endOADate As OADate

        If basalDictionary.Count = 0 Then
            Dim asDate As Date = PatientData.LastConduitUpdateServerDateTime.Epoch2PumpDateTime
            endOADate = New OADate(asDate)
        Else
            endOADate = basalDictionary.Last.Key
        End If

        Dim i As Integer = 0
        Dim maxBasalPerHour As Double = 0

        If basalDictionary.Count > 2 Then
            While i < basalDictionary.Count AndAlso basalDictionary.Keys(index:=i) <= endOADate
                Dim sum As Double = 0
                Dim j As Integer = i
                Dim startOADate As OADate = basalDictionary.Keys(index:=i)
                While j < basalDictionary.Count AndAlso
                      basalDictionary.Keys(index:=j) <= startOADate + OneHourAsOADate

                    sum += basalDictionary.Values(index:=j)
                    j += 1
                End While
                maxBasalPerHour = Math.Max(maxBasalPerHour, sum)
                MaxBasalPerDose = Math.Max(MaxBasalPerDose, basalDictionary.Values(index:=i))
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
