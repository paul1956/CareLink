' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module SmartGuardRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As SmartGuardRecord, sTable As StringTable, smartGuard As String)
        If sTable Is Nothing Then Return
        Try
            this.SmartGuard = smartGuard
            this.Target = sTable.GetSingleLineValue(Of Single)(key:="Target ")
            this.AutoCorrection = sTable.GetSingleLineValue(Of String)(key:="Auto Correction ")
        Catch
        End Try
    End Sub

End Module
