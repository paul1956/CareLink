' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO

''' <summary>
'''  Provides helper methods and properties for file and directory I/O operations within the CareLink™ project.
''' </summary>
Public Module FileIoHelpers

    ''' <summary>
    '''  Determines whether any files matching the specified pattern
    '''  exist in the given directory.
    ''' </summary>
    ''' <param name="path">The directory to search.</param>
    ''' <param name="searchPattern">The search pattern (e.g., "*.json").</param>
    ''' <returns>
    '''  True if any matching files are found;
    '''  otherwise, false.
    ''' </returns>
    ''' <exception cref="ArgumentNullException">
    '''  Thrown if the path is null or whitespace.
    ''' </exception>
    ''' <remarks>
    '''  This method checks for files matching the specified pattern in the given directory.
    '''  It returns true if at least one file matches the pattern, otherwise false.
    ''' </remarks>
    Friend Function AnyMatchingFiles(path As String, searchPattern As String) As Boolean
        If String.IsNullOrWhiteSpace(path) Then
            Throw New ArgumentNullException(paramName:=NameOf(path), message:="Path cannot be null.")
        End If
        Return Directory.GetFiles(path, searchPattern).Length > 0
    End Function

    ''' <summary>
    '''  Gets the full path to the file containing all users' login information.
    ''' </summary>
    ''' <returns>The full path to the login info CSV file.</returns>
    Friend Function GetAllUsersCsvPath() As String
        Return Path.Join(GetProjectDataDirectory(), "CareLink.Csv")
    End Function

    ''' <summary>
    '''  Gets the full path to the graph colors CSV file.
    ''' </summary>
    ''' <returns>The full path to "GraphColors.Csv" in the project data directory.</returns>
    Friend Function GetGraphColorsFileNameWithPath() As String
        Return Path.Join(GetProjectDataDirectory(), "GraphColors.Csv")
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
            mustBeUnique:=False).withPath
    End Function

    ''' <summary>
    '''  Gets the path to the directory where project data is
    '''  stored in the user's Documents folder.
    ''' </summary>
    Friend Function GetProjectDataDirectory() As String
        Return Path.Join(GetMyDocuments(), "CareLink")
    End Function

    ''' <summary>
    '''  Gets the full path to the WebCache.
    ''' </summary>
    ''' <returns>The full path to the WebCache directory.</returns>
    Friend Function GetProjectWebCache() As String
        Return Path.Join(GetProjectDataDirectory(), "WebCache")
    End Function

    ''' <summary>
    '''  Gets the full path to the sample user data JSON file for testing.
    ''' </summary>
    ''' <returns>The full path to "SampleUserV2Data.json" in the application's base directory.</returns>
    Friend Function GetTestDataPath() As String
        Return Path.Join(
        AppDomain.CurrentDomain.BaseDirectory,
        "SampleUserV2Data.json")

    End Function

    ''' <summary>
    '''  Gets the full path to the current user's settings PDF file.
    ''' </summary>
    ''' <returns>The full path to the user's settings PDF file.</returns>
    Friend Function GetUserPdfPath() As String
        Return Path.Join(GetSettingsDirectory(), $"{s_userName}Settings.pdf")
    End Function

    ''' <summary>
    '''  Gets the full path to the current user's settings JSON file.
    ''' </summary>
    ''' <returns>The full path to the user's settings JSON file.</returns>
    Friend Function GetUserSettingsPath() As String
        Return Path.Join(GetSettingsDirectory(), $"{s_userName}Settings.json")
    End Function

    ''' <summary>
    '''  Determines whether the specified file has not been modified for at least 30 days.
    ''' </summary>
    ''' <param name="path">The full path to the user settings file.</param>
    ''' <returns>
    '''  <see langword="True"/> if the file is stale;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <exception cref="ArgumentNullException">
    '''  Thrown if the path is null or whitespace.
    ''' </exception>
    ''' <remarks>
    '''  A file is considered stale if it has not been modified in the last 30 days.
    ''' </remarks>
    Friend Function IsFileStale(path As String) As Boolean
        If String.IsNullOrWhiteSpace(path) Then
            Throw New ArgumentNullException(
                paramName:=NameOf(path),
                message:="Path cannot be null or whitespace.")
        End If
        Return File.GetLastWriteTime(path) < Now - ThirtyDaysSpan
    End Function

    ''' <summary>
    '''  Moves all files matching the specified pattern from one directory to another.
    ''' </summary>
    ''' <param name="path">The source directory.</param>
    ''' <param name="currentDirectory">The destination directory.</param>
    ''' <param name="searchPattern">The search pattern for files to move.</param>
    ''' <exception cref="ArgumentNullException">
    '''  Thrown if the path or current directory is null or whitespace.
    ''' </exception>
    ''' <remarks>
    '''  This method moves all files matching the specified search pattern from the source directory
    '''  to the destination directory, which is typically the current working directory.
    ''' </remarks>"
    Friend Sub MoveFiles(path As String, currentDirectory As String, searchPattern As String)
        If String.IsNullOrWhiteSpace(path) Then
            Throw New ArgumentNullException(
                paramName:=NameOf(path),
                message:="Path cannot be null or whitespace.")
        End If
        If String.IsNullOrWhiteSpace(currentDirectory) Then
            Throw New ArgumentNullException(
                paramName:=NameOf(currentDirectory),
                message:="Current directory cannot be null or whitespace.")
        End If
        For Each sourceFileName As String In Directory.EnumerateFiles(path, searchPattern)
            Dim fileName As String = IO.Path.GetFileName(sourceFileName)
            File.Move(sourceFileName, destFileName:=IO.Path.Join(currentDirectory, fileName))
        Next
    End Sub

    ''' <summary>
    '''  The base name for the last download file, used to store the last downloaded data.
    ''' </summary>
    ''' <value>
    '''  The base name for the last download file, which is "SavedLastDownload".
    ''' </value>
    Public Function GetDownloadsDirectory() As String
        Dim folder As Environment.SpecialFolder = Environment.SpecialFolder.UserProfile
        Dim downloadsPath As String =
           IO.Path.Combine(Environment.GetFolderPath(folder), "Downloads")
        Return downloadsPath
    End Function

    ''' <summary>
    '''  The current users Documents directory.
    ''' </summary>
    Public Function GetMyDocuments() As String
        Return Environment.GetFolderPath(folder:=Environment.SpecialFolder.MyDocuments)
    End Function

    ''' <summary>
    '''  Gets the path to the settings directory where user settings are stored.
    ''' </summary>
    ''' <returns>The path to the settings directory.</returns>
    ''' <remarks>
    '''  This directory is typically located in the user's Documents folder
    '''  under "CareLink\Settings".
    ''' </remarks>
    Public Function GetSettingsDirectory() As String
        Dim folder As Environment.SpecialFolder = Environment.SpecialFolder.MyDocuments
        Return IO.Path.Combine(Environment.GetFolderPath(folder), "CareLink", "Settings")
    End Function

    ''' <summary>
    '''  Determines whether the specified file is read-only.
    ''' </summary>
    ''' <param name="path">The full path to the file.</param>
    ''' <returns>
    '''  <see langword="True"/> if the file is read-only; otherwise,
    '''  <see langword="False"/>.
    ''' </returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown if the file name is null or whitespace.
    ''' </exception>
    Public Function IsFileReadOnly(path As String) As Boolean
        If String.IsNullOrWhiteSpace(path) Then
            Throw New ArgumentException(
                message:=$"'{NameOf(path)}' cannot be null or whitespace.",
                paramName:=NameOf(path))
        End If

        Try
            If File.Exists(path) Then
                Return File.GetAttributes(path).HasFlag(flag:=FileAttributes.ReadOnly)
            End If
        Catch e As Exception
            Stop
            Debug.WriteLine(message:="Error: {0}", category:=e.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    '''  Updates the last write time of the specified file to the current UTC time.
    ''' </summary>
    ''' <param name="path">The full path to the file.</param>
    ''' <exception cref="ArgumentException">Thrown if the file name is null or whitespace.</exception>
    Public Sub TouchFile(path As String)
        If String.IsNullOrWhiteSpace(path) Then
            Throw New ArgumentException(
                message:=$"'{NameOf(path)}' cannot be null or whitespace.",
                paramName:=NameOf(path))
        End If
        If File.Exists(path) Then
            Using myFileStream As FileStream = File.Open(
                path,
                mode:=FileMode.OpenOrCreate,
                access:=FileAccess.ReadWrite,
                share:=FileShare.ReadWrite)

            End Using
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc:=Date.UtcNow)
        Else
            Throw New FileNotFoundException(
                message:="The specified file does not exist.",
                fileName:=path)
        End If
    End Sub

End Module
