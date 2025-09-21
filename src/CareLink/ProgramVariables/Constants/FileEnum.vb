' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module FileEnum

    ''' <summary>
    '''  Specifies the options for which file to load during login or data initialization.
    ''' </summary>
    Public Enum FileToLoadOptions As Integer
        LastSaved = 0
        Login = 1
        NewUser = 2
        Snapshot = 3
        TestData = 4
    End Enum

End Module
