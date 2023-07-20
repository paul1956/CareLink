' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Speech.Recognition
Imports System.Speech.Synthesis
Imports System.Text

Friend Module SpeechRecognition
    Private s_speechErrorReported As Boolean = False
    Private s_speechWakeWordFound As Boolean = False
    Private s_sre As SpeechRecognitionEngine
    Private s_ss As New SpeechSynthesizer()
    Friend s_shuttingDown As Boolean = False
    Friend Property SpeechOutputSupport As Boolean = True

    Private Sub AnnounceSG(recognizedText As String)
        Dim sgName As String
        Select Case True
            Case recognizedText.Contains("bg")
                sgName = "bg"
            Case recognizedText.Contains("blood glucose")
                sgName = "blood glucose"
            Case recognizedText.Contains("blood sugar")
                sgName = "blood sugar"
            Case recognizedText.Contains("sg")
                sgName = "sg"
            Case Else
                Exit Sub
        End Select

        Dim sgMessage As String
        Dim trend As String = ""
        If IsNumeric(Form1.CurrentSgLabel.Text) Then
            sgMessage = $"current {sgName} is { Form1.CurrentSgLabel.Text}"
            Dim arrows As String = Form1.LabelTrendArrows.Text
            Dim arrowCount As Integer = 0
            Select Case True
                Case Form1.LabelTrendArrows.Text.Contains("↓"c)
                    arrowCount = arrows.Count("↓"c)
                    trend = $" and is trending down with { arrowCount} Arrow"
                Case Form1.LabelTrendArrows.Text.Contains("↑"c)
                    arrowCount = arrows.Count("↑"c)
                    trend = $" and is trending up with { arrowCount} Arrow"
                Case Else
                    trend = $" with no trend arrows"
            End Select
            If arrowCount > 1 Then
                trend &= "s"
            End If
        Else
            sgMessage = $"current {sgName} and trend are Unknown"
        End If
        PlayText($"{s_firstName}'s {sgMessage}{trend}", False)
    End Sub

    Private Sub sre_AudioSignalProblemOccurred(sender As Object, e As AudioSignalProblemOccurredEventArgs)
        If s_shuttingDown OrElse s_speechErrorReported Then Exit Sub
        Select Case e.AudioSignalProblem
            Case AudioSignalProblem.NoSignal
                If Not s_speechErrorReported Then
                    s_speechErrorReported = True
                    Dim details As New StringBuilder()
                    details.AppendLine("Audio signal problem information:")
                    details.AppendLine($"    Audio level:               {e.AudioLevel}")
                    details.AppendLine($"    Audio position:            {e.AudioPosition}")
                    details.AppendLine($"    Audio signal problem:      {e.AudioSignalProblem}")
                    details.AppendLine($"    Recognizer audio position: {e.RecognizerAudioPosition}")
                    details.AppendLine($"Do you want to continue getting this message?")
                    s_speechErrorReported = MsgBox(details.ToString, MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2, "Audio Error") <> MsgBoxResult.Yes
                End If
                Form1.StatusStripSpacerLeft.Text = $"Speech signal issue {e.AudioSignalProblem}"
            Case AudioSignalProblem.TooNoisy
                Form1.StatusStripSpacerLeft.Text = "There is too much noise to understand you"
            Case AudioSignalProblem.TooLoud
                Form1.StatusStripSpacerLeft.Text = "You are speaking too loud"
            Case AudioSignalProblem.TooFast
                Form1.StatusStripSpacerLeft.Text = "Please speak slower"
            Case AudioSignalProblem.TooSlow
                Form1.StatusStripSpacerLeft.Text = "Please speak faster"
            Case AudioSignalProblem.None, AudioSignalProblem.TooSoft
                Form1.StatusStripSpacerLeft.Text = "Listening"
        End Select

    End Sub

    Friend Sub InitializeSpeechRecognition()
        If SpeechOutputSupport AndAlso s_firstName = s_firstName Then Return
        SpeechOutputSupport = True
        Try
            s_speechWakeWordFound = False
            s_ss = New SpeechSynthesizer()
            s_ss.SetOutputToDefaultAudioDevice()

            Dim culture As New CultureInfo("en-us")
            s_sre = New SpeechRecognitionEngine(culture)
            s_sre.SetInputToDefaultAudioDevice()

            Dim gb_Attention As New GrammarBuilder With {.Culture = culture}
            gb_Attention.Append(s_careLinkLower)
            s_sre.LoadGrammarAsync(New Grammar(gb_Attention))

            'Dim gb_StartStop As New GrammarBuilder With {.Culture = culture}
            'gb_StartStop.Append("alerts")
            'gb_StartStop.Append(New Choices("off", "on"))
            's_sre.LoadGrammarAsync(New Grammar(gb_StartStop))

            Dim gb_what As New GrammarBuilder With {.Culture = culture}
            gb_what.Append("What")
            gb_what.Append(New Choices("can I say", "is my SG", "is my BG", "is my Blood Sugar", "is my Blood Glucose"))
            s_sre.LoadGrammarAsync(New Grammar(gb_what))

            Dim gb_tellMe As New GrammarBuilder With {.Culture = culture}
            gb_tellMe.Append("Tell me")
            gb_tellMe.Append($"{s_firstName}'s")
            gb_tellMe.Append(New Choices("SG", "BG", "Blood Sugar", "Blood Glucose"))
            s_sre.LoadGrammarAsync(New Grammar(gb_tellMe))

            'Dim gb_showTab As New GrammarBuilder()
            'gb_showTab.Append("Show")
            'Dim showChoices As New Choices()
            'For Each tab As TabPage In Form1.TabControlPage1.TabPages
            '    showChoices.Add(tab.Text.TrimEnd("."c))
            'Next
            'For Each tab As TabPage In Form1.TabControlPage2.TabPages
            '    showChoices.Add(tab.Text.TrimEnd("."c))
            'Next

            'gb_showTab.Append(showChoices)
            's_sre.LoadGrammarAsync(New Grammar(gb_showTab))

            Form1.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            PlayText($"Speech recognition enabled for {s_firstName}, for a list of commands say, {ProjectName} what can I say", True)
            Form1.StatusStripSpacerLeft.Text = "Listening"
            s_sre.RecognizeAsync(RecognizeMode.Multiple)
            AddHandler s_sre.SpeechRecognized, AddressOf sre_SpeechRecognized
            For i As Integer = 1 To 40 ' sleep 2 seconds
                Threading.Thread.Sleep(50)
            Next

            Form1.Cursor = Cursors.Default
            AddHandler s_sre.AudioSignalProblemOccurred, AddressOf sre_AudioSignalProblemOccurred
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Stop
        End Try

    End Sub

    Friend Sub PlayText(text As String, sync As Boolean)
        If Not File.Exists(GetPathToAudioAlertsDisabledFile) Then
            If sync OrElse Not s_speechErrorReported Then
                s_ss.Speak(text)
            Else
                s_ss.SpeakAsync(text)
            End If
        End If
    End Sub

    Friend Sub sre_SpeechRecognized(sender As Object, e As SpeechRecognizedEventArgs)
        If File.Exists(GetPathToAudioAlertsDisabledFile) Then
            Form1.StatusStripSpacerLeft.Text = ""
            Exit Sub
        End If
        Dim recognizedTextLower As String = e.Result.Text.ToLower
        Dim confidence As Single = e.Result.Confidence
        If confidence < 0.8 Then
            Debug.WriteLine($"Heard: {recognizedTextLower} with confidence({confidence})")
            Form1.StatusStripSpacerLeft.Text = $"Rejected: '{recognizedTextLower}', Listening"
            Exit Sub
        End If

        If recognizedTextLower.StartsWith(s_careLinkLower) Then
            If recognizedTextLower = s_careLinkLower Then
                Debug.WriteLine($"Recognized: Wake word {recognizedTextLower} with confidence({confidence})")
                Form1.StatusStripSpacerLeft.Text = $"Heard: '{s_careLinkLower}' waiting..."
                Application.DoEvents()
                s_speechWakeWordFound = True
                Exit Sub
            End If
        End If
        If s_speechWakeWordFound Then
            Debug.WriteLine($"Heard: {recognizedTextLower} with confidence({confidence})")
            If confidence < 0.8 Then
                Exit Sub
            End If
            s_speechWakeWordFound = False
            Form1.StatusStripSpacerLeft.Text = $"Heard: {recognizedTextLower}"
            Application.DoEvents()
            recognizedTextLower = recognizedTextLower.Replace(s_careLinkLower, "").TrimEnd
            Select Case True
                'Case recognizedTextLower = "alerts on"
                '    Debug.WriteLine("Audible alerts are now ON")
                '    PlayText("Audible alerts are now ON")

                'Case recognizedTextLower = "alerts off"
                '    Debug.WriteLine("Audible alerts are now OFF")
                '    PlayText("Audible alerts are now Off")

                Case recognizedTextLower.StartsWith("what is my", StringComparison.CurrentCultureIgnoreCase)
                    AnnounceSG(recognizedTextLower)

                Case recognizedTextLower.StartsWith("tell me", StringComparison.CurrentCultureIgnoreCase)
                    If Not recognizedTextLower.Contains(s_firstName.ToLower) Then
                        Return
                    End If
                    AnnounceSG(recognizedTextLower)

                Case recognizedTextLower = "what can I say"
                    Dim prompt As New StringBuilder
                    prompt.AppendLine($"{ProjectName}: All commands start with this, a pause is allowed after saying {ProjectName}.")
                    prompt.AppendLine($"What can I say: This message will be displayed")
                    prompt.AppendLine()
                    prompt.AppendLine($"What is my SG/BG/Blood Glucose/Blood Sugar: Your current Sensor Glucose will be spoken")
                    prompt.AppendLine($"Tell me name's SG/BG/Blood Glucose/Blood Sugar: use when you support more than 1 user")
                    prompt.AppendLine($"     Example ""Tell me John's Sensor Glucose""")
                    'prompt.AppendLine($"Alerts On: Enables audio Alerts")
                    'prompt.AppendLine($"Alerts Off: Disables audio Alerts")
                    'prompt.AppendLine($"Show [any tab name]: Will make that tab have focus")
                    'prompt.AppendLine($"     Example ""Show Treatment Details""")
                    MsgBox(prompt.ToString, MsgBoxStyle.OkOnly, "Speech Help")

                    'Case recognizedTextLower.StartsWith("show", StringComparison.CurrentCultureIgnoreCase)
                    '    Dim tabText As String = recognizedTextLower.Substring("show ".Length).ToLower.TrimEnd("."c)
                    '    For Each tab As TabPage In Form1.TabControlPage1.TabPages
                    '        If tab.Text.ToLower.TrimEnd("."c) = tabText Then
                    '            Form1.TabControlPage1.Visible = True
                    '            Form1.TabControlPage1.SelectedTab = tab
                    '            Exit Select
                    '        End If
                    '    Next
                    '    For Each tab As TabPage In Form1.TabControlPage2.TabPages
                    '        If tab.Text.ToLower.TrimEnd("."c) = tabText Then
                    '            Form1.TabControlPage1.Visible = False
                    '            Form1.TabControlPage2.SelectedTab = tab
                    '            Exit Select
                    '        End If
                    '    Next
                Case Else
                    Stop
            End Select
            s_speechWakeWordFound = False
        End If
        Form1.StatusStripSpacerLeft.Text = "Listening"
    End Sub

End Module
