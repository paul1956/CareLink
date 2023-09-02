' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults

            ' Setting the application-wide default Font:
            ' e.Font = New Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular)

            ' Setting the HighDpiMode for the Application:
            e.HighDpiMode = HighDpiMode.PerMonitorV2

            '    ' If a splash dialog is used, this sets the minimum display time:
            '    e.MinimumSplashScreenDisplayTime = 4000
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ExceptionHandlerDialog.UnhandledException = e
            ExceptionHandlerDialog.ShowDialog()
        End Sub

    End Class

End Namespace
