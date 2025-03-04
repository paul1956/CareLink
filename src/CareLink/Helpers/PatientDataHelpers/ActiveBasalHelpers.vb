' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ActiveBasalHelpers

    Public Function GetActiveBasalRateRecords() As List(Of BasalRateRecord)
        Debug.Assert(CurrentPdf.IsValid)
        For Each namedBasal As KeyValuePair(Of String, NamedBasalRecord) In CurrentPdf.Basal.NamedBasal
            If namedBasal.Value.Active Then
                Return namedBasal.Value.basalRates
            End If
        Next
        Return New List(Of BasalRateRecord)
    End Function

End Module
