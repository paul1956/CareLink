' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class CarbRatioRecord

    <DisplayName(NameOf(StartTime))>
    <Column(Order:=0, TypeName:=NameOf(TimeOnly))>
    Public Property StartTime As TimeOnly

    <DisplayName(NameOf(EndTime))>
    <Column(Order:=1, TypeName:=NameOf(TimeOnly))>
    Public Property EndTime As TimeOnly

    <Column(Order:=2, TypeName:=NameOf(System.Single))>
    Public Property CarbRatio As Single

End Class
