' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization

Friend Module CultureHelpers
    Private s_currentDateCulture As CultureInfo

    Public Property CurrentDateCulture As CultureInfo
        Get
            If s_currentDateCulture Is Nothing Then
                s_currentDateCulture = CultureInfo.CurrentUICulture
            End If
            Return s_currentDateCulture
        End Get
        Set
            s_currentDateCulture = Value
        End Set
    End Property

    Public ReadOnly Property CultureInfoList As List(Of CultureInfo) = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList
    Public Property Provider As CultureInfo = CultureInfo.CurrentUICulture
    Public ReadOnly Property usDataCulture As New CultureInfo("en-US")
End Module
