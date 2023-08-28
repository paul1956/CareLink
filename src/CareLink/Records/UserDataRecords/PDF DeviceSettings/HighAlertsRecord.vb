' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertsRecord
    Public Property Snooze As TimeSpan = New TimeSpan(1, 0, 0)
    Public Property SnoozeOn As Boolean = False

    Public Property HighAlert As New HighAlertRecord

End Class
