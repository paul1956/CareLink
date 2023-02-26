' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO

Friend Module FileIoHelpers
    Private s_myDocuments As String = Nothing
    Private s_ProjectData As String = Nothing
    Public ReadOnly Property AllUserLoginInfoFileName As String = $"{ProjectName}.Csv"

    Public ReadOnly Property SavedErrorReportBaseName As String = $"{ProjectName}ErrorReport"

    Public ReadOnly Property SavedLastDownloadBaseName As String = $"{ProjectName}LastDownload"

    Public ReadOnly Property SavedSnapshotBaseName As String = $"{ProjectName}Snapshot"

    Friend Function AnyMatchingFiles(matchPattern As String) As Boolean
        Return Directory.GetFiles(GetDirectoryForProjectData(), matchPattern).Any
    End Function

    Friend Function GetDirectoryForMyDocuments() As String
        If s_myDocuments Is Nothing Then
            s_myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        End If
        Return s_myDocuments
    End Function

    Friend Function GetDirectoryForProjectData() As String
        If s_ProjectData Is Nothing Then
            s_ProjectData = Path.Combine(GetDirectoryForMyDocuments, ProjectName)
        End If
        Return s_ProjectData
    End Function

    Friend Function GetDirectoryForSettings() As String
        Return Path.Combine(GetDirectoryForProjectData(), "Settings")
    End Function

    Friend Function GetPathToAllUserLoginInfo(current As Boolean) As String
        If current Then
            Return Path.Combine(GetDirectoryForProjectData(), AllUserLoginInfoFileName)
        End If

        Return Path.Combine(GetDirectoryForMyDocuments(), AllUserLoginInfoFileName)
    End Function

    Friend Function GetPathToGraphColorsFile(current As Boolean) As String
        If current Then
            Return Path.Combine(GetDirectoryForProjectData(), "GraphColors.Csv")
        End If
        Return Path.Combine(GetDirectoryForMyDocuments(), $"{ProjectName}GraphColors.Csv")
    End Function

    Friend Function GetPathToLastDownloadFile() As String
        Return GetDataFileName(SavedLastDownloadBaseName, CultureInfo.CurrentUICulture.Name, "json", False).withPath
    End Function

    Friend Function GetPathToShowLegendFile(current As Boolean) As String
        If current Then
            Return Path.Combine(GetDirectoryForProjectData(), "ShowLegend.txt")
        End If
        Return Path.Combine(GetDirectoryForMyDocuments(), $"{ProjectName}ShowLegend.txt")
    End Function

    Friend Function GetPathToTestData() As String
        Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
    End Function

    Friend Function GetPathToTestSettingsFile() As String
        Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFileSettings.json")
    End Function

    Friend Function GetPathToUserSettingsFile(careLinkUserName As String) As String
        Dim currentUserSettingsFileName As String = $"{careLinkUserName}Settings.json"
        Return Path.Combine(GetDirectoryForSettings(), currentUserSettingsFileName)
    End Function

    Friend Sub MoveFiles(previousDirectory As String, currentDirectory As String, searchPattern As String)
        For Each f As String In Directory.EnumerateFiles(previousDirectory, searchPattern)
            Dim fileName As String = Path.GetFileName(f)
            File.Move(f, Path.Combine(currentDirectory, fileName))
        Next
    End Sub

    Friend Sub MoveIfExists(previousFile As String, currentFile As String, ByRef lastError As String)
        If File.Exists(previousFile) Then
            lastError = $"Can't move {previousFile} to {currentFile}"
            File.Move(previousFile, currentFile)
        End If
    End Sub

End Module
