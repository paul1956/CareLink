' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides helper methods for extracting manual basal values from marker data.
''' </summary>
Friend Module GetManualBasalPoints

    ''' <summary>
    '''  Calculates the manual basal values for a given marker,
    '''  returning a sorted dictionary of OADate to basal rate.
    ''' </summary>
    ''' <param name="markerWithIndex">
    '''  The <see cref="IndexClass(Of Marker)"/> containing the marker and
    '''  its index in the marker list.
    ''' </param>
    ''' <returns>
    '''  A <see cref="SortedDictionary(Of OADate, Single)"/> mapping OADate
    '''  timestamps to basal rates. Returns an empty dictionary if the marker
    '''  is not valid for manual basal calculation.
    ''' </returns>
    Friend Function GetManualBasalValues(markerWithIndex As IndexClass(Of Marker)) _
        As SortedDictionary(Of OADate, Single)

        Debug.Assert(condition:=CurrentPdf.IsValid)
        Dim item As Marker = markerWithIndex.Value
        Dim key As String = NameOf(LowGlucoseSuspended.deliverySuspended)
        If item.GetBooleanFromJson(key) Then
            Return New SortedDictionary(Of OADate, Single)
        End If
        Dim nextPumpSuspendTime As OADate
        Dim markerDateTime? As Date
        If s_markers.Count > 1 AndAlso markerWithIndex.Index = s_markers.Count - 2 Then
            Dim activationType As String =
                s_markers.Last().GetStringFromJson(NameOf(Insulin.ActivationType))
            If activationType = "MANUAL" Then
                markerDateTime = s_markers.Last().GetMarkerTimestamp
                If markerDateTime Is Nothing Then
                    Return New SortedDictionary(Of OADate, Single)
                End If
                nextPumpSuspendTime = New OADate(asDate:=markerDateTime.Value)
            Else
                nextPumpSuspendTime = New OADate(asDate:=PumpNow)
            End If
        Else
            markerDateTime = s_markers(index:=markerWithIndex.Index + 1).GetMarkerTimestamp
            If markerDateTime Is Nothing Then
                Return New SortedDictionary(Of OADate, Single)
            End If
            nextPumpSuspendTime = New OADate(asDate:=markerDateTime.Value)
        End If

        Dim lowGlucoseSuspend As New LowGlucoseSuspended(
            item:=s_markers.Last(),
            recordNumber:=s_markers.Count)
        If lowGlucoseSuspend.deliverySuspended Then
            Return New SortedDictionary(Of OADate, Single)
        End If

        Dim basalRateRecords As List(Of BasalRateRecord) = GetActiveBasalRateRecords()
        If basalRateRecords.Count = 0 Then
            Return New SortedDictionary(Of OADate, Single)
        End If

        markerDateTime = item.GetMarkerTimestamp
        If markerDateTime Is Nothing Then
            Return New SortedDictionary(Of OADate, Single)
        End If
        Dim currentMarkerTime As New OADate(asDate:=markerDateTime.Value)
        Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)
        While nextPumpSuspendTime > currentMarkerTime
            For Each e As IndexClass(Of BasalRateRecord) In basalRateRecords.WithIndex
                Dim basalRecord As BasalRateRecord = e.Value
                Dim start As TimeOnly = basalRecord.Time
                Dim [end] As TimeOnly = If(e.IsLast,
                                           Eleven59.AddMinutes(value:=1),
                                           basalRateRecords(index:=e.Index + 1).Time)
                Dim currentTimeOnly As TimeOnly =
                    TimeOnly.FromDateTime(Date.FromOADate(currentMarkerTime))

                If currentTimeOnly.IsBetween(start, [end]) Then
                    Dim rate As Single = basalRecord.UnitsPerHr / 12
                    Dim value As TimeSpan
                    If rate < 0.025 Then
                        If timeOrderedMarkers.ContainsKey(key:=currentMarkerTime) Then
                            timeOrderedMarkers(key:=currentMarkerTime) += 0.025!
                        Else
                            timeOrderedMarkers.Add(key:=currentMarkerTime, value:=0.025)
                        End If
                        Dim oaBaseDate As Date = Date.FromOADate(currentMarkerTime)
                        Dim increments As Integer =
                            CInt(Math.Ceiling((basalRecord.UnitsPerHr / 0.025).RoundTo025))
                        value = New TimeSpan(hours:=0, minutes:=60 \ increments, seconds:=0)
                        currentMarkerTime = New OADate(asDate:=oaBaseDate.Add(value))
                    Else
                        If timeOrderedMarkers.ContainsKey(key:=currentMarkerTime) Then
                            timeOrderedMarkers(key:=currentMarkerTime) += rate
                        Else
                            timeOrderedMarkers.Add(key:=currentMarkerTime, value:=rate)
                        End If
                        Dim oaBaseDate As Date = Date.FromOADate(currentMarkerTime)
                        currentMarkerTime =
                            New OADate(asDate:=oaBaseDate.Add(value:=FiveMinuteSpan))
                    End If
                    Exit For
                End If
            Next
        End While
        Return timeOrderedMarkers
    End Function

End Module
