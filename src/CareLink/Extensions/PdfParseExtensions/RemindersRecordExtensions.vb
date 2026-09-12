' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module RemindersRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As RemindersRecord, sTable As StringTable)
        If sTable Is Nothing Then Return
        Try
            this.LowReservoirWarning =
                sTable.GetSingleLineValue(Of String)(key:="Low Reservoir Warning ")
            this.Amount =
                sTable.GetSingleLineValue(Of String)(key:="Amount ")
            this.BolusBgCheck =
                sTable.GetSingleLineValue(Of String)(key:="Bolus BG Check ")
            this.SetChange =
                sTable.GetSingleLineValue(Of String)(key:="Set Change ")
        Catch
        End Try
    End Sub

End Module
