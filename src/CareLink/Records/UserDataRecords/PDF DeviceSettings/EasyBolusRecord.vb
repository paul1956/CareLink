' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class EasyBolusRecord

    Public Sub New(sTable As StringTable)
        Me.EasyBolus = sTable.GetSingleLineValue(Of String)(key:="Easy Bolus")
        ' Ensure EasyBolus is formatted correctly 0.01  U
        If Me.EasyBolus.Length = 6 Then
            Me.EasyBolus = Me.EasyBolus.Replace(oldValue:=" ", newValue:="  ")
        End If
        Me.BolusIncrement = sTable.GetSingleLineValue(Of Single)(key:="Bolus Increment")
        Me.BolusSpeed = sTable.GetSingleLineValue(Of String)(key:="Bolus Speed ")

        Dim line As String = sTable.GetSingleLineValue(Of String)(key:="Dual/Square")
        Me.DualSquare = New DualSquareRecord(line)
    End Sub

    Public Sub New()
    End Sub

    Public Property BolusIncrement As Single
    Public Property BolusSpeed As String
    Public Property DualSquare As New DualSquareRecord(EmptyString)
    Public Property EasyBolus As String = "Off"
End Class
