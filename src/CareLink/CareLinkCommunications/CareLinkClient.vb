Imports System
Imports System.IO
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json
Imports System.Security.Cryptography
Imports Microsoft.VisualBasic.Logging

Public Class CareLinkClient

    ' Constants
    Private Const VERSION As String = "1.2"

    Private Const DEFAULT_FILENAME As String = "logindata.json"
    Private Const CARELINK_CONFIG_URL As String = "https://clcloud.minimed.eu/connect/carepartner/v11/discover/android/3.2"

    Public Shared ReadOnly s_common_Headers As New Dictionary(Of String, String) From {
        {"Accept", "application/json"},
        {"Content-Type", "application/json"},
        {"User-Agent", "Dalvik/2.1.0 (Linux; U; Android 10; Nexus 5X Build/QQ3A.200805.001)"}
}

    Private ReadOnly _auth_Error_Codes As Integer() = {401, 403}
    Private ReadOnly _tokenFile As String
    Private ReadOnly _version As String
    Private _accessTokenPayload As Dictionary(Of String, Object)
    Private _config As Dictionary(Of String, Object)
    Private _country As String
    Private _lastApiStatus As Integer
    Private _patient As Dictionary(Of String, String)
    Private _tokenDataElement As JsonElement
    Private _user As Dictionary(Of String, Object)
    Private _username As String

    Public Sub New(Optional tokenFile As String = DEFAULT_FILENAME)
        _version = VERSION

        ' Authorization
        _tokenFile = tokenFile
        _tokenDataElement = Nothing
        _accessTokenPayload = Nothing

        ' API config
        _config = Nothing

        ' User info
        _username = Nothing
        _user = Nothing
        _patient = Nothing
        _country = Nothing

        ' API status
        _lastApiStatus = Nothing
    End Sub

    Private Shared Function ReadTokenFile(filename As String) As JsonElement
        Console.WriteLine(NameOf(ReadTokenFile))
        If File.Exists(filename) Then
            Try
                Dim jsonString As String = File.ReadAllText(filename)
                Dim tokenData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(jsonString)
                Dim requiredFields As String() = {"access_token", "refresh_token", "scope", "client_id", "client_secret", "mag-identifier"}
                For Each field As String In requiredFields
                    If Not tokenData.TryGetProperty(field, Nothing) Then
                        Console.WriteLine($"ERROR: field {field} is missing from token file")
                        Return Nothing
                    End If
                Next
                Return tokenData
            Catch ex As JsonException
                Console.WriteLine($"ERROR: failed parsing token file {filename}")
            End Try
        Else
            Console.WriteLine($"ERROR: token file {filename} not found")
        End If
        Return Nothing
    End Function

    Private Shared Sub WriteTokenFile(obj As JsonElement, filename As String)
        Console.WriteLine(NameOf(WriteTokenFile))
        File.WriteAllText(filename, JsonSerializer.Serialize(obj, s_jsonSerializerOptions))
    End Sub

    Private Shared Function GetConfig(discoveryUrl As String, country As String) As JsonElement
        Console.WriteLine(NameOf(GetConfig))
        Using client As New HttpClient()
            Dim response As String = client.GetStringAsync(discoveryUrl).Result
            Dim data As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(response)
            Dim region As JsonElement = Nothing
            Dim config As JsonElement

            For Each c As JsonElement In data.GetProperty("supportedCountries").EnumerateArray()
                If c.TryGetProperty(country.ToUpper(), region) Then
                    Exit For
                End If
            Next

            If region.ValueKind = JsonValueKind.Null Then
                Throw New Exception($"ERROR: country code {country} is not supported")
            End If
            Console.WriteLine($"   region: {region}")

            For Each c As JsonElement In data.GetProperty("CP").EnumerateArray()
                If region.ValueEquals(c.GetProperty("region").GetString()) Then
                    config = c
                    Exit For
                End If
            Next

            If config.ValueKind = JsonValueKind.Undefined Then
                Throw New Exception($"ERROR: failed to get config base URLs for region {region}")
            End If

            Dim ssoConfigResponse As String = client.GetStringAsync(config.GetProperty("SSOConfiguration").GetString()).Result
            Dim ssoConfig As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(ssoConfigResponse)
            Dim ssoBaseUrl As String = $"https://{ssoConfig.GetProperty("server").GetProperty("hostname").GetString()}:{ssoConfig.GetProperty("server").GetProperty("port").GetString()}/{ssoConfig.GetProperty("server").GetProperty("prefix").GetString()}"
            Dim tokenUrl As String = ssoBaseUrl & ssoConfig.GetProperty("oauth").GetProperty("system_endpoints").GetProperty("token_endpoint_path").GetString()

            Dim mutableConfig As Dictionary(Of String, JsonElement) = JsonSerializer.Deserialize(Of Dictionary(Of String, JsonElement))(config.GetRawText())
            mutableConfig("token_url") = JsonSerializer.Deserialize(Of JsonElement)($"""{tokenUrl}""")
            Return JsonSerializer.Deserialize(Of JsonElement)(JsonSerializer.Serialize(mutableConfig, s_jsonSerializerOptions))
        End Using
    End Function

    Private Function GetUser(config As JsonElement, tokenData As JsonElement) As JsonElement
        Console.WriteLine(NameOf(GetUser))
        Dim url As String = config.GetProperty("baseUrlCareLink").GetString() & "/users/me"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        headers("mag-identifier") = tokenData.GetProperty("mag-identifier").GetString()
        headers("Authorization") = "Bearer " & tokenData.GetProperty("access_token").GetString()

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim response As HttpResponseMessage = client.GetAsync(url).Result
            _lastApiStatus = CInt(response.StatusCode)
            Console.WriteLine($"   status: {_lastApiStatus}")

            If response.IsSuccessStatusCode Then
                Return JsonSerializer.Deserialize(Of JsonElement)(response.Content.ReadAsStringAsync().Result)
            End If
        End Using
        Return Nothing
    End Function

    Private Async Function GetPatient(config As JsonElement, token_data As JsonElement) As Task(Of Dictionary(Of String, String))
        Console.WriteLine(NameOf(GetPatient))
        Dim url As String = $"{CStr(config.ConvertJsonElementToDictionary("baseUrlCareLink"))}/links/patients"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        headers("mag-identifier") = CStr(token_data.ConvertJsonElementToDictionary("mag-identifier"))
        headers("Authorization") = $"Bearer {CStr(token_data.ConvertJsonElementToDictionary("access_token"))}"

        _lastApiStatus = Nothing

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim response As HttpResponseMessage = Await client.GetAsync(url)
            _lastApiStatus = CInt(response.StatusCode)
            Console.WriteLine($"   status: {_lastApiStatus}")

            If response.IsSuccessStatusCode Then
                Dim content As String = Await response.Content.ReadAsStringAsync()
                Dim patients As List(Of Dictionary(Of String, String)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, String)))(content)
                If patients.Count > 0 Then
                    Return patients(0)
                End If
            End If
        End Using

        Return Nothing
    End Function

    Private Function GetData(config As Dictionary(Of String, Object), tokenDataElement As JsonElement, username As String, role As String, patientId As String) As Dictionary(Of String, Object)
        Console.WriteLine("_get_data()")
        Dim url As String = $"{CStr(config("baseUrlCumulus"))}/display/message"
        Dim headers As New Dictionary(Of String, String)(s_common_Headers)
        Dim tokenData As Dictionary(Of String, String) = tokenDataElement.ConvertJsonElementToStringDictionary
        headers("mag-identifier") = tokenData("mag-identifier")
        headers("Authorization") = $"Bearer {tokenData("access_token")}"

        Dim data As New Dictionary(Of String, Object) From {
            {"username", username}
        }

        If role = "CARE_PARTNER" OrElse role = "CARE_PARTNER_OUS" Then
            data("role") = "carepartner"
            data("patientId") = patientId
        Else
            data("role") = "patient"
        End If

        _lastApiStatus = Nothing

        Using client As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                client.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim jsonContent As New StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
            Dim response As HttpResponseMessage = client.PostAsync(url, jsonContent).Result
            _lastApiStatus = CInt(response.StatusCode)
            Console.WriteLine($"   status: {_lastApiStatus}")

            If response.IsSuccessStatusCode Then
                Dim content As String = response.Content.ReadAsStringAsync().Result
                Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(content)
            End If
        End Using
        Return Nothing
    End Function

    Private Shared Function DoRefresh(config As Dictionary(Of String, Object), tokenDataElement As JsonElement) As JsonElement
        Console.WriteLine(NameOf(DoRefresh))
        Dim tokenUrl As String = CStr(config("token_url"))

        Dim tokenData As Dictionary(Of String, String) = tokenDataElement.ConvertJsonElementToStringDictionary

        Dim data As New Dictionary(Of String, String) From {
        {"refresh_token", tokenData("refresh_token")},
        {"client_id", tokenData("client_id")},
        {"client_secret", tokenData("client_secret")},
        {"grant_type", "refresh_token"}
    }

        Dim headers As New Dictionary(Of String, String) From {
        {"mag-identifier", tokenData("mag-identifier")}
    }

        Using httpClient As New HttpClient()
            For Each header As KeyValuePair(Of String, String) In headers
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value)
            Next

            Dim content As New FormUrlEncodedContent(data)
            Dim response As HttpResponseMessage = httpClient.PostAsync(tokenUrl, content).Result

            Console.WriteLine($"   status: {response.StatusCode}")

            If response.StatusCode <> Net.HttpStatusCode.OK Then
                Throw New Exception("ERROR: failed to refresh token")
            End If

            Dim responseBody As String = response.Content.ReadAsStringAsync().Result
            Dim newData As JsonElement = JsonSerializer.Deserialize(Of JsonElement)(responseBody)

            tokenData("access_token") = newData.GetProperty("access_token").GetString()
            tokenData("refresh_token") = newData.GetProperty("refresh_token").GetString()

            Return JsonSerializer.Deserialize(Of JsonElement)(JsonSerializer.Serialize(tokenData, s_jsonSerializerOptions))
        End Using
    End Function

    Private Shared Function GetAccessTokenPayload(token_data As JsonElement) As Dictionary(Of String, Object)
        Console.WriteLine(NameOf(GetAccessTokenPayload))
        Try
            Dim token As String = CStr(token_data.ConvertJsonElementToDictionary("access_token"))
            Dim payload_b64 As String = token.Split("."c)(1)
            Dim payload_b64_bytes As Byte() = Encoding.UTF8.GetBytes(payload_b64)
            Dim missing_padding As Integer = (4 - (payload_b64_bytes.Length Mod 4)) Mod 4
            If missing_padding > 0 Then
                payload_b64 &= New String("="c, missing_padding)
            End If
            Dim payload_bytes As Byte() = Convert.FromBase64String(payload_b64)
            Dim payload As String = Encoding.UTF8.GetString(payload_bytes)
            Return JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(payload)
        Catch ex As Exception
            Console.WriteLine("   no access token found or malformed access token")
            Return Nothing
        End Try
    End Function

    Private Shared Function IsTokenValid(access_token_payload As Dictionary(Of String, Object)) As Boolean
        Console.WriteLine(NameOf(IsTokenValid))
        Try
            ' Get expiration time stamp
            Dim tokenValidTo As Long = CType(access_token_payload("exp"), JsonElement).GetInt64()

            ' Check expiration time stamp
            Dim currentTime As Long = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            Dim tDiff As Long = tokenValidTo - currentTime

            If tDiff < 0 Then
                Console.WriteLine($"   access token has expired {Math.Abs(tDiff)}s ago")
                Return False
            End If

            If tDiff < 600 Then
                Console.WriteLine($"   access token is about to expire in {tDiff}s")
                Return False
            End If

            ' Token is valid
            Dim authTokenValidTo As String = DateTimeOffset.FromUnixTimeSeconds(tokenValidTo).ToString("ddd MMM dd HH:mm:ss UTC yyyy")
            Console.WriteLine($"   access token expires in {tDiff}s ({authTokenValidTo})")
            Return True
        Catch ex As Exception
            Console.WriteLine("   missing data in access token")
            Return False
        End Try
    End Function

    Private Function _init() As Boolean
        _tokenDataElement = ReadTokenFile(_tokenFile)
        If _tokenDataElement.ValueKind = JsonValueKind.Null Then
            Return False
        End If

        _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
        If _accessTokenPayload Is Nothing Then
            Return False
        End If
        Dim jsonConfigElement As JsonElement
        Try
            Dim payload As Dictionary(Of String, String) = CType(_accessTokenPayload("token_details"), Dictionary(Of String, String))
            _country = CStr(payload("country"))
            jsonConfigElement = GetConfig(CARELINK_CONFIG_URL, _country)
            _config = jsonConfigElement.ConvertJsonElementToDictionary
            _username = CStr(payload("preferred_username"))
            _user = Me.GetUser(jsonConfigElement, _tokenDataElement).ConvertJsonElementToDictionary
            Dim role As String = CStr(_user("role"))
            If role = "CARE_PARTNER" OrElse role = "CARE_PARTNER_OUS" Then
                _patient = Me.GetPatient(jsonConfigElement, _tokenDataElement).Result
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString())

            If _aUTH_ERROR_CODES.Contains(CInt(_lastApiStatus)) Then
                Try
                    _tokenDataElement = DoRefresh(jsonConfigElement.ConvertJsonElementToDictionary, _tokenDataElement)
                    _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
                    WriteTokenFile(_tokenDataElement, _tokenFile)
                Catch refreshEx As Exception
                    Console.WriteLine(refreshEx.ToString())
                End Try
            End If

            Return False
        End Try

        Return True
    End Function

    Public Function Init() As Boolean
        ' First try
        If Not Me._init() Then
            ' Second try (after token refresh)
            If Not Me._init() Then
                ' Failed permanently
                'Log.Error("ERROR: unable to initialize")
                Return False
            End If
        End If
        Return True
    End Function

    Public Sub PrintUserInfo()
        Console.WriteLine("User Info:")
        Console.WriteLine($"   user:     {_username} ({_user("firstName")} {_user("lastName")})")
        Console.WriteLine($"   role:     {_user("role")}")
        Console.WriteLine($"   country:  {_country}")
        If _patient IsNot Nothing Then
            Console.WriteLine($"   patient:  {_patient("username")} ({_patient("firstName")} {_patient("lastName")})")
        End If
    End Sub

    ''' <summary>
    ''' Get recent periodic pump data
    ''' </summary>
    Public Function GetRecentData() As Dictionary(Of String, Object)
        ' Check if access token is valid
        If Not IsTokenValid(_accessTokenPayload) Then
            _tokenDataElement = DoRefresh(_config, _tokenDataElement)
            _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
            WriteTokenFile(_tokenDataElement, _tokenFile)
            If Not IsTokenValid(_accessTokenPayload) Then
                Console.Error.WriteLine("ERROR: unable to get valid access token")
                Return Nothing
            End If
        End If

        Dim patientId As String = Nothing
        If _patient IsNot Nothing Then
            patientId = _patient("username")
        End If

        ' Get data: first try
        Dim data As Dictionary(Of String, Object) = Me.GetData(config:=_config,
                                           tokenDataElement:=_tokenDataElement,
                                           username:=_username,
                                           role:=CStr(_user("role")),
                                           patientId:=patientId)

        ' Check API response
        If _auth_Error_Codes.Contains(_lastApiStatus) Then
            ' Try to refresh token
            _tokenDataElement = DoRefresh(_config, _tokenDataElement)
            _accessTokenPayload = GetAccessTokenPayload(_tokenDataElement)
            WriteTokenFile(_tokenDataElement, _tokenFile)

            ' Get data: second try
            data = Me.GetData(config:=_config,
                           _tokenDataElement,
                           _username,
                           CStr(_user("role")),
                           patientId)

            ' Check API response
            If _auth_Error_Codes.Contains(_lastApiStatus) Then
                ' Failed permanently
                Console.Error.WriteLine("ERROR: unable to get data")
                Return Nothing
            End If
        End If

        Return data
    End Function

    ''' <summary>
    ''' Get last API response code
    ''' </summary>
    Public Function GetLastResponseCode() As Integer
        Return _lastApiStatus
    End Function

    ''' <summary>
    ''' Get Client library version
    ''' </summary>
    Public Function GetClientVersion() As String
        Return _version
    End Function

End Class
