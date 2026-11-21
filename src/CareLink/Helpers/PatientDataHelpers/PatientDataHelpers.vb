' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Nodes

Public Module PatientDataHelpers

    Private ReadOnly s_keyDictionary As New Dictionary(Of String, String)(Comparer) From {
        {NameOf(ServerDataEnum.firstName), "First"},
        {NameOf(ServerDataEnum.lastName), "Last"},
        {NameOf(ServerDataEnum.conduitSerialNumber), Guid.Empty.ToString()},
        {NameOf(MedicalDeviceInformation.SystemId), "40000000000 0000"},
        {NameOf(MedicalDeviceInformation.DeviceSerialNumber), "NG4000000H"}}

    Private Sub CleanPatientNode(node As JsonNode)
        If TypeOf node Is JsonObject Then
            Dim jsonObject As JsonObject = DirectCast(node, JsonObject)
            For Each entry As KeyValuePair(Of String, JsonNode) In jsonObject.ToArray()
                Dim replacement As String = Nothing
                If s_keyDictionary.TryGetValue(key:=entry.Key, value:=replacement) Then
                    jsonObject(propertyName:=entry.Key) = JsonValue.Create(value:=replacement)
                Else
                    CleanPatientNode(node:=entry.Value)
                End If
            Next
        ElseIf TypeOf node Is JsonArray Then
            For Each item As JsonNode In DirectCast(node, JsonArray)
                CleanPatientNode(node:=item)
            Next
        End If
    End Sub

    ''' <summary>
    '''  Serialize <see cref="PatientDataElement"/> while removing any personal information
    ''' </summary>
    ''' <returns>String without any personal information</returns>
    Public Function CleanPatientData() As String
        Dim value As String = PatientDataElement.ElementToString()
        If String.IsNullOrWhiteSpace(value) Then
            Return value
        End If
        Dim node As JsonNode = JsonNode.Parse(json:=value)
        CleanPatientNode(node)
        Return node.ToJsonString(options:=SerializerOptions)
    End Function

End Module
