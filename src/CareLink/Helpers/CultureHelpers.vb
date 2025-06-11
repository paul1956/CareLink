' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization

''' <summary>
'''  Provides helper properties and methods for working with culture and localization settings.
''' </summary>
Friend Module CultureHelpers
    ''' <summary>
    '''  Backing field for the <see cref="CurrentDateCulture"/> property.
    ''' </summary>
    Private s_currentDateCulture As CultureInfo

    ''' <summary>
    '''  Gets or sets the current culture used for date formatting.
    '''  Defaults to <see cref="CultureInfo.CurrentUICulture"/> if not set.
    ''' </summary>
    ''' <remarks>
    '''  This property allows overriding the default date culture for formatting and parsing operations.
    ''' </remarks>
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

    ''' <summary>
    '''  Gets a list of all available cultures on the system.
    ''' </summary>
    Public ReadOnly Property CultureInfoList As List(Of CultureInfo) =
        CultureInfo.GetCultures(CultureTypes.AllCultures).ToList

    ''' <summary>
    '''  Gets or sets the culture provider used for formatting and parsing operations.
    '''  Defaults to <see cref="CultureInfo.CurrentUICulture"/>.
    ''' </summary>
    Public Property Provider As CultureInfo = CultureInfo.CurrentUICulture

    ''' <summary>
    '''  Gets the US English culture ("en-US").
    ''' </summary>
    Public ReadOnly Property usDataCulture As New CultureInfo("en-US")
End Module
