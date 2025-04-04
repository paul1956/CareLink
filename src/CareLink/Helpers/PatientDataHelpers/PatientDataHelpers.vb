' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json

Public Module PatientDataHelpers
    Private ReadOnly s_keyDictionary As New Dictionary(Of String, String) From {
        {$"""{NameOf(ServerDataIndexes.firstName)}"": ", """First"""},
        {$"""{NameOf(ServerDataIndexes.lastName)}"": ", """Last"""},
        {$"""{NameOf(ServerDataIndexes.conduitSerialNumber)}"": ", $"""{New Guid()}"""},
        {$"""{NameOf(MedicalDeviceInformation.SystemId)}"": ", """40000000000 0000"""},
        {$"""{NameOf(MedicalDeviceInformation.DeviceSerialNumber)}"": ", """NG4000000H"""}}

    ''' <summary>
    '''  Serialize <see cref="PatientDataElement"/> while removing any personal information
    ''' </summary>
    ''' <returns>String without any personal information</returns>
    Public Function CleanPatientData() As String
        Dim str As String = JsonSerializer.Serialize(value:=PatientDataElement, options:=s_jsonSerializerOptions)
        If String.IsNullOrWhiteSpace(str) Then
            Return str
        End If
        Dim charList As New List(Of Char) From {","c, CChar(vbCr)}
        For Each kvp As KeyValuePair(Of String, String) In s_keyDictionary
            Dim startIndex As Integer = str.IndexOf(
                value:=kvp.Key,
                comparisonType:=StringComparison.OrdinalIgnoreCase) + Len(kvp.Key)
            If startIndex = -1 Then
                Continue For
            End If
            Dim endPos As Integer = FindIndexOfAnyChar(str, charList, startIndex)
            Dim length As Integer = endPos - startIndex
            str = str.Replace(str.Substring(startIndex, length), kvp.Value)
        Next
        Return str
    End Function

End Module
