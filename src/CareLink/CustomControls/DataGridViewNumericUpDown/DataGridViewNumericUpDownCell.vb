' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization

''' <summary>
'''  Defines a NumericUpDown cell type for the System.Windows.Forms.DataGridView control
''' </summary>
Public Class DataGridViewNumericUpDownCell
    Inherits DataGridViewTextBoxCell

    ' Used in TranslateAlignment function
    Private Shared ReadOnly s_anyRight As DataGridViewContentAlignment =
        DataGridViewContentAlignment.TopRight Or
        DataGridViewContentAlignment.MiddleRight Or
        DataGridViewContentAlignment.BottomRight

    Private Shared ReadOnly s_anyCenter As DataGridViewContentAlignment =
        DataGridViewContentAlignment.TopCenter Or
        DataGridViewContentAlignment.MiddleCenter Or
        DataGridViewContentAlignment.BottomCenter

    ' Default dimensions of the static rendering bitmap used for the painting
    ' of the non-edited cells
    Private Const DgvNumericUpDownCell_defaultRenderingBitmapWidth As Integer = 100

    Private Const DgvNumericUpDownCell_defaultRenderingBitmapHeight As Integer = 22

    ' Default value of the DecimalPlaces property
    Friend Const DgvNumericUpDownCell_defaultDecimalPlaces As Integer = 0

    ' Default value of the Increment property
    Friend Const DgvNumericUpDownCell_defaultIncrement As Decimal = Decimal.One

    ' Default value of the Maximum property
    Friend Const DgvNumericUpDownCell_defaultMaximum As Decimal = CDec(100.0)

    ' Default value of the Minimum property
    Friend Const DgvNumericUpDownCell_defaultMinimum As Decimal = Decimal.Zero

    ' Default value of the ThousandsSeparator property
    Friend Const DefaultThousandsSeparator As Boolean = False

    ' Type of this cell's editing control
    Private Shared ReadOnly s_defaultEditType As Type =
        GetType(DataGridViewNumericUpDownEditingControl)

    ' Type of this cell's value. The formatted value type is string,
    ' the same as the base class DataGridViewTextBoxCell
    Private Shared ReadOnly s_defaultValueType As Type = GetType(Decimal)

    ' The bitmap used to paint the non-edited cells
    ' via a call to NumericUpDown.DrawToBitmap
    <ThreadStatic>
    Private Shared s_renderingBitmap As Bitmap

    ' The NumericUpDown control used to paint the non-edited
    ' cells via a call to NumericUpDown.DrawToBitmap
    <ThreadStatic>
    Private Shared s_paintingNumericUpDown As NumericUpDown

    ' Caches the value of the DecimalPlaces property
    Private _decimalPlaces As Integer

    ' Caches the value of the Increment property
    Private _increment As Decimal

    ' Caches the value of the Minimum property
    Private _minimum As Decimal

    ' Caches the value of the Maximum property
    Private _maximum As Decimal

    ' Caches the value of the ThousandsSeparator property
    Private _thousandsSeparator As Boolean

    ''' <summary>
    '''  Constructor for the DataGridViewNumericUpDownCell cell type
    ''' </summary>
    Public Sub New()
        ' Create a thread specific bitmap used for the painting of the non-edited cells
        If s_renderingBitmap Is Nothing Then
            s_renderingBitmap = New Bitmap(
                width:=DgvNumericUpDownCell_defaultRenderingBitmapWidth,
                height:=DgvNumericUpDownCell_defaultRenderingBitmapHeight)
        End If

        ' Create a thread specific NumericUpDown control used for the painting
        ' of the non-edited cells
        If s_paintingNumericUpDown Is Nothing Then
            ' Some properties only need to be set once for the lifetime of the control:
            s_paintingNumericUpDown = New NumericUpDown With {
                .BorderStyle = BorderStyle.None,
                .Maximum = Decimal.MaxValue / 10,
                .Minimum = Decimal.MinValue / 10
            }
        End If

        ' Set the default values of the properties:
        _decimalPlaces = DgvNumericUpDownCell_defaultDecimalPlaces
        _increment = DgvNumericUpDownCell_defaultIncrement
        _minimum = DgvNumericUpDownCell_defaultMinimum
        _maximum = DgvNumericUpDownCell_defaultMaximum
        _thousandsSeparator = DefaultThousandsSeparator
    End Sub

    ''' <summary>
    '''  The DecimalPlaces property replicates the one from the
    '''  <see cref="NumericUpDown"/> control
    ''' </summary>
    <DefaultValue(DgvNumericUpDownCell_defaultDecimalPlaces)>
    Public Property DecimalPlaces As Integer
        Get
            Return _decimalPlaces
        End Get

        Set(value As Integer)
            Const message As String =
                    "The DecimalPlaces property cannot be smaller" &
                    "than 0 or larger than 99."
            If value < 0 OrElse value > 99 Then
                Throw New ArgumentOutOfRangeException(
                  paramName:=NameOf(value),
                    message)
            End If

            If _decimalPlaces <> value Then
                Me.SetDecimalPlaces(Me.RowIndex, value)
                ' Assure that the cell or column gets repainted and autosized if needed
                Me.OnCommonChange()
            End If
        End Set
    End Property

    ''' <summary>
    '''  Returns the current DataGridView EditingControl as a
    '''  <see cref="DataGridViewNumericUpDownEditingControl"/> control
    ''' </summary>
    Private ReadOnly Property EditingNumericUpDown As _
        DataGridViewNumericUpDownEditingControl

        Get
            Return TryCast(
                Me.DataGridView.EditingControl,
                DataGridViewNumericUpDownEditingControl)
        End Get
    End Property

    ''' <summary>
    '''  Define the type of the cell's editing control
    ''' </summary>
    Public Overrides ReadOnly Property EditType As Type
        Get
            Return s_defaultEditType ' the type is DataGridViewNumericUpDownEditingControl
        End Get
    End Property

    ''' <summary>
    '''  The Increment property replicates the one from the NumericUpDown control
    ''' </summary>
    Public Property Increment As Decimal
        Get
            Return _increment
        End Get

        Set(value As Decimal)
            If value < CDec(0.0) Then
                Const message As String = "The Increment property cannot be smaller than 0."
                Throw New ArgumentOutOfRangeException(paramName:=NameOf(value), message)
            End If
            _increment = value
            Me.SetIncrement(Me.RowIndex, value)
            ' No call to OnCommonChange is needed since the increment value
            ' does not affect the rendering of the cell.
        End Set
    End Property

    ''' <summary>
    '''  The Maximum property replicates the one from the NumericUpDown control
    ''' </summary>
    Public Property Maximum As Decimal
        Get
            Return _maximum
        End Get

        Set(value As Decimal)
            If _maximum <> value Then
                Me.SetMaximum(Me.RowIndex, value)
                Me.OnCommonChange()
            End If
        End Set
    End Property

    ''' <summary>
    '''  The Minimum property replicates the one from the NumericUpDown control
    ''' </summary>
    Public Property Minimum As Decimal
        Get
            Return _minimum
        End Get

        Set(value As Decimal)
            If _minimum <> value Then
                Me.SetMinimum(Me.RowIndex, value)
                Me.OnCommonChange()
            End If
        End Set
    End Property

    ''' <summary>
    '''  The ThousandsSeparator property replicates the one from the NumericUpDown control
    ''' </summary>
    <DefaultValue(DefaultThousandsSeparator)>
    Public Property ThousandsSeparator As Boolean
        Get
            Return _thousandsSeparator
        End Get

        Set(value As Boolean)
            If _thousandsSeparator <> value Then
                Me.SetThousandsSeparator(Me.RowIndex, value)
                Me.OnCommonChange()
            End If
        End Set
    End Property

    ''' <summary>
    '''  Returns the type of the cell's Value property
    ''' </summary>
    Public Overrides ReadOnly Property ValueType As Type
        Get
            Return If(MyBase.ValueType, s_defaultValueType)
        End Get
    End Property

    ''' <summary>
    '''  Clones a <see cref="DataGridViewNumericUpDownCell"/> cell,
    '''  copying all the custom properties.
    ''' </summary>
    ''' <returns>
    '''  A new <see cref="DataGridViewNumericUpDownCell"/> with the same property values.
    ''' </returns>
    Public Overrides Function Clone() As Object
        Dim dataGridViewCell As DataGridViewNumericUpDownCell =
            TryCast(MyBase.Clone(), DataGridViewNumericUpDownCell)
        If dataGridViewCell IsNot Nothing Then
            dataGridViewCell.DecimalPlaces = _decimalPlaces
            dataGridViewCell.Increment = _increment
            dataGridViewCell.Maximum = _maximum
            dataGridViewCell.Minimum = _minimum
            dataGridViewCell.ThousandsSeparator = _thousandsSeparator
        End If
        Return dataGridViewCell
    End Function

    ''' <summary>
    '''  Returns the provided value constrained to be within the minimum and maximum.
    ''' </summary>
    ''' <param name="value">The value to constrain.</param>
    ''' <returns>The constrained value.</returns>
    Private Function Constrain(value As Decimal) As Decimal
        Debug.Assert(_minimum <= _maximum)
        If value < _minimum Then
            value = _minimum
        End If
        If value > _maximum Then
            value = _maximum
        End If
        Return value
    End Function

    ''' <summary>
    '''  <see cref="DataGridView.DetachEditingControl"/> gets called by
    '''  the <see cref="DataGridView"/> control when the editing session is ending.
    '''  Clears the undo buffer of the editing control's textbox
    '''  to avoid interference between sessions.
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Public Overrides Sub DetachEditingControl()
        Dim dataGridView As DataGridView = Me.DataGridView
        If dataGridView Is Nothing OrElse dataGridView.EditingControl Is Nothing Then
            Const message As String = "Cell is detached or its grid has no editing control."
            Throw New InvalidOperationException(message)
        End If

        Dim numericUpDown As NumericUpDown =
            TryCast(dataGridView.EditingControl, NumericUpDown)
        If numericUpDown IsNot Nothing Then
            ' Editing controls get recycled. Indeed, when a DataGridViewNumericUpDownCell
            ' cell gets edited after another DataGridViewNumericUpDownCell cell,
            ' the same editing control gets reused for performance reasons
            ' (to avoid an unnecessary control destruction and creation).
            ' Here the undo buffer of the TextBox inside the NumericUpDown control
            ' gets cleared to avoid interferences between the editing sessions.
            TryCast(numericUpDown.Controls(index:=1), TextBox)?.ClearUndo()
        End If

        MyBase.DetachEditingControl()
    End Sub

    ''' <summary>
    '''  Adjusts the location and size of the editing control given
    '''  the alignment characteristics of the cell.
    ''' </summary>
    ''' <param name="editingControlBounds">The bounds of the editing control.</param>
    ''' <param name="cellStyle">The cell style to use for alignment and font.</param>
    ''' <returns>The adjusted bounds for the editing control.</returns>
    Private Shared Function GetAdjustedEditingControlBounds(
            editingControlBounds As Rectangle,
            cellStyle As DataGridViewCellStyle) As Rectangle

        ' Add a 1 pixel padding on the left and right of the editing control
        editingControlBounds.X += 1
        editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2)

        ' Adjust the vertical location of the editing control:
        Dim preferredHeight As Integer = cellStyle.Font.Height + 3
        If preferredHeight < editingControlBounds.Height Then
            Select Case cellStyle.Alignment
                Case DataGridViewContentAlignment.MiddleLeft,
                     DataGridViewContentAlignment.MiddleCenter,
                     DataGridViewContentAlignment.MiddleRight

                    editingControlBounds.Y +=
                        CInt((editingControlBounds.Height - preferredHeight) / 2)
                Case DataGridViewContentAlignment.BottomLeft,
                     DataGridViewContentAlignment.BottomCenter,
                     DataGridViewContentAlignment.BottomRight

                    editingControlBounds.Y += editingControlBounds.Height - preferredHeight
            End Select
        End If
        Return editingControlBounds
    End Function

    ''' <summary>
    '''  Customized implementation of the <see cref="GetErrorIconBounds"/>
    '''  function in order to draw the potential
    '''  error icon next to the up/down buttons and not on top of them.
    ''' </summary>
    ''' <param name="graphics">The graphics context.</param>
    ''' <param name="cellStyle">The cell style.</param>
    ''' <param name="rowIndex">The row index.</param>
    ''' <returns>The bounds for the error icon.</returns>
    Protected Overrides Function GetErrorIconBounds(
            graphics As Graphics,
            cellStyle As DataGridViewCellStyle,
            rowIndex As Integer) As Rectangle
        Const buttonsWidth As Integer = 16

        Dim errorIconBounds As Rectangle =
            MyBase.GetErrorIconBounds(graphics, cellStyle, rowIndex)
        errorIconBounds.X =
            If(Me.DataGridView.RightToLeft = RightToLeft.Yes,
               errorIconBounds.Left + buttonsWidth,
               errorIconBounds.Left - buttonsWidth)
        Return errorIconBounds
    End Function

    ''' <summary>
    '''  Customized implementation of the <see cref="GetFormattedValue"/> function
    '''  in order to include the decimal and thousand separator characters in the
    '''  formatted representation of the cell value.
    ''' </summary>
    ''' <param name="value">The value to format.</param>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="cellStyle">The cell style.</param>
    ''' <param name="valueTypeConverter">The value type converter.</param>
    ''' <param name="formattedValueTypeConverter">
    '''  The formatted value type converter.
    ''' </param>
    ''' <param name="context">The data error context.</param>
    ''' <returns>The formatted value as an object.</returns>
    Protected Overrides Function GetFormattedValue(
        value As Object,
        rowIndex As Integer,
        ByRef cellStyle As DataGridViewCellStyle,
        valueTypeConverter As TypeConverter,
        formattedValueTypeConverter As TypeConverter,
        context As DataGridViewDataErrorContexts) As Object

        ' By default, the base implementation converts the Decimal 1234.5
        ' into the string "1234.5"
        Dim formattedValue As Object = MyBase.GetFormattedValue(
            value,
            rowIndex,
            cellStyle,
            valueTypeConverter,
            formattedValueTypeConverter,
            context)
        Dim formattedNumber As String = TryCast(formattedValue, String)
        If Not String.IsNullOrEmpty(formattedNumber) AndAlso value IsNot Nothing Then
            Dim unformattedDecimal As Decimal = Convert.ToDecimal(value)
            Dim formattedDecimal As Decimal = Convert.ToDecimal(formattedNumber)
            If unformattedDecimal = formattedDecimal Then
                ' The base implementation of GetFormattedValue
                ' (which triggers the CellFormatting event) did nothing else than
                ' the typical 1234.5 to "1234.5" conversion.
                ' But depending on the values of ThousandsSeparator and DecimalPlaces,
                ' this may not be the actual string displayed.
                ' The real formatted value may be "1,234.500"
                Dim format As String =
                    $"{If(_thousandsSeparator, "N", "F")}{_decimalPlaces}"
                Return formattedDecimal.ToString(format)
            End If
        End If
        Return formattedValue
    End Function

    ''' <summary>
    '''  Custom implementation of the <see cref="GetPreferredSize"/> function.
    '''  This implementation uses the preferred size of the base
    '''  <see cref="DataGridViewTextBoxCell"/> cell and adds room for the up/down buttons.
    ''' </summary>
    ''' <param name="graphics">The graphics context.</param>
    ''' <param name="cellStyle">The cell style.</param>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="constraintSize">The constraint size.</param>
    ''' <returns>The preferred size for the cell.</returns>
    Protected Overrides Function GetPreferredSize(
        graphics As Graphics,
        cellStyle As DataGridViewCellStyle,
        rowIndex As Integer,
        constraintSize As Size) As Size
        If Me.DataGridView Is Nothing Then
            Return New Size(-1, -1)
        End If

        Dim preferredSize As Size =
            MyBase.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize)
        If constraintSize.Width = 0 Then
            ' Account for the width of the up/down buttons.
            Const buttonsWidth As Integer = 16
            ' Account for some blank pixels between the text and buttons.
            Const buttonMargin As Integer = 8
            preferredSize.Width += buttonsWidth + buttonMargin
        End If
        Return preferredSize
    End Function

    ''' <summary>
    '''  Custom implementation of the <see cref="InitializeEditingControl"/> function.
    '''  This function is called by the <see cref="DataGridView"/> control
    '''  at the beginning of an editing session.
    '''  It makes sure that the properties of the NumericUpDown editing control are
    '''  set according to the cell properties.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="initialFormattedValue">The initial formatted value.</param>
    ''' <param name="dataGridViewCellStyle">The cell style.</param>
    Public Overrides Sub InitializeEditingControl(
        rowIndex As Integer,
        initialFormattedValue As Object,
        dataGridViewCellStyle As DataGridViewCellStyle)

        MyBase.InitializeEditingControl(
            rowIndex,
            initialFormattedValue,
            dataGridViewCellStyle)
        Dim numericUpDown As NumericUpDown =
            TryCast(Me.DataGridView.EditingControl, NumericUpDown)

        If numericUpDown IsNot Nothing Then
            numericUpDown.BorderStyle = BorderStyle.None
            numericUpDown.DecimalPlaces = Me.DecimalPlaces
            numericUpDown.Increment = Me.Increment
            numericUpDown.Maximum = Me.Maximum
            numericUpDown.Minimum = Me.Minimum
            numericUpDown.ThousandsSeparator = Me.ThousandsSeparator
            Dim initialFormattedValueStr As String = TryCast(initialFormattedValue, String)
            numericUpDown.Text = If(initialFormattedValueStr, String.Empty)
        End If
    End Sub

    ''' <summary>
    '''  Custom implementation of the <see cref="KeyEntersEditMode"/> function.
    '''  This function is called by the DataGridView control
    '''  to decide whether a keystroke must start an editing session or not.
    '''  In this case, a new session is started when a digit or negative sign key is hit.
    ''' </summary>
    ''' <param name="e">The key event arguments.</param>
    ''' <returns>
    '''  <see langword="True"/> if the key should start editing;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Overrides Function KeyEntersEditMode(e As KeyEventArgs) As Boolean
        Dim numberFormatInfo As NumberFormatInfo = CultureInfo.CurrentCulture.NumberFormat
        Dim negativeSignKey As Keys = Keys.None
        Dim negativeSignStr As String = numberFormatInfo.NegativeSign
        If Not String.IsNullOrEmpty(negativeSignStr) AndAlso
                negativeSignStr.Length = 1 Then
            negativeSignKey = CType(VkKeyScan(negativeSignStr(index:=0)), Keys)
        End If

        Return (Char.IsDigit(ChrW(e.KeyCode)) OrElse
            (e.KeyCode >= Keys.NumPad0 AndAlso e.KeyCode <= Keys.NumPad9) OrElse
             negativeSignKey = e.KeyCode OrElse
             Keys.Subtract = e.KeyCode) AndAlso
            Not e.Shift AndAlso Not e.Alt AndAlso Not e.Control
    End Function

    ''' <summary>
    '''  Called when a cell characteristic that affects its rendering
    '''  and/or preferred size has changed. This implementation only takes
    '''  care of repainting the cells. The DataGridView's autosizing methods
    '''  also need to be called in cases where some grid elements AutoSize.
    ''' </summary>
    Private Sub OnCommonChange()
        If Me.DataGridView IsNot Nothing AndAlso
           Not Me.DataGridView.IsDisposed AndAlso
           Not Me.DataGridView.Disposing Then

            If Me.RowIndex = -1 Then
                ' Invalidate and AutoSize column
                Me.DataGridView.InvalidateColumn(Me.ColumnIndex)
            Else
                ' The DataGridView control exposes a public method called UpdateCellValue
                ' that invalidates the cell so that it gets repainted and also triggers all
                ' the necessary autosizing: the cell's column and/or row, the column headers
                ' and the row headers are autosized depending on their AutoSize settings.
                Me.DataGridView.UpdateCellValue(Me.ColumnIndex, Me.RowIndex)

                ' TODO: Add code to AutoSize the cell's column, the rows, the column headers
                ' and the row headers depending on their AutoSize settings.
                ' The DataGridView control does not expose a public method
                ' that takes care of this.
            End If
        End If
    End Sub

    ''' <summary>
    '''  Determines whether this cell, at the given row index,
    '''  shows the grid's editing control or not.
    '''  The row index needs to be provided as a parameter because
    '''  this cell may be shared among multiple rows.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <returns>
    '''  <see langword="True"/> if this cell owns the editing control;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Private Function OwnsEditingNumericUpDown(rowIndex As Integer) As Boolean
        If rowIndex = -1 OrElse Me.DataGridView Is Nothing Then
            Return False
        End If
        Dim upDownEditingControl As DataGridViewNumericUpDownEditingControl =
            TryCast(Me.DataGridView.EditingControl, DataGridViewNumericUpDownEditingControl)
        Return upDownEditingControl IsNot Nothing AndAlso rowIndex =
            CType(upDownEditingControl, IDataGridViewEditingControl).EditingControlRowIndex
    End Function

    ''' <summary>
    '''  Custom paints the cell. The base implementation of the
    '''  <see cref="DataGridViewTextBoxCell"/> type is called first,
    '''  dropping the icon error and content foreground parts.
    '''  Those two parts are painted by this custom implementation.
    '''  In this sample, the non-edited <see cref="NumericUpDown"/> control
    '''  is painted by using a call to <see cref="Control.DrawToBitmap"/>.
    '''  This is an easy solution for painting controls but it's not
    '''  necessarily the most performant.
    ''' </summary>
    ''' <param name="graphics">The graphics context.</param>
    ''' <param name="clipBounds">The clip bounds.</param>
    ''' <param name="cellBounds">The cell bounds.</param>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="cellState">The cell state.</param>
    ''' <param name="value">The cell value.</param>
    ''' <param name="formattedValue">The formatted value.</param>
    ''' <param name="errorText">The error text.</param>
    ''' <param name="cellStyle">The cell style.</param>
    ''' <param name="advancedBorderStyle">The advanced border style.</param>
    ''' <param name="paintParts">The paint parts.</param>
    <DebuggerNonUserCode()>
    Protected Overrides Sub Paint(
        graphics As Graphics,
        clipBounds As Rectangle,
        cellBounds As Rectangle,
        rowIndex As Integer,
        cellState As DataGridViewElementStates,
        value As Object,
        formattedValue As Object,
        errorText As String,
        cellStyle As DataGridViewCellStyle,
        advancedBorderStyle As DataGridViewAdvancedBorderStyle,
        paintParts As DataGridViewPaintParts)

        If Me.DataGridView Is Nothing Then
            Return
        End If

        ' First paint the borders and background of the cell.
        Const dgvPaintParts As DataGridViewPaintParts =
            Not (DataGridViewPaintParts.ErrorIcon Or
                 DataGridViewPaintParts.ContentForeground)

        MyBase.Paint(
            graphics,
            clipBounds,
            cellBounds,
            rowIndex,
            cellState,
            value,
            formattedValue,
            errorText,
            cellStyle,
            advancedBorderStyle,
            paintParts:=paintParts And dgvPaintParts)

        Dim ptCurrentCell As Point = Me.DataGridView.CurrentCellAddress
        Dim cellCurrent As Boolean =
            ptCurrentCell.X = Me.ColumnIndex AndAlso ptCurrentCell.Y = rowIndex
        Dim cellEdited As Boolean =
            cellCurrent AndAlso Me.DataGridView.EditingControl IsNot Nothing

        ' If the cell is in editing mode, there is nothing else to paint
        If Not cellEdited Then
            If (paintParts And DataGridViewPaintParts.ContentForeground) <> 0 Then
                ' Paint a NumericUpDown control
                ' Take the borders into account
                Dim borderWidths As Rectangle = Me.BorderWidths(advancedBorderStyle)
                Dim editingControlBounds As Rectangle = cellBounds
                editingControlBounds.Offset(borderWidths.X, borderWidths.Y)
                editingControlBounds.Width -= borderWidths.Right
                editingControlBounds.Height -= borderWidths.Bottom
                ' Also take the padding into account
                If cellStyle.Padding <> Padding.Empty Then
                    If Me.DataGridView.RightToLeft = RightToLeft.Yes Then
                        editingControlBounds.Offset(
                            x:=cellStyle.Padding.Right,
                            y:=cellStyle.Padding.Top)
                    Else
                        editingControlBounds.Offset(
                            x:=cellStyle.Padding.Left,
                            y:=cellStyle.Padding.Top)
                    End If
                    editingControlBounds.Width -= cellStyle.Padding.Horizontal
                    editingControlBounds.Height -= cellStyle.Padding.Vertical
                End If
                ' Determine the NumericUpDown control location
                editingControlBounds =
                    GetAdjustedEditingControlBounds(editingControlBounds, cellStyle)

                Dim cellSelected As Boolean =
                    (cellState And DataGridViewElementStates.Selected) <> 0

                If s_renderingBitmap.Width < editingControlBounds.Width OrElse
                    s_renderingBitmap.Height < editingControlBounds.Height Then
                    ' The static bitmap is too small, a bigger one needs to be allocated.
                    s_renderingBitmap.Dispose()
                    s_renderingBitmap =
                        New Bitmap(editingControlBounds.Width, editingControlBounds.Height)
                End If
                ' Make sure the NumericUpDown control is parented to a visible control
                If s_paintingNumericUpDown.Parent Is Nothing OrElse
                    Not s_paintingNumericUpDown.Parent.Visible Then

                    s_paintingNumericUpDown.Parent = Me.DataGridView
                End If
                ' Set all the relevant properties
                s_paintingNumericUpDown.TextAlign =
                    TranslateAlignment(align:=cellStyle.Alignment)
                s_paintingNumericUpDown.DecimalPlaces = Me.DecimalPlaces
                s_paintingNumericUpDown.ThousandsSeparator = Me.ThousandsSeparator
                s_paintingNumericUpDown.Font = cellStyle.Font
                s_paintingNumericUpDown.Width = editingControlBounds.Width
                s_paintingNumericUpDown.Height = editingControlBounds.Height
                s_paintingNumericUpDown.RightToLeft = Me.DataGridView.RightToLeft
                s_paintingNumericUpDown.Location =
                    New Point(x:=0, y:=-s_paintingNumericUpDown.Height - 100)
                s_paintingNumericUpDown.Text = TryCast(formattedValue, String)

                Dim parts As DataGridViewPaintParts =
                    paintParts And DataGridViewPaintParts.SelectionBackground
                Dim baseColor As Color =
                    If(parts <> 0 AndAlso cellSelected,
                       cellStyle.SelectionBackColor,
                       cellStyle.BackColor)

                If (paintParts And DataGridViewPaintParts.Background) <> 0 Then
                    If baseColor.A < 255 Then
                        ' The NumericUpDown control does not support transparent back colors
                        baseColor = Color.FromArgb(alpha:=255, baseColor)
                    End If
                    s_paintingNumericUpDown.BackColor = baseColor
                End If
                ' Finally paint the NumericUpDown control
                Dim targetBounds As New Rectangle(
                    x:=0,
                    y:=0,
                    editingControlBounds.Width,
                    editingControlBounds.Height)
                If targetBounds.Width > 0 AndAlso targetBounds.Height > 0 Then
                    s_paintingNumericUpDown.DrawToBitmap(
                        bitmap:=s_renderingBitmap, targetBounds)
                    graphics.DrawImage(
                        image:=s_renderingBitmap,
                        destRect:=New Rectangle(
                            editingControlBounds.Location,
                            editingControlBounds.Size),
                        srcRect:=targetBounds,
                        srcUnit:=GraphicsUnit.Pixel)
                End If
            End If
            If (paintParts And DataGridViewPaintParts.ErrorIcon) <> 0 Then
                ' Paint the potential error icon on top of the NumericUpDown control
                MyBase.Paint(
                    graphics,
                    clipBounds,
                    cellBounds,
                    rowIndex,
                    cellState,
                    value,
                    formattedValue,
                    errorText,
                    cellStyle,
                    advancedBorderStyle,
                    paintParts:=DataGridViewPaintParts.ErrorIcon)
            End If
        End If
    End Sub

    ''' <summary>
    '''  Custom implementation of the <see cref="PositionEditingControl"/> method
    '''  called by the <see cref="DataGridView"/> control when it
    '''  needs to relocate and/or resize the editing control.
    ''' </summary>
    ''' <param name="setLocation">Whether to set the location.</param>
    ''' <param name="setSize">Whether to set the size.</param>
    ''' <param name="cellBounds">The cell bounds.</param>
    ''' <param name="cellClip">The cell clip rectangle.</param>
    ''' <param name="cellStyle">The cell style.</param>
    ''' <param name="singleVerticalBorderAdded">
    '''  Whether a single vertical border is added.
    ''' </param>
    ''' <param name="singleHorizontalBorderAdded">
    '''  Whether a single horizontal border is added.
    ''' </param>
    ''' <param name="isFirstDisplayedColumn">
    '''  Whether this is the first displayed column.
    ''' </param>
    ''' <param name="isFirstDisplayedRow">Whether this is the first displayed row.</param>
    Public Overrides Sub PositionEditingControl(
        setLocation As Boolean,
        setSize As Boolean,
        cellBounds As Rectangle,
        cellClip As Rectangle,
        cellStyle As DataGridViewCellStyle,
        singleVerticalBorderAdded As Boolean,
        singleHorizontalBorderAdded As Boolean,
        isFirstDisplayedColumn As Boolean,
        isFirstDisplayedRow As Boolean)

        Dim editingControlBounds As Rectangle =
            Me.PositionEditingPanel(
                cellBounds,
                cellClip,
                cellStyle,
                singleVerticalBorderAdded,
                singleHorizontalBorderAdded,
                isFirstDisplayedColumn,
                isFirstDisplayedRow)
        editingControlBounds =
            GetAdjustedEditingControlBounds(editingControlBounds, cellStyle)
        Me.DataGridView.EditingControl.Location =
            New Point(editingControlBounds.X, editingControlBounds.Y)
        Me.DataGridView.EditingControl.Size =
            New Size(editingControlBounds.Width, editingControlBounds.Height)
    End Sub

    ''' <summary>
    '''  Utility function that sets a new value for the
    '''  <see cref="DecimalPlaces"/> property of the cell.
    '''  This function is used by the cell and column <see cref="DecimalPlaces"/> property.
    '''  The column uses this method instead of the <see cref="DecimalPlaces"/>
    '''  property for performance reasons.
    '''  This way the column can invalidate the entire column at once instead
    '''  of invalidating each cell of the column individually.
    '''  A row index needs to be provided as a parameter because
    '''  this cell may be shared among multiple rows.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="value">The new decimal places value.</param>
    Friend Sub SetDecimalPlaces(rowIndex As Integer, value As Integer)
        Debug.Assert(value >= 0 AndAlso value <= 99)
        _decimalPlaces = value
        If Me.OwnsEditingNumericUpDown(rowIndex) Then
            Me.EditingNumericUpDown.DecimalPlaces = value
        End If
    End Sub

    ''' <summary>
    '''  Utility function that sets a new value for the <see cref="Increment"/> property
    '''  of the cell. This function is used by the cell and
    '''  column <see cref="Increment"/> property. A row index needs to be provided
    '''  as a parameter because this cell may be shared among multiple rows.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="value">The new increment value.</param>
    Friend Sub SetIncrement(rowIndex As Integer, value As Decimal)
        Debug.Assert(condition:=value >= CDec(0.0))
        _increment = value
        If Me.OwnsEditingNumericUpDown(rowIndex) Then
            Me.EditingNumericUpDown.Increment = value
        End If
    End Sub

    ''' <summary>
    '''  Utility function that sets a new value for the <see cref="Maximum"/> property
    '''  of the cell. This function is used by the cell and
    '''  column <see cref="Maximum"/> property.
    '''  The column uses this method instead of the <see cref="Maximum"/> property
    '''  for performance reasons.
    '''  This way the column can invalidate the entire column at once instead
    '''  of invalidating each cell of the column individually.
    '''  A row index needs to be provided as a parameter because
    '''  this cell may be shared among multiple rows.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="value">The new maximum value.</param>
    Friend Sub SetMaximum(rowIndex As Integer, value As Decimal)
        Debug.Assert(value >= 0D)
        _maximum = value
        If _minimum > _maximum Then
            _minimum = _maximum
        End If
        Dim cellValue As Object = Me.GetValue(rowIndex)
        If cellValue IsNot Nothing Then
            Dim currentValue As Decimal = Convert.ToDecimal(cellValue)
            Dim constrainedValue As Decimal = Me.Constrain(currentValue)
            If constrainedValue <> currentValue Then
                Me.SetValue(rowIndex, value:=constrainedValue)
            End If
        End If
        Debug.Assert(condition:=_maximum = value)
        If Me.OwnsEditingNumericUpDown(rowIndex) Then
            Me.EditingNumericUpDown.Maximum = value
        End If
    End Sub

    ''' <summary>
    '''  Utility function that sets a new value for the <see cref="Minimum"/> property
    '''  of the cell. This function is used by the cell and column Minimum property.
    '''  The column uses this method instead of the Minimum property for performance
    '''  reasons. This way the column can invalidate the entire column at once instead
    '''  of invalidating each cell of the column individually.
    '''  A row index needs to be provided as a parameter because this cell may
    '''  be shared among multiple rows.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="value">The new minimum value.</param>
    Friend Sub SetMinimum(rowIndex As Integer, value As Decimal)
        _minimum = value
        If _minimum > _maximum Then
            _maximum = value
        End If
        Dim cellValue As Object = Me.GetValue(rowIndex)
        If cellValue IsNot Nothing Then
            Dim currentValue As Decimal = Convert.ToDecimal(value:=cellValue)
            Dim constrainedValue As Decimal = Me.Constrain(value:=currentValue)
            If constrainedValue <> currentValue Then
                Me.SetValue(rowIndex, value:=constrainedValue)
            End If
        End If
        Debug.Assert(condition:=_minimum = value)
        If Me.OwnsEditingNumericUpDown(rowIndex) Then
            Me.EditingNumericUpDown.Minimum = value
        End If
    End Sub

    ''' <summary>
    '''  Utility function that sets a new value for the <see cref="ThousandsSeparator"
    '''  /> property of the cell. This function is used by the cell and column
    '''  ThousandsSeparator property. The column uses this method instead of the
    '''  ThousandsSeparator property for performance reasons.
    '''  This way the column can invalidate the entire column at once instead of
    '''  invalidating each cell of the column individually.
    '''  A row index needs to be provided as a parameter because this cell may be shared
    '''  among multiple rows.
    ''' </summary>
    ''' <param name="rowIndex">The row index.</param>
    ''' <param name="value">The new thousands separator value.</param>
    Friend Sub SetThousandsSeparator(rowIndex As Integer, value As Boolean)
        _thousandsSeparator = value
        If Me.OwnsEditingNumericUpDown(rowIndex) Then
            Me.EditingNumericUpDown.ThousandsSeparator = value
        End If
    End Sub

    ''' <summary>
    '''  Returns a standard textual representation of the cell.
    ''' </summary>
    ''' <returns>A string describing the cell's column and row index.</returns>
    Public Overrides Function ToString() As String
        Dim col As String =
            Me.ColumnIndex.ToString(CultureInfo.CurrentCulture)
        Dim row As String =
            Me.RowIndex.ToString(CultureInfo.CurrentCulture)
        Return $"DataGridViewNumericUpDownCell {{ ColumnIndex={col}, RowIndex={row} }}"
    End Function

    ''' <summary>
    '''  Little utility function used by both the cell and column types to translate
    '''  a <see cref="DataGridViewContentAlignment"/> value into a
    '''  <see cref="HorizontalAlignment"/> value.
    ''' </summary>
    ''' <param name="align">The content alignment value.</param>
    ''' <returns>The corresponding <see cref="HorizontalAlignment"/>.</returns>
    Friend Shared Function TranslateAlignment(
        align As DataGridViewContentAlignment) As HorizontalAlignment

        If (align And s_anyRight) <> 0 Then
            Return HorizontalAlignment.Right
        ElseIf (align And s_anyCenter) <> 0 Then
            Return HorizontalAlignment.Center
        Else
            Return HorizontalAlignment.Left
        End If
    End Function

End Class
