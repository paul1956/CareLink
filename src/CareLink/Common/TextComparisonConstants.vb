' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TextComparisonConstants

    ''' <summary>
    '''  Centralized comparer for case-insensitive text lookups.
    ''' </summary>
    Friend ReadOnly Property Comparer As StringComparer =
        StringComparer.OrdinalIgnoreCase

    ''' <summary>
    '''  Centralized string comparison type used across the repo.
    ''' </summary>
    Friend ReadOnly Property ComparisonType As StringComparison =
        StringComparison.OrdinalIgnoreCase

    ''' <summary>
    '''  Centralized default string-split options used across the repo.
    ''' </summary>
    Friend ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

End Module
