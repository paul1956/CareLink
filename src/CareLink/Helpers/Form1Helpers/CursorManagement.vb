' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Threading

Friend Module CursorManagement

    Private ReadOnly s_states As New ConditionalWeakTable(Of Form, CursorState)()

    ''' <summary>
    '''  Indicates that a data binding operation has started,
    '''  incrementing the pending bindings count and
    '''  setting the cursor to WaitCursor if it is not already set.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor.</param>
    ''' <param name="pumpMessages">
    '''  Indicates whether to pump messages during the operation.
    ''' </param>
    Private Sub BindingStarted(owner As Form, Optional pumpMessages As Boolean = False)
        Dim state As CursorState = GetState(owner)
        Dim newPending As Integer = Interlocked.Increment(location:=state.pendingBindings)
        If owner.Cursor <> Cursors.WaitCursor Then
            Try
                SetOwnerCursor(owner, newCursor:=Cursors.WaitCursor, pumpMessages)
            Catch
                ' Ignore errors setting cursor
                Stop
            End Try
        End If
    End Sub

    ''' <summary>
    '''   Retrieves the CursorState associated with the specified form owner, creating a new instance if one does not already exist.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor state.</param>
    ''' <returns>The CursorState associated with the specified form.</returns>
    Private Function GetState(owner As Form) As CursorState
        ArgumentNullException.ThrowIfNull(argument:=owner)
        Dim createValueCallback As ConditionalWeakTable(Of Form, CursorState).CreateValueCallback =
            Function(f As Form) As CursorState
                Return New CursorState()
            End Function
        Return s_states.GetValue(key:=owner, createValueCallback)
    End Function

    ''' <summary>
    '''  Indicates that a data binding operation has finished,
    '''  decrementing the pending bindings count and
    '''  resetting the cursor to Default if it reaches zero.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor state.</param>
    ''' <param name="pumpMessages">
    '''  Indicates whether to pump messages during the operation.
    ''' </param>
    <Extension>
    Friend Sub BindingFinished(owner As Form,
                               Optional pumpMessages As Boolean = False)
        Dim state As CursorState = GetState(owner)
        Dim newPending As Integer = Interlocked.Decrement(location:=state.pendingBindings)
        If newPending <= 0 Then
            state.pendingBindings = 0
            Try
                SetOwnerCursor(owner, newCursor:=Cursors.Default, pumpMessages)
            Catch
                ' Ignore errors setting cursor
                Stop
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Indicates that a data binding operation has finished for the specified DataGridView,
    '''  decrementing the reference count for that DataGridView and resetting the cursor to Default
    '''  if all bindings have completed.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor state.</param>
    ''' <param name="dgv">The DataGridView that has finished binding.</param>
    <Extension>
    Friend Sub BindingFinished(owner As Form, dgv As DataGridView)
        ArgumentNullException.ThrowIfNull(argument:=dgv)
        Dim state As CursorState = GetState(owner)
        Dim key As Integer = RuntimeHelpers.GetHashCode(dgv)
        SyncLock state.dgvBindingRefs
            Dim count As Integer = 0
            If state.dgvBindingRefs.TryGetValue(key, count) Then
                If count > 1 Then
                    state.dgvBindingRefs(key) = count - 1
                Else
                    state.dgvBindingRefs.Remove(key)
                    BindingFinished(owner, pumpMessages:=False)
                End If
            End If
        End SyncLock
    End Sub

    ''' <summary>
    '''  Indicates that a data binding operation has started for the specified DataGridView, incrementing the pending bindings count and setting the cursor to WaitCursor if it is not already set.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor.</param>
    ''' <param name="dgv">The DataGridView that is being bound.</param>
    <Extension>
    Friend Sub BindingStarted(owner As Form, dgv As DataGridView)
        ArgumentNullException.ThrowIfNull(argument:=dgv)
        Dim state As CursorState = GetState(owner)
        Dim key As Integer = RuntimeHelpers.GetHashCode(dgv)
        SyncLock state.dgvBindingRefs
            Dim count As Integer = 0
            If state.dgvBindingRefs.TryGetValue(key, count) Then
                state.dgvBindingRefs(key) = count + 1
            Else
                state.dgvBindingRefs(key) = 1
                BindingStarted(owner, pumpMessages:=False)
            End If
        End SyncLock
    End Sub

    ''' <summary>
    '''  Indicates that a cursor operation has finished,
    '''  decrementing the pending cursor operations count
    '''  and resetting the cursor to Default if it reaches zero.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor.</param>
    ''' <param name="pumpMessages">
    '''  Whether to pump messages during the operation.
    ''' </param>
    <Extension>
    Friend Sub CursorFinished(owner As Form, Optional pumpMessages As Boolean = False)
        Dim state As CursorState = GetState(owner)
        Dim newPending As Integer = Interlocked.Decrement(location:=state.cursorOperations)
        If newPending <= 0 Then
            state.cursorOperations = 0
            Try
                SetOwnerCursor(owner, newCursor:=Cursors.Default, pumpMessages)
            Catch
                ' Ignore errors setting cursor
                Stop
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Indicates that a cursor operation has started, incrementing the pending cursor operations count and setting the cursor to WaitCursor if it is not already set.
    ''' </summary>
    ''' <param name="owner">The form that owns the cursor.</param>
    <Extension>
    Friend Sub CursorStarted(owner As Form, Optional pumpMessages As Boolean = False)
        Dim state As CursorState = GetState(owner)
        Dim newPending As Integer = Interlocked.Increment(location:=state.cursorOperations)
        If owner.Cursor <> Cursors.WaitCursor Then
            Try
                SetOwnerCursor(owner, newCursor:=Cursors.WaitCursor, pumpMessages)
            Catch
                ' Ignore errors setting cursor
                Stop
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Diagnostic: returns the number of pending (logical) bindings for the given form.
    '''  Use this at runtime to verify whether all binding-related work has completed.
    ''' </summary>
    ''' <param name="owner">
    '''  The form whose pending bindings count is to be retrieved.
    ''' </param>
    Friend Function GetPendingBindingsCount(owner As Form) As Integer
        Dim state As CursorState = GetState(owner)
        Return state.pendingBindings
    End Function

    ''' <summary>
    '''  Sets the cursor for the specified form owner to the given new cursor,
    '''  optionally pumping messages during the operation.
    '''  If called from a non-UI thread, it will invoke the operation on the UI thread.
    ''' </summary>
    ''' <param name="owner">The form whose cursor is to be set.</param>
    ''' <param name="newCursor">The new cursor to apply to the form.</param>
    ''' <param name="pumpMessages">If true, messages will be pumped during the operation.</param>
    <Extension>
    Friend Sub SetOwnerCursor(owner As Form, newCursor As Cursor, Optional pumpMessages As Boolean = False)
        Dim state As CursorState = GetState(owner)
        Dim tid As Integer = Environment.CurrentManagedThreadId
        If owner.InvokeRequired Then
            Dim method As New Action(
                Sub()
                    Try
                        owner.Cursor = newCursor
                    Catch
                        ' Ignore errors setting cursor
                        Stop
                    End Try
                End Sub)
            owner.BeginInvoke(method)
            If pumpMessages Then
                Try
                    Application.DoEvents()
                Catch
                    ' Ignore
                End Try
            End If
        Else
            Try
                owner.Cursor = newCursor
            Catch
                ' Ignore errors setting cursor
                Stop
            End Try
            If pumpMessages Then
                Try
                    Application.DoEvents()
                Catch
                    ' Ignore
                End Try
            End If
        End If
    End Sub

End Module
