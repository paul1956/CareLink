' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Security.Cryptography
Imports System.Text

''' <summary>
'''  Provides utility functions for generating random values and identifiers.
''' </summary>
Public Module RandomFunctions

    ''' <summary>
    '''  Generates a random Base64-encoded string of the specified length.
    ''' </summary>
    ''' <param name="count">The desired length of the resulting Base64 string.</param>
    ''' <returns>A random Base64-encoded string of the specified length.</returns>
    Public Function GenerateRandomBase64String(count As Integer) As String
        Dim random As New Random()
        Dim element As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        Dim selector As Func(Of String, Char) = Function(str)
                                                    Return str(index:=random.Next(maxValue:=str.Length))
                                                End Function

        Dim s As New String(value:=Enumerable.Repeat(element, count:=count + 10).Select(selector).ToArray())
        Return Convert.ToBase64String(inArray:=Encoding.UTF8.GetBytes(s)).Substring(startIndex:=0, length:=count)
    End Function

    ''' <summary>
    '''  Generates a new random UUID (Universally Unique Identifier).
    ''' </summary>
    ''' <returns>A string representation of a new UUID.</returns>
    Public Function RandomUuid() As String
        Return Guid.NewGuid().ToString()
    End Function

    ''' <summary>
    '''  Returns a random Android device model from a predefined list.
    ''' </summary>
    ''' <returns>A string representing a random Android device model.</returns>
    Public Function RandomAndroidModel() As String
        Dim models() As String = {"SM-G973F", "SM-G988U1", "SM-G981W", "SM-G9600"}
        Return models(New Random().Next(maxValue:=models.Length))
    End Function

    ''' <summary>
    '''  Generates a random device ID as a lowercase hexadecimal string.
    ''' </summary>
    ''' <returns>A random device ID in lowercase hexadecimal format.</returns>
    Public Function RandomDeviceId() As String
        Dim data(39) As Byte
        Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
            rng.GetBytes(data)
        End Using
        Dim inArray As Byte() = SHA256.HashData(source:=data)
        Return Convert.ToHexStringLower(inArray)
    End Function

End Module
