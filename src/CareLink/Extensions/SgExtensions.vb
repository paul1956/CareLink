' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text.Json

''' <summary>
'''  Provides extension methods for working with sensor glucose (SG) values.
''' </summary>
''' <remarks>
'''  The extensions in this module provide consistent formatting and filtering
'''  logic for SG values across the application. They take into account the
'''  application's unit settings (via <see cref="NativeMmolL"/> and
'''  <see cref="MmolLUnitsDivisor"/>) and reuse existing helpers such as
'''  <see cref="Single.Parse"/> and <see cref="RoundToSingle"/>.
''' </remarks>
Friend Module SgExtensions

    ''' <summary>
    '''  Converts a <see cref="JsonElement"/> to a string representation of the value,
    '''  scaled according to the current <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <param name="item">
    '''  The <see cref="JsonElement"/> that contains the numeric value or a string representation of a number.
    ''' </param>
    ''' <returns>
    '''  A culture-aware string representation of the parsed and scaled SG value.
    '''  If <paramref name="item"/> is <c>null</c>, <see cref="JsonValueKind.Null"/>,
    '''  or <see cref="JsonValueKind.Undefined"/>, an empty string is returned.
    ''' </returns>
    ''' <remarks>
    '''  - When <see cref="NativeMmolL"/> is enabled, the returned value is converted
    '''    to mmol/L using <see cref="MmolLUnitsDivisor"/> and rounded to one decimal place
    '''    by <see cref="RoundToSingle"/>.
    '''  - If <paramref name="item"/> has an unexpected <see cref="JsonValueKind"/>, the code
    '''    triggers a debugger break (via <c>Stop</c>) in debug builds; such cases indicate
    '''    a programming error or malformed JSON and should be investigated.
    ''' </remarks>
    ''' <exception cref="FormatException">Thrown when a string value cannot be parsed to a Single.</exception>
    ''' <exception cref="OverflowException">Thrown when a numeric value is outside the range of a Single.</exception>
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
    '''  Counts the number of <see cref="SG"/> records whose SG value, when expressed as mg/dL,
    '''  falls within the inclusive range [<see cref="My.Settings.TiTrLowThreshold"/>, 140].
    ''' </summary>
    ''' <param name="sgList">
    '''  An <see cref="IEnumerable(Of SG)"/> to evaluate. If <c>Nothing</c>, the method returns 0.
    ''' </param>
    ''' <returns>
    '''  The count of SG records that:
    '''  - have a valid SG value (as determined by <c>entry.sg.IsSgValid</c>), and
    '''  - whose mg/dL equivalent is greater than or equal to <see cref="My.Settings.TiTrLowThreshold"/>
    '''    and less than or equal to 140.0.
    ''' </returns>
    ''' <remarks>
    '''  - The method ignores SG values that are not valid (for example, <see cref="Single.NaN"/>).
    '''  - The low threshold is read from application settings: <c>My.Settings.TiTrLowThreshold</c>.
    '''  - The upper threshold is fixed at 140 mg/dL by design.
    '''  - This method performs a simple linear scan of <paramref name="sgList"/>; for large sequences
    '''    consider providing an already-filtered or materialized list to avoid repeated enumeration.
    ''' </remarks>
    <Extension>
    Friend Function CountSgInTightRange(sgList As IEnumerable(Of SG)) As Integer
        If sgList Is Nothing Then
            Return 0
        End If

        Dim lowThreshold As Double = My.Settings.TiTrLowThreshold
        Dim highThreshold As Double = 140.0
        Dim result As Integer = 0

        For Each sg As SG In sgList
            Dim sgValue As Single = sg.sg
            If sgValue.IsSgValid Then
                Dim mgdL As Double = sg.sgMgdL
                If mgdL >= lowThreshold AndAlso mgdL <= highThreshold Then
                    result += 1
                End If
            End If
        Next

        Return result
    End Function

    ''' <summary>
    '''  Retrieves all valid <see cref="SG"/> records from the global list.
    ''' </summary>
    ''' <returns>
    '''  An <see cref="IEnumerable(Of SG)"/> that yields only records for which
    '''  <c>entry.sg.IsSgValid</c> is <see langword="True"/><.
    ''' </returns>
    ''' <remarks>
    ''' - This method references the module-level/global collection <see cref="s_sgRecords"/>.
    ''' - The returned sequence is lazily evaluated; calling code should be aware that
    '''   the underlying collection may change between enumeration calls.
    ''' </remarks>
    Public Function GetValidSgRecords() As IEnumerable(Of SG)
        Dim predicate As Func(Of SG, Boolean) = Function(entry As SG) As Boolean
                                                    Return entry.sg.IsSgValid
                                                End Function

        Return s_sgRecords.Where(predicate)
    End Function

    ''' <summary>
    '''  Converts a <see cref="Single"/> SG value to a localized string, applying unit conversion
    '''  when the application is configured to use mmol/L.
    ''' </summary>
    ''' <param name="value">
    '''  The raw SG value (assumed to be in mg/dL units unless <see cref="NativeMmolL"/> is <c>True</c>).
    ''' </param>
    ''' <returns>
    '''  A culture-aware string representation of the value. When <see cref="NativeMmolL"/> is <c>True</c>,
    '''  the numeric value is divided by <see cref="MmolLUnitsDivisor"/> and rounded to one decimal place.
    ''' </returns>
    ''' <remarks>
    '''  - Uses <see cref="CultureInfo.CurrentUICulture"/> for numeric formatting.
    '''  - Rounding is performed by <see cref="RoundToSingle"/> with <c>digits:=1</c>.
    ''' </remarks>
    <Extension>
    Public Function ScaleSg(value As Single) As String
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        If NativeMmolL Then
            value = (value / MmolLUnitsDivisor).RoundToSingle(digits:=1, considerValue:=True)
        End If
        Return value.ToString(provider)
    End Function

    ''' <summary>
    '''  Converts the value portion of a <see cref="KeyValuePair(Of String, Object)"/> into
    '''  a scaled SG string using the same rules as <see cref="ScaleSg(JsonElement)"/>.
    ''' </summary>
    ''' <param name="item">A key/value pair whose value is expected to be a <see cref="JsonElement"/>.</param>
    ''' <returns>A scaled, culture-aware string representation of the value.</returns>
    ''' <remarks>
    '''  The method casts <c>item.Value</c> to <see cref="JsonElement"/> and
    '''  forwards to <see cref="ScaleSg(JsonElement)"/>.
    ''' </remarks>
    <Extension>
    Public Function ScaleSg(item As KeyValuePair(Of String, Object)) As String
        Return CType(item.Value, JsonElement).ScaleSg()
    End Function

    ''' <summary>
    '''  Parses a string to a <see cref="Single"/> and returns a scaled string representation
    '''  according to application unit settings.
    ''' </summary>
    ''' <param name="value">A string containing a numeric representation of the SG value.</param>
    ''' <returns>
    '''  The scaled and formatted string representation of the parsed value.
    '''  If parsing fails, <see cref="ParseSingle"/> may throw a <see cref="FormatException"/>
    '''  or <see cref="OverflowException"/>.
    ''' </returns>
    ''' <exception cref="FormatException">If <paramref name="value"/> is not a valid number.</exception>
    ''' <exception cref="OverflowException">
    '''  If <paramref name="value"/> represents a number outside the range of <see cref="Single"/>.
    ''' </exception>
    <Extension>
    Public Function ScaleSg(value As String) As String
        Return value.ParseSingle().ScaleSg()
    End Function

End Module
