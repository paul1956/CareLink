' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Represents the state of the cursor and related operations
'''  in the application.
''' </summary>
Friend Class CursorState
    Public cursorOperations As Integer
    Public dgvBindingRefs As New Dictionary(Of Integer, Integer)()
    Public pendingBindings As Integer
    ' Timer used to clear a stuck WaitCursor after a safety timeout.
    ' Stored on the CursorState so it is tied to the lifetime of the owning Form via the ConditionalWeakTable.
    Public waitTimer As System.Threading.Timer
    ' Lock object to synchronize access to the waitTimer
    Public waitTimerLock As New Object()
End Class
