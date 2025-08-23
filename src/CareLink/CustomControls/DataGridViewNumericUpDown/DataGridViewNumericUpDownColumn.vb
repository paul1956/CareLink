' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization
Imports System.Text

''' <summary>
'''  Custom column type dedicated to the
'''  <see cref="DataGridViewNumericUpDownCell"/> cell type.
''' </summary>
Public Class DataGridViewNumericUpDownColumn
    Inherits DataGridViewColumn

    Private Const Browsable As Boolean = False
    Private ReadOnly Property Message As String =
        "Operation cannot be completed because this " &
        NameOf(DataGridViewColumn) & " does not have a CellTemplate."

    ''' <summary>
    '''  Constructor for the <see cref="DataGridViewNumericUpDownColumn"/> class.
    ''' </summary>
    Public Sub New()
        MyBase.New(cellTemplate:=New DataGridViewNumericUpDownCell)
    End Sub

    ''' <summary>
    '''  Represents the implicit cell that gets cloned when adding rows to the grid.
    ''' </summary>
    <Browsable(Browsable), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Overrides Property CellTemplate As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get

        Set(value As DataGridViewCell)
            Dim dataGridViewNumericUpDownCell1 As DataGridViewNumericUpDownCell =
                TryCast(value, DataGridViewNumericUpDownCell)
            If value IsNot Nothing AndAlso dataGridViewNumericUpDownCell1 Is Nothing Then
                Dim message As String =
                    $"Value provided for {NameOf(CellTemplate)} must be of type" &
                    $" {NameOf(DataGridViewNumericUpDownCell)} or derive from it."
                Throw New InvalidCastException(message)
            End If

            MyBase.CellTemplate = value
        End Set
    End Property

    ''' <summary>
    '''  Replicates the DecimalPlaces property of the
    '''  DataGridViewNumericUpDownCell cell type.
    ''' </summary>
    <Category("Appearance"),
        DefaultValue(DataGridViewNumericUpDownCell.DgvNumericUpDownCell_defaultDecimalPlaces),
        Description("Indicates the number of decimal places to display.")>
    Public Property DecimalPlaces As Integer
        Get
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Return Me.NumericUpDownCellTemplate.DecimalPlaces
        End Get

        Set(value As Integer)
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            ' Update the template cell so that subsequent cloned cells use the new value.
            Me.NumericUpDownCellTemplate.DecimalPlaces = value
            If Me.DataGridView IsNot Nothing Then
                ' Update all the existing DataGridViewNumericUpDownCell cells
                ' in the column accordingly.
                Dim dataGridViewRows As DataGridViewRowCollection = Me.DataGridView.Rows
                Dim rowCount As Integer = dataGridViewRows.Count
                For rowIndex As Integer = 0 To rowCount - 1
                    ' Be careful not to unshare rows unnecessarily.
                    ' This could have severe performance repercussions.
                    Dim dgvRow As DataGridViewRow =
                        dataGridViewRows.SharedRow(rowIndex)

                    Dim dataGridViewCell1 As DataGridViewNumericUpDownCell =
                        TryCast(dgvRow.Cells(Me.Index), DataGridViewNumericUpDownCell)
                    ' Call the internal SetDecimalPlaces method instead of
                    ' the property to avoid invalidation of each cell.
                    ' The whole column is invalidated later in a single operation
                    ' for better performance.
                    dataGridViewCell1?.SetDecimalPlaces(rowIndex, value)
                Next

                Me.DataGridView.InvalidateColumn(Me.Index)
                ' TODO: Call the grid's autosizing methods to autosize the
                ' column, rows, column headers / row headers as needed.
            End If
        End Set
    End Property

    ''' <summary>
    '''  Replicates the Increment property of the DataGridViewNumericUpDownCell cell type.
    ''' </summary>
    <Category("Data"), Description("Indicates the amount to increment or decrement on each button click.")>
    Public Property Increment As Decimal
        Get
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Return Me.NumericUpDownCellTemplate.Increment
        End Get

        Set(value As Decimal)
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Me.NumericUpDownCellTemplate.Increment = value
            If Me.DataGridView IsNot Nothing Then
                Dim dataGridViewRows As DataGridViewRowCollection = Me.DataGridView.Rows
                Dim rowCount As Integer = dataGridViewRows.Count
                For rowIndex As Integer = 0 To rowCount - 1
                    Dim dgvRow As DataGridViewRow = dataGridViewRows.SharedRow(rowIndex)
                    Dim dgvCell1 As DataGridViewNumericUpDownCell =
                        TryCast(dgvRow.Cells(Me.Index), DataGridViewNumericUpDownCell)
                    dgvCell1?.SetIncrement(rowIndex, value)
                Next
            End If
        End Set
    End Property

    ''' <summary>
    '''  Indicates whether the Increment property should be persisted.
    '''  </summary>
    Private Function ShouldSerializeIncrement() As Boolean
        Const value As Decimal =
            DataGridViewNumericUpDownCell.DgvNumericUpDownCell_defaultIncrement
        Return Not Me.Increment.Equals(value)
    End Function

    ''' <summary>
    '''  Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
    ''' </summary>
    <Category("Data"),
        Description("Indicates the maximum value for the numeric up-down cells."),
        RefreshProperties(RefreshProperties.All)>
    Public Property Maximum As Decimal
        Get
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Return Me.NumericUpDownCellTemplate.Maximum
        End Get

        Set(value As Decimal)
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Me.NumericUpDownCellTemplate.Maximum = value
            If Me.DataGridView IsNot Nothing Then
                Dim dataGridViewRows As DataGridViewRowCollection = Me.DataGridView.Rows
                Dim rowCount As Integer = dataGridViewRows.Count
                For rowIndex As Integer = 0 To rowCount - 1
                    Dim dgvRow As DataGridViewRow =
                        dataGridViewRows.SharedRow(rowIndex)
                    Dim dgvCell As DataGridViewNumericUpDownCell =
                        TryCast(dgvRow.Cells(Me.Index), DataGridViewNumericUpDownCell)
                    dgvCell?.SetMaximum(rowIndex, value)
                Next

                Me.DataGridView.InvalidateColumn(Me.Index)
                ' TODO: Call the grid's autosizing methods to autosize the
                ' column, rows, column headers / row headers as needed.
                ' Call the autosizing methods to autosize the column, rows,
                ' column headers / row headers as needed.
            End If
        End Set
    End Property

    ''' <summary>
    '''  Indicates whether the Maximum property should be persisted.
    ''' </summary>
    Private Function ShouldSerializeMaximum() As Boolean
        Const value As Decimal =
            DataGridViewNumericUpDownCell.DgvNumericUpDownCell_defaultMaximum
        Return Not Me.Maximum.Equals(value)
    End Function

    ''' <summary>
    '''  Replicates the Minimum property of the DataGridViewNumericUpDownCell cell type.
    ''' </summary>
    <Category("Data"),
        Description("Indicates the minimum value for the numeric up-down cells."),
        RefreshProperties(RefreshProperties.All)>
    Public Property Minimum As Decimal
        Get
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Return Me.NumericUpDownCellTemplate.Minimum
        End Get

        Set(value As Decimal)
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Me.NumericUpDownCellTemplate.Minimum = value
            If Me.DataGridView IsNot Nothing Then
                Dim dataGridViewRows As DataGridViewRowCollection = Me.DataGridView.Rows
                Dim rowCount As Integer = dataGridViewRows.Count
                For rowIndex As Integer = 0 To rowCount - 1
                    Dim gvRow As DataGridViewRow = dataGridViewRows.SharedRow(rowIndex)
                    Dim dgvCell As DataGridViewNumericUpDownCell =
                        TryCast(gvRow.Cells(Me.Index), DataGridViewNumericUpDownCell)
                    dgvCell?.SetMinimum(rowIndex, value)
                Next

                Me.DataGridView.InvalidateColumn(Me.Index)
                ' TODO: This column and/or grid rows may need to be autosized
                '       depending on their AutoSize settings. Call the autosizing
                '       methods to autosize the column, rows, column headers / row headers
                '       as needed.
            End If
        End Set
    End Property

    ''' <summary>
    '''  Indicates whether the Maximum property should be persisted.
    ''' </summary>
    Private Function ShouldSerializeMinimum() As Boolean
        Const value As Decimal =
            DataGridViewNumericUpDownCell.DgvNumericUpDownCell_defaultMinimum
        Return Not Me.Minimum.Equals(value)
    End Function

    ''' <summary>
    '''  Replicates the ThousandsSeparator property of the
    '''  <see cref="DataGridViewNumericUpDownCell"/> cell type.
    ''' </summary>
    <Category("Data"),
        DefaultValue(DataGridViewNumericUpDownCell.DefaultThousandsSeparator),
        Description("Should separator be inserted between 3 decimal digits.")>
    Public Property ThousandsSeparator As Boolean
        Get
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Return Me.NumericUpDownCellTemplate.ThousandsSeparator
        End Get

        Set(value As Boolean)
            If Me.NumericUpDownCellTemplate Is Nothing Then
                Throw New InvalidOperationException(Me.Message)
            End If

            Me.NumericUpDownCellTemplate.ThousandsSeparator = value
            If Me.DataGridView IsNot Nothing Then
                Dim dataGridViewRows As DataGridViewRowCollection = Me.DataGridView.Rows
                Dim rowCount As Integer = dataGridViewRows.Count
                For rowIndex As Integer = 0 To rowCount - 1
                    Dim dataGridViewRow1 As DataGridViewRow =
                        dataGridViewRows.SharedRow(rowIndex)

                    Dim cell As DataGridViewCell = dataGridViewRow1.Cells(Me.Index)
                    Dim dataGridViewCell1 As DataGridViewNumericUpDownCell =
                        TryCast(cell, DataGridViewNumericUpDownCell)
                    dataGridViewCell1?.SetThousandsSeparator(rowIndex, value)
                Next

                Me.DataGridView.InvalidateColumn(Me.Index)
                ' TODO: Call the grid's autosizing methods to autosize the
                ' column, rows, column headers / row headers as needed.
            End If
        End Set
    End Property

    ''' <summary>
    '''  Small utility function that returns the template cell as a
    '''  <see cref="DataGridViewNumericUpDownCell"/>
    ''' </summary>
    Private ReadOnly Property NumericUpDownCellTemplate As DataGridViewNumericUpDownCell
        Get
            Return CType(Me.CellTemplate, DataGridViewNumericUpDownCell)
        End Get
    End Property

    ''' <summary>
    '''  Returns a standard compact string representation of the column.
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder(capacity:=100)
        sb.Append(value:="DataGridViewNumericUpDownColumn { Name=")
        sb.Append(value:=Me.Name)
        sb.Append(value:=", Index=")
        sb.Append(value:=Me.Index.ToString(provider:=CultureInfo.CurrentCulture))
        sb.Append(value:=" }")
        Return sb.ToString()
    End Function

End Class
