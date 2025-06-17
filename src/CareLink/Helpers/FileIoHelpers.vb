' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO

''' <summary>
'''  Provides helper methods and properties for file and directory I/O operations within the CareLink project.
''' </summary>
Friend Module FileIoHelpers

    ''' <summary>
    '''  The file name for storing all users' login information.
    ''' </summary>
    Private Const AllUsersLoginInfoFileName As String = "CareLink.Csv"

    ''' <summary>
    '''  The path to the project's web cache directory in the user's Documents folder.
    ''' </summary>
    Friend ReadOnly s_projectWebCache As String = Path.Join(GetMyDocuments(), "CareLink", "WebCache")

    ''' <summary>
    '''  Gets the path to the directory where project data is stored in the user's Documents folder.
    ''' </summary>
    Friend ReadOnly Property DirectoryForProjectData As String = Path.Join(GetMyDocuments(), "CareLink")

    ''' <summary>
    '''  Gets the full path to the sample user data JSON file for testing.
    ''' </summary>
    Friend ReadOnly Property TestDataFileNameWithPath As String = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "SampleUserV2Data.json")

    ''' <summary>
    '''  Gets the full path to the test settings JSON file.
    ''' </summary>
    Friend ReadOnly Property TestSettingsFileNameWithPath As String = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "TestFileSettings.json")

    ''' <summary>
    '''  Gets the path to the settings directory in the user's Documents folder.
    ''' </summary>
    Public ReadOnly Property SettingsDirectory As String = Path.Join(GetMyDocuments(), "CareLink", "Settings")

    ''' <summary>
    '''  Determines whether any files matching the specified pattern exist in the given directory.
    ''' </summary>
    ''' <param name="path">The directory to search.</param>
    ''' <param name="matchPattern">The search pattern (e.g., "*.json").</param>
    ''' <returns>True if any matching files are found; otherwise, false.</returns>
    Friend Function AnyMatchingFiles(path As String, matchPattern As String) As Boolean
        Return Directory.GetFiles(path, matchPattern).Length > 0
    End Function

    ''' <summary>
    '''  Gets the full path to the graph colors CSV file.
    ''' </summary>
    ''' <returns>The full path to "GraphColors.Csv" in the project data directory.</returns>
    Friend Function GetGraphColorsFileNameWithPath() As String
        Return Path.Join(DirectoryForProjectData, "GraphColors.Csv")
    End Function

    ''' <summary>
    '''  Gets the full path to the last download JSON file, using the current UI culture.
    ''' </summary>
    ''' <returns>The full path to the last download file.</returns>
    Friend Function GetLastDownloadFileWithPath() As String
        Return GetUniqueDataFileName(
            baseName:=BaseNameSavedLastDownload,
            cultureName:=CultureInfo.CurrentUICulture.Name,
            extension:="json",
            MustBeUnique:=False).withPath
    End Function

    ''' <summary>
    '''  Gets the full path to the current user's settings JSON file.
    ''' </summary>
    ''' <returns>The full path to the user's settings JSON file.</returns>
    Friend Function GetUserSettingsJsonFileNameWithPath() As String
        Return Path.Join(SettingsDirectory, $"{s_userName}Settings.json")
    End Function

    ''' <summary>
    '''  Gets the full path to the current user's settings PDF file.
    ''' </summary>
    ''' <returns>The full path to the user's settings PDF file.</returns>
    Friend Function GetUserSettingsPdfFileNameWithPath() As String
        Return Path.Join(SettingsDirectory, $"{s_userName}Settings.pdf")
    End Function

    ''' <summary>
    '''  Gets the full path to the file containing all users' login information.
    ''' </summary>
    ''' <returns>The full path to the login info CSV file.</returns>
    Friend Function GetUsersLoginInfoFileWithPath() As String
        Return Path.Join(DirectoryForProjectData, AllUsersLoginInfoFileName)
    End Function

    ''' <summary>
    '''  Determines whether the specified file has not been modified for at least 30 days.
    ''' </summary>
    ''' <param name="userSettingsFileWithPath">The full path to the user settings file.</param>
    ''' <returns><see langword="True"/> if the file is stale; otherwise, <see langword="False"/>.</returns>
    Friend Function IsFileStale(userSettingsFileWithPath As String) As Boolean
        Return File.GetLastWriteTime(userSettingsFileWithPath) < Now - ThirtyDaysSpan
    End Function

    ''' <summary>
    '''  Moves all files matching the specified pattern from one directory to another.
    ''' </summary>
    ''' <param name="previousDirectory">The source directory.</param>
    ''' <param name="currentDirectory">The destination directory.</param>
    ''' <param name="searchPattern">The search pattern for files to move.</param>
    Friend Sub MoveFiles(previousDirectory As String, currentDirectory As String, searchPattern As String)
        For Each f As String In Directory.EnumerateFiles(previousDirectory, searchPattern)
            Dim fileName As String = Path.GetFileName(f)
            File.Move(f, Path.Join(currentDirectory, fileName))
        Next
    End Sub

    ''' <summary>
    '''  Gets the path to the user's "My Documents" folder.
    ''' </summary>
    ''' <returns>The full path to the "My Documents" folder.</returns>
    Public Function GetMyDocuments() As String
        Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    End Function

    ''' <summary>
    '''  Determines whether the specified file is read-only.
    ''' </summary>
    ''' <param name="fileNameWithPath">The full path to the file.</param>
    ''' <returns><see langword="True"/> if the file is read-only; otherwise, <see langword="False"/>.</returns>
    ''' <exception cref="ArgumentException">Thrown if the file name is null or whitespace.</exception>
    Public Function IsFileReadOnly(fileNameWithPath As String) As Boolean
        If String.IsNullOrWhiteSpace(fileNameWithPath) Then
            Throw New ArgumentException($"'{NameOf(fileNameWithPath)}' cannot be null or whitespace.", NameOf(fileNameWithPath))
        End If

        Try
            If File.Exists(fileNameWithPath) Then
                Dim attributes As FileAttributes = File.GetAttributes(fileNameWithPath)
                Return attributes.HasFlag(FileAttributes.ReadOnly)
            End If
        Catch e As Exception
            Stop
            Debug.WriteLine("Error: {0}", e.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    '''  Updates the last write time of the specified file to the current UTC time.
    ''' </summary>
    ''' <param name="fileNameWithPath">The full path to the file.</param>
    ''' <exception cref="ArgumentException">Thrown if the file name is null or whitespace.</exception>
    Public Sub TouchFile(fileNameWithPath As String)
        If String.IsNullOrWhiteSpace(fileNameWithPath) Then
            Throw New ArgumentException($"'{NameOf(fileNameWithPath)}' cannot be null or whitespace.", NameOf(fileNameWithPath))
        End If
        If File.Exists(fileNameWithPath) Then
            Dim myFileStream As FileStream = File.Open(fileNameWithPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
            myFileStream.Close()
            myFileStream.Dispose()
            File.SetLastWriteTimeUtc(fileNameWithPath, lastWriteTimeUtc:=Date.UtcNow)
        End If
    End Sub

End Module
