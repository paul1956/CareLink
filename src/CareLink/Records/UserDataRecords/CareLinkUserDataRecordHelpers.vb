' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class CareLinkUserDataRecordHelpers

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(CareLinkUserDataRecord.AlertPhoneNumber),
                        NameOf(CareLinkUserDataRecord.CareLinkPassword),
                        NameOf(CareLinkUserDataRecord.CarrierTextingDomain),
                        NameOf(CareLinkUserDataRecord.MailserverPassword),
                        NameOf(CareLinkUserDataRecord.MailServerPort),
                        NameOf(CareLinkUserDataRecord.MailserverUserName),
                        NameOf(CareLinkUserDataRecord.OutgoingMailServer)
                    }

    Friend Shared ReadOnly s_headerColumns As New List(Of String) From
            {
            NameOf(My.Settings.CareLinkUserName),
            NameOf(My.Settings.CareLinkPassword),
            NameOf(My.Settings.AIT),
            NameOf(My.Settings.AlertPhoneNumber),
            NameOf(My.Settings.CarrierTextingDomain),
            NameOf(My.Settings.CountryCode),
            NameOf(My.Settings.MailServerPassword),
            NameOf(My.Settings.MailServerPort),
            NameOf(My.Settings.MailServerUserName),
            NameOf(My.Settings.SettingsVersion),
            NameOf(My.Settings.OutGoingMailServer),
            NameOf(My.Settings.UseAdvancedAITDecay),
            NameOf(My.Settings.UseLocalTimeZone),
            NameOf(My.Settings.AutoLogin)
         }

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        If String.IsNullOrWhiteSpace(dataPropertyName) Then
            Return False
        End If
        Return Not (Debugger.IsAttached AndAlso Not s_filterJsonData) AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(CareLinkUserDataRecord.CareLinkUserName),
                 NameOf(CareLinkUserDataRecord.CareLinkPassword),
                 NameOf(CareLinkUserDataRecord.AIT),
                 NameOf(CareLinkUserDataRecord.AlertPhoneNumber),
                 NameOf(CareLinkUserDataRecord.CarrierTextingDomain),
                 NameOf(CareLinkUserDataRecord.CountryCode),
                 NameOf(CareLinkUserDataRecord.MailserverPassword),
                 NameOf(CareLinkUserDataRecord.MailserverUserName),
                 NameOf(CareLinkUserDataRecord.OutgoingMailServer)
                cellStyle.CellStyleMiddleLeft
            Case NameOf(CareLinkUserDataRecord.AutoLogin),
                 NameOf(CareLinkUserDataRecord.UseAdvancedAITDecay),
                 NameOf(CareLinkUserDataRecord.UseLocalTimeZone)
                cellStyle = cellStyle.CellStyleMiddleCenter
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(CareLinkUserDataRecord.MailServerPort),
                 NameOf(CareLinkUserDataRecord.SettingsVersion),
                 ""
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Public Shared Function GetColumnName(index As Integer) As String
        Return s_headerColumns(index)
    End Function

End Class
