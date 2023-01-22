' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices

Friend Module MailSetupDialogHelpers
    Friend Const InvalidNameCharacters As String = "\/:*?""<>| "

    <StringSyntax(StringSyntaxAttribute.Regex)>
    Friend Const SpacePattern As String = "\s+"

    Friend ReadOnly s_knownDefaultPorts As New Dictionary(Of String, Integer) From {
                    {"Microsoft Exchange", 0},
                    {"smtp.comcast.net", 587},
                    {"smtp.Gmail.com", 587},
                    {"smtpOut.SecureServer.net", 587},
                    {"smtp.mail.yahoo.com", 587}
                }

    Friend ReadOnly s_knownMailServers As New Dictionary(Of String, String) From {
                    {"Microsoft Exchange", ""},
                    {"Comcast/Xfinity", "smtp.comcast.net"},
                    {"Gmail", "smtp.Gmail.com"},
                    {"GoDaddy", "smtpOut.SecureServer.net"},
                    {"Yahoo", "smtp.mail.yahoo.com"}
                }

    <Extension>
    Friend Function IsValidEmailAddress(MailServerUserName As String, ByRef errorMsg As String) As Boolean
        If String.IsNullOrWhiteSpace(MailServerUserName) Then
            errorMsg = "Required"
            Return False
        End If

        Try
            Dim tempVar As New Net.Mail.MailAddress(MailServerUserName)
        Catch e1 As ArgumentException
            errorMsg = "Required"
            Return False
        Catch e2 As FormatException
            'textBox contains no valid mail address
            errorMsg = e2.Message
            Return False
        End Try
        Return True
    End Function

End Module
