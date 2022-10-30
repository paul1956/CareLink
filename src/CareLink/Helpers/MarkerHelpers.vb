' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module MarkerHelpers

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
        For Each newMarker As Dictionary(Of String, String) In LoadList(jsonRow)
            Select Case newMarker("type")
                Case "AUTO_BASAL_DELIVERY"
                    s_markers.Add(newMarker)
                    Dim item As AutoBasalDeliveryRecord = DictionaryToClass(Of AutoBasalDeliveryRecord)(newMarker, s_listOfAutoBasalDeliveryMarkers.Count + 1)
                    s_listOfAutoBasalDeliveryMarkers.Add(item)
                    basalDictionary.Add(item.OAdateTime, item.bolusAmount)
                Case "AUTO_MODE_STATUS"
                    s_listOfAutoModeStatusMarkers.Add(DictionaryToClass(Of AutoModeStatusRecord)(newMarker, s_listOfAutoModeStatusMarkers.Count + 1))
                Case "BG_READING"
                    s_listOfBgReadingMarkers.Add(DictionaryToClass(Of BGReadingRecord)(newMarker.ScaleMarker(), s_listOfBgReadingMarkers.Count + 1))
                Case "CALIBRATION"
                    s_markers.Add(newMarker.ScaleMarker)
                    s_listOfCalibrationMarkers.Add(DictionaryToClass(Of CalibrationRecord)(newMarker.ScaleMarker(), s_listOfCalibrationMarkers.Count + 1))
                Case "INSULIN"
                    s_markers.Add(newMarker)
                    Dim lastInsulinRecord As InsulinRecord = DictionaryToClass(Of InsulinRecord)(newMarker, s_listOfInsulinMarkers.Count + 1)
                    s_listOfInsulinMarkers.Add(lastInsulinRecord)
                    Select Case newMarker(NameOf(InsulinRecord.activationType))
                        Case "AUTOCORRECTION"
                            basalDictionary.Add(lastInsulinRecord.OAdateTime, lastInsulinRecord.deliveredFastAmount)
                        Case "UNDETERMINED",
                             "RECOMMENDED"
                            '
                        Case Else
                            Stop
                            Throw UnreachableException(NameOf(CollectMarkers))
                    End Select
                Case "LOW_GLUCOSE_SUSPENDED"
                    s_listOfLowGlucoseSuspendedMarkers.Add(DictionaryToClass(Of LowGlusoceSuspendRecord)(newMarker, s_listOfLowGlucoseSuspendedMarkers.Count + 1))
                Case "MEAL"
                    s_listOfMealMarkers.Add(DictionaryToClass(Of MealRecord)(newMarker, s_listOfMealMarkers.Count + 1))
                    s_markers.Add(newMarker)
                Case "TIME_CHANGE"
                    s_markers.Add(newMarker)
                    s_listOfTimeChangeMarkers.Add(New TimeChangeRecord(newMarker))
                Case Else
                    Stop
                    Throw UnreachableException(NameOf(CollectMarkers))
            End Select
        Next
        Dim endOADate As OADate = If(basalDictionary.Count = 0, New OADate(Now), basalDictionary.Last.Key)
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
            i += 1
        End While
        Return $"Max Basal/Hr ~ {MaxBasalPerHour.RoundSingle(3)} U"
    End Function

End Module
