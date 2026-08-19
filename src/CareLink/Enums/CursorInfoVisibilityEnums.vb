' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module CursorInfoVisibilityEnums

    Public Enum CursorInfoVisibility As Integer
        None = 0
        Show1 = &B1
        Show2 = &B11
        Mask2 = &B10
        Show3 = &B111
        Mask3 = &B100
        Show4 = &B1111
        Mask4 = &B1000
        ShowAll = &B11111
        PictureBoxMask = &B10000
    End Enum

End Module
