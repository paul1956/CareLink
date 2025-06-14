' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Defines the editing control for the DataGridViewNumericUpDownCell custom cell type.
''' </summary>
Friend Class DataGridViewNumericUpDownEditingControl
    Inherits NumericUpDown
    Implements IDataGridViewEditingControl

    ' Needed to forward keyboard messages to the child TextBox control.
    <Runtime.InteropServices.DllImport("USER32.DLL", CharSet:=Runtime.InteropServices.CharSet.Auto)>
    Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

    ' The grid that owns this editing control
    Private _dataGridView As DataGridView

    ' Stores whether the editing control's value has changed or not
    Private _valueChanged As Boolean

    ' Stores the row index in which the editing control resides
    Private _rowIndex As Integer

    ''' <summary>
    '''  Constructor of the editing control class
    ''' </summary>
    Public Sub New()
        ' The editing control must not be part of the tabbing loop
        Me.TabStop = False
    End Sub

    ' Beginning of the IDataGridViewEditingControl interface implementation
    ''' <summary>
    '''  Property which caches the grid that uses this editing control
    ''' </summary>
    Public Overridable Property EditingControlDataGridView As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return _dataGridView
        End Get

        Set(value As DataGridView)
            _dataGridView = value
        End Set
    End Property

    ''' <summary>
    '''  Property which represents the current formatted value of the editing control
    ''' </summary>
    Public Overridable Property EditingControlFormattedValue As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Me.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting)
        End Get

        Set(value As Object)
            Me.Text = CStr(value)
        End Set
    End Property

    ''' <summary>
    '''  Property which represents the row in which the editing control resides
    ''' </summary>
    Public Overridable Property EditingControl_RowIndex As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return _rowIndex
        End Get

        Set(value As Integer)
            _rowIndex = value
        End Set
    End Property

    ''' <summary>
    '''  Property which indicates whether the value of the editing control has changed or not
    ''' </summary>
    Public Overridable Property EditingControl_ValueChanged As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return _valueChanged
        End Get

        Set(value As Boolean)
            _valueChanged = value
        End Set
    End Property

    ''' <summary>
    '''  Property which determines which cursor must be used for the editing panel,
    '''  i.e. the parent of the editing control.
    ''' </summary>
    Public Overridable ReadOnly Property EditingPanelCursor As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return Cursors.Default
        End Get
    End Property

    ''' <summary>
    '''  Property which indicates whether the editing control needs to be repositioned
    '''  when its value changes.
    ''' </summary>
    Public Overridable ReadOnly Property RepositionEditingControlOnValueChange As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    '''  Method called by the grid before the editing control is shown so it can adapt to the
    '''  provided cell style.
    ''' </summary>
    Public Overridable Sub ApplyCellStyleToEditingControl(dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        Me.Font = dataGridViewCellStyle.Font
        If dataGridViewCellStyle.BackColor.A < 255 Then
            ' The NumericUpDown control does not support transparent back colors
            Dim opaqueBackColor As Color = Color.FromArgb(255, dataGridViewCellStyle.BackColor)
            Me.BackColor = opaqueBackColor
            _dataGridView.EditingPanel.BackColor = opaqueBackColor
        Else
            Me.BackColor = dataGridViewCellStyle.BackColor
        End If
        Me.ForeColor = dataGridViewCellStyle.ForeColor
        Me.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(dataGridViewCellStyle.Alignment)
    End Sub

    ''' <summary>
    '''  Method called by the grid on keystrokes to determine if the editing control is
    '''  interested in the key or not.
    ''' </summary>
    Public Overridable Function EditingControlWantsInputKey(keyData As Keys, dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case keyData And Keys.KeyCode
            Case Keys.Right
                Dim textBox As TextBox = TryCast(Me.Controls(1), TextBox)
                If textBox IsNot Nothing Then
                    ' If the end of the selection is at the end of the string,
                    ' let the DataGridView treat the key message
                    If (Me.RightToLeft = RightToLeft.No AndAlso Not (textBox.SelectionLength = 0 AndAlso textBox.SelectionStart = textBox.Text.Length)) OrElse
                        (Me.RightToLeft = RightToLeft.Yes AndAlso Not (textBox.SelectionLength = 0 AndAlso textBox.SelectionStart = 0)) Then
                        Return True
                    End If
                End If
                Exit Select

            Case Keys.Left
                Dim textBox As TextBox = TryCast(Me.Controls(1), TextBox)
                If textBox IsNot Nothing Then
                    ' If the end of the selection is at the beginning of the string
                    ' or if the entire text is selected and we did not start editing,
                    ' send this character to the dataGridView, else process the key message
                    If (Me.RightToLeft = RightToLeft.No AndAlso Not (textBox.SelectionLength = 0 AndAlso textBox.SelectionStart = 0)) OrElse
                        (Me.RightToLeft = RightToLeft.Yes AndAlso Not (textBox.SelectionLength = 0 AndAlso textBox.SelectionStart = textBox.Text.Length)) Then
                        Return True
                    End If
                End If
                Exit Select

            Case Keys.Down
                ' If the current value hasn't reached its minimum yet, handle the key. Otherwise let
                ' the grid handle it.
                If Me.Value > Me.Minimum Then
                    Return True
                End If

            Case Keys.Up
                ' If the current value hasn't reached its maximum yet, handle the key. Otherwise let
                ' the grid handle it.
                If Me.Value < Me.Maximum Then
                    Return True
                End If

            Case Keys.Home, Keys.[End]
                ' Let the grid handle the key if the entire text is selected.
                Dim textBox As TextBox = TryCast(Me.Controls(1), TextBox)
                If textBox IsNot Nothing Then
                    If textBox.SelectionLength <> textBox.Text.Length Then
                        Return True
                    End If
                End If
                Exit Select

            Case Keys.Delete
                ' Let the grid handle the key if the caret is at the end of the text.
                Dim textBox As TextBox = TryCast(Me.Controls(1), TextBox)
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
    '''  Returns the current value of the editing control.
    ''' </summary>
    Public Overridable Function GetEditingControlFormattedValue(context As DataGridViewDataErrorContexts) As Object Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Dim userEdit As Boolean = Me.UserEdit
        Try
            ' Prevent the Value from being set to Maximum or Minimum when the cell is being painted.
            Me.UserEdit = (context And DataGridViewDataErrorContexts.Display) = 0
            Return Me.Value.ToString($"{If(Me.ThousandsSeparator, "N", "F")}{Me.DecimalPlaces}")
        Finally
            Me.UserEdit = userEdit
        End Try
    End Function

    ''' <summary>
    '''  Called by the grid to give the editing control a chance to prepare itself for
    '''  the editing session.
    ''' </summary>
    Public Overridable Sub PrepareEditingControlForEdit(selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
        Dim textBox As TextBox = TryCast(Me.Controls(1), TextBox)
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
    ' End of the IDataGridViewEditingControl interface implementation

    ''' <summary>
    '''  Small utility function that updates the local dirty state and
    '''  notifies the grid of the value change.
    ''' </summary>
    Private Sub NotifyDataGridViewOfValueChange()
        If Not _valueChanged Then
            _valueChanged = True
            _dataGridView.NotifyCurrentCellDirty(dirty:=True)
        End If
    End Sub

    ''' <summary>
    '''  Listen to the KeyPress notification to know when the value changed, and
    '''  notify the grid of the change.
    ''' </summary>
    Protected Overrides Sub OnKeyPress(e As KeyPressEventArgs)
        MyBase.OnKeyPress(e)

        ' The value changes when a digit, the decimal separator, the group separator or
        ' the negative sign is pressed.
        Dim notifyValueChange As Boolean = False
        If Char.IsDigit(e.KeyChar) Then
            notifyValueChange = True
        Else
            Dim numberFormatInfo As Globalization.NumberFormatInfo = Globalization.CultureInfo.CurrentCulture.NumberFormat
            Dim decimalSeparatorStr As String = numberFormatInfo.NumberDecimalSeparator
            Dim groupSeparatorStr As String = numberFormatInfo.NumberGroupSeparator
            Dim negativeSignStr As String = numberFormatInfo.NegativeSign
            If Not String.IsNullOrEmpty(decimalSeparatorStr) AndAlso decimalSeparatorStr.Length = 1 Then
                notifyValueChange = decimalSeparatorStr(0) = e.KeyChar
            End If
            If Not notifyValueChange AndAlso Not String.IsNullOrEmpty(groupSeparatorStr) AndAlso groupSeparatorStr.Length = 1 Then
                notifyValueChange = groupSeparatorStr(0) = e.KeyChar
            End If
            If Not notifyValueChange AndAlso Not String.IsNullOrEmpty(negativeSignStr) AndAlso negativeSignStr.Length = 1 Then
                notifyValueChange = negativeSignStr(0) = e.KeyChar
            End If
        End If

        If notifyValueChange Then
            ' Let the DataGridView know about the value change
            Me.NotifyDataGridViewOfValueChange()
        End If
    End Sub

    '''' <summary>
    ''''  Listen to the _valueChanged notification to forward the change to the grid.
    '''' </summary>
    Protected Overrides Sub OnValueChanged(e As EventArgs)
        MyBase.OnValueChanged(e)
        If Me.Focused Then
            ' Let the DataGridView know about the value change
            Me.NotifyDataGridViewOfValueChange()
        End If
    End Sub

    ''' <summary>
    '''  A few keyboard messages need to be forwarded to the inner <see cref="TextBox"/> of the
    '''  <see cref="NumericUpDown"/> control so that the first character pressed appears in it.
    ''' </summary>
    Protected Overrides Function ProcessKeyEventArgs(ByRef m As Message) As Boolean
        Dim textBox As TextBox = TryCast(Me.Controls(1), TextBox)
        If textBox IsNot Nothing Then
            SendMessage(textBox.Handle, m.Msg, m.WParam, m.LParam)
            Return True
        Else
            Return MyBase.ProcessKeyEventArgs(m)
        End If
    End Function

End Class
