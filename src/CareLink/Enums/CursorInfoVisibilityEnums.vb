' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module CursorInfoVisibilityEnums

    Public Enum CursorInfo As Integer
        Hide1 = &B1110
        Mask2 = &B__10
        Mask3 = &B_100
        Mask4 = &B1000
        Show1 = &B___1
        Show3 = &B_111
        ShowAll = &B1111
    End Enum

End Module
