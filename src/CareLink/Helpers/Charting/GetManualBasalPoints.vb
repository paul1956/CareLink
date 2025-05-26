' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides helper methods for extracting manual basal values from marker data.
''' </summary>
Friend Module GetManualBasalPoints

    ''' <summary>
    '''  Calculates the manual basal values for a given marker, returning a sorted dictionary of OADate to basal rate.
    ''' </summary>
    ''' <param name="markerWithIndex">
    '''  The <see cref="IndexClass(Of Marker)"/> containing the marker and its index in the marker list.
    ''' </param>
    ''' <returns>
    '''  A <see cref="SortedDictionary(Of OADate, Single)"/> mapping OADate timestamps to basal rates.
    '''  Returns an empty dictionary if the marker is not valid for manual basal calculation.
    ''' </returns>
    Friend Function GetManualBasalValues(markerWithIndex As IndexClass(Of Marker)) As SortedDictionary(Of OADate, Single)
        Debug.Assert(CurrentPdf.IsValid)
        Dim markerEntry As Marker = markerWithIndex.Value
        If markerEntry.GetBooleanValueFromJson(NameOf(LowGlucoseSuspended.deliverySuspended)) Then
            Return New SortedDictionary(Of OADate, Single)
        End If
        Dim nextPumpSuspendTime As OADate
        Dim markerDateTime? As Date
        If s_markers.Count > 1 AndAlso markerWithIndex.Index = s_markers.Count - 2 Then
            Dim activationType As String = s_markers.Last().GetStringValueFromJson(NameOf(Insulin.ActivationType))
            If activationType = "MANUAL" Then
                markerDateTime = s_markers.Last().GetMarkerTimestamp
                If markerDateTime Is Nothing Then
                    Return New SortedDictionary(Of OADate, Single)
                End If
                nextPumpSuspendTime = New OADate(markerDateTime.Value)
            Else
                nextPumpSuspendTime = New OADate(PumpNow)
            End If
        Else
            markerDateTime = s_markers(markerWithIndex.Index + 1).GetMarkerTimestamp
            If markerDateTime Is Nothing Then
                Return New SortedDictionary(Of OADate, Single)
            End If
            nextPumpSuspendTime = New OADate(markerDateTime.Value)
        End If

        Dim lowGlucoseSuspend As New LowGlucoseSuspended(s_markers.Last(), s_markers.Count)
        If lowGlucoseSuspend.deliverySuspended Then
            Return New SortedDictionary(Of OADate, Single)
        End If

        Dim basalRateRecords As List(Of BasalRateRecord) = GetActiveBasalRateRecords()
        If basalRateRecords.Count = 0 Then
            Return New SortedDictionary(Of OADate, Single)
        End If

        markerDateTime = markerEntry.GetMarkerTimestamp
        If markerDateTime Is Nothing Then
            Return New SortedDictionary(Of OADate, Single)
        End If
        Dim currentMarkerTime As New OADate(markerDateTime.Value)
        Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)
        While nextPumpSuspendTime > currentMarkerTime
            For Each e As IndexClass(Of BasalRateRecord) In basalRateRecords.WithIndex
                Dim basalRecord As BasalRateRecord = e.Value
                Dim startTime As TimeOnly = basalRecord.Time
                Dim endTime As TimeOnly = If(e.IsLast,
                                             Eleven59.AddMinutes(1),
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
