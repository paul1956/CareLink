' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions

Public Module StringExtensions

    Private ReadOnly s_commaOrPeriod As Char() = {"."c, ","c}

    Private ReadOnly s_keyDictionary As New Dictionary(Of String, String) From {
        {$"""{NameOf(ServerDataIndexes.firstName)}"": ", """First"""},
        {$"""{NameOf(ServerDataIndexes.lastName)}"": ", """Last"""},
        {$"""{NameOf(ServerDataIndexes.conduitSerialNumber)}"": ", $"""{New Guid()}"""},
        {$"""{NameOf(MedicalDeviceInformation.SystemId)}"": ", """40000000000 0000"""},
        {$"""{NameOf(MedicalDeviceInformation.DeviceSerialNumber)}"": ", """NG4000000H"""}}

    ''' <summary>
    '''  Serialize <see cref="PatientData"/> while removing any personal information
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns>String without any personal information</returns>
    Friend Function CleanPatientData() As String
        Dim str As String = JsonSerializer.Serialize(value:=PatientData, options:=s_jsonSerializerOptions)
        If String.IsNullOrWhiteSpace(str) Then
            Return str
        End If
        Stop

        Dim startIndex As Integer
        For Each kvp As KeyValuePair(Of String, String) In s_keyDictionary
            startIndex = str.IndexOf(
                value:=kvp.Key,
                comparisonType:=StringComparison.OrdinalIgnoreCase) + Len(kvp.Key)
            If startIndex = -1 Then
                Continue For
            End If
            Dim endPos As Integer = str.IndexOf(
                value:=",",
                startIndex,
                comparisonType:=StringComparison.OrdinalIgnoreCase)
            str = str.Replace(str.Substring(startIndex, length:=endPos - startIndex), newValue:=kvp.Value)
        Next
        Return str
    End Function

    <Extension()>
    Friend Function Count(s As String, c As Char) As Integer
        Return s.Count(Function(c1 As Char) c1 = c)
    End Function

    ''' <summary>
    '''  Converts a string where the first letter of the string is not capitalized
    ''' </summary>
    ''' <param name="inStr">A string like THIS_IS A TITLE</param>
    ''' <returns>doNotCapitalizedFirstLetterString</returns>
    <Extension()>
    Friend Function ToLowerCamelCase(inStr As String) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If

        Dim result As New StringBuilder(Char.ToLowerInvariant(inStr(0)))
        If inStr.Length > 1 Then
            result.Append(inStr.AsSpan(1))
        End If
        Return result.ToString
    End Function

    ''' <summary>
    '''  Converts a string of words separated by a space or underscore
    '''  to a Title where the first letter of every word capitalized and the rest are not
    ''' </summary>
    ''' <param name="inStr">A string like THIS_IS A TITLE</param>
    ''' <returns>This Is A Title</returns>
    <Extension()>
    Friend Function ToTitle(inStr As String, Optional separateNumbers As Boolean = False) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If

        Dim result As New StringBuilder(Char.ToUpperInvariant(inStr(0)))
        Dim firstLetterOfWord As Boolean = False
        For Each c As Char In inStr.Substring(1)
            If c = " "c Or c = "_"c Then
                firstLetterOfWord = True
                result.Append(" "c)
            ElseIf firstLetterOfWord Then
                firstLetterOfWord = False
                result.Append(Char.ToUpperInvariant(c))
            ElseIf separateNumbers AndAlso IsNumeric(c) Then
                firstLetterOfWord = True
                result.Append(" "c)
                result.Append(Char.ToLowerInvariant(c))
            Else
                result.Append(Char.ToLowerInvariant(c))
            End If
        Next
        Return result.ToString.Replace("Bg ", "BG ").Replace("Sg ", "SG ")
    End Function

    ''' <summary>
    '''  Converts a single string of characters that contains words that
    '''  start with an uppercase character without spaces
    '''  to a Title where the first letter of every word capitalized and
    '''  words are separated by spaces
    ''' </summary>
    ''' <param name="inStr">A string like ThisIsATitle</param>
    ''' <returns>This Is A Title</returns>
    <Extension()>
    Friend Function ToTitleCase(inStr As String, Optional separateNumbers As Boolean = True) As String
        If String.IsNullOrWhiteSpace(inStr) Then
            Return ""
        End If
        If inStr.Contains("MmolL", StringComparison.InvariantCultureIgnoreCase) Then
            Return inStr
        End If
        Dim result As New StringBuilder(Char.ToUpperInvariant(inStr(0)))
        Dim lastWasNumeric As Boolean = Char.IsNumber(inStr(0))
        For Each c As Char In inStr.Substring(1)
            If Char.IsLower(c) OrElse lastWasNumeric Then
                result.Append(c)
                lastWasNumeric = False
            ElseIf Char.IsNumber(c) AndAlso Not separateNumbers Then
                result.Append(c)
                lastWasNumeric = True
            Else
                result.Append($" {Char.ToUpperInvariant(c)}")
                lastWasNumeric = False
            End If
        Next
        Dim resultString As String = result.Replace("Care Link", "CareLink").ToString
        If Not resultString.Contains("™"c) Then
            resultString = resultString.Replace("CareLink", "CareLink™")
        End If
        resultString = resultString.Replace("S G", "Sensor Glucose", StringComparison.InvariantCulture)
        Return resultString.Replace("time", " Time", False, Provider)
    End Function

    ''' <summary>
    '''  Replace multiple spaces with 1 and trim the ends
    ''' </summary>
    ''' <param name="value">String</param>
    ''' <returns>String</returns>
    <Extension>
    Public Function CleanSpaces(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return ""
        Return Regex.Replace(value, "\s+", " ").Trim
    End Function

    <Extension>
    Public Function TruncateSingleString(s As String, decimalDigits As Integer) As String
        Dim i As Integer = s.IndexOfAny(s_commaOrPeriod)
        If i < 0 Then
            If Not IsNumeric(s) Then
                Return s
            End If
            i = s.Length
            s &= Provider.NumberFormat.NumberDecimalSeparator
        End If
        s &= New String("0"c, decimalDigits + 1)
        Return s.Substring(0, i + decimalDigits + 1)
    End Function

End Module
