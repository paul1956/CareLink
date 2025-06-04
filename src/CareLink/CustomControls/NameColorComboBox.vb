' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Represents a ComboBox control that displays a list of color names and their associated <see cref="KnownColor"/> values.
'''  Provides strongly-typed access to the selected color item and custom drawing for color visualization.
''' </summary>
Public Class NameColorComboBox
    Inherits ComboBox

    ''' <summary>
    '''  Initializes a new instance of the <see cref="NameColorComboBox"/> class.
    ''' </summary>
    Public Sub New()
        MyBase.New
    End Sub

    ''' <summary>
    '''  Gets or sets the currently selected item as a <see cref="KeyValuePair(Of String, KnownColor)"/>.
    '''  The key represents the display name, and the value is the associated <see cref="KnownColor"/>.
    ''' </summary>
    ''' <remarks>
    '''  This property shadows the base <see cref="ComboBox.SelectedItem"/> property to provide
    '''  strongly-typed access to the selected color item.
    ''' </remarks>
    Public Shadows Property SelectedItem() As KeyValuePair(Of String, KnownColor)
        Get
            Return CType(MyBase.SelectedItem, KeyValuePair(Of String, KnownColor))
        End Get
        Set(value As KeyValuePair(Of String, KnownColor))
            MyBase.SelectedItem = value
        End Set
    End Property

    ''' <summary>
    '''  Gets the display name of the currently selected color item.
    ''' </summary>
    Public Shadows ReadOnly Property SelectedText() As String
        Get
            Return Me.SelectedItem.Key
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets the <see cref="KnownColor"/> value of the currently selected item.
    ''' </summary>
    Public Shadows Property SelectedValue() As KnownColor
        Get
            Return Me.SelectedItem.Value
        End Get
        Set(value As KnownColor)
            Me.SelectedItem = KeyValuePair.Create(Me.Name, value)
        End Set
    End Property

    ''' <summary>
    '''  Initializes the layout of the control, populating the ComboBox with color items and configuring drawing settings.
    ''' </summary>
    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()
        If Me.DesignMode Then Exit Sub
        Me.Items.Clear()
        Me.DrawMode = DrawMode.OwnerDrawFixed
        Me.DropDownStyle = ComboBoxStyle.DropDownList
        Me.Sorted = False
        For Each item As KeyValuePair(Of String, KnownColor) In GraphColorDictionary
            Me.Items.Add(item)
        Next
    End Sub

    ''' <summary>
    '''  Handles the drawing of each item in the ComboBox, displaying the color name and a color swatch.
    ''' </summary>
    ''' <param name="e">A <see cref="DrawItemEventArgs"/> that contains the event data.</param>
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
