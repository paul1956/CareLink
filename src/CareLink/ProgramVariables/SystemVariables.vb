' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization

Public Module SystemVariables

#Region "Used for painting"

    Friend ReadOnly s_activeInsulinMarkers As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_summaryMarkersInsulin As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_summaryMarkersMeal As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_treatmentMarkersInsulin As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_treatmentMarkersMeal As New Dictionary(Of OADate, Single)

#End Region ' Used for painting

    Private s_userName As String
    Friend s_allUserSettingsData As New CareLinkUserDataList
    Friend s_countryCode As String = String.Empty
    Friend s_currentSummaryRow As Integer = 0
    Friend s_formLoaded As Boolean = False
    Friend s_password As String = String.Empty
    Friend s_useLocalTimeZone As Boolean
    Friend ReadOnly Property CareLinkDecimalSeparator As Char = "."c
    Friend Property CurrentUser As CurrentUserRecord
    Friend Property DecimalSeparator As String = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator
    Friend Property MaxBasalPerDose As Double
    Friend Property TreatmentInsulinRow As Single

    Friend Function GetUserName() As String
        If IsNullOrWhiteSpace(value:=s_userName) Then
            s_userName = $"{My.Settings.CareLinkUserName} ".Trim()
        End If
        Return s_userName
    End Function

    Friend Sub SetUserName(value As String)
        s_userName = value
    End Sub

End Module
