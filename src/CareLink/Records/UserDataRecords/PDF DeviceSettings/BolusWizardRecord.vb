' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BolusWizardRecord

    Public Sub New(lines As List(Of String))
        Me.BolusWizard = lines.GetSingleLineValue(Of String)("Bolus Wizard ")
        Me.Units = New DeviceUnitsRecord(lines.GetSingleLineValue(Of String)("Units "))
        Dim value As String = lines.GetSingleLineValue(Of String)("(h:mm) ")
        Me.ActiveInsulinTime = If(value = "",
                                  2,
                                  s_aitLengths(value)
                                 )
        Me.MaximumBolus = lines.GetSingleLineValue(Of Single)("Maximum Bolus ")
    End Sub

    Public Property BolusWizard As String = "Off"
    Public Property Units As DeviceUnitsRecord

    ' Pump Ait
    Public Property ActiveInsulinTime As Single

    Public Property MaximumBolus As Single

End Class
