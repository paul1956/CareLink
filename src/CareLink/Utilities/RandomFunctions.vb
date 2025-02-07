' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Security.Cryptography
Imports System.Text

Public Module RandomFunctions
    Public Function GenerateRandomBase64String(length As Integer) As String
        Dim random As New Random()
        Dim chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim result As New String(Enumerable.Repeat(chars, length + 10).Select(Function(s) s(random.Next(s.Length))).ToArray())
        Return Convert.ToBase64String(Encoding.UTF8.GetBytes(result)).Substring(0, length)
    End Function

    Public Function RandomUuid() As String
        Return Guid.NewGuid().ToString()
    End Function

    Public Function RandomAndroidModel() As String
        Dim models() As String = {"SM-G973F", "SM-G988U1", "SM-G981W", "SM-G9600"}
        Return models(New Random().Next(models.Length))
    End Function

    Public Function RandomDeviceId() As String
        Dim rng As RandomNumberGenerator = RandomNumberGenerator.Create()
        Dim randomBytes(39) As Byte
        rng.GetBytes(randomBytes)
        Dim hashBytes() As Byte = SHA256.HashData(randomBytes)
        Return "NzgxMjQwY2Q2NjNmOGJiYTZjN2I4ZDk4ZWVlMWE1NmM0NDc1Y2M4MGYxMWE2YmU4MmYxODYyNGFmMDJmMWVhYg=="
        Return Convert.ToHexStringLower(hashBytes)
    End Function

End Module
