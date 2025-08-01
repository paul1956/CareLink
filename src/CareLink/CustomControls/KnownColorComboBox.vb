' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides a <see cref="ComboBox"/> that displays all known colors except system and transparent colors.
''' </summary>
''' <remarks>
'''  <para>
'''   The <see cref="KnownColorComboBox"/> control allows users to select from a list of known colors,
'''   excluding system and transparent colors. The control supports owner-drawn items to display color
'''   swatches and their names.
'''  </para>
'''  <para>
'''   The <see cref="SelectedItem"/>, <see cref="SelectedText"/>, and <see cref="SelectedValue"/> properties
'''   provide convenient access to the selected color's name and value.
'''  </para>
''' </remarks>
Public Class KnownColorComboBox
    Inherits ComboBox

    Private ReadOnly _allKnownColors As New SortedDictionary(Of String, KnownColor)

    ''' <summary>
    '''  Initializes a new instance of the <see cref="KnownColorComboBox"/> class and populates the color list.
    ''' </summary>
    ''' <remarks>
    '''  <para>
    '''   The constructor filters out <see cref="KnownColor.Transparent"/> and all system colors, ensuring only
    '''   unique, non-system known colors are available for selection.
    '''  </para>
    ''' </remarks>
    Public Sub New()
        MyBase.New
        Dim kColor As Color
        For Each value As KnownColor In [Enum].GetValues(Of KnownColor)()
            If value = KnownColor.Transparent Then Continue For
            kColor = Color.FromKnownColor(color:=value)
            If kColor.IsSystemColor OrElse _allKnownColors.ContainsValue(value) Then
                Continue For
            End If
            _allKnownColors.Add(key:=kColor.Name, value)
        Next
    End Sub

    ''' <summary>
    '''  Gets or sets the selected item as a <see cref="KeyValuePair(Of String, KnownColor)"/>.
    ''' </summary>
    ''' <value>
    '''  The selected <see cref="KeyValuePair(Of String, KnownColor)"/> in the combo box.
    ''' </value>
    ''' <remarks>
    '''  <para>
    '''   This property shadows the base <see cref="ComboBox.SelectedItem"/> property to provide
    '''   strongly-typed access to the selected color.
    '''  </para>
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
    '''  Gets the name of the selected color.
    ''' </summary>
    ''' <value>
    '''  The name of the selected color as a <see langword="String"/>.
    ''' </value>
    ''' <remarks>
    '''  <para>
    '''   This property provides the color name of the currently selected item.
    '''  </para>
    ''' </remarks>
    Public Shadows ReadOnly Property SelectedText() As String
        Get
            Return Me.SelectedItem.Key
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets the selected color value.
    ''' </summary>
    ''' <value>
    '''  The selected <see cref="KnownColor"/> value.
    ''' </value>
    ''' <remarks>
    '''  <para>
    '''   Setting this property updates the selected item to match the specified <see cref="KnownColor"/>.
    '''  </para>
    ''' </remarks>
    Public Shadows Property SelectedValue() As KnownColor
        Get
            Return Me.SelectedItem.Value
        End Get
        Set(item As KnownColor)
            Me.SelectedItem = KeyValuePair.Create(key:=GetNameFromKnownColor(item), value:=item)
        End Set
    End Property

    ''' <summary>
    '''  Initializes the layout and populates the combo box with known colors.
    ''' </summary>
    ''' <remarks>
    '''  <para>
    '''   This method is called during control initialization to set up the owner-drawn mode and
    '''   populate the items list with all known colors.
    '''  </para>
    ''' </remarks>
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

    ''' <summary>
    '''  Draws the items in the combo box with their respective color swatches.
    ''' </summary>
    ''' <param name="e">The <see cref="DrawItemEventArgs"/> containing event data.</Param>
    ''' <remarks>
    '''  <para>
    '''   This method overrides the default drawing behavior to display each color with its name and
    '''   a background swatch.
    '''  </para>
    ''' </remarks>
    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        If e.Index = -1 Then Exit Sub

        Dim item As KeyValuePair(Of String, KnownColor) = CType(Me.Items(e.Index), KeyValuePair(Of String, KnownColor))
        Dim backColor As Color = Color.FromKnownColor(color:=item.Value)
        Dim eBounds As Rectangle = e.Bounds
        Using b As Brush = New SolidBrush(color:=backColor)
            Dim pt As New Point(x:=eBounds.X, y:=eBounds.Top)
            e.Graphics.FillRectangle(brush:=b, eBounds.X, eBounds.Y, eBounds.Width, eBounds.Height)
            TextRenderer.DrawText(
                dc:=e.Graphics,
                text:=item.Key,
                Me.Font,
                pt,
                foreColor:=backColor.ContrastingColor(),
                backColor)
        End Using
        e.DrawFocusRectangle()
    End Sub

End Class
