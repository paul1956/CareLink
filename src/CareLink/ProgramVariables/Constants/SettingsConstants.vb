' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module SettingsConstants

    Friend ReadOnly s_headerColumns As New List(Of String) From {
        NameOf(My.Settings.CareLinkUserName),
        NameOf(My.Settings.CareLinkPassword),
        NameOf(My.Settings.CountryCode),
        NameOf(My.Settings.UseLocalTimeZone),
        NameOf(My.Settings.AutoLogin),
        NameOf(My.Settings.CareLinkPartner),
        NameOf(My.Settings.CareLinkPatientUserID)}

End Module
