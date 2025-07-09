' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Speech.Recognition
Imports System.Speech.Synthesis
Imports System.Text

''' <summary>
'''  Provides speech recognition and synthesis support for CareLink.
'''  Handles initialization, recognition events, audio alerts, and user feedback.
''' </summary>
Friend Module SpeechSupport

    ''' <summary>
    '''  Stores the last message spoken by the synthesizer.
    ''' </summary>
    Private s_lastSpokenMessage As String

    ''' <summary>
    '''  Represents the current speech prompt being spoken asynchronously.
    ''' </summary>
    Private s_promptBusy As Prompt = Nothing

    ''' <summary>
    '''  Indicates whether a speech error has been reported to the user.
    ''' </summary>
    Private s_speechErrorReported As Boolean = False

    ''' <summary>
    '''  The speech recognition engine instance.
    ''' </summary>
    Private s_speechRecognitionEngine As SpeechRecognitionEngine

    ''' <summary>
    '''  The speech synthesizer instance.
    ''' </summary>
    Private s_speechSynthesizer As SpeechSynthesizer

    ''' <summary>
    '''  Stores the name of the user for whom speech recognition is enabled.
    ''' </summary>
    Private s_speechUserName As String = ""

    ''' <summary>
    '''  Indicates whether the speech wake word has been detected.
    ''' </summary>
    Private s_speechWakeWordFound As Boolean = False

    ''' <summary>
    '''  Stores the current status text for the speech status strip.
    ''' </summary>
    Private s_statusStripSpeechText As String = ""

    ''' <summary>
    '''  Stores the time of the last alert spoken.
    ''' </summary>
    Private s_timeOfLastAlert As Date

    ''' <summary>
    '''  Indicates whether the application is shutting down.
    ''' </summary>
    Friend s_shuttingDown As Boolean = False

    ''' <summary>
    '''  Announces the current sensor glucose (SG) value using speech synthesis.
    ''' </summary>
    ''' <param name="recognizedText">The recognized speech text to determine which value to announce.</param>
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

        Dim currentSgStr As String = Form1.CurrentSgLabel.Text
        If IsNumeric(currentSgStr) Then
            Dim sgMessage As String = $"current {sgName} is {currentSgStr}"
            PlayText(textToSpeak:=$"{PatientData.FirstName}'s {sgMessage}{GetTrendText()}")
        Else
            PlayText(textToSpeak:=$"{PatientData.FirstName}'s current {sgName} and trend are Unknown")
        End If
    End Sub

    ''' <summary>
    '''  Handles audio signal problems detected by the speech recognition engine.
    '''  Provides user feedback and error reporting.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing audio signal problem details.</param>
    <DebuggerStepThrough()>
    Private Sub AudioSignalProblemOccurred(sender As Object, e As AudioSignalProblemOccurredEventArgs)
        If s_shuttingDown OrElse s_speechErrorReported OrElse s_speechRecognitionEngine Is Nothing Then Exit Sub
        Dim errorMsg As String = "Listening"
        Select Case e.AudioSignalProblem
            Case AudioSignalProblem.NoSignal
                If Not s_speechErrorReported Then
                    s_speechErrorReported = True
                    Dim details As New StringBuilder()
                    details.AppendLine("Details:")
                    details.AppendLine($"    Audio level:               {e.AudioLevel}")
                    details.AppendLine($"    Audio position:            {e.AudioPosition}")
                    details.AppendLine($"    Audio signal problem:      {e.AudioSignalProblem}")
                    details.AppendLine($"    Recognizer audio position: {e.RecognizerAudioPosition}")

                    Dim page As New TaskDialogPage
                    MsgBox(
                        heading:="Audio signal problem",
                        text:=details.ToString,
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                        title:="Audio Error",
                        autoCloseTimeOutSeconds:=15,
                        page)
                    s_speechErrorReported = page.Verification.Checked
                End If
                errorMsg = $"Speech: signal issue {e.AudioSignalProblem}"
            Case AudioSignalProblem.TooNoisy
                errorMsg = "Speech: Too much noise to understand you"
            Case AudioSignalProblem.TooLoud
                errorMsg = "Speech: You are speaking too loud"
            Case AudioSignalProblem.TooFast
                errorMsg = "Speech: Please speak slower"
            Case AudioSignalProblem.TooSlow
                errorMsg = "Speech: Please speak faster"
            Case AudioSignalProblem.TooSoft
                errorMsg = "Speech: Please speak louder"
            Case Else
        End Select
        If Not Form1.StatusStripSpeech.Text.StartsWith("Speech:") Then
            Form1.StatusStripSpeech.Text = errorMsg
        End If

    End Sub

    ''' <summary>
    '''  Gets a description of the current trend based on the trend arrows.
    ''' </summary>
    ''' <returns>A string describing the trend direction and arrow count.</returns>
    Private Function GetTrendText() As String
        Dim arrows As String = Form1.TrendArrowsLabel.Text
        Dim arrowCount As Integer
        Select Case True
            Case arrows.Contains("↓"c)
                arrowCount = arrows.Count("↓"c)
                Return $" and is trending down with {arrowCount} Arrow{If(arrowCount > 1, "s", "")}"
            Case arrows.Contains("↑"c)
                arrowCount = arrows.Count("↑"c)
                Return $" and is trending up with {arrowCount} Arrow{If(arrowCount > 1, "s", "")}"
            Case Else
                Return " with no trend arrows"
        End Select
    End Function

    ''' <summary>
    '''  Handles the SpeechRecognized event, processes recognized speech, and triggers appropriate actions.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing recognition results.</param>
    Private Sub SpeechRecognized(sender As Object, e As SpeechRecognizedEventArgs)
        If My.Settings.SystemSpeechRecognitionThreshold >= 1 Then
            Form1.StatusStripSpeech.Text = ""
            Exit Sub
        End If

        Dim message As String = ""
        Dim recognizedTextLower As String = e.Result.Text.ToLower
        Dim confidence As Single = e.Result.Confidence.RoundSingle(2, False)
        If confidence < 0.8 Then
            message = $"Rejected: {recognizedTextLower} with confidence {confidence}%"
            Debug.WriteLine(message)
            Form1.StatusStripSpeech.Text = message
            Exit Sub
        End If

        If recognizedTextLower.StartsWith("carelink") Then
            If confidence >= My.Settings.SystemSpeechRecognitionThreshold Then
                s_speechWakeWordFound = True
                message = $"Heard: Wake word {recognizedTextLower} with confidence {confidence}%), waiting.."
                Debug.WriteLine(message)
                Form1.StatusStripSpeech.Text = message
                Application.DoEvents()
                If recognizedTextLower = "carelink" Then
                    Exit Sub
                End If
                recognizedTextLower = recognizedTextLower.Replace("carelink", "").TrimEnd
            Else
                message = $"Rejected: {recognizedTextLower} with confidence {confidence}%"
                Debug.WriteLine(message)
                Form1.StatusStripSpeech.Text = message
                Exit Sub
            End If
        End If

        message = $"Heard: {e.Result.Text.ToLower} with confidence {confidence}%."
        Debug.WriteLine(message)
        Form1.StatusStripSpeech.Text = message
        Application.DoEvents()
        If s_speechWakeWordFound Then
            s_speechWakeWordFound = False
            If confidence < My.Settings.SystemSpeechRecognitionThreshold - 0.05 Then
                message = $"Rejected: {recognizedTextLower} with confidence {confidence}%"
                Debug.WriteLine(message)
                Form1.StatusStripSpeech.Text = message
                Exit Sub
            End If

            Select Case True
                Case recognizedTextLower.StartsWith("what is my", StringComparison.CurrentCultureIgnoreCase)
                    Form1.StatusStripSpeech.Text = message
                    AnnounceSG(recognizedTextLower)

                Case recognizedTextLower.StartsWith("tell me", StringComparison.CurrentCultureIgnoreCase)
                    If Not recognizedTextLower.Contains(PatientData.FirstName, StringComparison.CurrentCultureIgnoreCase) Then
                        Return
                    End If
                    Form1.StatusStripSpeech.Text = message
                    AnnounceSG(recognizedTextLower)

                Case recognizedTextLower = "what can I say"
                    If My.Settings.SystemSpeechHelpShown Then
                        s_speechWakeWordFound = False
                        Exit Select
                    End If
                    Dim text As New StringBuilder
                    text.AppendLine("CareLink:")
                    text.AppendLine("    All commands start with this")
                    text.AppendLine("    A pause is allowed after saying CareLink.")
                    text.AppendLine()
                    text.AppendLine("What can I say:")
                    text.AppendLine("    This message will be displayed")
                    text.AppendLine()
                    text.AppendLine("What is my SG/BG/Blood Glucose/Blood Sugar:")
                    text.AppendLine($"    Your {CurrentSgMsg} will be spoken")
                    text.AppendLine()
                    text.AppendLine("Tell me name's SG/BG/Blood Glucose/Blood Sugar:")
                    text.AppendLine("    Used when you support more than 1 user")
                    text.AppendLine("    Example ""Tell me John's Sensor Glucose""")
                    Dim page As New TaskDialogPage
                    MsgBox(
                        heading:="",
                        text:=text.ToString,
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                        title:="Speech Recognition Help",
                        autoCloseTimeOutSeconds:=30,
                        page)
                    My.Settings.SystemSpeechHelpShown = page.Verification.Checked
                Case Else
                    Stop
            End Select
        End If
        If Not Form1.StatusStripSpeech.Text.Contains("too soon") Then
            If message.Length > 0 Then message &= ", "
            Form1.StatusStripSpeech.Text = $"{message}Listening..."
        End If
    End Sub

    ''' <summary>
    '''  Cancels and disposes the current speech recognition engine, removes event handlers, and updates UI state.
    ''' </summary>
    Friend Sub CancelSpeechRecognition()
        If s_speechRecognitionEngine IsNot Nothing Then
            RemoveHandler s_speechRecognitionEngine.AudioSignalProblemOccurred, AddressOf AudioSignalProblemOccurred
            RemoveHandler s_speechRecognitionEngine.SpeechRecognized, AddressOf SpeechRecognized
            s_speechRecognitionEngine.RecognizeAsyncCancel()
            s_speechRecognitionEngine.Dispose()
            s_speechRecognitionEngine = Nothing
            s_speechUserName = ""
            Form1.StatusStripSpeech.Text = ""
            Form1.MenuOptionsSpeechRecognitionEnabled.Checked = False
        End If
    End Sub

    ''' <summary>
    '''  Initializes the speech synthesizer for audio alerts.
    ''' </summary>
    Friend Sub InitializeAudioAlerts()
        If s_speechSynthesizer Is Nothing Then
            s_speechSynthesizer = New SpeechSynthesizer()
            s_speechSynthesizer.SetOutputToDefaultAudioDevice()
        End If

    End Sub

    ''' <summary>
    '''  Initializes and configures the speech recognition engine, loads grammars, and starts recognition.
    ''' </summary>
    Friend Sub InitializeSpeechRecognition()
        Dim oldUserName As String = s_speechUserName
        If s_speechUserName = PatientData.FirstName AndAlso s_speechRecognitionEngine IsNot Nothing Then
            Exit Sub
        End If
        CancelSpeechRecognition()

        Try
            s_speechWakeWordFound = False

            Dim culture As New CultureInfo("en-us")
            s_speechRecognitionEngine = New SpeechRecognitionEngine(culture)
            s_speechRecognitionEngine.SetInputToDefaultAudioDevice()

            Dim gb_Attention As New GrammarBuilder With {.Culture = culture}
            gb_Attention.Append("carelink")
            s_speechRecognitionEngine.LoadGrammarAsync(New Grammar(gb_Attention))

            'Dim gb_StartStop As New GrammarBuilder With {.Culture = culture}
            'gb_StartStop.Append("alerts")
            'gb_StartStop.Append(New Choices("off", "on"))
            's_sre.LoadGrammarAsync(New Grammar(gb_StartStop))

            Dim gb_what As New GrammarBuilder With {.Culture = culture}
            gb_what.Append("What")
            gb_what.Append(New Choices("can I say", "is my SG", "is my BG", "is my Blood Sugar", "is my Blood Glucose"))
            s_speechRecognitionEngine.LoadGrammarAsync(New Grammar(gb_what))

            Dim gb_tellMe As New GrammarBuilder With {.Culture = culture}
            gb_tellMe.Append("Tell me")
            gb_tellMe.Append($"{PatientData.FirstName}'s")
            gb_tellMe.Append(New Choices("SG", "BG", "Blood Sugar", "Blood Glucose"))
            s_speechRecognitionEngine.LoadGrammarAsync(New Grammar(gb_tellMe))

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
            If String.IsNullOrWhiteSpace(s_speechUserName) Then
                Dim textToSpeak As String
                textToSpeak = $"Speech recognition enabled for {PatientData.FirstName}"

                If String.IsNullOrWhiteSpace(oldUserName) Then
                    textToSpeak &= " for a list of commands say, CareLink what can I say"
                End If

                PlayText(textToSpeak)
            End If
            s_speechUserName = PatientData.FirstName
            Form1.StatusStripSpeech.Text = "Listening"
            s_speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple)
            AddHandler s_speechRecognitionEngine.SpeechRecognized, AddressOf SpeechRecognized

            Form1.Cursor = Cursors.Default
            AddHandler s_speechRecognitionEngine.AudioSignalProblemOccurred, AddressOf AudioSignalProblemOccurred
            Form1.MenuOptionsSpeechRecognitionEnabled.Checked = True
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Stop
        End Try

    End Sub

    ''' <summary>
    '''  Speaks the specified text using the speech synthesizer, with duplicate and timing checks.
    ''' </summary>
    ''' <param name="textToSpeak">The text to be spoken.</param>
    Friend Sub PlayText(textToSpeak As String)
        If Not My.Settings.SystemAudioAlertsEnabled Then
            Form1.StatusStripSpeech.Text = "Audio Alerts Disabled"
            Return
        End If
        If s_lastSpokenMessage = textToSpeak AndAlso DateDiff(DateInterval.Minute, Now, s_timeOfLastAlert) < ThirtySecondInMilliseconds Then
            Form1.StatusStripSpeech.Text = $"Rejected: '{textToSpeak}' too soon, Listening"
            s_statusStripSpeechText = textToSpeak
        End If
        If Form1.StatusStripSpeech.Text.Contains("too soon") Then
            Form1.StatusStripSpeech.Text = "Listening"
        End If
        s_timeOfLastAlert = Now
        s_lastSpokenMessage = textToSpeak
        If Not s_speechErrorReported Then
            If s_speechSynthesizer Is Nothing Then
                InitializeAudioAlerts()
            End If
        End If
        While s_promptBusy IsNot Nothing AndAlso Not s_promptBusy.IsCompleted
            Threading.Thread.Sleep(millisecondsTimeout:=10)
        End While
        s_promptBusy = s_speechSynthesizer.SpeakAsync(textToSpeak)
    End Sub

End Module
