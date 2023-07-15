' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Speech.Recognition
Imports System.Speech.Synthesis
Imports System.Text

Friend Module SpeechRecognition
    Private currentUserFirstName As String = Nothing
    Friend s_speechDone As Boolean = False
    Friend s_speechOn As Boolean = True
    Friend s_sre As SpeechRecognitionEngine
    Friend s_ss As New SpeechSynthesizer()
    Friend Property SpeechSupportReported As Boolean = True
    Friend Property SpeechWakeWordFound As Boolean = False

    Friend Sub InitializeSpeechRecognition()
        If SpeechSupportReported AndAlso currentUserFirstName = s_firstName Then Return

        Try
            s_ss = New SpeechSynthesizer()
            s_ss.SetOutputToDefaultAudioDevice()
            s_sre = New SpeechRecognitionEngine(New CultureInfo("en-us"))
            s_sre.SetInputToDefaultAudioDevice()
            Dim gb_Attention As New GrammarBuilder
            gb_Attention.Append(s_careLinkLower)
            s_sre.LoadGrammarAsync(New Grammar(gb_Attention))

            Dim gb_StartStop As New GrammarBuilder()
            gb_StartStop.Append("alerts")
            gb_StartStop.Append(New Choices("off", "on"))
            s_sre.LoadGrammarAsync(New Grammar(gb_StartStop))

            Dim gb_what As New GrammarBuilder()
            gb_what.Append("What")
            gb_what.Append(New Choices("can I say", "is my BG", "is my Blood Sugar", "is my Blood Glucose"))
            s_sre.LoadGrammarAsync(New Grammar(gb_what))

            Dim gb_tellMe As New GrammarBuilder()
            currentUserFirstName = s_firstName
            gb_tellMe.Append("Tell me")
            gb_tellMe.Append($"{currentUserFirstName}'s")
            gb_tellMe.Append(New Choices("BG", "Blood Sugar", "Blood Glucose"))
            s_sre.LoadGrammarAsync(New Grammar(gb_tellMe))

            Dim gb_showTab As New GrammarBuilder()
            gb_showTab.Append("Show")
            Dim showChoices As New Choices()
            For Each tab As TabPage In Form1.TabControlPage1.TabPages
                showChoices.Add(tab.Text.TrimEnd("."c))
            Next
            For Each tab As TabPage In Form1.TabControlPage2.TabPages
                showChoices.Add(tab.Text.TrimEnd("."c))
            Next

            gb_showTab.Append(showChoices)
            Dim g_showTab As New Grammar(gb_showTab)
            s_sre.LoadGrammarAsync(g_showTab)

            s_sre.RecognizeAsync(RecognizeMode.Multiple)
            s_ss.Speak($"Speech recognition enabled for {currentUserFirstName}, for a list of commands say {ProjectName} what can I say")
            Form1.StatusStripSpacerLeft.Text = "Listening"
            SpeechSupportReported = True
            SpeechWakeWordFound = False
            AddHandler s_sre.SpeechRecognized, AddressOf sre_SpeechRecognized
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Stop
        End Try

    End Sub

    Friend Sub sre_SpeechRecognized(sender As Object, e As SpeechRecognizedEventArgs)
        Dim recognizedText As String = e.Result.Text.ToLower
        Dim confidence As Single = e.Result.Confidence
        Debug.WriteLine(vbLf & "Recognized: " & recognizedText)
        If recognizedText.StartsWith(s_careLinkLower) Then
            If confidence < 0.6 Then
                SpeechWakeWordFound = False
                Exit Sub
            End If
            If recognizedText = s_careLinkLower Then
                Form1.StatusStripSpacerLeft.Text = "Heard: 'CareLink' waiting..."
                Application.DoEvents()
                SpeechWakeWordFound = True
                Exit Sub

            End If
        End If
        If confidence < 0.6 Then
            Exit Sub
        End If

        If SpeechWakeWordFound Then
            Form1.StatusStripSpacerLeft.Text = $"Heard: {recognizedText}"
            Debug.WriteLine(recognizedText)
            Application.DoEvents()
            recognizedText = recognizedText.Replace(s_careLinkLower, "")
            Select Case True
                Case recognizedText = "alerts on"
                    Debug.WriteLine("alerts are now ON")
                    s_speechOn = True
                Case recognizedText = "alerts off"
                    Debug.WriteLine("alerts are now OFF")
                    s_speechOn = False
                Case recognizedText.StartsWith("what is my", StringComparison.CurrentCultureIgnoreCase)
                    If recognizedText.Contains("BG", StringComparison.CurrentCultureIgnoreCase) OrElse
                        recognizedText.Contains("Blood Glucose", StringComparison.CurrentCultureIgnoreCase) OrElse
                        recognizedText.Contains("Blood Sugar", StringComparison.CurrentCultureIgnoreCase) Then
                        s_ss.SpeakAsync($"{s_firstName}'s Current Blood Glucose is {If(IsNumeric(Form1.CurrentSgLabel.Text), Form1.CurrentSgLabel.Text, "Unknown")}")
                    End If
                Case recognizedText.StartsWith("tell me", StringComparison.CurrentCultureIgnoreCase)
                    If Not recognizedText.Contains(s_firstName.ToLower) Then
                        Return
                    End If
                    If recognizedText.Contains("BG", StringComparison.CurrentCultureIgnoreCase) OrElse
                        recognizedText.Contains("Blood Glucose", StringComparison.CurrentCultureIgnoreCase) OrElse
                        recognizedText.Contains("Blood Sugar", StringComparison.CurrentCultureIgnoreCase) Then
                        s_ss.SpeakAsync($"{s_firstName}'s Current Blood Glucose is {If(IsNumeric(Form1.CurrentSgLabel.Text), Form1.CurrentSgLabel.Text, "Unknown")}")
                    End If
                Case recognizedText = "what can I say"
                    Dim prompt As New StringBuilder
                    prompt.AppendLine($"{ProjectName}: All commands state with this, a pause is allowed after saying {ProjectName}.")
                    prompt.AppendLine($"Alerts On: Enables audio Alerts")
                    prompt.AppendLine($"Alerts Off: Disables audio Alerts")
                    prompt.AppendLine($"What is my BG/Blood Glucose/Blood Sugar: Your current BG will be spoken")
                    prompt.AppendLine($"What can I say: This message will be displayed")
                    prompt.AppendLine($"Show [any tab name]: Will make that tab have focus")
                    prompt.AppendLine($"     Example ""Show Treatment Details""")
                    prompt.AppendLine($"Tell me name's BG/Blood Glucose/Blood Sugar: use when you support more than 1 user")
                    prompt.AppendLine($"     Example ""Tell me John's BG""")

                    MsgBox(prompt.ToString, MsgBoxStyle.OkOnly, "Voice Help")
                Case recognizedText.StartsWith("show", StringComparison.CurrentCultureIgnoreCase)
                    Dim tabText As String = recognizedText.Substring("show ".Length).ToLower.TrimEnd("."c)
                    For Each tab As TabPage In Form1.TabControlPage1.TabPages
                        If tab.Text.ToLower.TrimEnd("."c) = tabText Then
                            Form1.TabControlPage1.Visible = True
                            Form1.TabControlPage1.SelectedTab = tab
                            Exit Select
                        End If
                    Next
                    For Each tab As TabPage In Form1.TabControlPage2.TabPages
                        If tab.Text.ToLower.TrimEnd("."c) = tabText Then
                            Form1.TabControlPage1.Visible = False
                            Form1.TabControlPage2.SelectedTab = tab
                            Exit Select
                        End If
                    Next
                Case Else

            End Select

            SpeechWakeWordFound = False
        End If
        Form1.StatusStripSpacerLeft.Text = "Listening"
    End Sub

End Module
