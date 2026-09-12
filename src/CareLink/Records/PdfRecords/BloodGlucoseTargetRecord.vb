' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BloodGlucoseTargetRecord

    Public Property [Time] As New TimeOnly
    Public Property High As Single = 180
    Public Property IsValid As Boolean = False
    Public Property Low As Single = 70
End Class
