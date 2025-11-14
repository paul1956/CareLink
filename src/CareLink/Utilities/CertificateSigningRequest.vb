' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Public Module CertificateSigningRequest

    ''' <summary>
    '''  Creates a Certificate Signing Request in PEM format using
    '''  the specified RSA key pair and subject details.
    ''' </summary>
    ''' <param name="key">The RSA key pair to use for signing the CSR.</param>
    ''' <param name="cn">The Common Name (CN) for the subject.</param>
    ''' <param name="ou">The Organizational Unit (OU) for the subject.</param>
    ''' <param name="dc">The Domain Component (DC) for the subject.</param>
    ''' <param name="o">The Organization (O) for the subject.</param>
    ''' <returns>
    '''  A string containing the CSR in PEM format, including header and footer.
    ''' </returns>
    Public Function CreateCertificateSigningRequest(
        key As RSA,
        cn As String,
        ou As String,
        dc As String,
        o As String) As String

        Dim distinguishedName As String = $"CN={cn}, OU={ou}, DC={dc}, O={o}"
        Dim subjectName As New X500DistinguishedName(distinguishedName)
        Dim request As New CertificateRequest(
            subjectName,
            key,
            hashAlgorithm:=HashAlgorithmName.SHA256,
            padding:=RSASignaturePadding.Pkcs1)

        Dim inArray As Byte() = request.CreateSigningRequest()

        ' Convert the Certificate Signing Request to PEM format
        Dim pem As New StringBuilder()
        pem.AppendLine(value:="-----BEGIN CERTIFICATE REQUEST-----")
        Const options As Base64FormattingOptions = Base64FormattingOptions.InsertLineBreaks
        pem.AppendLine(value:=Convert.ToBase64String(inArray, options))
        pem.AppendLine(value:="-----END CERTIFICATE REQUEST-----")

        Return pem.ToString()
    End Function

    ''' <summary>
    '''  Reformats a PEM-encoded Certificate Signing Request by
    '''  removing the header and footer, and re-encodes it using URL-safe base64 encoding.
    ''' </summary>
    ''' <param name="csr">The Certificate Signing Request string in PEM format.</param>
    ''' <returns>
    '''  A URL-safe base64 encoded string representing
    '''  the raw Certificate Signing Request data.
    ''' </returns>
    Public Function ReformatCertificateSigningRequest(csr As String) As String
        ' Remove footer & header, re-encode with URL-safe base64
        csr = csr.Replace(oldValue:=vbCrLf, newValue:=EmptyString)
        csr = csr.Replace(oldValue:="-----BEGIN CERTIFICATE REQUEST-----", newValue:=EmptyString)
        csr = csr.Replace(oldValue:="-----END CERTIFICATE REQUEST-----", newValue:=EmptyString)

        Dim inArray As Byte() = Convert.FromBase64String(csr)
        csr = Convert.ToBase64String(inArray) _
                     .Replace(oldValue:="+", newValue:="-") _
                     .Replace(oldValue:="/", newValue:="_") _
                     .Replace(oldValue:="=", newValue:=EmptyString)

        Return csr
    End Function

End Module
