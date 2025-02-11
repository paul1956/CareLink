' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Org.BouncyCastle.Asn1.X509
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.Security

Public Module CSR

    Public Function CreateCSR(keypair As AsymmetricCipherKeyPair, cn As String, ou As String, dc As String, o As String) As String
        Dim subject As New X509Name($"CN={cn}, OU={ou}, DC={dc}, O={o}")
        Dim csr As New Pkcs10CertificationRequest(
            "SHA256WITHRSA",
            subject,
            keypair.Public,
            Nothing,
            keypair.Private
        )
        Return Convert.ToBase64String(csr.GetEncoded())
    End Function

    Public Function GenerateKeyPair() As AsymmetricCipherKeyPair
        Dim generator As New RsaKeyPairGenerator()
        generator.Init(New KeyGenerationParameters(New SecureRandom(), 2048))
        Return generator.GenerateKeyPair()
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
