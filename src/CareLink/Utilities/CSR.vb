' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Public Module CSR

    Public Function CreateCSR(keypair As RSA, cn As String, ou As String, dc As String, o As String) As String
        Dim subject As New X500DistinguishedName($"CN={cn}, OU={ou}, DC={dc}, O={o}")
        Dim request As New CertificateRequest(subject, keypair, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)
        Dim csr As Byte() = request.CreateSigningRequest()

        ' Convert the CSR to PEM format
        Dim pem As New StringBuilder()
        pem.AppendLine("-----BEGIN CERTIFICATE REQUEST-----")
        pem.AppendLine(Convert.ToBase64String(csr, Base64FormattingOptions.InsertLineBreaks))
        pem.AppendLine("-----END CERTIFICATE REQUEST-----")

        Return pem.ToString()
    End Function

    Public Function ReformatCsr(csr As String) As String
        ' Remove footer & header, re-encode with URL-safe base64
        csr = csr.Replace(vbCrLf, "")
        csr = csr.Replace("-----BEGIN CERTIFICATE REQUEST-----", "")
        csr = csr.Replace("-----END CERTIFICATE REQUEST-----", "")

        Dim csrRaw As Byte() = Convert.FromBase64String(csr)
        csr = Convert.ToBase64String(csrRaw).Replace("+", "-").Replace("/", "_").Replace("=", "")

        Return csr
    End Function

End Module
