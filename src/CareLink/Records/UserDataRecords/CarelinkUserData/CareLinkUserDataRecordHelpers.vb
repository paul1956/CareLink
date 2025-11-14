' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Module CareLinkUserDataRecordHelpers

    ''' <summary>
    '''  Returns a <see cref="DataGridViewCellStyle"/> configured for
    '''  the specified column name.
    ''' </summary>
    ''' <param name="columnName">
    '''  The name of the column for which to get the cell style.
    ''' </param>
    ''' <returns>
    '''  A <see cref="DataGridViewCellStyle"/> instance with align and pad set
    '''  according to the column's expected content.
    ''' </returns>
    Friend Function GetCellStyleForCareLinkUser(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(CareLinkUserDataRecord.ID),
                 NameOf(CareLinkUserDataRecord.CareLinkUserName),
                 NameOf(CareLinkUserDataRecord.CareLinkPassword),
                 NameOf(CareLinkUserDataRecord.CareLinkPatientUserID)

                cellStyle.SetCellStyle(align:=DataGridViewContentAlignment.MiddleLeft, pad:=New Padding(all:=1))
            Case NameOf(CareLinkUserDataRecord.AutoLogin),
                 NameOf(CareLinkUserDataRecord.CareLinkPartner),
                 NameOf(CareLinkUserDataRecord.CountryCode),
                 NameOf(CareLinkUserDataRecord.UseLocalTimeZone),
                 "DeleteRow"

                cellStyle.SetCellStyle(align:=DataGridViewContentAlignment.MiddleCenter, pad:=New Padding(all:=0))
            Case Else
                If String.IsNullOrWhiteSpace(columnName) Then
                    cellStyle.SetCellStyle(align:=DataGridViewContentAlignment.MiddleRight, pad:=New Padding(all:=1))
                Else
                    Stop
                    Throw UnreachableException(
                        paramName:=$"Column {NameOf(columnName)} = {columnName}",
                        memberName:=NameOf(CareLinkUserDataRecordHelpers))
                End If
        End Select
        Return cellStyle
    End Function

    ''' <summary>
    '''  Determines whether the file containing all user login information exists.
    ''' </summary>
    ''' <returns>
    '''  <see langword="True"/> if the user login info file exists;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Function AllUserLoginInfoFileExists() As Boolean
        Return SavedUsersFileExists()
    End Function

    ''' <summary>
    '''  Determines whether the specified user settings CSV file exists.
    ''' </summary>
    ''' <param name="userSettingsCsvFileWithPath">
    '''  The full path to the user settings CSV file.</param>
    ''' <returns>
    '''  <see langword="True"/> if the file exists at the specified path;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Function SavedUsersFileExists() As Boolean
        Return File.Exists(path:=GetAllUsersCsvPath())
    End Function

End Module
