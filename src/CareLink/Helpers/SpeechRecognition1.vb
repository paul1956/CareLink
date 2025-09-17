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
    ''' <param name="recognizedText">
    '''  The recognized speech text to determine which value to announce.
    ''' </param>
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

        Dim firstName As String = $"{PatientData.FirstName}'s"
        Dim currentSgStr As String = Form1.CurrentSgLabel.Text
        Dim textToSpeak As String
        If IsNumeric(Expression:=currentSgStr) Then
            Dim sgMessage As String = $"current {sgName} is {currentSgStr}"
            Dim arrows As String = Form1.TrendArrowsLabel.Text
            textToSpeak = $"{firstName} {sgMessage}{GetTrendText(arrows)}"
        Else
            textToSpeak = $"{firstName} current {sgName} and trend are Unknown"
        End If
        PlayText(textToSpeak)
    End Sub

    ''' <summary>
    '''  Handles audio signal problems detected by the speech recognition engine.
    '''  Provides user feedback and error reporting.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing audio signal problem details.</param>
    <DebuggerStepThrough()>
    Private Sub AudioSignalProblemOccurred(
        sender As Object,
        e As AudioSignalProblemOccurredEventArgs)

        If s_shuttingDown OrElse
           s_speechErrorReported OrElse
           s_speechRecognitionEngine Is Nothing Then

            Exit Sub
        End If
        Dim errorMsg As String = "Listening"
        Select Case e.AudioSignalProblem
            Case AudioSignalProblem.NoSignal
                If Not s_speechErrorReported Then
                    s_speechErrorReported = True
                    Dim details As New StringBuilder()
                    details.AppendLine("Details:")
                    Dim value As String = $"    Audio level:               {e.AudioLevel}"
                    details.AppendLine(value)
                    value = $"    Audio signal problem:      {e.AudioSignalProblem}"
                    details.AppendLine(value)
                    value = $"    Audio position:            {e.AudioPosition}"
                    details.AppendLine(value)
                    value = $"    Audio signal problem:      {e.AudioSignalProblem}"
                    details.AppendLine(value)
                    value = $"    Recognizer audio position: {e.RecognizerAudioPosition}"
                    details.AppendLine(value)

                    Dim page As New TaskDialogPage
                    MsgBox(
                        heading:="Audio signal problem",
                        prompt:=details.ToString,
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                        title:="Audio Error",
                        autoCloseTimeOut:=15,
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
    Friend Function GetTrendText(arrows As String) As String
        Dim arrowCount As Integer
        Const unit As String = "arrow"
        Dim prefix As String
        Select Case True
            Case arrows.Contains(value:="↓"c)
                arrowCount = arrows.Count(c:="↓"c)
                prefix = $" and is trending down with {arrowCount} "
                Return arrowCount.ToUnits(unit, prefix, includeValue:=False)
            Case arrows.Contains(value:="↑"c)
                arrowCount = arrows.Count(c:="↑"c)
                prefix = $" and is trending up with {arrowCount} "
                Return arrowCount.ToUnits(unit, prefix, includeValue:=False)
            Case Else
                Return " with no trend arrows"
        End Select
    End Function

    ''' <summary>
    '''  Handles the SpeechRecognized event, processes recognized speech,
    '''  and triggers appropriate actions.
    ''' </summary>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">Event arguments containing recognition results.</param>
    Private Sub SpeechRecognized(sender As Object, e As SpeechRecognizedEventArgs)
        If My.Settings.SystemSpeechRecognitionThreshold >= 1 Then
            Form1.StatusStripSpeech.Text = ""
            Exit Sub
        End If

        Dim message As String = ""
        Dim recognizedText As String = e.Result.Text.ToLower
        Dim confidence As Single = e.Result.Confidence.RoundToSingle(digits:=2)
        If confidence < 0.8 Then
            message = $"Rejected: {recognizedText} with confidence {confidence}%"
            Debug.WriteLine(message)
            Form1.StatusStripSpeech.Text = message
            Exit Sub
        End If

        If recognizedText.StartsWith("carelink") Then
            If confidence >= My.Settings.SystemSpeechRecognitionThreshold Then
                s_speechWakeWordFound = True
                message =
                    $"Heard: Wake word {recognizedText} with confidence " &
                    $"{confidence}%), waiting.."
                Debug.WriteLine(message)
                Form1.StatusStripSpeech.Text = message
                Application.DoEvents()
                If recognizedText = "carelink" Then
                    Exit Sub
                End If
                recognizedText =
                    recognizedText.Replace(oldValue:="carelink", newValue:="").TrimEnd
            Else
                message = $"Rejected: {recognizedText} with confidence {confidence}%"
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
                message = $"Rejected: {recognizedText} with confidence {confidence}%"
                Debug.WriteLine(message)
                Form1.StatusStripSpeech.Text = message
                Exit Sub
            End If

            Select Case True
                Case recognizedText.StartsWithNoCase(value:="what is my")
                    Form1.StatusStripSpeech.Text = message
                    AnnounceSG(recognizedText)

                Case recognizedText.StartsWithNoCase(value:="tell me")
                    If Not recognizedText.ContainsNoCase(value:=PatientData.FirstName) Then
                        Return
                    End If
                    Form1.StatusStripSpeech.Text = message
                    AnnounceSG(recognizedText)

                Case recognizedText = "what can I say"
                    If My.Settings.SystemSpeechHelpShown Then
                        s_speechWakeWordFound = False
                        Exit Select
                    End If
                    Dim sb As New StringBuilder
                    sb.AppendLine("CareLink:")
                    sb.AppendLine("    All commands start with this")
                    sb.AppendLine("    A pause is allowed after saying CareLink.")
                    sb.AppendLine()
                    sb.AppendLine("What can I say:")
                    sb.AppendLine("    This message will be displayed")
                    sb.AppendLine()
                    sb.AppendLine("What is my SG/BG/Blood Glucose/Blood Sugar:")
                    sb.AppendLine($"    Your {CurrentSgMsg} will be spoken")
                    sb.AppendLine()
                    sb.AppendLine("Tell me name's SG/BG/Blood Glucose/Blood Sugar:")
                    sb.AppendLine("    Used when you support more than 1 user")
                    sb.AppendLine("    Example ""Tell me John's Sensor Glucose""")
                    Dim page As New TaskDialogPage
                    MsgBox(
                        heading:="",
                        prompt:=sb.ToString,
                        buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Information,
                        title:="Speech Recognition Help",
                        autoCloseTimeOut:=30,
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
    '''  Cancels and disposes the current speech recognition engine,
    '''  removes event handlers, and updates UI state.
    ''' </summary>
    Friend Sub CancelSpeechRecognition()
        If s_speechRecognitionEngine IsNot Nothing Then
            RemoveHandler s_speechRecognitionEngine.AudioSignalProblemOccurred,
                AddressOf AudioSignalProblemOccurred
            RemoveHandler s_speechRecognitionEngine.SpeechRecognized,
                AddressOf SpeechRecognized
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
    '''  Initializes and configures the speech recognition engine,
    '''  loads grammars, and starts recognition.
    ''' </summary>
    Friend Sub InitializeSpeechRecognition()
        Dim oldUserName As String = s_speechUserName
        If s_speechUserName = PatientData.FirstName AndAlso
           s_speechRecognitionEngine IsNot Nothing Then
            Exit Sub
        End If
        CancelSpeechRecognition()

        Try
            s_speechWakeWordFound = False

            Dim culture As New CultureInfo(name:="en-us")
            s_speechRecognitionEngine = New SpeechRecognitionEngine(culture)
            s_speechRecognitionEngine.SetInputToDefaultAudioDevice()

            Dim builder As New GrammarBuilder With {.Culture = culture}
            builder.Append(phrase:="carelink")
            s_speechRecognitionEngine.LoadGrammarAsync(grammar:=New Grammar(builder))

            builder = New GrammarBuilder With {.Culture = culture}
            builder.Append(phrase:="What")
            Dim alternateChoices As New Choices(
                "can I say",
                "is my SG",
                "is my BG",
                "is my Blood Sugar",
                "is my Blood Glucose")
            builder.Append(alternateChoices)
            s_speechRecognitionEngine.LoadGrammarAsync(grammar:=New Grammar(builder))

            Dim gb_tellMe As New GrammarBuilder With {.Culture = culture}
            gb_tellMe.Append(phrase:="Tell me")
            gb_tellMe.Append(phrase:=$"{PatientData.FirstName}'s")
            alternateChoices = New Choices("SG", "BG", "Blood Sugar", "Blood Glucose")
            gb_tellMe.Append(alternateChoices)
            s_speechRecognitionEngine.LoadGrammarAsync(
                grammar:=New Grammar(builder:=gb_tellMe))

            Form1.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            If String.IsNullOrWhiteSpace(s_speechUserName) Then
                Dim textToSpeak As String
                textToSpeak = $"Speech recognition enabled for {PatientData.FirstName}"

                If String.IsNullOrWhiteSpace(value:=oldUserName) Then
                    textToSpeak &= " for a list of commands say, CareLink what can I say"
                End If

                PlayText(textToSpeak)
            End If
            s_speechUserName = PatientData.FirstName
            Form1.StatusStripSpeech.Text = "Listening"
            s_speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple)
            AddHandler s_speechRecognitionEngine.SpeechRecognized, AddressOf SpeechRecognized

            Form1.Cursor = Cursors.Default
            AddHandler s_speechRecognitionEngine.AudioSignalProblemOccurred,
                AddressOf AudioSignalProblemOccurred

            Form1.MenuOptionsSpeechRecognitionEnabled.Checked = True
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Stop
        End Try

    End Sub

    ''' <summary>
    '''  Speaks the specified text using the speech synthesizer,
    '''  with duplicate and timing checks.
    ''' </summary>
    ''' <param name="textToSpeak">The text to be spoken.</param>
    Friend Sub PlayText(textToSpeak As String)
        If Not My.Settings.SystemAudioAlertsEnabled Then
            Form1.StatusStripSpeech.Text = "Audio Alerts Disabled"
            Return
        End If

        Dim diff As Long = DateDiff(Interval:=DateInterval.Minute, Now, s_timeOfLastAlert)
        If s_lastSpokenMessage = textToSpeak AndAlso diff < ThirtySecondsInMilliseconds Then
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
