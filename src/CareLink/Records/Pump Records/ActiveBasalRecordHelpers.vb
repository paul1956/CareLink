' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ActiveBasalRecordHelpers

    Public Function GetActiveBasalRateRecords() As List(Of BasalRateRecord)
        Dim activeBasalRecords As New List(Of BasalRateRecord)
        For Each namedBasal As KeyValuePair(Of String, NamedBasalRecord) In CurrentPdf.Basal.NamedBasal
            If namedBasal.Value.Active Then
                activeBasalRecords = namedBasal.Value.basalRates
                Exit For
            End If
        Next

        Return activeBasalRecords
    End Function

End Module
