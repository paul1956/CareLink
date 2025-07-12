' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Internal structure to hold user data fields.
''' </summary>
Friend Structure CareLinkUserData
    Friend _autoLogin As Boolean
    Friend _careLinkPartner As Boolean
    Friend _careLinkPassword As String
    Friend _careLinkPatientUserID As String
    Friend _careLinkUserName As String
    Friend _countryCode As String
    Friend _iD As Integer
    Friend _useLocalTimeZone As Boolean
End Structure
