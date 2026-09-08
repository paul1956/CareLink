' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Microsoft.VisualBasic.ApplicationServices

Namespace My

    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.
    '           This event is not raised if the application terminates abnormally.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub Me_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) _
            Handles Me.ApplyApplicationDefaults

            ' Setting the application-wide default Font:
            ' e.Font = New Font(FontFamily.GenericSansSerif, 9, FontStyle.Regular)

            ' Setting the HighDpiMode for the Application:
            e.HighDpiMode = HighDpiMode.PerMonitorV2
            e.ColorMode = SystemColorMode.Dark
            e.FormRevealMode = FormRevealMode.Deferred
            '    ' If a splash dialog is used, this sets the minimum display time:
            '    e.MinimumSplashScreenDisplayTime = 4000
            e.VisualStylesMode = VisualStylesMode.Latest
        End Sub

        Private Sub Me_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) _
            Handles Me.UnhandledException

            ExceptionHandlerDialog.UnhandledException = e
            ExceptionHandlerDialog.ShowDialog()
        End Sub

        Private Sub Me_Startup(sender As Object, e As StartupEventArgs) _
            Handles Me.Startup
            ' Prefetch discovery data for the configured country/region so DiscoveryRoot
            ' is obtained at application startup instead of on first use. This runs
            ' on a background thread to avoid delaying UI initialization.
            Try
                Dim country As String = If(IsNullOrWhiteSpace(value:=My.Settings.CountryCode), "US", My.Settings.CountryCode)
                Dim worldRegion As WorldRegion = country.GetRegionFromCode()
                Dim serverMapping As String = s_regionToServerMapping(key:=worldRegion)
                Dim serverRegion As ServerLocation = If(serverMapping.EqualsNoCase("US"), ServerLocation.US, If(serverMapping.EqualsNoCase("Eu"), ServerLocation.Eu, ServerLocation.Clinical))
                Task.Run(Function() Discover.GetCachedDiscoveryAsync(serverRegion))
            Catch
                ' Ignore errors during prefetch; discovery will be fetched lazily if needed.
            End Try
        End Sub

    End Class
End Namespace
