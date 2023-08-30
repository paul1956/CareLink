' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

Public Class StringTable

    Public ReadOnly Property IsValid As Boolean
        Get
            Return Me.Rows.Count > 0
        End Get
    End Property

    Public Property Rows As New List(Of Row)

    Public Sub Add(r As Row)
        Me.Rows.Add(r)
    End Sub

    <DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
    Public Class Row

        Public Sub New(columns As List(Of String))
            _Columns = columns
        End Sub

        Public Property Columns As List(Of String)

        Private Function GetDebuggerDisplay() As String
            Dim s As New StringBuilder
            For Each e As IndexClass(Of String) In _Columns.WithIndex
                s.Append($"Column {e.Index + 1} = {e.Value}")
                If Not e.IsLast Then
                    s.Append(", ")
                End If
            Next
            Return s.ToString()
        End Function

    End Class

End Class
