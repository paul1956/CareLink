' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class UtilitiesRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable)
        Me.InitializeFromStringTable(sTable)
    End Sub

    Public Property AlarmVolume As String = "Unknown"
    Public Property AudioOptions As String = "Unknown"
    Public Property AutoSuspend As New AutoSuspendRecord
    Public Property BackLightTimeout As New TimeSpan
    Public Property BlockMode As String = "Off"
    Public Property Brightness As String = "Unknown"
    Public Property LostCommunication As String = "N/A"
    Public Property PumpSounds As String = "N/A"
    Public Property PumpVibrations As String = "N/A"
    Public Property TimeFormat As String = "12 hr"
End Class
