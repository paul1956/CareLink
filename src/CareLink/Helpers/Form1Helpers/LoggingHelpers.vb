' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module LoggerManager
    Private s_loggerForm As LoggerForm

    ' Initialize logger (call from Program startup or MainForm)
    Public Sub InitLogger()
#If DEBUG Then
        If s_loggerForm Is Nothing OrElse s_loggerForm.IsDisposed Then
            s_loggerForm = New LoggerForm()
            s_loggerForm.Show()
        End If
#End If
    End Sub

    ' Public method to log messages from anywhere
    <Extension>
    Public Sub LogMessage(message As String)
#If DEBUG Then
        ' Also send to Visual Studio Output window
        Debug.WriteLine(message)

        If s_loggerForm IsNot Nothing AndAlso Not s_loggerForm.IsDisposed Then
            s_loggerForm.LogMessage(message)
        End If
#End If
    End Sub

End Module
