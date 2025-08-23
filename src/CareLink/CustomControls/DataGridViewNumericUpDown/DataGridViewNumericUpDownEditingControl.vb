' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.InteropServices

''' <summary>
'''  Defines the editing control for the <see cref="DataGridViewNumericUpDownCell"/>
'''  custom cell type. This control is hosted within a <see cref="DataGridView"/>
'''  when a cell enters edit mode, providing numeric up-down editing functionality.
''' </summary>
Friend Class DataGridViewNumericUpDownEditingControl
    Inherits NumericUpDown
    Implements IDataGridViewEditingControl

    ''' <summary>
    '''  Forwards keyboard messages to the child <see cref="TextBox"/> control.
    ''' </summary>
    ''' <param name="hWnd">Handle to the window receiving the message.</param>
    ''' <param name="msg">Message ID.</param>
    ''' <param name="wParam">First message parameter.</param>
    ''' <param name="lParam">Second message parameter.</param>
    ''' <returns>Result of the message processing.</returns>
    <DllImport("USER32.DLL", CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Private Shared Function SendMessage(
        hWnd As IntPtr,
        msg As Integer,
        wParam As IntPtr,
        lParam As IntPtr) As IntPtr
    End Function

    ''' <summary>
    '''  The <see cref="DataGridView"/> that owns this editing control.
    ''' </summary>
    Private _dataGridView As DataGridView

    ''' <summary>
    '''  Indicates whether the editing control's value has changed.
    ''' </summary>
    Private _valueChanged As Boolean

    ''' <summary>
    '''  The row index in which the editing control resides.
    ''' </summary>
    Private _rowIndex As Integer

    ''' <summary>
    '''  Initializes a new instance of the
    '''  <see cref="DataGridViewNumericUpDownEditingControl"/> class.
    ''' </summary>
    Public Sub New()
        ' The editing control must not be part of the tabbing loop
        Me.TabStop = False
    End Sub

    ' Beginning of the IDataGridViewEditingControl interface implementation

    ''' <summary>
    '''  Gets or sets the <see cref="DataGridView"/> that uses this editing control.
    ''' </summary>
    Public Overridable Property EditingControlDataGridView As DataGridView _
        Implements IDataGridViewEditingControl.EditingControlDataGridView

        Get
            Return _dataGridView
        End Get

        Set(value As DataGridView)
            _dataGridView = value
        End Set
    End Property

    ''' <summary>
    '''  Gets or sets the current formatted value of the editing control.
    ''' </summary>
    Public Overridable Property EditingControlFormattedValue As Object _
        Implements IDataGridViewEditingControl.EditingControlFormattedValue

        Get
            Const context As DataGridViewDataErrorContexts =
                DataGridViewDataErrorContexts.Formatting
            Return Me.GetEditingControlFormattedValue(context)
        End Get

        Set(value As Object)
            Me.Text = CStr(value)
        End Set
    End Property

    ''' <summary>
    '''  Gets or sets the row index in which the editing control resides.
    ''' </summary>
    Public Overridable Property EditingControl_RowIndex As Integer _
        Implements IDataGridViewEditingControl.EditingControlRowIndex

        Get
            Return _rowIndex
        End Get

        Set(value As Integer)
            _rowIndex = value
        End Set
    End Property

    ''' <summary>
    '''  Gets or sets a value indicating whether the value
    '''  of the editing control has changed.
    ''' </summary>
    Public Overridable Property EditingControl_ValueChanged As Boolean _
        Implements IDataGridViewEditingControl.EditingControlValueChanged

        Get
            Return _valueChanged
        End Get

        Set(value As Boolean)
            _valueChanged = value
        End Set
    End Property

    ''' <summary>
    '''  Gets the cursor that must be used for the editing panel
    '''  (the parent of the editing control).
    ''' </summary>
    Public Overridable ReadOnly Property EditingPanelCursor As Cursor _
        Implements IDataGridViewEditingControl.EditingPanelCursor

        Get
            Return Cursors.Default
        End Get
    End Property

    ''' <summary>
    '''  Gets a value indicating whether the editing control needs to be
    '''  repositioned when its value changes.
    ''' </summary>
    Public Overridable ReadOnly Property RepositionEditingControlOnValueChange As Boolean _
        Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange

        Get
            Return False
        End Get
    End Property

    ''' <summary>
    '''  Applies the specified cell style to the editing control.
    '''  Called by the grid before the editing control is shown so it can
    '''  adapt to the provided cell style.
    ''' </summary>
    ''' <param name="dataGridViewCellStyle">
    '''  The cell style to apply to the editing control.
    '''  This style is typically the style of the cell that is being edited,
    '''  and it may include properties such as font, back color, fore color,
    '''  and text alignment that should be applied to the control.
    ''' </param>
    Public Overridable Sub ApplyCellStyleToEditingControl(
        dataGridViewCellStyle As DataGridViewCellStyle) _
        Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

        Me.Font = dataGridViewCellStyle.Font
        If dataGridViewCellStyle.BackColor.A < 255 Then
            ' The NumericUpDown control does not support transparent back colors
            Dim opaqueBackColor As Color =
                Color.FromArgb(alpha:=255, baseColor:=dataGridViewCellStyle.BackColor)
            Me.BackColor = opaqueBackColor
            _dataGridView.EditingPanel.BackColor = opaqueBackColor
        Else
            Me.BackColor = dataGridViewCellStyle.BackColor
        End If
        Me.ForeColor = dataGridViewCellStyle.ForeColor

        Dim align As DataGridViewContentAlignment = dataGridViewCellStyle.Alignment
        Me.TextAlign =
            DataGridViewNumericUpDownCell.TranslateAlignment(align)
    End Sub

    ''' <summary>
    '''  Determines whether the editing control is interested in the specified key.
    '''  Called by the grid on keystrokes to determine if the editing control wants the key.
    ''' </summary>
    ''' <param name="keyData">The key data.</param>
    ''' <param name="dataGridViewWantsInputKey">
    '''  Whether the DataGridView wants the input key.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the editing control wants the key;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Public Overridable Function EditingControlWantsInputKey(
        keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean _
        Implements IDataGridViewEditingControl.EditingControlWantsInputKey

        Select Case keyData And Keys.KeyCode
            Case Keys.Right
                Dim textBox As TextBox = TryCast(Me.Controls(index:=1), TextBox)
                If textBox IsNot Nothing Then
                    ' If the end of the selection is at the end of the string,
                    ' let the DataGridView treat the key message
                    Dim isRTL As Boolean = Me.RightToLeft = RightToLeft.Yes
                    Dim hasSelectionAtEnd As Boolean =
                        textBox.SelectionLength = 0 AndAlso
                        textBox.SelectionStart = textBox.Text.Length
                    Dim hasSelectionAtStart As Boolean =
                        textBox.SelectionLength = 0 AndAlso
                        textBox.SelectionStart = 0

                    If (Not isRTL AndAlso Not hasSelectionAtEnd) OrElse
                        (isRTL AndAlso Not hasSelectionAtStart) Then
                        Return True
                    End If
                End If
                Exit Select

            Case Keys.Left
                Dim textBox As TextBox = TryCast(Me.Controls(index:=1), TextBox)
                If textBox IsNot Nothing Then
                    ' If the end of the selection is at the beginning of the string
                    ' or if the entire text is selected and we did not start editing,
                    ' send this character to the DataGridView, else process the key message.

                    Dim isLtr As Boolean = Me.RightToLeft = RightToLeft.No
                    Dim isRtl As Boolean = Me.RightToLeft = RightToLeft.Yes

                    Dim atStart As Boolean =
                        textBox.SelectionLength = 0 AndAlso textBox.SelectionStart = 0

                    Dim atEnd As Boolean =
                        textBox.SelectionLength = 0 AndAlso
                        textBox.SelectionStart = textBox.Text.Length

                    If (isLtr AndAlso Not atStart) OrElse (isRtl AndAlso Not atEnd) Then
                        Return True
                    End If

                End If
                Exit Select

            Case Keys.Down
                ' If the current value hasn't reached its minimum yet,
                ' handle the key. Otherwise let
                ' the grid handle it.
                If Me.Value > Me.Minimum Then
                    Return True
                End If

            Case Keys.Up
                ' If the current value hasn't reached its maximum yet,
                ' handle the key. Otherwise let
                ' the grid handle it.
                If Me.Value < Me.Maximum Then
                    Return True
                End If

            Case Keys.Home, Keys.[End]
                ' Let the grid handle the key if the entire text is selected.
                Dim textBox As TextBox = TryCast(Me.Controls(index:=1), TextBox)
                If textBox IsNot Nothing Then
                    If textBox.SelectionLength <> textBox.Text.Length Then
                        Return True
                    End If
                End If
                Exit Select

            Case Keys.Delete
                ' Let the grid handle the key if the caret is at the end of the text.
                Dim textBox As TextBox = TryCast(Me.Controls(index:=1), TextBox)
                If textBox IsNot Nothing Then
                    If textBox.SelectionLength > 0 OrElse
                            textBox.SelectionStart < textBox.Text.Length Then
                        Return True
                    End If
                End If
                Exit Select
        End Select
        Return Not dataGridViewWantsInputKey
    End Function

    ''' <summary>
    '''  Returns the current formatted value of the editing control.
    ''' </summary>
    ''' <param name="context">
    '''  A bitwise combination of <see cref="DataGridViewDataErrorContexts"/>
    '''  values that specifies the context in which the data is needed.
    ''' </param>
    ''' <returns>The formatted value of the editing control.</returns>
    Public Overridable Function GetEditingControlFormattedValue(
        context As DataGridViewDataErrorContexts) As Object _
        Implements IDataGridViewEditingControl.GetEditingControlFormattedValue

        Dim userEdit As Boolean = Me.UserEdit
        Try
            ' Prevent the Value from being set to Maximum or Minimum when
            ' the cell is being painted.
            Me.UserEdit = (context And DataGridViewDataErrorContexts.Display) = 0
            Dim format As String =
                $"{If(Me.ThousandsSeparator, "N", "F")}{Me.DecimalPlaces}"
            Return Me.Value.ToString(format)
        Finally
            Me.UserEdit = userEdit
        End Try
    End Function

    ''' <summary>
    '''  Prepares the editing control for editing.
    '''  Called by the grid to give the editing control a chance
    '''  to prepare itself for the editing session.
    ''' </summary>
    ''' <param name="selectAll">true to select all text; otherwise, false.</param>
    Public Overridable Sub PrepareEditingControlForEdit(selectAll As Boolean) _
        Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

        Dim textBox As TextBox = TryCast(Me.Controls(index:=1), TextBox)
        If textBox IsNot Nothing Then
            If selectAll Then
                textBox.SelectAll()
            Else
                ' Do not select all the text, but
                ' position the caret at the end of the text
                textBox.SelectionStart = textBox.Text.Length
            End If
        End If
    End Sub

    ''' <summary>
    '''  Updates the local dirty state and notifies the grid of the value change.
    ''' </summary>
    Private Sub NotifyDataGridViewOfValueChange()
        If Not _valueChanged Then
            _valueChanged = True
            _dataGridView.NotifyCurrentCellDirty(dirty:=True)
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Control.KeyPress"/> event to detect value changes
    '''  and notify the grid.
    ''' </summary>
    ''' <param name="e">
    '''  A <see cref="KeyPressEventArgs"/> that contains the event data.
    ''' </param>
    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        ' The value changes when a digit, the decimal separator, the group separator or
        ' the negative sign is pressed.
        Dim notifyValueChange As Boolean = False
        If Char.IsDigit(e.KeyChar) Then
            notifyValueChange = True
        Else
            Dim numberFormatInfo As Globalization.NumberFormatInfo =
                Globalization.CultureInfo.CurrentCulture.NumberFormat
            Dim decimalSeparatorStr As String = numberFormatInfo.NumberDecimalSeparator
            Dim groupSeparatorStr As String = numberFormatInfo.NumberGroupSeparator
            Dim negativeSignStr As String = numberFormatInfo.NegativeSign
            If Not String.IsNullOrEmpty(decimalSeparatorStr) AndAlso
               decimalSeparatorStr.Length = 1 Then

                notifyValueChange = decimalSeparatorStr(index:=0) = e.KeyChar
            End If
            If Not notifyValueChange AndAlso
               Not String.IsNullOrEmpty(value:=groupSeparatorStr) AndAlso
               groupSeparatorStr.Length = 1 Then

                notifyValueChange = groupSeparatorStr(index:=0) = e.KeyChar
            End If
            If Not notifyValueChange AndAlso
               Not String.IsNullOrEmpty(value:=negativeSignStr) AndAlso
               negativeSignStr.Length = 1 Then

                notifyValueChange = negativeSignStr(index:=0) = e.KeyChar
            End If
        End If

        If notifyValueChange Then
            ' Let the DataGridView know about the value change
            Me.NotifyDataGridViewOfValueChange()
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="NumericUpDown.ValueChanged"/> event to notify the
    '''  grid of value changes.
    ''' </summary>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overrides Sub OnValueChanged(e As EventArgs)
        MyBase.OnValueChanged(e)
        If Me.Focused Then
            ' Let the DataGridView know about the value change
            Me.NotifyDataGridViewOfValueChange()
        End If
    End Sub

    ''' <summary>
    '''  Forwards certain keyboard messages to the inner <see cref="TextBox"/>
    '''  of the <see cref="NumericUpDown"/> control
    '''  so that the first character pressed appears in it.
    ''' </summary>
    ''' <param name="m">
    '''  A <see cref="Message"/> that represents the window message to process.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if the message was processed;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    Protected Overrides Function ProcessKeyEventArgs(ByRef m As Message) As Boolean
        Dim textBox As TextBox = TryCast(Me.Controls(index:=1), TextBox)
        If textBox IsNot Nothing Then
            SendMessage(hWnd:=textBox.Handle, m.Msg, m.WParam, m.LParam)
            Return True
        Else
            Return MyBase.ProcessKeyEventArgs(m)
        End If
    End Function

End Class
