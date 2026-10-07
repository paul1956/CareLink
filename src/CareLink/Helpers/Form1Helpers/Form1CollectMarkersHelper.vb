' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module Form1CollectMarkersHelper

    ''' <summary>
    '''  Scales the "unitValue" in the marker's data, converting it to a string
    '''  representation if necessary.
    ''' </summary>
    ''' <param name="item">The marker to scale.</param>
    ''' <returns>A new <cref name="Marker"/> with the scaled "unitValue".</returns>
    <Extension>
    Private Function ScaleMarker(item As Marker) As Marker
        Dim newMarker As Marker = item
        item.Data.DataValues.UnitValue =
            item.Data.DataValues.UnitValue.ScaleSg
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
        SuspendedMarkers.Sort(comparison)

        Dim tmpList As New List(Of LowGlucoseSuspended)(collection:=SuspendedMarkers)
        SuspendedMarkers.Clear()

        For Each r As IndexClass(Of LowGlucoseSuspended) In tmpList.WithIndex
            Dim item As LowGlucoseSuspended = r.Value
            item.RecordNumber = SuspendedMarkers.Count + 1
            If r.IsFirst Then
                SuspendedMarkers.Add(item)
                Continue For
            End If
            If SuspendedMarkers.Last.DeliverySuspended OrElse item.DeliverySuspended Then
                SuspendedMarkers.Add(item)
            End If
        Next
    End Sub

    ''' <summary>
    '''  Collect up markers
    ''' </summary>
    ''' <param name="jsonRow">JSON Marker Row</param>
    ''' <returns>Max Basal/Hr</returns>
    Friend Function CollectMarkers() As String
        AutoBasalDeliveryMarkers.Clear()
        AutoModeStatusMarkers.Clear()
        BasalPerHourList.Clear()
        For index As Integer = 0 To 11
            BasalPerHourList.Add(item:=New BasalPerHour(hour:=index * 2))
        Next
        BgReadingMarkers.Clear()
        CalibrationMarkers.Clear()
        InsulinMarkers.Clear()
        SuspendedMarkers.Clear()
        MealMarkers.Clear()
        TimeChangeMarkers.Clear()
        AllMarkers.Clear()

        MaxBasalPerDose = 0

        Dim markers As List(Of Marker) = PatientData.Markers
        Dim basalDic As New SortedDictionary(Of OADate, Double)
        For Each e As IndexClass(Of Marker) In markers.WithIndex
            Dim item As Marker = e.Value
            Select Case item.Type
                Case "AUTO_BASAL_DELIVERY"
                    AllMarkers.Add(item)
                    Dim basalDelMarker As New AutoBasalDelivery(
                        item,
                        recordNumber:=AutoBasalDeliveryMarkers.Count + 1)
                    InsulinPerHour.AddBasalAmountToInsulinPerHour(basalDelMarker)
                    AutoBasalDeliveryMarkers.Add(item:=basalDelMarker)
                    If Not basalDic.TryAdd(key:=basalDelMarker.OAdateTime, value:=basalDelMarker.BolusAmount) Then
                        basalDic(key:=basalDelMarker.OAdateTime) += basalDelMarker.BolusAmount
                    End If
                    SuspendedMarkers.Add(item:=New LowGlucoseSuspended(
                       item,
                       recordNumber:=SuspendedMarkers.Count + 1))
                Case "AUTO_MODE_STATUS"
                    Dim item1 As New AutoModeStatus(item, recordNumber:=AutoModeStatusMarkers.Count + 1)
                    AutoModeStatusMarkers.Add(item:=item1)
                    Dim item2 As New LowGlucoseSuspended(item, recordNumber:=SuspendedMarkers.Count + 1)
                    SuspendedMarkers.Add(item:=item2)
                Case "BG_READING"
                    AllMarkers.Add(item)
                    Dim item3 As New BgReading(item, recordNumber:=BgReadingMarkers.Count + 1)
                    BgReadingMarkers.Add(item:=item3)
                Case "CALIBRATION"
                    AllMarkers.Add(item:=item.ScaleMarker)
                    Dim item4 As New Calibration(item:=item.ScaleMarker(),
                                                 recordNumber:=CalibrationMarkers.Count + 1)
                    CalibrationMarkers.Add(item:=item4)
                Case "INSULIN"
                    AllMarkers.Add(item)
                    Dim lastInsulinRecord As New Insulin(item, recordNumber:=InsulinMarkers.Count + 1)
                    InsulinMarkers.Add(item:=lastInsulinRecord)
                    Dim item5 As New LowGlucoseSuspended(item, recordNumber:=SuspendedMarkers.Count + 1)
                    SuspendedMarkers.Add(item:=item5)
                    Select Case item.Data.DataValues.ActivationType
                        Case "AUTOCORRECTION", "MANUAL"
                            Dim key As OADate = lastInsulinRecord.OAdateTime
                            Dim value As Single = lastInsulinRecord.DeliveredFastAmount
                            If Not basalDic.TryAdd(key, value) Then
                                basalDic(key) += value
                            End If
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
                        SuspendedMarkers.Add(item:=New LowGlucoseSuspended(
                            item,
                            recordNumber:=SuspendedMarkers.Count + 1))
                    End If
                    AllMarkers.Add(item)
                Case "MEAL"
                    MealMarkers.Add(item:=New Meal(
                        item,
                        recordNumber:=MealMarkers.Count + 1))
                    AllMarkers.Add(item)
                Case "TIME_CHANGE"
                    AllMarkers.Add(item)
                    TimeChangeMarkers.Add(item:=New TimeChange(
                        item,
                        recordNumber:=TimeChangeMarkers.Count + 1))
                Case "OTHER"
                    AllMarkers.Add(item)
                Case Else
                    Stop
                    Throw UnreachableException(paramName:=item.Type)
            End Select
        Next

        SortAndFilterListOfLowGlucoseSuspendedMarkers()
        Dim endOADate As OADate

        If basalDic.Count = 0 Then
            Dim asDate As Date = PatientData.LastConduitUpdateServerDateTime.Epoch2PumpDateTime
            endOADate = New OADate(asDate)
        Else
            endOADate = basalDic.Last.Key
        End If

        Dim i As Integer = 0
        Dim maxBasalPerHour As Double = 0

        If basalDic.Count > 2 Then
            While i < basalDic.Count AndAlso
                  basalDic.Keys(index:=i) <= endOADate

                Dim sum As Double = 0
                Dim j As Integer = i
                Dim startOADate As OADate = basalDic.Keys(index:=i)
                While j < basalDic.Count AndAlso
                      basalDic.Keys(index:=j) <= startOADate + OneHourAsOADate

                    sum += basalDic.Values(index:=j)
                    j += 1
                End While
                maxBasalPerHour = Math.Max(maxBasalPerHour, sum)
                MaxBasalPerDose = Math.Max(MaxBasalPerDose, basalDic.Values(index:=i))
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
        Return $"Max Basal/Hr ~{maxBasalPerHour.RoundToStep(IsFlex())}U"
    End Function

End Module
