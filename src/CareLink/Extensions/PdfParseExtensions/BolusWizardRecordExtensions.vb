' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module BolusWizardRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As BolusWizardRecord, sTable As StringTable)
        If sTable Is Nothing Then Return
        Try
            this.BolusWizard = sTable.GetSingleLineValue(Of String)(key:="Bolus Wizard")
            Dim unitsLine As String = sTable.GetSingleLineValue(Of String)(key:="Units")
            Dim units As New DeviceUnitsRecord()
            units.InitializeFromString(unitsLine)
            this.Units = units
            Dim key As String =
                sTable.GetSingleLineValue(Of String)(key:="Active Insulin Time", endsWith:="(h:mm)")
            this.ActiveInsulinTime = If(key = String.Empty,
                                        2,
                                        AitLengths(key))

            this.MaximumBolus = sTable.GetSingleLineValue(Of Single)(key:="Maximum Bolus")
        Catch
        End Try
    End Sub

End Module
