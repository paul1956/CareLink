' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
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

    Public Shared ReadOnly s_allUserSettingsData As New Dictionary(Of String, CareLinkUserDataRecord)(StringComparer.OrdinalIgnoreCase)
    Public Shared ReadOnly s_settingsCsvFile As String = Path.Combine(MyDocumentsPath, SavedCsvFileName)

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

    Public Shared Function GetCellStyle() As DataGridViewCellStyle
        Return New DataGridViewCellStyle().CellStyleMiddleLeft
    End Function

    Public Shared Sub LoadAllUserRecords(BindingSource1 As BindingSource)
        s_allUserSettingsData.Clear()

        If File.Exists(s_settingsCsvFile) Then
            Using myReader As New FileIO.TextFieldParser(s_settingsCsvFile)
                myReader.TextFieldType = FileIO.FieldType.Delimited
                myReader.Delimiters = New String() {","}
                Dim currentRow As String()
                'Loop through all of the fields in the file.
                'If any lines are corrupt, report an error and continue parsing.
                Dim rowIndex As Integer = 0
                BindingSource1.Clear()

                While Not myReader.EndOfData
                    Try
                        currentRow = myReader.ReadFields()
                        ' Include code here to handle the row.
                        If rowIndex <> 0 Then
                            Dim oneUser As New CareLinkUserDataRecord(currentRow)
                            BindingSource1.Add(oneUser)
                            s_allUserSettingsData.Add(currentRow(0), oneUser)
                        End If
                        rowIndex += 1
                    Catch ex As FileIO.MalformedLineException
                        MsgBox($"Line {ex.Message} is invalid.  Skipping")
                    End Try
                End While
            End Using
        End If
    End Sub

    Public Shared Sub SaveAllUserRecords(loggedOnUser As CareLinkUserDataRecord, Key As String, Value As String)
        If Not Key.Equals(NameOf(My.Settings.CareLinkUserName).ToString, StringComparison.OrdinalIgnoreCase) Then
            ' We are changing something other than the user name
            ' Update logged on user and the saved file
            loggedOnUser.Update(Key, Value)
            If Not s_allUserSettingsData.TryAdd(loggedOnUser.CareLinkUserName, loggedOnUser) Then
                s_allUserSettingsData(loggedOnUser.CareLinkUserName) = loggedOnUser
            End If
        Else
            ' We are changing the user name, first try to load it
            If s_allUserSettingsData.ContainsKey(Value) Then
                loggedOnUser = s_allUserSettingsData(Value)
                Exit Sub
            End If
            ' We have a new user
            loggedOnUser.clean(Value)
            s_allUserSettingsData.Add(Value, loggedOnUser)
        End If

        Dim sb As New StringBuilder
        sb.AppendLine(String.Join(",", _headerColumns))
        For Each r As CareLinkUserDataRecord In s_allUserSettingsData.Values
            sb.AppendLine(r.ToCsvString)
        Next
        My.Computer.FileSystem.WriteAllText(s_settingsCsvFile, sb.ToString, False)
    End Sub

End Class
