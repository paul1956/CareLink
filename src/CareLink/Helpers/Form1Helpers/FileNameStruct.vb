' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Represents a file name with both its full path and its name without the path.
''' </summary>
Friend Structure FileNameStruct

    ''' <summary>
    '''  Gets or sets the file name including its full path.
    ''' </summary>
    Public withPath As String

    ''' <summary>
    '''  Gets or sets the file name without its path.
    ''' </summary>
    Public withoutPath As String

    ''' <summary>
    '''  Initializes a new instance of the <see cref="FileNameStruct"/> structure.
    ''' </summary>
    ''' <param name="withPath">The file name including its full path.</param>
    ''' <param name="withoutPath">The file name without its path.</param>
    Public Sub New(withPath As String, withoutPath As String)
        Me.withPath = withPath
        Me.withoutPath = withoutPath
    End Sub

    ''' <summary>
    '''  Determines whether the specified object is equal to the current <see cref="FileNameStruct"/>.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current structure.</param>
    ''' <returns><see langword="True"/> if the specified object is equal to the current structure; otherwise, <c>False</c>.</returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        If Not (TypeOf obj Is FileNameStruct) Then
            Return False
        End If

        Dim other As FileNameStruct = DirectCast(obj, FileNameStruct)
        Return withPath = other.withPath AndAlso
               withoutPath = other.withoutPath
    End Function

    ''' <summary>
    '''  Returns a hash code for the current <see cref="FileNameStruct"/>.
    ''' </summary>
    ''' <returns>A hash code for the current structure.</returns>
    Public Overrides Function GetHashCode() As Integer
        Return HashCode.Combine(withPath, withoutPath)
    End Function

End Structure
