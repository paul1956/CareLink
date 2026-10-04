' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Reflection
Imports System.Globalization
Imports System.Threading

Public Module LoggerManager
    Private s_loggerForm As LoggerForm
    Private s_loggerThread As Thread
    Private s_loggerReady As AutoResetEvent

    ' Controls whether the on-screen logger is shown/used. Initialized in InitLogger.
    ' Do NOT base this on Debugger.IsAttached — the app is frequently run under
    ' a debugger during development and that should not change runtime logging.
    Private s_showLogger As Boolean = False

    ' Controls whether verbose logging is enabled. Verbose messages are only
    ' emitted when this flag is true.
    Private s_verboseLogging As Boolean = False

    ''' <summary>
    ''' Enable or disable verbose logging at runtime. Verbose messages passed to
    ''' LogMessage(message, verbose:=True) will only be logged when enabled.
    ''' </summary>
    Public Sub SetVerboseLogging(enabled As Boolean)
        s_verboseLogging = enabled
    End Sub

    Public Enum LogLevel
        ErrorLevel
        WarningLevel
        InfoLevel
        VerboseLevel
    End Enum

    ' Fallback log folder for first-run users when the logger window is not visible.
    Private ReadOnly s_logFolder As String = Path.Combine(Environment.GetFolderPath(folder:=Environment.SpecialFolder.LocalApplicationData), "CareLink", "Logs")

    ''' <summary>
    '''  Initializes the logger form. If the logger form is not already created
    '''  or has been disposed, it creates a new instance of LoggerForm.
    '''  If a debugger is attached, it shows the logger form.
    ''' </summary>
    Public Sub InitLogger(show As Boolean)
        If s_loggerForm Is Nothing OrElse s_loggerForm.IsDisposed Then
            ' Start the logger on a dedicated STA UI thread so it remains
            ' responsive while modal dialogs run on the main UI thread.
            s_loggerReady = New AutoResetEvent(initialState:=False)
            s_loggerThread = New Thread(
                start:=Sub()
                           Dim form As New LoggerForm()
                           s_loggerForm = form
                           Try
                               ' Ensure the form's window handle is created on this (logger) thread
                               Dim handle As IntPtr = form.Handle

                               s_loggerReady.Set()
                               Application.Run(mainForm:=form)
                           Finally
                               ' Ensure the event is set if Run exits unexpectedly
                               s_loggerReady.Set()
                           End Try
                       End Sub)
            s_loggerThread.SetApartmentState(state:=ApartmentState.STA)
            s_loggerThread.IsBackground = True
            s_loggerThread.Start()

            ' Wait for the logger form to be created on its thread
            Try
                s_loggerReady.WaitOne()
            Catch
            End Try

            ' Keep the menu checkbox in sync when possible.
            Try
                Form1.MenuViewShowLogger.Checked = show
            Catch
                ' Ignore failures accessing the main form
            End Try
        End If
        ' Only enable the on-screen logger when explicitly requested by the caller.
        s_showLogger = show
        If s_showLogger Then
            ' Show it on its own thread so it remains
            ' clickable/movable while modal dialogs are displayed on the main UI thread.
            Try
                If s_loggerForm IsNot Nothing Then
                    Dim act As New Action(
                        start:=Sub()
                                   Try
                                       If Not s_loggerForm.Visible Then
                                           s_loggerForm.Show()
                                       End If
                                   Catch
                                   End Try
                               End Sub)
                    If s_loggerForm.InvokeRequired Then
                        s_loggerForm.BeginInvoke(act)
                    Else
                        act()
                    End If
                End If
            Catch
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Logs a message to the logger form and the Visual Studio Output window if a debugger is attached.
    ''' </summary>
    ''' <param name="message">
    '''  The message to log.
    ''' </param>
    <Extension>
    Public Sub LogMessage(message As String, Optional verbose As Boolean = False, Optional level As LogLevel = LogLevel.InfoLevel)
        Try
            ' If this is a verbose message and verbose logging is disabled, skip it.
            If verbose AndAlso Not s_verboseLogging Then
                Return
            End If

            ' Compose an output string that includes the log level for clarity.
            Dim output As String =
                If(level = LogLevel.InfoLevel,
                   message,
                   $"[{level}] {message}")

            ' Always write to Debug output so developers can see messages when attached
            'Debug.WriteLine(output)

            If s_showLogger Then
                If s_loggerForm IsNot Nothing AndAlso Not s_loggerForm.IsDisposed Then
                    s_loggerForm.LogMessage(message:=output)
                End If
            Else
                ' Logger window not visible (likely a first-time user). Write a fallback log
                ' to disk so the user can attach it when reporting failures.
                WriteFallbackLog(message:=output)
            End If
        Catch
            ' Swallow any logging exceptions; logging should never crash the app
        End Try
    End Sub

    ''' <summary>
    '''  Updates a log message in the logger form based on the provided start and
    '''  end keys. If the end key is an empty string, it replaces the entire line
    '''  starting from the start key.
    ''' </summary>
    ''' <param name="message">
    '''  The new message to replace the existing one.
    ''' </param>
    ''' <param name="startKey">
    '''  The key identifying the start of the message to update.
    ''' </param>
    ''' <param name="endKey">
    '''  The key identifying the end of the message to update.
    '''  </param>
    Public Sub UpdateMessage(message As String,
                             Optional startKey As String = "",
                             Optional endKey As String = "")
        If s_showLogger Then
            If s_loggerForm IsNot Nothing AndAlso Not s_loggerForm.IsDisposed Then
                s_loggerForm.UpdateLogMessage(startKey, endKey, message)
            End If
        Else
            ' Provide a best-effort update to the fallback log if available
            Try
                WriteFallbackLog(
                    message:=$"UPDATE {startKey}->{endKey}: {message}")
            Catch
            End Try
        End If
    End Sub

    Private Sub WriteFallbackLog(message As String)
        Try
            If String.IsNullOrWhiteSpace(value:=s_logFolder) Then
                Return
            End If

            Directory.CreateDirectory(path:=s_logFolder)

            Dim timestamp As String =
                Date.UtcNow.ToString(format:="yyyyMMdd_HHmmss_fff",
                                     provider:=CultureInfo.InvariantCulture)
            Dim fileName As String =
                Path.Combine(s_logFolder, $"CareLink_Log_{timestamp}.log")

            Dim asmVersion As String = "?"
            Try
                Dim entry As Assembly = Assembly.GetEntryAssembly()
                If entry IsNot Nothing Then
                    Dim ver As Version = entry.GetName().Version
                    If ver IsNot Nothing Then
                        asmVersion = ver.ToString()
                    End If
                Else
                    asmVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion
                End If
            Catch
            End Try

            Dim lines As New List(Of String) From {
                $"TimestampUTC: {Date.UtcNow.ToString(format:="o",
                                                      provider:=CultureInfo.InvariantCulture)}",
                $"AppVersion: {asmVersion}",
                $"OS: {Environment.OSVersion}",
                $"Machine: {Environment.MachineName}",
                $"Culture: {CultureInfo.CurrentCulture.Name}",
                $"UICulture: {CultureInfo.CurrentUICulture.Name}",
                $"ThreadId: {Environment.CurrentManagedThreadId}",
                "--- Message ---",
                message
            }

            File.AppendAllLines(path:=fileName, contents:=lines)
        Catch
            ' Ignore logging failures
        End Try
    End Sub

End Module
