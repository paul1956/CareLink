' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class EasyBolusRecord

    Public Sub New(sTable As StringTable)
        Me.EasyBolus = sTable.GetSingleLineValue(Of String)("Easy Bolus")
        Me.BolusIncrement = sTable.GetSingleLineValue(Of Single)("Bolus Increment")
        Me.BolusSpeed = sTable.GetSingleLineValue(Of String)("Bolus Speed ")
        Me.DualSquare = New DualSquareRecord(sTable.GetSingleLineValue(Of String)("Dual/Square"))
    End Sub

    Public Sub New()
    End Sub

    Public Property BolusIncrement As Single
    Public Property BolusSpeed As String
    Public Property DualSquare As New DualSquareRecord("")
    Public Property EasyBolus As String = "Off"
End Class
