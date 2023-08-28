' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class EasyBolusRecord

    Public Sub New(lines As List(Of String))
        Me.EasyBolus = lines.GetSingleLineValue(Of String)("Easy Bolus ")
        Me.BolusIncrement = lines.GetSingleLineValue(Of Single)("Bolus Increment ")
        Me.BolusSpeed = lines.GetSingleLineValue(Of String)("Bolus Speed ")
        Me.DualSquare = New DualSquareRecord(lines.GetSingleLineValue(Of String)("Dual/Square "))
    End Sub

    Public Sub New()
    End Sub

    Public Property EasyBolus As String = "Off"
    Public Property BolusIncrement As Single
    Public Property BolusSpeed As String
    Public Property DualSquare As New DualSquareRecord("")

End Class
