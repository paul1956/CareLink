' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class AccessTokenDetails
    Public Property Scope As String
    Public Property ExpiresIn As Integer
    Public Property TokenType As String
    Public Property PreferredUsername As String
    Public Property Name As String
    Public Property GivenName As String
    Public Property FamilyName As String
    Public Property Locale As String
    Public Property Country As String
    Public Property Roles As List(Of String)
End Class
