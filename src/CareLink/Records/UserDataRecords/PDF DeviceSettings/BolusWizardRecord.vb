' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BolusWizardRecord

    Public Sub New(sTable As StringTable)
        Me.BolusWizard = sTable.GetSingleLineValue(Of String)(key:="Bolus Wizard")
        Me.Units = New DeviceUnitsRecord(line:=sTable.GetSingleLineValue(Of String)(key:="Units"))
        Dim key As String = sTable.GetSingleLineValue(Of String)(key:="Active Insulin Time (h:mm)")
        Me.ActiveInsulinTime = If(key = String.Empty,
                                  2,
                                  s_aitLengths(key))

        Me.MaximumBolus = sTable.GetSingleLineValue(Of Single)(key:="Maximum Bolus")
    End Sub

    Public Property ActiveInsulinTime As Single
    Public Property BolusWizard As String = "Off"
    Public Property MaximumBolus As Single
    Public Property Units As DeviceUnitsRecord
End Class
