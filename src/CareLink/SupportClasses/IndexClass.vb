' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Threading

''' <summary>
'''  Represents an element in a sequence along with its index and enumeration state.
'''  Used to provide context when iterating with index and to track first/last elements.
''' </summary>
''' <typeparam name="T">The type of the value being enumerated.</typeparam>
Public Class IndexClass(Of T)

    ''' <summary>
    '''  Gets or sets the value of the current element in the sequence.
    ''' </summary>
    Public Property Value As T

    ''' <summary>
    '''  Gets or sets the zero-based index of the current element.
    '''  The first element has index = 0.
    ''' </summary>
    Public Property Index As Integer

    ''' <summary>
    '''  Gets a value indicating whether this is the first element in the sequence.
    ''' </summary>
    Public ReadOnly Property IsFirst As Boolean
        Get
            Return Me.Index = 0
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets a value indicating whether this is the last element in the sequence.
    ''' </summary>
    Public Property IsLast As Boolean

    ''' <summary>
    '''  Gets or sets the enumerator used to iterate the sequence.
    ''' </summary>
    Public Property Enumerator As IEnumerator(Of T)

    ''' <summary>
    '''  Advances the enumerator to the next element, updates the value and index,
    '''  and sets the IsLast property.
    ''' </summary>
    Public Sub MoveNext()
        Me.Value = Me.Enumerator.Current
        Me.IsLast = Not Me.Enumerator.MoveNext()
        ' may be called with .AsParallel
        Interlocked.Increment(Me.Index)
    End Sub

End Class
