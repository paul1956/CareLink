' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO

''' <summary>
'''  Provides helper methods and properties for file and directory I/O operations
'''  within the CareLink™ project.
''' </summary>
Public Module FileIoHelpers

    ''' <summary>
    '''  Determines whether any files matching the specified pattern
    '''  exist in the given directory.
    ''' </summary>
    ''' <param name="path">The directory to search.</param>
    ''' <param name="searchPattern">The search pattern (e.g., "*.json").</param>
    ''' <returns>
    '''  <see langword="True"/> if any matching files are found;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    ''' <exception cref="ArgumentNullException">
    '''  Thrown if the path is null or whitespace.
    ''' </exception>
    Friend Function AnyMatchingFiles(path As String, searchPattern As String) As Boolean
        If String.IsNullOrWhiteSpace(value:=path) Then
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
        Return GetUniqueDataFileName(baseName:=BaseDownloadName,
                                     cultureName:=CultureInfo.CurrentUICulture.Name,
                                     extension:="json",
                                     mustBeUnique:=False).WithPath
    End Function

    ''' <summary>
    '''  Gets the path to the directory where project data is
    '''  stored in the user's Documents folder.
    ''' </summary>
    Friend Function GetProjectDataDirectory() As String
        Return Path.Join(GetMyDocuments(), "CareLink")
    End Function

    ''' <summary>
    '''  Gets the full path to the sample user data JSON file for testing.
    ''' </summary>
    ''' <returns>
    '''  The full path to "SampleUserV2Data.json" in the application's base directory.
    ''' </returns>
    Friend Function GetTestDataPath() As String
        Return Path.Join(AppDomain.CurrentDomain.BaseDirectory, "SampleUserV2Data.json")
    End Function

    ''' <summary>
    '''  Gets the full path to the current user's settings PDF file.
    ''' </summary>
    ''' <returns>The full path to the user's settings PDF file.</returns>
    Friend Function GetUserPdfPath() As String
        Return Path.Join(GetSettingsDirectory(), $"{GetUserName()}Settings.pdf")
    End Function

    ''' <summary>
    '''  Gets the full path to the current user's settings JSON file.
    ''' </summary>
    ''' <returns>The full path to the user's settings JSON file.</returns>
    Friend Function GetUserSettingsPath() As String
        Return Path.Join(GetSettingsDirectory(), $"{GetUserName()}Settings.json")
    End Function

    ''' <summary>
    '''  The base name for the last download file, used to store the last downloaded data.
    ''' </summary>
    ''' <value>
    '''  The base name for the last download file, which is "SavedLastDownload".
    ''' </value>
    Public Function GetDownloadsDirectory() As String
        Return Path.Combine(Environment.GetFolderPath(folder:=Environment.SpecialFolder.UserProfile),
                            "Downloads")
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
        Return Path.Combine(GetProjectDataDirectory(), "Settings")
    End Function

    ''' <summary>
    '''  Determines whether the specified file is marked as read-only.
    ''' </summary>
    ''' <param name="path">Full path to the file to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the file exists and is read-only;
    '''  otherwise <see langword="False"/>.
    ''' </returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown if the file name is null or whitespace.
    ''' </exception>
    Public Function IsFileReadOnly(path As String) As Boolean
        If String.IsNullOrWhiteSpace(value:=path) Then
            Throw New ArgumentNullException(paramName:=NameOf(path))
        End If

        Try
            If File.Exists(path) Then
                Return File.GetAttributes(path).HasFlag(flag:=FileAttributes.ReadOnly)
            End If
        Catch e As Exception
            Debug.WriteLine(message:=$"IsFileReadOnly error: {e.Message}")
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Attempts to delete the specified file safely.
    ''' The method will normalize file attributes and retry on transient errors.
    ''' All exceptions are caught and logged instead of being propagated.
    ''' </summary>
    ''' <param name="path">Full path to the file to delete.</param>
    Public Sub SafeDeleteFile(path As String)
        Try
            If String.IsNullOrWhiteSpace(value:=path) Then Return
            If Not File.Exists(path) Then Return
            ' try to clear read-only attribute
            Try
                File.SetAttributes(path, fileAttributes:=FileAttributes.Normal)
            Catch
            End Try
            ' attempt delete with a couple of retries for transient locks
            Dim attempts As Integer = 0
            While attempts < 3
                Try
                    File.Delete(path)
                    Exit While
                Catch ex As IOException
                    attempts += 1
                    Threading.Thread.Sleep(millisecondsTimeout:=100)
                Catch ex As UnauthorizedAccessException
                    attempts += 1
                    Threading.Thread.Sleep(millisecondsTimeout:=100)
                Catch ex As Exception
                    Debug.WriteLine(message:=$"SafeDeleteFile failed: {ex.Message}")
                    Exit While
                End Try
            End While
        Catch ex As Exception
            Debug.WriteLine(message:=$"SafeDeleteFile outer: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    '''  Updates the last write time of the specified file to the current UTC time.
    ''' </summary>
    ''' <param name="path">The full path to the file.</param>
    ''' <exception cref="ArgumentNullException">
    '''  Thrown if the file name is null or whitespace.
    ''' </exception>
    Public Sub TouchFile(path As String)
        If String.IsNullOrWhiteSpace(value:=path) Then
            Throw New ArgumentNullException(paramName:=NameOf(path))
        End If
        If File.Exists(path) Then
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc:=Date.UtcNow)
        Else
            Throw New FileNotFoundException(message:="The specified file does not exist.", fileName:=path)
        End If
    End Sub

End Module
