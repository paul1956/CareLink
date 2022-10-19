' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module FXAssembly
    ' NB: this must never-ever change to facilitate type-forwarding from
    ' .NET Framework, if those are referenced in .NET project.
    Friend Const Version As String = "4.0.0.0"
End Module

Friend Module AssemblyRef
    Friend Const MicrosoftPublicKey As String = "b03f5f7f11d50a3a"
    Friend Const SystemDesign As String = "System.Design, Version=" & FXAssembly.Version & ", Culture=neutral, PublicKeyToken=" & MicrosoftPublicKey
    Friend Const SystemDrawingDesign As String = "System.Drawing.Design, Version=" & FXAssembly.Version & ", Culture=neutral, PublicKeyToken=" & MicrosoftPublicKey
    Friend Const SystemDrawing As String = "System.Drawing, Version=" & FXAssembly.Version & ", Culture=neutral, PublicKeyToken=" & MicrosoftPublicKey
End Module
