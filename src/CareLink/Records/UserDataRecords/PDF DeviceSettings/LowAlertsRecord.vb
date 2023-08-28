' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LowAlertsRecord
    Public Property Snooze As TimeSpan = New TimeSpan(0, 20, 0)
    Public Property SnoozeOn As Boolean = False

    Public Property LowAlert As New LowAlertRecord

End Class
