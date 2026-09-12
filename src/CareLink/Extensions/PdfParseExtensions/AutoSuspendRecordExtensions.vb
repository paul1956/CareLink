' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module AutoSuspendRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As AutoSuspendRecord, sTable As StringTable)
        If sTable Is Nothing Then Return
        Try
            this.Alarm = sTable.GetSingleLineValue(Of String)(key:="Auto Suspend ")
            If this.Alarm <> "Off" Then
                this.Time = sTable.GetSingleLineValue(Of TimeSpan)(key:="Time ")
            End If
        Catch
        End Try
    End Sub

End Module
