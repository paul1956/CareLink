namespace Namespace {
    
    using json;
    
    using requests;
    
    using urlparse = urllib.parse.urlparse;
    
    using parse_qsl = urllib.parse.parse_qsl;
    
    using System.Collections.Generic;
    
    using System;
    
    public static class Module {
        
        public static object VERSION = "0.1";
        
        public static object CARELINK_CONNECT_SERVER_EU = "carelink.minimed.eu";
        
        public static object CARELINK_CONNECT_SERVER_US = "carelink.minimed.com";
        
        public static object CARELINK_LANGUAGE_EN = "en";
        
        public static object CARELINK_LOCALE_EN = "en";
        
        public static object CARELINK_AUTH_TOKEN_COOKIE_NAME = "auth_tmp_token";
        
        public static object CARELINK_TOKEN_VALIDTO_COOKIE_NAME = "c_token_valid_to";
        
        public static object AUTH_EXPIRE_DEADLINE_MINUTES = 1;
        
        public static object DEBUG = false;
        
        public static object printdbg(object msg) {
            if (DEBUG) {
                Console.WriteLine(msg);
            }
        }
        
        public class CareLinkClient
            : object {
            
            public CareLinkClient(object carelinkUsername, object carelinkPassword, object carelinkCountry) {
                // User info
                this.@__carelinkUsername = carelinkUsername;
                this.@__carelinkPassword = carelinkPassword;
                this.@__carelinkCountry = carelinkCountry;
                // Session info
                this.@__sessionUser = null;
                this.@__sessionProfile = null;
                this.@__sessionCountrySettings = null;
                this.@__sessionMonitorData = null;
                // State info
                this.@__loginInProcess = false;
                this.@__loggedIn = false;
                this.@__lastDataSuccess = false;
                this.@__lastResponseCode = null;
                this.@__lastErrorMessage = null;
                this.@__commonHeaders = new Dictionary<object, object> {
                    {
                        "Accept-Language",
                        "en;q=0.9, *;q=0.8"},
                    {
                        "Connection",
                        "keep-alive"},
                    {
                        "sec-ch-ua",
                        "\"Google Chrome\";v=\"87\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"87\""},
                    {
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36"},
                    {
                        "Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"}};
                // Create main http client session with CookieJar
                this.@__httpClient = requests.Session();
            }
            
            public virtual object getLastDataSuccess() {
                return this.@__lastDataSuccess;
            }
            
            public virtual object getLastResponseCode() {
                return this.@__lastResponseCode;
            }
            
            public virtual object getLastErrorMessage() {
                return this.@__lastErrorMessage;
            }
            
            // Get server URL
            public virtual object @__careLinkServer() {
                return this.@__carelinkCountry == "us" ? CARELINK_CONNECT_SERVER_US : CARELINK_CONNECT_SERVER_EU;
            }
            
            public virtual object @__extractResponseData(object responseBody, object begstr, object endstr) {
                var beg = responseBody.find(begstr) + begstr.Count;
                var end = responseBody.find(endstr, beg);
                return responseBody[beg::end].strip("\"");
            }
            
            public virtual object @__getLoginSession() {
                var url = "https://" + this.@__careLinkServer() + "/patient/sso/login";
                var payload = new Dictionary<object, object> {
                    {
                        "country",
                        this.@__carelinkCountry},
                    {
                        "lang",
                        CARELINK_LANGUAGE_EN}};
                try {
                    var response = this.@__httpClient.get(url, headers: this.@__commonHeaders, params: payload);
                    if (!response.ok) {
                        throw ValueError("session response is not OK");
                    }
                } catch (Exception) {
                    printdbg(e);
                    printdbg("__getLoginSession() failed");
                }
                return response;
            }
            
            public virtual object @__doLogin(object loginSessionResponse) {
                var queryParameters = parse_qsl(urlparse(loginSessionResponse.url).query).ToDictionary();
                var url = "https://mdtlogin.medtronic.com" + "/mmcl/auth/oauth/v2/authorize/login";
                var payload = new Dictionary<object, object> {
                    {
                        "country",
                        this.@__carelinkCountry},
                    {
                        "locale",
                        CARELINK_LOCALE_EN}};
                var form = new Dictionary<object, object> {
                    {
                        "sessionID",
                        queryParameters["sessionID"]},
                    {
                        "sessionData",
                        queryParameters["sessionData"]},
                    {
                        "locale",
                        CARELINK_LOCALE_EN},
                    {
                        "action",
                        "login"},
                    {
                        "username",
                        this.@__carelinkUsername},
                    {
                        "password",
                        this.@__carelinkPassword},
                    {
                        "actionButton",
                        "Log in"}};
                try {
                    var response = this.@__httpClient.post(url, headers: this.@__commonHeaders, params: payload, data: form);
                    if (!response.ok) {
                        throw ValueError("session response is not OK");
                    }
                } catch (Exception) {
                    printdbg(e);
                    printdbg("__doLogin() failed");
                }
                return response;
            }
            
            public virtual object @__doConsent(object doLoginResponse) {
                // Extract data for consent
                var doLoginRespBody = doLoginResponse.text;
                var url = this.@__extractResponseData(doLoginRespBody, "<form action=", " ");
                var sessionID = this.@__extractResponseData(doLoginRespBody, "<input type=\"hidden\" name=\"sessionID\" value=", ">");
                var sessionData = this.@__extractResponseData(doLoginRespBody, "<input type=\"hidden\" name=\"sessionData\" value=", ">");
                // Send consent
                var form = new Dictionary<object, object> {
                    {
                        "action",
                        "consent"},
                    {
                        "sessionID",
                        sessionID},
                    {
                        "sessionData",
                        sessionData},
                    {
                        "response_type",
                        "code"},
                    {
                        "response_mode",
                        "query"}};
                // Add header
                var consentHeaders = this.@__commonHeaders;
                consentHeaders["Content-Type"] = "application/x-www-form-urlencoded";
                try {
                    var response = this.@__httpClient.post(url, headers: consentHeaders, data: form);
                    if (!response.ok) {
                        throw ValueError("session response is not OK");
                    }
                } catch (Exception) {
                    printdbg(e);
                    printdbg("__doConsent() failed");
                }
                return response;
            }
            
            public virtual object @__getData(object host, object path, object queryParams, object requestBody) {
                object response;
                object url;
                printdbg("__getData()");
                this.@__lastDataSuccess = false;
                if (host == null) {
                    url = path;
                } else {
                    url = "https://" + host + "/" + path;
                }
                var payload = queryParams;
                var data = requestBody;
                object jsondata = null;
                // Get auth token
                var authToken = this.@__getAuthorizationToken();
                if (authToken != null) {
                    try {
                        // Add header
                        var headers = this.@__commonHeaders;
                        headers["Authorization"] = authToken;
                        if (data == null) {
                            headers["Accept"] = "application/json, text/plain, */*";
                            headers["Content-Type"] = "application/json; charset=utf-8";
                            response = this.@__httpClient.get(url, headers: headers, params: payload);
                            this.@__lastResponseCode = response.status_code;
                            if (!response.ok) {
                                throw ValueError("session get response is not OK");
                            }
                        } else {
                            headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                            headers["Content-Type"] = "application/x-www-form-urlencoded";
                            response = this.@__httpClient.post(url, headers: headers, data: data);
                            this.@__lastResponseCode = response.status_code;
                            if (!response.ok) {
                                printdbg(response.status_code);
                                throw ValueError("session post response is not OK");
                            }
                        }
                    } catch (Exception) {
                        printdbg(e);
                        printdbg("__getData() failed");
                    }
                }
                return jsondata;
            }
            
            public virtual object @__getMyUser() {
                printdbg("__getMyUser()");
                return this.@__getData(this.@__careLinkServer(), "patient/users/me", null, null);
            }
            
            public virtual object @__getMyProfile() {
                printdbg("__getMyProfile()");
                return this.@__getData(this.@__careLinkServer(), "patient/users/me/profile", null, null);
            }
            
            public virtual object @__getCountrySettings(object country, object language) {
                printdbg("__getCountrySettings()");
                var queryParams = new Dictionary<object, object> {
                    {
                        "countryCode",
                        country},
                    {
                        "language",
                        language}};
                return this.@__getData(this.@__careLinkServer(), "patient/countries/settings", queryParams, null);
            }
            
            public virtual object @__getMonitorData() {
                printdbg("__getMonitorData()");
                return this.@__getData(this.@__careLinkServer(), "patient/monitor/data", null, null);
            }
            
            // Old last24hours webapp data
            public virtual object @__getLast24Hours() {
                printdbg("__getLast24Hours");
                var queryParams = new Dictionary<object, object> {
                    {
                        "cpSerialNumber",
                        "NONE"},
                    {
                        "msgType",
                        "last24hours"},
                    {
                        "requestTime",
                        Convert.ToInt32(time.time() * 1000).ToString()}};
                return this.@__getData(this.@__careLinkServer(), "patient/connect/data", queryParams, null);
            }
            
            // Periodic data from CareLink Cloud
            public virtual object @__getConnectDisplayMessage(object username, object role, object endpointUrl) {
                printdbg("__getConnectDisplayMessage()");
                // Build user json for request
                var userJson = new Dictionary<object, object> {
                    {
                        "username",
                        username},
                    {
                        "role",
                        role}};
                var requestBody = json.dumps(userJson);
                var recentData = this.@__getData(null, endpointUrl, null, requestBody);
                if (recentData != null) {
                    this.@__correctTimeInRecentData(recentData);
                }
                return recentData;
            }
            
            public virtual object @__correctTimeInRecentData(object recentData) {
                // TODO
            }
            
            public virtual object @__executeLoginProcedure() {
                var lastLoginSuccess = false;
                this.@__loginInProcess = true;
                this.@__lastErrorMessage = null;
                try {
                    // Clear cookies
                    this.@__httpClient.cookies.clear_session_cookies();
                    // Clear basic infos
                    this.@__sessionUser = null;
                    this.@__sessionProfile = null;
                    this.@__sessionCountrySettings = null;
                    this.@__sessionMonitorData = null;
                    // Open login (get SessionId and SessionData)
                    var loginSessionResponse = this.@__getLoginSession();
                    this.@__lastResponseCode = loginSessionResponse.status_code;
                    // Login
                    var doLoginResponse = this.@__doLogin(loginSessionResponse);
                    this.@__lastResponseCode = doLoginResponse.status_code;
                    //setLastResponseBody(loginSessionResponse)
                    loginSessionResponse.close();
                    // Consent
                    var consentResponse = this.@__doConsent(doLoginResponse);
                    this.@__lastResponseCode = consentResponse.status_code;
                    //setLastResponseBody(consentResponse);
                    doLoginResponse.close();
                    consentResponse.close();
                    // Get sessions infos if required
                    if (this.@__sessionUser == null) {
                        this.@__sessionUser = this.@__getMyUser();
                    }
                    if (this.@__sessionProfile == null) {
                        this.@__sessionProfile = this.@__getMyProfile();
                    }
                    if (this.@__sessionCountrySettings == null) {
                        this.@__sessionCountrySettings = this.@__getCountrySettings(this.@__carelinkCountry, CARELINK_LANGUAGE_EN);
                    }
                    if (this.@__sessionMonitorData == null) {
                        this.@__sessionMonitorData = this.@__getMonitorData();
                    }
                    // Set login success if everything was ok:
                    if (this.@__sessionUser != null && this.@__sessionProfile != null && this.@__sessionCountrySettings != null && this.@__sessionMonitorData != null) {
                        lastLoginSuccess = true;
                    }
                } catch (Exception) {
                    printdbg(e);
                    this.@__lastErrorMessage = e;
                }
                this.@__loginInProcess = false;
                this.@__loggedIn = lastLoginSuccess;
                return lastLoginSuccess;
            }
            
            public virtual object @__getAuthorizationToken() {
                var auth_token = this.@__httpClient.cookies.get(CARELINK_AUTH_TOKEN_COOKIE_NAME);
                var auth_token_validto = this.@__httpClient.cookies.get(CARELINK_TOKEN_VALIDTO_COOKIE_NAME);
                // New token is needed:
                // a) no token or about to expire => execute authentication
                // b) last response 401
                if (auth_token == null || auth_token_validto == null || new List<object> {
                    401,
                    403
                }.Contains(this.@__lastResponseCode)) {
                    // TODO: add check for expired token
                    // execute new login process | null, if error OR already doing login
                    //if loginInProcess or not executeLoginProcedure():
                    if (this.@__loginInProcess) {
                        printdbg("loginInProcess");
                        return null;
                    }
                    if (!this.@__executeLoginProcedure()) {
                        printdbg("__executeLoginProcedure failed");
                        return null;
                    }
                    printdbg("auth_token_validto = " + this.@__httpClient.cookies.get(CARELINK_TOKEN_VALIDTO_COOKIE_NAME));
                }
                // there can be only one
                return "Bearer " + this.@__httpClient.cookies.get(CARELINK_AUTH_TOKEN_COOKIE_NAME);
            }
            
            // Wrapper for data retrival methods
            public virtual object getRecentData() {
                // Force login to get basic info
                if (this.@__getAuthorizationToken() != null) {
                    if (this.@__carelinkCountry == "us" || this.@__sessionMonitorData["deviceFamily"] == "BLE_X") {
                        var role = new List<object> {
                            "CARE_PARTNER",
                            "CARE_PARTNER_OUS"
                        }.Contains(this.@__sessionUser["role"]) ? "carepartner" : "patient";
                        return this.@__getConnectDisplayMessage(this.@__sessionProfile["username"], role, this.@__sessionCountrySettings["blePereodicDataEndpoint"]);
                    } else {
                        return this.@__getLast24Hours();
                    }
                } else {
                    return null;
                }
            }
            
            // Authentication methods
            public virtual object login() {
                if (!this.@__loggedIn) {
                    this.@__executeLoginProcedure();
                }
                return this.@__loggedIn;
            }
        }
    }
}
