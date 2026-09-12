' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module UtilitiesRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As UtilitiesRecord, sTable As StringTable)
        If sTable Is Nothing Then Return
        Try
            Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
            If Not sTable.IsValid Then
                Stop
                Exit Sub
            End If
            this.TimeFormat = sTable.GetSingleLineValue(Of String)("Time Format ")
            this.Brightness = sTable.GetSingleLineValue(Of String)("Brightness ")
            Dim s As String = sTable.GetSingleLineValue(Of String)("Backlight Timeout ")
            If s.EndsWith(value:="s"c) Then
                this.BackLightTimeout = New TimeSpan(
                hours:=0,
                minutes:=0,
                seconds:=CInt(s.Split(separator:=" ", options)(0)))
            Else
                Stop
            End If
            this.BlockMode = sTable.GetSingleLineValue(Of String)(key:="Block Mode ")
            this.AudioOptions = sTable.GetSingleLineValue(Of String)(key:="Audio Options ")
            this.AlarmVolume = sTable.GetSingleLineValue(Of String)(key:="Alarm Volume ")

            this.AutoSuspend = New AutoSuspendRecord(sTable)
        Catch
        End Try
    End Sub

End Module
