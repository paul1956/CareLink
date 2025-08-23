' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class UtilitiesRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable)
        Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
        If Not sTable.IsValid Then
            Stop
            Exit Sub
        End If
        Me.TimeFormat = sTable.GetSingleLineValue(Of String)("Time Format ")
        Me.Brightness = sTable.GetSingleLineValue(Of String)("Brightness ")
        Dim s As String = sTable.GetSingleLineValue(Of String)("Backlight Timeout ")
        If s.EndsWith(value:="s"c) Then
            Me.BackLightTimeout = New TimeSpan(
                hours:=0,
                minutes:=0,
                seconds:=CInt(s.Split(separator:=" ", options)(0)))
        Else
            Stop
        End If
        Me.BlockMode = sTable.GetSingleLineValue(Of String)(key:="Block Mode ")
        Me.AudioOptions = sTable.GetSingleLineValue(Of String)(key:="Audio Options ")
        Me.AlarmVolume = sTable.GetSingleLineValue(Of String)(key:="Alarm Volume ")

        Me.AutoSuspend = New AutoSuspendRecord(sTable)

    End Sub

    Public Property AlarmVolume As String = "Unknown"
    Public Property AudioOptions As String = "Unknown"
    Public Property AutoSuspend As New AutoSuspendRecord
    Public Property BackLightTimeout As New TimeSpan
    Public Property BlockMode As String = "Off"
    Public Property Brightness As String = "Unknown"
    Public Property TimeFormat As String = ""
End Class
