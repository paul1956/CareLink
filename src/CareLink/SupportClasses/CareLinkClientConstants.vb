' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Partial Public Class CareLinkClient
    Private Const BadRequestMessage As String = "Login Failure __doConsent() failed with HttpStatusCode.BadRequest"
    Private Const CarelinkAuthTokenCookieName As String = "auth_tmp_token"
    Private Const CarelinkConnectServerEu As String = "carelink.minimed.eu"
    Private Const CarelinkConnectServerOther As String = "carelink.minimed.eu"
    Private Const CarelinkConnectServerUs As String = "carelink.minimed.com"
    Private Const CarelinkLanguageEn As String = "en"
    Private Const CarelinkLocaleEn As String = "en"
    Private Const CarelinkTokenValidtoCookieName As String = "c_token_valid_to"

    Private ReadOnly _carelinkPartnerType As New List(Of String) From {
                        "CARE_PARTNER",
                        "CARE_PARTNER_OUS"}

    Private ReadOnly _commonHeaders As New Dictionary(Of String, String) From {
                        {
                            "Accept-Language",
                            "en;q=0.9, *;q=0.8"},
                        {
                            "Connection",
                            "keep-alive"},
                        {
                            "sec-ch-ua",
                            """Google Chrome"";deviceFamily=""87"", "" Not;A Brand"";deviceFamily=""99"", ""Chromium"";deviceFamily=""87"""},
                        {
                            "User-Agent",
                            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36"},
                        {
                            "Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;deviceFamily=b3;q=0.9"}}

End Class
