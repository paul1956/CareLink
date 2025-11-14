' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json

Public Module PatientDataHelpers

    Private ReadOnly s_keyDictionary As New Dictionary(Of String, String) From {
        {$"""{NameOf(ServerDataEnum.firstName)}"": ", """First"""},
        {$"""{NameOf(ServerDataEnum.lastName)}"": ", """Last"""},
        {$"""{NameOf(ServerDataEnum.conduitSerialNumber)}"": ", $"""{New Guid()}"""},
        {$"""{NameOf(MedicalDeviceInformation.SystemId)}"": ", """40000000000 0000"""},
        {$"""{NameOf(MedicalDeviceInformation.DeviceSerialNumber)}"": ", """NG4000000H"""}}

    ''' <summary>
    '''  Serialize <see cref="PatientDataElement"/> while removing any personal information
    ''' </summary>
    ''' <returns>String without any personal information</returns>
    Public Function CleanPatientData() As String
        Dim value As String = JsonSerializer.Serialize(value:=PatientDataElement, options:=s_jsonSerializerOptions)
        If String.IsNullOrWhiteSpace(value) Then
            Return value
        End If
        Dim charList As New List(Of Char) From {","c, CChar(vbCr)}
        For Each kvp As KeyValuePair(Of String, String) In s_keyDictionary
            Dim startIndex As Integer = value.IndexOfNoCase(value:=kvp.Key) + kvp.Key.Length
            If startIndex = -1 Then
                Continue For
            End If
            Dim endPos As Integer = FindIndexOfAnyChar(inputString:=value, chars:=charList, startIndex)
            Dim length As Integer = endPos - startIndex
            value = value.Replace(oldValue:=value.Substring(startIndex, length), newValue:=kvp.Value)
        Next
        Return value
    End Function

End Module
