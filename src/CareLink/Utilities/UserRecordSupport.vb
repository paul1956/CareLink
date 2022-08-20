' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.Devices

Friend Module UserRecordsSupport
    Public ReadOnly s_allUserSettingsData As New Dictionary(Of String, UserDataRecord)(StringComparer.OrdinalIgnoreCase)
    Public ReadOnly s_settingsCsvFile As String = Path.Combine(MyDocumentsPath, SavedCsvFileName)

    Public Sub LoadAllUserData()
        Dim userRecords As New List(Of UserDataRecord)
        s_allUserSettingsData.Clear()

        If File.Exists(s_settingsCsvFile) Then
            Using myReader As New FileIO.TextFieldParser(s_settingsCsvFile)
                myReader.TextFieldType = FileIO.FieldType.Delimited
                myReader.Delimiters = New String() {","}
                Dim currentRow As String()
                'Loop through all of the fields in the file.
                'If any lines are corrupt, report an error and continue parsing.
                Dim rowIndex As Integer = 0
                While Not myReader.EndOfData
                    Try
                        currentRow = myReader.ReadFields()
                        ' Include code here to handle the row.
                        If rowIndex <> 0 Then
                            s_allUserSettingsData.Add(currentRow(0), New UserDataRecord(currentRow))
                        End If
                        rowIndex += 1
                    Catch ex As FileIO.MalformedLineException
                        MsgBox($"Line {ex.Message} is invalid.  Skipping")
                    End Try
                End While
            End Using
        End If
    End Sub

    Public Sub SaveAllUserData(Key As String, Value As String)
        Dim currentSettings As New UserDataRecord
        If Not Key.Equals(NameOf(My.Settings.CareLinkUserName).ToString, StringComparison.OrdinalIgnoreCase) Then
            currentSettings.Update(Key, Value)
        Else
            currentSettings.Update(Key, Value)
            If Not s_allUserSettingsData.TryAdd(Value, currentSettings) Then
                currentSettings = s_allUserSettingsData(Value)
                currentSettings.Update(Key, Value)
                s_allUserSettingsData(currentSettings.CareLinkUserName) = currentSettings
            End If
        End If

        Dim sb As New StringBuilder
        sb.AppendLine(String.Join(",", UserDataRecord._headerColumns))
        For Each r As UserDataRecord In s_allUserSettingsData.Values
            sb.AppendLine(r.ToCsvString)
        Next
        My.Computer.FileSystem.WriteAllText(s_settingsCsvFile, sb.ToString, False)
    End Sub

End Module
