' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeZoneExtensions

    Private ReadOnly s_specialKnownTimeZones As New Dictionary(Of String, String) From {
            {"Amazon Standard Time", "Central Brazilian Standard Time"},
            {"Argentina Standard Time", "Argentina Standard Time"},
            {"Bolivia Time", "SA Western Standard Time"},
            {"Brasilia Standard Time", "Central Brazilian Standard Time"},
            {"Central European Summer Time", "Central European Daylight Time"},
            {"Eastern European Summer Time", "E. Europe Daylight Time"},
            {"Eastern European Standard Time", "E. Europe Standard Time"},
            {"Mitteleuropäische Zeit", "Central European Standard Time"}
        }

    Friend Function CalculateTimeZone(clientTimeZoneName As String) As TimeZoneInfo
        If My.Settings.UseLocalTimeZone Then
            Return TimeZoneInfo.Local
        End If
        If clientTimeZoneName = "NaN" Then
            Return Nothing
        End If
        Dim clientTimeZone As TimeZoneInfo
        Dim id As String = ""
        If Not s_specialKnownTimeZones.TryGetValue(clientTimeZoneName, id) Then
            id = clientTimeZoneName
        End If

        If id.Contains("Daylight") Then
            clientTimeZone = s_timeZoneList.Where(Function(t As TimeZoneInfo)
                                                      Return t.DaylightName = id
                                                  End Function).FirstOrDefault
            If clientTimeZone IsNot Nothing Then
                Return clientTimeZone
            End If
        End If

        clientTimeZone = s_timeZoneList.Where(Function(t As TimeZoneInfo)
                                                  Return t.StandardName = id
                                              End Function).FirstOrDefault
        If clientTimeZone IsNot Nothing Then
            Return clientTimeZone
        End If

        Return s_timeZoneList.Where(Function(t As TimeZoneInfo)
                                        Return t.DisplayName = id
                                    End Function).FirstOrDefault
    End Function

End Module
