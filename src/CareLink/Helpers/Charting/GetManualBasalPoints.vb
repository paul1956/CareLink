' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module GetManualBasalPoints

    Friend Function GetManualBasalValues(markerWithIndex As IndexClass(Of Dictionary(Of String, String))) As SortedDictionary(Of OADate, Single)
        Debug.Assert(CurrentPdf.IsValid)
        Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)
        Dim markerEntry As Dictionary(Of String, String) = markerWithIndex.Value
        If markerEntry.GetBooleanValue(NameOf(LowGlucoseSuspendRecord.deliverySuspended)) Then
            Return timeOrderedMarkers
        End If
        Dim nextPumpSuspendTime As OADate
        If s_markers.Count > 1 AndAlso markerWithIndex.Index = s_markers.Count - 2 Then
            Dim activationType As String = ""
            nextPumpSuspendTime = If(s_markers.Last().TryGetValue("activationType", activationType) AndAlso activationType = "MANUAL",
                                     New OADate(s_markers.Last().GetMarkerDateTime),
                                     New OADate(PumpNow)
                                    )
        Else
            nextPumpSuspendTime = New OADate(s_markers(markerWithIndex.Index + 1).GetMarkerDateTime)
        End If

        Dim lowGlucoseSuspend As LowGlucoseSuspendRecord = DictionaryToClass(Of LowGlucoseSuspendRecord)(markerEntry, 0)
        If lowGlucoseSuspend.deliverySuspended Then
            Return timeOrderedMarkers
        End If

        Dim basalRateRecords As List(Of BasalRateRecord) = GetActiveBasalRateRecords()
        If basalRateRecords.Count = 0 Then
            Return timeOrderedMarkers
        End If

        Dim currentMarkerTime As New OADate(markerEntry.GetMarkerDateTime)

        While nextPumpSuspendTime > currentMarkerTime
            For Each e As IndexClass(Of BasalRateRecord) In basalRateRecords.WithIndex
                Dim basalRecord As BasalRateRecord = e.Value
                Dim startTime As TimeOnly = basalRecord.Time
                Dim endTime As TimeOnly = If(e.IsLast,
                                             s_eleven59.AddMinutes(1),
                                             basalRateRecords(e.Index + 1).Time
                                            )
                If TimeOnly.FromDateTime(Date.FromOADate(currentMarkerTime)).IsBetween(startTime, endTime) Then
                    Dim rate As Single = basalRecord.UnitsPerHr / 12
                    If rate < 0.025 Then
                        Dim increments As Integer = CInt(Math.Ceiling((basalRecord.UnitsPerHr / 0.025).RoundTo025))
                        If timeOrderedMarkers.ContainsKey(currentMarkerTime) Then
                            timeOrderedMarkers(currentMarkerTime) += 0.025!
                        Else
                            timeOrderedMarkers.Add(currentMarkerTime, 0.025)
                        End If
                        Dim oaBaseDate As Date = Date.FromOADate(currentMarkerTime)
                        currentMarkerTime = New OADate(oaBaseDate.Add(New TimeSpan(0, 60 \ increments, 0)))
                    Else
                        If timeOrderedMarkers.ContainsKey(currentMarkerTime) Then
                            timeOrderedMarkers(currentMarkerTime) += rate
                        Else
                            timeOrderedMarkers.Add(currentMarkerTime, rate)
                        End If
                        Dim oaBaseDate As Date = Date.FromOADate(currentMarkerTime)
                        currentMarkerTime = New OADate(oaBaseDate.Add(New TimeSpan(0, 5, 0)))
                    End If
                    Exit For
                End If
            Next
        End While
        Return timeOrderedMarkers
    End Function

End Module
