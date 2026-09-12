' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module EasyBolusRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As EasyBolusRecord, sTable As StringTable)
        If sTable Is Nothing Then Return
        Try
            this.EasyBolus = sTable.GetSingleLineValue(Of String)(key:="Easy Bolus")
            ' Ensure EasyBolus is formatted correctly 0.01  U
            If this.EasyBolus.Length = 6 Then
                this.EasyBolus = this.EasyBolus.Replace(oldValue:=" ", newValue:="  ")
            End If
            this.BolusIncrement = sTable.GetSingleLineValue(Of Single)(key:="Bolus Increment")
            this.BolusSpeed = sTable.GetSingleLineValue(Of String)(key:="Bolus Speed ")

            Dim line As String = sTable.GetSingleLineValue(Of String)(key:="Dual/Square")
            Dim ds As New DualSquareRecord()
            ds.InitializeFromString(line)
            this.DualSquare = ds
        Catch
        End Try
    End Sub

End Module
