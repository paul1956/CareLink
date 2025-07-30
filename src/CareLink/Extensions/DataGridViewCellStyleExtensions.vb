' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for <see cref="DataGridViewCell"/> and <see cref="DataGridViewCellStyle"/>.
''' </summary>
Friend Module DataGridViewCellStyleExtensions

    ''' <summary>
    '''  Gets the formatted <see cref="DataGridViewCellStyle"/> for the specified <see cref="DataGridViewCell"/>,
    '''  simulating the formatting event to retrieve the effective style.
    ''' </summary>
    ''' <param name="cell">The <see cref="DataGridViewCell"/> to get the formatted style for.</param>
    ''' <returns>
    '''  The <see cref="DataGridViewCellStyle"/> as it would appear after formatting.
    ''' </returns>
    <Extension>
    Public Function GetFormattedStyle(cell As DataGridViewCell) As DataGridViewCellStyle
        Dim dgv As DataGridView = cell.DataGridView
        If dgv Is Nothing Then
            Return cell.InheritedStyle
        End If

        Dim m As MethodInfo = dgv.GetType().GetMethod(
            name:="OnCellFormatting",
            bindingAttr:=BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.IgnoreCase,
            binder:=Nothing,
            types:=New Type() {GetType(DataGridViewCellFormattingEventArgs)},
            modifiers:=Nothing)
        Dim e As New DataGridViewCellFormattingEventArgs(
            cell.ColumnIndex,
            cell.RowIndex,
            cell.Value,
            desiredType:=cell.FormattedValueType,
            cellStyle:=cell.InheritedStyle)
        m.Invoke(dgv, New Object() {e})
        Return e.CellStyle
    End Function

    ''' <summary>
    '''  Sets the <see cref="DataGridViewCellStyle.Alignment"/> and
    '''  <see cref="DataGridViewCellStyle.Padding"/> properties
    '''  and returns the modified style.
    ''' </summary>
    ''' <param name="cellStyle">The <see cref="DataGridViewCellStyle"/> to modify.</param>
    ''' <param name="alignment">The <see cref="DataGridViewContentAlignment"/> to set.</param>
    ''' <param name="padding">The <see cref="Padding"/> to set.</param>
    ''' <returns>
    '''  The modified <see cref="DataGridViewCellStyle"/>.
    ''' </returns>
    <Extension>
    Public Function SetCellStyle(
        cellStyle As DataGridViewCellStyle,
        alignment As DataGridViewContentAlignment,
        padding As Padding) As DataGridViewCellStyle

        cellStyle.Alignment = alignment
        cellStyle.Padding = padding
        Return cellStyle
    End Function

End Module
