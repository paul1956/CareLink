' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

Public Class CareLinkUserDataRecordHelpers

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(CareLinkUserDataRecord.AlertPhoneNumber),
                        NameOf(CareLinkUserDataRecord.CareLinkPassword),
                        NameOf(CareLinkUserDataRecord.CarrierTextingDomain),
                        NameOf(CareLinkUserDataRecord.MailserverPassword),
                        NameOf(CareLinkUserDataRecord.MailServerPort),
                        NameOf(CareLinkUserDataRecord.MailserverUserName),
                        NameOf(CareLinkUserDataRecord.OutGoingMailServer)
                    }
    Private Shared ReadOnly _headerColumns As New List(Of String) From
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
#If SupportMailServer <> "True" Then
        If dataPropertyName.Contains("Mail") Then
            Return True
        End If
#End If
        If String.IsNullOrWhiteSpace(dataPropertyName) Then
            Return False
        End If
        Return Not (Debugger.IsAttached AndAlso Not s_filterJsonData) AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Function GetColumnName(index As Integer) As String
        Return _headerColumns(index)
    End Function

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        If columnName = NameOf(CareLinkUserDataRecord.AIT) Then
            Return New DataGridViewCellStyle().CellStyleMiddleLeft()

        End If
        Return New DataGridViewCellStyle().CellStyleMiddleLeft()
    End Function

    Public Shared Sub SaveAllUserRecords(loggedOnUser As CareLinkUserDataRecord, Key As String, Value As String)
        If Not Key.Equals(NameOf(My.Settings.CareLinkUserName).ToString, StringComparison.OrdinalIgnoreCase) Then
            ' We are changing something other than the user name
            ' Update logged on user and the saved file
            loggedOnUser.UpdateValue(Key, Value)
            If Not s_allUserSettingsData.TryAdd(loggedOnUser) Then
                s_allUserSettingsData(loggedOnUser.CareLinkUserName) = loggedOnUser
            End If
        Else
            ' We are changing the user name, first try to load it
            If Not s_allUserSettingsData.ContainsKey(Value) Then
                ' We have a new user
                loggedOnUser.clean()
                s_allUserSettingsData.Add(loggedOnUser)
            Else
                loggedOnUser = s_allUserSettingsData(Value)
            End If
        End If

        Dim sb As New StringBuilder
        sb.AppendLine(String.Join(",", _headerColumns))
        For Each r As CareLinkUserDataRecord In s_allUserSettingsData.Values
            sb.AppendLine(r.ToCsvString)
        Next
        My.Computer.FileSystem.WriteAllText(s_settingsCsvFile, sb.ToString, False)
    End Sub

End Class
