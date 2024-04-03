' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BolusWizardRecord

    Public Sub New(sTables As StringTable)
        Me.BolusWizard = sTables.GetSingleLineValue(Of String)("Bolus Wizard")
        Me.Units = New DeviceUnitsRecord(sTables.GetSingleLineValue(Of String)("Units"))
        Dim value As String = sTables.GetSingleLineValue(Of String)("Active Insulin Time (h:mm)")
        Me.ActiveInsulinTime = If(value = "",
                                  2,
                                  s_aitLengths(value)
                                 )
        Me.MaximumBolus = sTables.GetSingleLineValue(Of Single)("Maximum Bolus")
    End Sub

    Public Property ActiveInsulinTime As Single
    Public Property BolusWizard As String = "Off"
    Public Property MaximumBolus As Single
    Public Property Units As DeviceUnitsRecord
End Class
