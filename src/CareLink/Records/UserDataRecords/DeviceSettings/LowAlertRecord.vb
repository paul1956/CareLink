' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class LowAlertRecord
    Public Property StartTime As TimeOnly
    Public Property Low As Single
    Public Property Suspend As Boolean
    Public Property AlertOnLow As Boolean
    Public Property AlertBeforeLow As Boolean
    Public Property ResumeBasalAlert As Boolean

End Class
