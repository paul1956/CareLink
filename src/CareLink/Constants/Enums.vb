' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Enums

    Friend Enum FileToLoadOptions As Integer
        LastSaved = 0
        TestData = 1
        Login = 2
    End Enum

    Friend Enum GetAuthorizationTokenResult
        InLoginProcess
        LoginFailed
        NetworkDown
        OK
    End Enum

End Module
