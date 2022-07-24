' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Text.Json

Public Module SystemConstants

#Region "Must Be On Top"

    Public ReadOnly Property CurrentDataCulture As CultureInfo = New CultureInfo("en-US")
    Public Property CurrentUICulture As CultureInfo = CultureInfo.CurrentUICulture
    Public ReadOnly Property MyDocumentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    Public ReadOnly Property OwnerName As String = "paul1956"
    Public ReadOnly Property RepoName As String = "CareLink"

#End Region

    Public ReadOnly VersionSearchKey As String = $"<a href=""/{OwnerName}/{RepoName}/releases/tag/v"
    Public ReadOnly Property CareLinkLastDownloadDocPath As String = Path.Combine(MyDocumentsPath, $"{RepoName}LastDownload({CurrentUICulture.Name}).json")
    Public ReadOnly Property ErrorReportName As String = $"{RepoName}ErrorReport"
    Public ReadOnly Property ExceptionStartingString As String = "--- Start of Exception ---"
    Public ReadOnly Property ExceptionTerminatingString As String = "--- End of Exception ---"
    Public Property GitHubCareLinkUrl As String = $"https://github.com/{OwnerName}/{RepoName}/"
    Public ReadOnly Property JsonFormattingOptions As New JsonSerializerOptions With {.WriteIndented = True}
    Public ReadOnly Property MyDocumentsCareLinkSnapshotDocPath As String = Path.Combine(MyDocumentsPath, $"{RepoName}Snapshot({CurrentUICulture.Name}).json")
    Public ReadOnly Property StackTraceStartingString As String = "--- Start of stack trace ---"
    Public ReadOnly Property StackTraceTerminatingString As String = "--- End of stack trace from previous location ---"
End Module
