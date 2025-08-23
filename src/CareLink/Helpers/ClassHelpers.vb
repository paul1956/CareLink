' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Reflection

Public Module ClassHelpers
    ''' <summary>
    '''  Creates a <see cref="Dictionary"/> that maps class property names to
    '''  <see cref="DataGridViewCellStyle"/> for column alignment.
    '''  Determines alignment and padding based on the <see cref="ColumnAttribute"/> type name.
    ''' </summary>
    ''' <typeparam name="T">The type of the class whose properties are mapped.</typeparam>
    ''' <param name="alignmentTable">
    '''  A dictionary to populate with property name to cell style mappings.
    ''' </param>
    ''' <param name="name">The column name to retrieve or add alignment for.</param>
    ''' <returns>The <see cref="DataGridViewCellStyle"/> for the specified column.</returns>
    Public Function ClassPropertiesToColumnAlignment(Of T As Class)(
            ByRef alignmentTable As Dictionary(Of String, DataGridViewCellStyle),
            name As String) As DataGridViewCellStyle

        Dim classType As Type = GetType(T)
        Dim cellStyle As New DataGridViewCellStyle
        If alignmentTable.Count = 0 Then
            For Each [property] As PropertyInfo In classType.GetProperties()
                cellStyle = New DataGridViewCellStyle
                Dim typeName As String = [property].GetCustomAttributes(
                    attributeType:=GetType(ColumnAttribute),
                    inherit:=True).Cast(Of ColumnAttribute)().SingleOrDefault()?.TypeName

                Select Case typeName
                    Case "additionalInfo",
                         "Date",
                         "DateTime",
                         NameOf(OADate),
                         "RecordNumber",
                         NameOf([String]),
                         "Version"

                        cellStyle = cellStyle.SetCellStyle(
                            alignment:=DataGridViewContentAlignment.MiddleLeft,
                            padding:=New Padding(all:=1))
                    Case "AdditionalInfo"
                        cellStyle = cellStyle.SetCellStyle(
                            alignment:=DataGridViewContentAlignment.MiddleLeft,
                            padding:=New Padding(all:=1))
                    Case NameOf([Decimal]),
                         NameOf([Double]),
                         NameOf([Int32]),
                         NameOf([Single]),
                         NameOf([TimeSpan])
                        cellStyle = cellStyle.SetCellStyle(
                            alignment:=DataGridViewContentAlignment.MiddleRight,
                            padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))
                    Case NameOf([Boolean]),
                         "DeleteRow"
                        cellStyle = cellStyle.SetCellStyle(
                            alignment:=DataGridViewContentAlignment.MiddleCenter,
                            padding:=New Padding(all:=0))
                    Case "CustomProperty"
                        cellStyle = cellStyle.SetCellStyle(
                            alignment:=DataGridViewContentAlignment.MiddleRight,
                            padding:=New Padding(left:=0, top:=2, right:=2, bottom:=2))
                    Case Else
                        Throw UnreachableException(paramName:=[property].PropertyType.Name)
                End Select
                alignmentTable.Add(key:=[property].Name, value:=cellStyle)
            Next
        End If
        If Not alignmentTable.TryGetValue(key:=name, value:=cellStyle) Then
            Dim alignMiddle As Boolean =
                name = NameOf(SummaryRecord.RecordNumber) OrElse
                                name = NameOf(Limit.Index)
            cellStyle = If(alignMiddle,
                           (New DataGridViewCellStyle).SetCellStyle(
                               alignment:=DataGridViewContentAlignment.MiddleCenter,
                               padding:=New Padding(all:=0)),
                           (New DataGridViewCellStyle).SetCellStyle(
                               alignment:=DataGridViewContentAlignment.MiddleLeft,
                               padding:=New Padding(all:=1)))
            alignmentTable.Add(key:=name, value:=cellStyle)
        End If
        Return cellStyle
    End Function

End Module
