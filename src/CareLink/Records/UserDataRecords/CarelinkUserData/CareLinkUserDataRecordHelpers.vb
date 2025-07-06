' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Module CareLinkUserDataRecordHelpers

    ''' <summary>
    '''  Returns a <see cref="DataGridViewCellStyle"/> configured for the specified column name.
    ''' </summary>
    ''' <param name="columnName">The name of the column for which to get the cell style.</param>
    ''' <returns>
    '''  A <see cref="DataGridViewCellStyle"/> instance with alignment and padding set
    '''  according to the column's expected content.
    ''' </returns>
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

    ''' <summary>
    '''  Determines whether the file containing all user login information exists.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if the user login info file exists; otherwise, <see langword="False"/>.
    ''' </returns>
    Public Function AllUserLoginInfoFileExists() As Boolean
        Return SavedUsersFileExists(UserSettingsCsvFileWithPath)
    End Function

    ''' <summary>
    '''  Determines whether the specified user settings CSV file exists.
    ''' </summary>
    ''' <param name="userSettingsCsvFileWithPath">The full path to the user settings CSV file.</param>
    ''' <returns>
    '''  <see langword="True"/> if the file exists at the specified path; otherwise, <see langword="False"/>.
    ''' </returns>
    Public Function SavedUsersFileExists(userSettingsCsvFileWithPath As String) As Boolean
        Return File.Exists(userSettingsCsvFileWithPath)
    End Function

End Module
