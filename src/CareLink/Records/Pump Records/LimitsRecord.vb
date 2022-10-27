' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LimitsRecord

    <DisplayName("High Limit")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property highLimit As Single

    <DisplayName(NameOf(index))>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Low Limit")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public Property lowLimit As Single

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Version")>
    <Column(Order:=5, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

End Class
