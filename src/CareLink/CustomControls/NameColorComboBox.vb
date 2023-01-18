' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class NameColorComboBox
    Inherits ComboBox

    Public Sub New()
        MyBase.New
    End Sub

    Public Shadows Property SelectedItem() As KeyValuePair(Of String, KnownColor)
        Get
            Return CType(MyBase.SelectedItem, KeyValuePair(Of String, KnownColor))
        End Get
        Set(value As KeyValuePair(Of String, KnownColor))
            MyBase.SelectedItem = value
        End Set
    End Property

    Public Shadows ReadOnly Property SelectedText() As String
        Get
            Return Me.SelectedItem.Key
        End Get
    End Property

    Public Shadows Property SelectedValue() As KnownColor
        Get
            Return Me.SelectedItem.Value
        End Get
        Set(value As KnownColor)
            Me.SelectedItem = KeyValuePair.Create(Me.Name, value)
        End Set
    End Property

    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()
        If Me.DesignMode = True Then Exit Sub
        Me.Items.Clear()
        Me.DrawMode = DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
        Me.Sorted = False
        For Each item As KeyValuePair(Of String, KnownColor) In GetGraphColorsBindingSource()
            Me.Items.Add(item)
        Next
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        If e.Index = -1 Then Exit Sub
        Dim eBounds As Rectangle = e.Bounds
        Dim item As KeyValuePair(Of String, KnownColor) = CType(Me.Items(e.Index), KeyValuePair(Of String, KnownColor))
        Dim key As String = item.Key
        Using b As Brush = New SolidBrush(SystemColors.Control)
            Dim pt As New Point(eBounds.X, eBounds.Top)
            e.Graphics.FillRectangle(b, eBounds.X, eBounds.Y, eBounds.Width \ 2, eBounds.Height)
            TextRenderer.DrawText(e.Graphics, item.Key, Me.Font, pt, SystemColors.ControlText, SystemColors.Control)
        End Using

        Dim paintColor As Color = item.Value.ToColor

        Using b As Brush = New SolidBrush(paintColor)
            e.Graphics.FillRectangle(b, eBounds.X + (eBounds.Width \ 2), eBounds.Y, eBounds.Width \ 2, eBounds.Height)
        End Using
        e.DrawFocusRectangle()
    End Sub

End Class
