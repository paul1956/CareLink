' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Represents a record of insulin activation, including the up count and
'''  active insulin time (AIT) in hours.
''' </summary>
Public Class InsulinActivationRecord

    ''' <summary>
    '''  Gets or sets the up count value, which may represent the number
    '''  of activations or changes.
    ''' </summary>
    Public Property UpCount As Integer

    ''' <summary>
    '''  Gets or sets the active insulin time (AIT) in hours.
    ''' </summary>
    Public Property AitHours As Single

    ''' <summary>
    '''  Initializes a new instance of the <see cref="InsulinActivationRecord"/> class.
    ''' </summary>
    ''' <param name="upCount">The up count value.</param>
    ''' <param name="aitHours">The active insulin time in hours.</param>
    Public Sub New(upCount As Integer, aitHours As Integer)
        Me.UpCount = upCount
        Me.AitHours = aitHours
    End Sub

End Class
