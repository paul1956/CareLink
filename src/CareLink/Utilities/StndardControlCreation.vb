' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module StndardControlCreation
    ''' <summary>
    ''' Create a TextBox
    ''' .AutoSize = True
    ''' .Anchor Left and Right
    ''' .BorderStyle = BorderStyle.FixedSingle
    ''' .ReadOnly = True
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>New TextBox</returns>
    Friend Function CreateBasicTextBox(text As String) As TextBox
        Return New TextBox With {
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
            .AutoSize = True,
            .BorderStyle = BorderStyle.FixedSingle,
            .ReadOnly = True,
            .Text = text
        }
    End Function

    ''' <summary>
    ''' Create a Label
    ''' .AutoSize = True
    ''' .Anchor Left and Right
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>New Label</returns>
    Public Function CreateBasicLabel(text As String) As Label
        Return New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                               .AutoSize = True,
                               .Text = text}
    End Function

End Module
