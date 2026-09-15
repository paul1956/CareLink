' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

Public Module AsyncExtensions

    ''' <summary>
    '''  Helper extension that calls ConfigureAwait(False) on a non-generic Task.
    '''  Usage: Await someTask.ConfigureAwaitFalse()
    ''' </summary>
    <Extension>
    Public Function ConfigureAwaitFalse(task As Task) As ConfiguredTaskAwaitable
        Return task.ConfigureAwait(continueOnCapturedContext:=False)
    End Function

    ''' <summary>
    '''  Helper extension that calls ConfigureAwait(False) on a Task(Of T).
    '''  Usage: Dim result = Await someTaskOfT.ConfigureAwaitFalse()
    ''' </summary>
    <Extension>
    Public Function ConfigureAwaitFalse(Of T)(task As Task(Of T)) As ConfiguredTaskAwaitable(Of T)
        Return task.ConfigureAwait(continueOnCapturedContext:=False)
    End Function

End Module
