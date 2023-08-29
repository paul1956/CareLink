' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class HighAlertRecord
    Public Property HighLimit As DeviceLimitRecord
    Public Property AlertBeforeHigh As Boolean
    Public Property TimeBeforeHigh As TimeOnly
    Public Property AlertOnHigh As Boolean
    Public Property RiseAlert As Boolean
    Public Property RaiseLimit As String
End Class
