' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class KnownColorComboBox
    Inherits ComboBox

    Private ReadOnly _allKnownColors As New SortedDictionary(Of String, KnownColor)

    Public Sub New()
        MyBase.New
        Dim kColor As Color
        For Each known As KnownColor In [Enum].GetValues(Of KnownColor)()
            If known = KnownColor.Transparent Then Continue For
            kColor = Color.FromKnownColor(known)
            If kColor.IsSystemColor OrElse _allKnownColors.ContainsValue(known) Then
                Continue For
            End If
            _allKnownColors.Add(kColor.Name, known)
        Next
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
            Me.SelectedItem = KeyValuePair.Create(GetNameFromKnownColor(value), value)
        End Set
    End Property

    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()
        If Me.DesignMode Then Exit Sub
        Me.Items.Clear()
        Me.DrawMode = DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
        Me.Sorted = True
        For Each item As KeyValuePair(Of String, KnownColor) In _allKnownColors
            Me.Items.Add(item)
        Next
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        If e.Index = -1 Then Exit Sub

        Dim item As KeyValuePair(Of String, KnownColor) = CType(Me.Items(e.Index), KeyValuePair(Of String, KnownColor))
        Dim key As String = item.Key
        Dim backColor As Color = Color.FromKnownColor(item.Value)
        Dim eBounds As Rectangle = e.Bounds
        Using b As Brush = New SolidBrush(backColor)
            Dim pt As New Point(eBounds.X, eBounds.Top)
            e.Graphics.FillRectangle(b, eBounds.X, eBounds.Y, eBounds.Width, eBounds.Height)
            TextRenderer.DrawText(e.Graphics, key, Me.Font, pt, backColor.GetContrastingColor(), backColor)
        End Using
        e.DrawFocusRectangle()
    End Sub

End Class
