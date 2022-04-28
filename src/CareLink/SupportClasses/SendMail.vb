' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Net
Imports System.Net.Mail
Imports Microsoft.Exchange.WebServices.Data

Public Class SendMail

    Public client As New SmtpClient()

    Public service As New ExchangeService()

    Public Sub New(UseExchange As Boolean, userEmailAddress As String, userPassword As String, Optional Host As String = "", Optional Port As Integer = 0)
        If String.IsNullOrWhiteSpace(userEmailAddress) Then
            Throw New ArgumentException($"'{NameOf(userEmailAddress)}' cannot be null or whitespace.", NameOf(userEmailAddress))
        End If

        If String.IsNullOrWhiteSpace(userPassword) Then
            Throw New ArgumentException($"'{NameOf(userPassword)}' cannot be null or whitespace.", NameOf(userPassword))
        End If
        If UseExchange Then
            ' Set specific credentials.
            service.Credentials = New NetworkCredential(userEmailAddress, userPassword)
            service.AutodiscoverUrl(userEmailAddress, AddressOf RedirectionCallback)
        Else
            If String.IsNullOrWhiteSpace(Host) Then
                Throw New ArgumentException($"'{NameOf(Host)}' cannot be null or whitespace.", NameOf(Host))
            End If

            If Port = 0 Then
                Throw New ArgumentException($"'{NameOf(Port)}' cannot be 0.", NameOf(Port))
            End If
            'setup SMTP Host Here
            client.Host = Host
            client.Port = Port
            client.UseDefaultCredentials = False
            client.DeliveryMethod = SmtpDeliveryMethod.Network
            client.Credentials = New NetworkCredential(userEmailAddress, userPassword)
            client.TargetName = $"STARTTLS/{Host}"
            client.EnableSsl = True

        End If

    End Sub

    Private Shared Function RedirectionCallback(url As String) As Boolean
        ' Return true if the URL is an HTTPS URL.
        Return url.ToLower().StartsWith("https://")
    End Function

    Public Sub Send(sendTo As String, sendFrom As String, subject As String, Optional body As String = "")
        If String.IsNullOrWhiteSpace(sendTo) Then
            Throw New ArgumentException($"'{NameOf(sendTo)}' cannot be null or empty.", NameOf(sendTo))
        End If

        If String.IsNullOrWhiteSpace(sendFrom) Then
            Throw New ArgumentException($"'{NameOf(sendFrom)}' cannot be null or empty.", NameOf(sendFrom))
        End If

        If String.IsNullOrWhiteSpace(subject) Then
            Throw New ArgumentException($"'{NameOf(subject)}' cannot be null or empty.", NameOf(subject))
        End If

        If body Is Nothing Then
            Throw New ArgumentException($"'{NameOf(body)}' cannot be null or empty.", NameOf(body))
        End If

        'Set up message settings
        ' Enviar E-mail
        client.Send(sendFrom, sendTo, subject, body)
    End Sub

    Public Sub SendUsingExchange(sendTo As String, subject As String, Optional body As String = "")
        If String.IsNullOrWhiteSpace(sendTo) Then
            Throw New ArgumentException($"'{NameOf(sendTo)}' cannot be null or empty.", NameOf(sendTo))
        End If

        If String.IsNullOrWhiteSpace(subject) Then
            Throw New ArgumentException($"'{NameOf(subject)}' cannot be null or empty.", NameOf(subject))
        End If

        If body Is Nothing Then
            Throw New ArgumentException($"'{NameOf(body)}' cannot be null or empty.", NameOf(body))
        End If

        Dim message As New EmailMessage(service) With {
            .Subject = subject,
            .Body = body
        }
        message.ToRecipients.Add(sendTo)
        message.Save()

        message.SendAndSaveCopy()

    End Sub

End Class
