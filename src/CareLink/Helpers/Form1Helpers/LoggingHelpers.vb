' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module LoggerManager
    Private s_loggerForm As LoggerForm

    ''' <summary>
    '''  Initializes the logger form. If the logger form is not already created
    '''  or has been disposed, it creates a new instance of LoggerForm. If a debugger is attached, it shows the logger form.
    ''' </summary>
    Public Sub InitLogger()
        If s_loggerForm Is Nothing OrElse s_loggerForm.IsDisposed Then
            s_loggerForm = New LoggerForm()
            If Debugger.IsAttached Then
                s_loggerForm.Show()
            End If
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
        If Debugger.IsAttached Then
            ' Also send to Visual Studio Output window
            Debug.WriteLine(message)

            If s_loggerForm IsNot Nothing AndAlso Not s_loggerForm.IsDisposed Then
                s_loggerForm.LogMessage(message)
            End If
        End If
    End Sub

    ''' <summary>
    '''  Updates a log message in the logger form based on the provided start and
    '''  end keys. If the end key is an empty string, it replaces the entire line starting from the start key.
    ''' </summary>
    ''' <param name="startKey">
    '''  The key identifying the start of the message to update.
    ''' </param>
    ''' <param name="endKey">
    '''  The key identifying the end of the message to update.
    '''  </param>
    ''' <param name="message">
    '''  The new message to replace the existing one.
    ''' </param>
    <Extension>
    Public Sub UpdateMessage(startKey As String,
                             endKey As String,
                             message As String)
        If Debugger.IsAttached Then
            If s_loggerForm IsNot Nothing AndAlso Not s_loggerForm.IsDisposed Then
                s_loggerForm.UpdateLogMessage(startKey, endKey, message)
            End If
        End If
    End Sub

End Module
