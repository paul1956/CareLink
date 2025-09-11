' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module SgExtensions

    ''' <summary>
    '''  Converts a JsonElement to a string representation of the value,
    '''  scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="item">The JsonElement to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Private Function ScaleSg(item As JsonElement) As String
        Dim itemAsSingle As Single
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Select Case item.ValueKind
            Case JsonValueKind.String
                itemAsSingle = Single.Parse(item.GetString(), provider)
            Case JsonValueKind.Null
                Return String.Empty
            Case JsonValueKind.Undefined
                Return String.Empty
            Case JsonValueKind.Number
                itemAsSingle = item.GetSingle
            Case Else
                Stop
        End Select

        Return itemAsSingle.ScaleSg()
    End Function

    ''' <summary>
    '''  Counts the number of SG.sg values in the specified range
    '''  [OptionsConfigureTiTR.LowThreshold, 140], excluding Single.NaN.
    ''' </summary>
    ''' <param name="sgList">The list of SG records to evaluate.</param>
    ''' <returns>
    '''  The count of SG.sg values within the range and not NaN.
    ''' </returns>
    <Extension>
    Friend Function CountSgInTightRange(sgList As IEnumerable(Of SG)) As Integer
        Dim predicate As Func(Of SG, Boolean) =
            Function(sg As SG) As Boolean
                Dim tiTrLowThreshold As Integer = My.Settings.TiTrLowThreshold
                Return Not Single.IsNaN(sg.sg) AndAlso
                       sg.sgMgdL >= tiTrLowThreshold AndAlso
                       sg.sgMgdL <= 140.0
            End Function
        Return sgList.Count(predicate)
    End Function

    ''' <summary>
    '''  Retrieves all valid <see cref="SG"/> records from the global list.
    ''' </summary>
    ''' <returns>An <see cref="enumerable"/> of valid <see cref="SG"/> records.</returns>
    Public Function GetValidSgRecords() As IEnumerable(Of SG)
        Return s_sgRecords.Where(predicate:=Function(entry) entry.sg.IsSgValid)
    End Function

    ''' <summary>
    '''  Converts a Single value to a string representation,
    '''  scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="value">The Single value to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSg(value As Single) As String
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        If NativeMmolL Then
            value =
                (value / MmolLUnitsDivisor).RoundToSingle(digits:=1, considerValue:=True)
        End If
        Return value.ToString(provider)
    End Function

    ''' <summary>
    '''  Converts a KeyValuePair to a string representation of the value,
    '''  scaled according to the NativeMmolL setting.
    ''' </summary>
    ''' <param name="item">The KeyValuePair to convert.</param>
    ''' <returns>A string representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSg(item As KeyValuePair(Of String, Object)) As String
        Return CType(item.Value, JsonElement).ScaleSg()
    End Function

    ''' <summary>
    '''  Converts a String representation of a value to a
    '''  string representation of the value, scaled according
    '''  to the <see cref="NativeMmolLSupport.NativeMmolL"/> setting.
    ''' </summary>
    ''' <param name="value">The string representation of the value to convert.</param>
    ''' <returns>A <see langword="String"/> representation of the scaled value.</returns>
    <Extension>
    Public Function ScaleSg(value As String) As String
        Return value.ParseSingle().ScaleSg()
    End Function

End Module
