' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class MealStartEndRecord

    Public Sub New()
    End Sub

    ''' <summary>
    '''  Initializes a new instance of the <see cref="MealStartEndRecord"/> class.
    ''' </summary>
    ''' <param name="r">The row from which to extract meal start and end times.</param>
    ''' <param name="key">The key used to replace in the row's columns.</param>
    ''' <remarks>
    '''  The constructor replaces the old value in the row's columns with an empty string and cleans up spaces.
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">Thrown if the row is null.</exception>
    Public Sub New(r As StringTable.Row, key As String)
        Me.Start = r.Columns(index:=0).Replace(oldValue:=key, newValue:="").CleanSpaces
        Me.End = r.Columns(index:=1).Replace(oldValue:=key, newValue:="").CleanSpaces
    End Sub

    Public Property [End] As String = "Off"
    Public Property Start As String = "Off"
End Class
