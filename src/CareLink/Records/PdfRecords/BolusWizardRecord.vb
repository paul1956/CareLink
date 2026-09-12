' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BolusWizardRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable)
        Me.InitializeFromStringTable(sTable)
    End Sub

    Public Property ActiveInsulinTime As Single
    Public Property BolusWizard As String = "Off"
    Public Property MaximumBolus As Single
    Public Property Units As DeviceUnitsRecord
End Class
