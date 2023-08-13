' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports Spire.Pdf.Utilities

Friend Module Form1LoginHelpers
    Public ReadOnly Property LoginDialog As New LoginForm1

    Friend Function DoOptionalLoginAndUpdateData(UpdateAllTabs As Boolean, fileToLoad As FileToLoadOptions) As Boolean
        Dim serverTimerEnabled As Boolean = Form1.ServerUpdateTimer.Enabled
        Form1.ServerUpdateTimer.Stop()
        Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(Form1.ServerUpdateTimer)} stopped at {Now.ToLongTimeString}")
        s_listOfAutoBasalDeliveryMarkers.Clear()
        s_listOfManualBasal.Clear()
        Dim fromFile As Boolean
        Select Case fileToLoad
            Case FileToLoadOptions.LastSaved
                Form1.Text = $"{SavedTitle} Using Last Saved Data"
                CurrentDateCulture = GetPathToLastDownloadFile().ExtractCultureFromFileName(SavedLastDownloadBaseName)
                RecentData = Loads(File.ReadAllText(GetPathToLastDownloadFile()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetPathToLastDownloadFile())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(GetPathToTestSettingsFile())
                fromFile = True
            Case FileToLoadOptions.TestData
                Form1.Text = $"{SavedTitle} Using Test Data from 'SampleUserData.json'"
                CurrentDateCulture = New CultureInfo("en-US")
                RecentData = Loads(File.ReadAllText(GetPathToTestData()))
                Form1.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                Dim fileDate As Date = File.GetLastWriteTime(GetPathToTestData())
                SetLastUpdateTime(fileDate.ToShortDateTimeString, "from file", False, fileDate.IsDaylightSavingTime)
                SetUpCareLinkUser(GetPathToTestSettingsFile)
                fromFile = True
            Case FileToLoadOptions.Login
                Form1.Text = SavedTitle
                Do While True
                    Dim result As DialogResult = LoginDialog.ShowDialog
                    Select Case result
                        Case DialogResult.OK
                            Exit Do
                        Case DialogResult.Cancel
                            If serverTimerEnabled Then
                                Form1.ServerUpdateTimer.Start()
                            End If
                            Return False
                        Case DialogResult.Retry
                    End Select
                Loop

                If Form1.Client Is Nothing OrElse Not Form1.Client.LoggedIn Then
                    Form1.ServerUpdateTimer.Interval = CInt(s_5MinutesInMilliseconds)
                    Form1.ServerUpdateTimer.Start()
                    Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(Form1.ServerUpdateTimer)} started at {Now.ToLongTimeString}")
                    If NetworkUnavailable() Then
                        ReportLoginStatus(Form1.LoginStatus)
                        Return False
                    End If

                    SetLastUpdateTime("Last Update time is unknown!", "", True, Nothing)
                    Return False
                End If

                Dim userSettingsPath As String = GetPathToUserSettingsFile(My.Settings.CareLinkUserName)
                RecentData = Form1.Client.GetRecentData()
                SetUpCareLinkUser(userSettingsPath)
                Form1.ServerUpdateTimer.Interval = CInt(s_1MinutesInMilliseconds)
                Form1.ServerUpdateTimer.Start()
                Debug.Print($"In {NameOf(DoOptionalLoginAndUpdateData)}, {NameOf(Form1.ServerUpdateTimer)} started at {Now.ToLongTimeString}")

                If NetworkUnavailable() Then
                    ReportLoginStatus(Form1.LoginStatus)
                    Return False
                End If

                ReportLoginStatus(Form1.LoginStatus, RecentDataEmpty, Form1.Client.GetLastErrorMessage)
                Form1.MenuShowMiniDisplay.Visible = True
                fromFile = False
        End Select

        If Form1.Client IsNot Nothing Then
            Form1.Client.SessionProfile?.SetInsulinType(CurrentUser.InsulinTypeName)
            With Form1.DgvSessionProfile
                .InitializeDgv()
                .DataSource = Form1.Client.SessionProfile.ToDataSource
            End With
        End If

        Form1.PumpAITLabel.Text = CurrentUser.GetPumpAitString
        Form1.InsulinTypeLabel.Text = CurrentUser.InsulinTypeName
        FinishInitialization()
        If UpdateAllTabs Then
            Form1.UpdateAllTabPages(fromFile)
        End If
        Return True
    End Function

    Friend Sub FinishInitialization()
        Form1.Cursor = Cursors.Default
        Application.DoEvents()

        Form1.InitializeSummaryTabCharts()
        Form1.InitializeActiveInsulinTabChart()
        Form1.InitializeTimeInRangeArea()

        ProgramInitialized = True
    End Sub

    <Extension>
    Friend Sub SetLastUpdateTime(msg As String, suffixMessage As String, highLight As Boolean, isDaylightSavingTime? As Boolean)
        Dim foreColor As Color
        Dim backColor As Color

        If highLight = True Then
            foreColor = GetGraphLineColor("High Limit")
            backColor = foreColor.GetContrastingColor()
        Else
            foreColor = SystemColors.ControlText
            backColor = SystemColors.Control
        End If

        With Form1.LastUpdateTimeToolStripStatusLabel
            If Not String.IsNullOrWhiteSpace(msg) Then
                .Text = $"{msg}"
            End If
            .ForeColor = foreColor
            .BackColor = backColor
        End With

        With Form1.TimeZoneToolStripStatusLabel
            If isDaylightSavingTime Is Nothing Then
                .Text = ""
            Else
                Dim timeZoneName As String = Nothing
                If RecentData?.TryGetValue(NameOf(ItemIndexes.clientTimeZoneName), timeZoneName) Then
                    Dim timeZoneInfo As TimeZoneInfo = CalculateTimeZone(timeZoneName)
                    .Text = $"{If(isDaylightSavingTime, timeZoneInfo.DaylightName, timeZoneInfo.StandardName)} {suffixMessage}".Trim
                Else
                    .Text = ""
                End If
            End If
            .ForeColor = foreColor
            .BackColor = backColor
        End With

    End Sub

    Friend Sub SetUpCareLinkUser(userSettingsPath As String)
        Dim ait As Single = 2
        Dim carbRatios As List(Of CarbRatioRecord) = Nothing
        If Path.Exists(userSettingsPath) Then
            Dim contents As String = File.ReadAllText(userSettingsPath)
            CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, JsonFormattingOptions)
        Else
            If MsgBox($"Would you like to upload a {ProjectName}™ Device Settings PDF File", "", MsgBoxStyle.YesNo, $"Use {ProjectName}™ Settings File") = MsgBoxResult.Yes Then
                Dim openFileDialog1 As New OpenFileDialog With {
                            .AddToRecent = True,
                            .CheckFileExists = True,
                            .CheckPathExists = True,
                            .Filter = $"Settings file (*.pdf)|*.pdf",
                            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                            .Multiselect = False,
                            .ReadOnlyChecked = True,
                            .RestoreDirectory = True,
                            .SupportMultiDottedExtensions = False,
                            .Title = $"Select downloaded {ProjectName}™ Settings file.",
                            .ValidateNames = True
                        }
                If openFileDialog1.ShowDialog() = DialogResult.OK Then
                    Dim tables As List(Of PdfTable) = GetTableList(openFileDialog1.FileName, 0)
                    Dim aitTableLines As List(Of String) = ExtractPdfTableLines(tables, "Bolus Wizard On")
                    For Each e As IndexClass(Of String) In aitTableLines.WithIndex
                        If e.IsFirst Then Continue For
                        If String.IsNullOrWhiteSpace(e.Value) Then Exit For
                        Dim lineParts() As String = e.Value.Split(" ")
                        If lineParts(0) = "(h:mm)" Then
                            ait = s_aitLengths(lineParts(1))
                            Exit For
                        End If
                    Next
                    Dim carbRatioLines As List(Of String) = ExtractPdfTableLines(tables, "Time Ratio")
                    For Each e As IndexClass(Of String) In carbRatioLines.WithIndex
                        If e.IsFirst Then Continue For
                        If String.IsNullOrWhiteSpace(e.Value) Then Exit For
                        Dim lineParts() As String = e.Value.Split(" ")
                        Dim startTimeString As String = lineParts(0)
                        Dim item As New CarbRatioRecord With {
                            .StartTime = TimeOnly.Parse(lineParts(0)),
                            .CarbRatio = lineParts(1).ParseSingle(2),
                            .EndTime = If(e.IsLast OrElse carbRatioLines(e.Index + 1).Trim.Length = 0,
                                          TimeOnly.Parse(s_midnight),
                                          TimeOnly.Parse(carbRatioLines(e.Index + 1).Split(" ")(0))
                                         )
                        }
                        carbRatios.Add(item)
                    Next
                    Stop
                End If

            End If
            CurrentUser = New CurrentUserRecord(My.Settings.CareLinkUserName, If(Not Is770G(), CheckState.Checked, CheckState.Indeterminate))
            Dim f As New InitializeDialog(CurrentUser, ait, carbRatios)
            f.ShowDialog()
            CurrentUser = f.CurrentUser
        End If

    End Sub

End Module
