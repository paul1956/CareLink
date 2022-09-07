' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class ActiveInsulinRecord

    Public Sub New(dic As Dictionary(Of String, String))
        Me.Dictionary = dic
        For Each kvp As KeyValuePair(Of String, String) In dic
            Select Case kvp.Key
                Case NameOf(amount)
                    Me.amount = kvp.Value.ParseSingle(3)
                Case NameOf(Me.datetime)
                    Me.datetime = kvp.Value.ParseDate(NameOf(Me.datetime), NameOf(Me.datetime))
                    Me.datetimeAsString = kvp.Value
                    Me.currentOADate = New OADate(Me.datetime)
                Case NameOf(precision)
                    Me.precision = kvp.Value
                Case NameOf(kind)
                    Me.kind = kvp.Value
                Case NameOf(version)
                    Me.version = CInt(kvp.Value)
                Case Else
                    Stop
            End Select
        Next
    End Sub

    Friend ReadOnly Property Dictionary As Dictionary(Of String, String)

#If True Then ' Prevent reordering

    Public Property amount As Single
    Public Property datetime As Date
    Public Property datetimeAsString As String
    Public Property currentOADate As OADate
    Public Property precision As String
    Public Property kind As String
    Public Property version As Integer
#End If


End Class
