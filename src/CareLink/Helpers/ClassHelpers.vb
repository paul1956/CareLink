' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Reflection

Public Module ClassHelpers

    ''' <summary>
    '''  Creates a <see cref="Dictionary"/> that maps class property names to
    '''  <see cref="DataGridViewCellStyle"/> for column alignment.
    '''  Determines alignment and padding based on the
    '''  <see cref="ColumnAttribute"/> type name.
    ''' </summary>
    ''' <typeparam name="T">
    '''  The type of the class whose properties are mapped.
    ''' </typeparam>
    ''' <param name="alignmentTable">
    '''  A dictionary to populate with property name to cell style mappings.
    ''' </param>
    ''' <param name="name">The column name to retrieve or add alignment for.</param>
    ''' <returns>
    '''  The <see cref="DataGridViewCellStyle"/> for the specified column.
    ''' </returns>
    Public Function ClassPropertiesToColumnAlignment(Of T As Class)(
        ByRef alignmentTable As Dictionary(Of String, DataGridViewCellStyle),
        name As String) As DataGridViewCellStyle

        Dim classType As Type = GetType(T)
        If alignmentTable.Count = 0 Then
            Dim leftAlignedTypes As New HashSet(Of String) From {
                "additionalInfo",
                "AdditionalInfo",
                "Date",
                "DateTime",
                NameOf(OADate),
                "RecordNumber",
                NameOf([String]),
                "Version"}

            Dim rightAlignedTypes As New HashSet(Of String) From {
                "CustomProperty",
                NameOf([Decimal]),
                NameOf([Double]),
                NameOf([Int32]),
                NameOf([Single]),
                NameOf([TimeSpan])}

            Dim centerAlignedTypes As New HashSet(Of String) From {
                NameOf([Boolean]), "DeleteRow"}

            For Each [property] As PropertyInfo In classType.GetProperties()
                Dim cellStyle As New DataGridViewCellStyle
                Dim objects As Object() =
                    [property].GetCustomAttributes(attributeType:=GetType(ColumnAttribute), inherit:=True)

                Dim typeName As String = objects.Cast(Of ColumnAttribute)().SingleOrDefault()?.TypeName

                If typeName Is Nothing Then
                    cellStyle = cellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleLeft,
                        padding:=New Padding(all:=1))
                ElseIf leftAlignedTypes.Contains(item:=typeName) Then
                    cellStyle = cellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleLeft,
                        padding:=New Padding(all:=1))
                ElseIf rightAlignedTypes.Contains(item:=typeName) Then
                    cellStyle = cellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleRight,
                        padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))
                ElseIf centerAlignedTypes.Contains(item:=typeName) Then
                    cellStyle = cellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleCenter,
                        padding:=New Padding(all:=0))
                Else
                    cellStyle = cellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleLeft,
                        padding:=New Padding(all:=1))
                End If
                alignmentTable(key:=[property].Name) = cellStyle
            Next
        End If

        Dim resultStyle As DataGridViewCellStyle = Nothing
        If Not alignmentTable.TryGetValue(key:=name, value:=resultStyle) Then
            Dim alignMiddle As Boolean = name = NameOf(SummaryRecord.RecordNumber) OrElse name = NameOf(Limit.Index)
            resultStyle = If(alignMiddle,
                             (New DataGridViewCellStyle).SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleCenter,
                                padding:=New Padding(all:=0)),
                             (New DataGridViewCellStyle).SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleLeft,
                                padding:=New Padding(all:=1)))
            alignmentTable(key:=name) = resultStyle
        End If
        Return resultStyle
    End Function

End Module
