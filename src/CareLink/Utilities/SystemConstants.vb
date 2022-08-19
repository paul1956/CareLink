' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Text.Json

Public Module SystemConstants

    Public Const ExceptionStartingString As String = "--- Start of Exception ---"

    Public Const ExceptionTerminatingString As String = "--- End of Exception ---"

    Public Const MilitaryTimeWithMinuteFormat As String = "HH:mm"

    Public Const MilitaryTimeWithoutMinuteFormat As String = "HH:mm"

    Public Const OwnerName As String = "paul1956"

    Public Const RepoDownloadName As String = "CareLinkLastDownload"

    Public Const RepoErrorReportName As String = "CareLinkErrorReport"

    Public Const RepoName As String = "CareLink"

    Public Const RepoSnapshotName As String = "CareLinkSnapshot"

    Public Const SavedCsvFileName As String = "CareLink.Csv"
    Public Const SavedTitle As String = "CareLink For Windows"

    Public Const StackTraceStartingString As String = "--- Start of stack trace ---"

    Public Const StackTraceTerminatingString As String = "--- End of stack trace from previous location ---"

    Public Const TwelveHourTimeWithMinuteFormat As String = "h:mm tt"

    Public Const TwelveHourTimeWithoutMinuteFormat As String = "h:mm tt"

    Public ReadOnly s_oneToNineteen As New List(Of String) From {
                    "zero", "one", "two", "three", "four", "five",
                "six", "seven", "eight", "nine", "ten", "eleven",
                "twelve", "thirteen", "fourteen", "fifteen",
                "sixteen", "seventeen", "eighteen", "nineteen"}

    Public ReadOnly s_unitsStrings As New Dictionary(Of String, String) From {
                {"MG_DL", "mg/dl"},
                {"MGDL", "mg/dl"},
                {"MMOL_L", "mmol/L"},
                {"MMOLL", "mmol/L"}
            }

    Public ReadOnly Trends As New Dictionary(Of String, String) From {
                {"DOWN", "↓"},
                {"DOWN_DOUBLE", "↓↓"},
                {"DOWN_TRIPLE", "↓↓↓"},
                {"UP", "↑"},
                {"UP_DOUBLE", "↑↑"},
                {"UP_TRIPLE", "↑↑↑"},
                {"NONE", "↔"}
            }

    Public Property GitHubCareLinkUrl As String = $"https://github.com/{OwnerName}/{RepoName}/"
    Public ReadOnly Property JsonFormattingOptions As New JsonSerializerOptions With {.WriteIndented = True}
    Public ReadOnly Property MyDocumentsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

#Region "All Culture Info"

    Private _CurrentDateCulture As CultureInfo
    Public ReadOnly s_cultureInfos As CultureInfo() = CultureInfo.GetCultures(CultureTypes.AllCultures)

    Public Property CurrentDateCulture As CultureInfo
        Get
            If _CurrentDateCulture Is Nothing Then
                Throw New ArgumentNullException(NameOf(_CurrentDateCulture))

            End If
            Return _CurrentDateCulture
        End Get
        Set
            _CurrentDateCulture = Value
        End Set
    End Property

    Public ReadOnly Property LastDownloadWithPath As String
        Get
            Return GetDataFileName(RepoDownloadName, CultureInfo.CurrentUICulture.Name, "json", False).withPath
        End Get
    End Property

    Public ReadOnly Property CurrentDataCulture As New CultureInfo("en-US")
    Public Property CurrentUICulture As CultureInfo = CultureInfo.CurrentUICulture

#End Region

End Module
