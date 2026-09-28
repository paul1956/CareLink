' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Namespace Json

    Public Class AdditionalInfo

        <DisplayName("Alert Clear Type")>
        <Column(Order:=0, TypeName:=NameOf([String]))>
        <JsonPropertyName("alert_Clear_Type")>
        Public Property AlertClearType As String

        <DisplayName("Resource Bundle Key")>
        <Column(Order:=1, TypeName:=NameOf([String]))>
        <JsonPropertyName("resourceBundleKey")>
        Public Property ResourceBundleKey As String

    End Class
End Namespace
