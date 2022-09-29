Imports System.Text

' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CareLinkUserDataListHelpers

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
            If s_allUserSettingsData.ContainsKey(Value) Then
                loggedOnUser = s_allUserSettingsData(Value)
            Else
                ' We have a new user
                loggedOnUser.clean()
                s_allUserSettingsData.Add(loggedOnUser)
            End If
        End If

        SaveAllUserRecords()
    End Sub

    Public Shared Sub SaveAllUserRecords()
        Dim sb As New StringBuilder
        sb.AppendLine(String.Join(",", CareLinkUserDataRecordHelpers.s_headerColumns))
        For Each r As CareLinkUserDataRecord In s_allUserSettingsData.Values
            sb.AppendLine(r.ToCsvString)
        Next
        My.Computer.FileSystem.WriteAllText(s_settingsCsvFile, sb.ToString, False)
    End Sub

End Class
