' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class UtilitiesRecord

    Public Sub New(sTable As StringTable)
        If Not sTable.IsValid Then
            Stop
            Exit Sub
        End If
        Me.BlockMode = sTable.GetSingleLineValue(Of String)("Block Mode ")
        Me.TimeFormat = sTable.GetSingleLineValue(Of String)("Time Format ")
        Me.Brightness = sTable.GetSingleLineValue(Of String)("Brightness ")
        Dim s As String = sTable.GetSingleLineValue(Of String)("Backlight Timeout ")
        If s.EndsWith("s") Then
            Me.BackLightTimeout = New TimeSpan(0, 0, CInt(s.Split(" ")(0)))
        Else
            Stop
        End If
        Me.AudioOptions = sTable.GetSingleLineValue(Of String)("Audio Options ")
        Me.AlarmVolume = sTable.GetSingleLineValue(Of String)("Alarm Volume ")

        Me.AutoSuspend = New AutoSuspendRecord(sTable)

    End Sub

    Public Property BlockMode As String
    Public Property TimeFormat As String

    Public Property Brightness As String
    Public Property BackLightTimeout As TimeSpan
    Public Property AudioOptions As String
    Public Property AlarmVolume As String
    Public Property AutoSuspend As AutoSuspendRecord

End Class
