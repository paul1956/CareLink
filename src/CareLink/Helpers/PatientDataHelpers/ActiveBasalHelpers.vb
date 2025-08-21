' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ActiveBasalHelpers

    ''' <summary>
    '''  Gets the active basal rate records from the current PDF.
    '''  This function iterates through the named basal records in the current PDF and returns
    '''  a list of basal rate records that are marked as active.
    '''  If no active basal rates are found, it returns an empty list.
    ''' </summary>
    ''' <returns>
    '''  A list of <see cref="BasalRateRecord"/> objects representing the active basal rates.
    ''' </returns>
    ''' <remarks>
    '''  This function assumes that the current PDF is valid and contains named basal records.
    '''  It will return an empty list if no active basal rates are found.
    ''' </remarks>
    Public Function GetActiveBasalRateRecords() As List(Of BasalRateRecord)
        Debug.Assert(condition:=CurrentPdf.IsValid)
        For Each namedBasal As KeyValuePair(Of String, NamedBasalRecord) In
            CurrentPdf.Basal.NamedBasal

            If namedBasal.Value.Active Then
                Dim basalRates As List(Of BasalRateRecord) = namedBasal.Value.basalRates
                If basalRates IsNot Nothing AndAlso basalRates.Count > 0 Then
                    Return basalRates
                End If
            End If
        Next
        Return New List(Of BasalRateRecord)()
    End Function

End Module
