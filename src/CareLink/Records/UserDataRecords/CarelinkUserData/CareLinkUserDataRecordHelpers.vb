' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Module CareLinkUserDataRecordHelpers

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(CareLinkUserDataRecord.ID),
                 NameOf(CareLinkUserDataRecord.CareLinkUserName),
                 NameOf(CareLinkUserDataRecord.CareLinkPassword),
                 NameOf(CareLinkUserDataRecord.CareLinkPatientUserID)
                cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case NameOf(CareLinkUserDataRecord.AutoLogin),
                 NameOf(CareLinkUserDataRecord.CareLinkPartner),
                 NameOf(CareLinkUserDataRecord.CountryCode),
                 NameOf(CareLinkUserDataRecord.UseLocalTimeZone),
                 "DeleteRow"
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0))
            Case ""
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(1, 1, 1, 1))
            Case Else
                Stop
                'Throw UnreachableException($"{NameOf(CareLinkUserDataRecordHelpers)}.{NameOf(GetCellStyle)}, {NameOf(columnName)} = {columnName}")
        End Select
        Return cellStyle
    End Function

    Public Function AllUserLoginInfoFileExists() As Boolean
        Return SavedUsersFileExists(GetUsersLoginInfoFileWithPath())
    End Function

    Public Function SavedUsersFileExists(userSettingsCsvFileWithPath As String) As Boolean
        Return File.Exists(userSettingsCsvFileWithPath)
    End Function

End Module
