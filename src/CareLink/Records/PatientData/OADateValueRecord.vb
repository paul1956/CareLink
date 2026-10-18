' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
''' Structure holding an OADate and a Single value.
''' </summary>
Public Class OADateValueRecord
    Public Property OaDate As OADate
    Public Property Value As Single

    Public Sub New(oaDate As OADate, sg As Single)
        Me.OaDate = oaDate
        Me.Value = sg
    End Sub

    ''' <summary>
    ''' True when either OaDate or Value is zero.
    ''' </summary>
    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return Me.OaDate = New OADate(oADateAsDouble:=0D) OrElse Me.Value = 0D
        End Get
    End Property

    ''' <summary>
    ''' True when both OaDate and Value are non-zero.
    ''' </summary>
    Public ReadOnly Property IsNotEmpty As Boolean
        Get
            Return Not Me.IsEmpty
        End Get
    End Property

End Class
