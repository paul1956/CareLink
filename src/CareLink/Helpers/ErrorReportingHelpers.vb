' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ErrorReportingHelpers

    Public Function GetTitleFromStack(stackFrame As StackFrame) As String
        Return $"{stackFrame.GetFileName.Split("\").Last} line:{stackFrame.GetFileLineNumber()}"
    End Function

End Module
