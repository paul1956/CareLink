' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Configuration
Imports System.Text

Namespace My

    Partial Friend NotInheritable Class MySettings
        Private Sub MySettings_SettingChanging(sender As Object, e As SettingChangingEventArgs) Handles Me.SettingChanging
            If Settings(e.SettingName).ToString.ToUpperInvariant.Equals(e.NewValue.ToString.ToUpperInvariant) Then
                Exit Sub
            End If
            SaveAllUserData(e.SettingName, e.NewValue.ToString)
        End Sub

        Private Sub MySettings_SettingsLoaded(sender As Object, e As SettingsLoadedEventArgs) Handles Me.SettingsLoaded
            LoadAllUserData()
        End Sub
    End Class

End Namespace
