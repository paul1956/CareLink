' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BasalRecord

    <DisplayName("Active Basal Pattern")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property activeBasalPattern As String

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    Public Property basalRate As Single

End Class
