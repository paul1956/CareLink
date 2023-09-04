' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO

Friend Module FileIoHelpers
    Private Const AllUsersLoginInfoFileName As String = "CareLink.Csv"
    Private ReadOnly s_myDocuments As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    Private ReadOnly s_projectData As String = Path.Combine(MyDocuments, "CareLink")
    Private ReadOnly s_settingsDirectory As String = Path.Combine(MyDocuments, "CareLink", "Settings")
    Private ReadOnly s_testDataFileNameWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleUserData.json")
    Private ReadOnly s_testSettingFileNameWithPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFileSettings.json")

    Friend ReadOnly Property DirectoryForProjectData As String
        Get
            Return s_projectData
        End Get
    End Property

    Friend ReadOnly Property TestDataFileNameWithPath As String
        Get
            Return s_testDataFileNameWithPath
        End Get
    End Property

    Friend ReadOnly Property TestSettingsFileNameWihtPath As String
        Get
            Return s_testSettingFileNameWithPath
        End Get
    End Property

    Public ReadOnly Property MyDocuments As String
        Get
            Return s_myDocuments
        End Get
    End Property

    Public ReadOnly Property SettingsDirectory As String
        Get
            Return s_settingsDirectory
        End Get
    End Property

    Friend Function AnyMatchingFiles(path As String, matchPattern As String) As Boolean
        Return Directory.GetFiles(path, matchPattern).Length > 0
    End Function

    Friend Function GetGraphColorsFileNameWithPath(current As Boolean) As String
        Return If(current,
                  Path.Combine(DirectoryForProjectData, "GraphColors.Csv"),
                  Path.Combine(MyDocuments, "CareLinkGraphColors.Csv")
                 )
    End Function

    Friend Function GetLastDownloadFileWithPath() As String
        Return GetUniqueDataFileName(BaseNameSavedLastDownload, CultureInfo.CurrentUICulture.Name, "json", False).withPath
    End Function

    Friend Function GetUserSettingsJsonFileNameWithPath() As String
        Return Path.Combine(SettingsDirectory, $"{My.Settings.CareLinkUserName}Settings.json")
    End Function

    Friend Function GetUserSettingsPdfFileNameWithPath() As String
        Return Path.Combine(SettingsDirectory, $"{My.Settings.CareLinkUserName}Settings.pdf")
    End Function

    Friend Function GetUsersLoginInfoFileWithPath(current As Boolean) As String
        Return If(current,
                  Path.Combine(DirectoryForProjectData, AllUsersLoginInfoFileName),
                  Path.Combine(MyDocuments, AllUsersLoginInfoFileName)
                 )
    End Function

    ''' <summary>
    ''' File hasn.t been touched for at least 30 days
    ''' </summary>
    ''' <param name="userSettingsFileWithPath"></param>
    ''' <returns>True if Stale</returns>
    Friend Function IsFileStale(userSettingsFileWithPath As String) As Boolean
        Return File.GetLastWriteTime(userSettingsFileWithPath) < Now - s_30DaysSpan
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

    Public Sub TouchFile(fileNameWithPath As String)
        Try
            If String.IsNullOrWhiteSpace(fileNameWithPath) Then
                Throw New ArgumentException($"'{NameOf(fileNameWithPath)}' cannot be null or whitespace.", NameOf(fileNameWithPath))
            End If

            ' Update the CreationTime, LastWriteTime and LastAccessTime.
            Dim fsi As FileSystemInfo = New FileInfo(fileNameWithPath) With {
                .CreationTime = Date.Now,
                .LastAccessTime = Date.Now,
                .LastWriteTime = Date.Now
            }
        Catch e As Exception
            Stop
            Console.WriteLine("Error: {0}", e.Message)
        End Try

    End Sub

End Module
