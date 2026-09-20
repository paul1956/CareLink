' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Reflection
Imports System.Globalization

Public Module LoggerManager
    Private s_loggerForm As LoggerForm

    ' Controls whether the on-screen logger is shown/used. Initialized in InitLogger.
    Private s_showLogger As Boolean = Debugger.IsAttached

    ' Fallback log folder for first-run users when the logger window is not visible.
    Private ReadOnly s_logFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CareLink", "Logs")

    ''' <summary>
    '''  Initializes the logger form. If the logger form is not already created
    '''  or has been disposed, it creates a new instance of LoggerForm.
    '''  If a debugger is attached, it shows the logger form.
    ''' </summary>
    Public Sub InitLogger(show As Boolean)
        If s_loggerForm Is Nothing OrElse s_loggerForm.IsDisposed Then
            s_loggerForm = New LoggerForm()
        End If
        s_showLogger = show Or Debugger.IsAttached
        If s_showLogger Then
            s_loggerForm.Show()
        End If
    End Sub

    ''' <summary>
    '''  Logs a message to the logger form and the Visual Studio Output window if a debugger is attached.
    ''' </summary>
    ''' <param name="message">
    '''  The message to log.
    ''' </param>
    <Extension>
    Public Sub LogMessage(message As String)
        Try
            ' Always write to Debug output so developers can see messages when attached
            Debug.WriteLine(message)

            If s_showLogger Then
                If s_loggerForm IsNot Nothing AndAlso Not s_loggerForm.IsDisposed Then
                    s_loggerForm.LogMessage(message)
                End If
            Else
                ' Logger window not visible (likely a first-time user). Write a fallback log
                ' to disk so the user can attach it when reporting failures.
                WriteFallbackLog(message)
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
                WriteFallbackLog($"UPDATE {startKey}->{endKey}: {message}")
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
