' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetTypeRecord
    Private _rate As Single
    Private _percent As Single
    Private _typeIsRate As Boolean

    Public Sub New(s As String)
        Stop
    End Sub

    Public WriteOnly Property Rate As Single
        Set(value As Single)
            _rate = value
            _typeIsRate = True
        End Set
    End Property

    Public WriteOnly Property Percent As Integer
        Set(value As Integer)
            _percent = value
            _typeIsRate = False
        End Set
    End Property

    Public ReadOnly Property Value As String
        Get
            Return If(_typeIsRate, $"Rate: {_rate:F1}U/hr", $"Percent: {_percent}%")
        End Get
    End Property

End Class
