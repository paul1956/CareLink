Imports System.IO
Imports System.Net
Imports System.Text
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Devices

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_NetworkAvailabilityChanged(sender As Object, e As NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged

        End Sub

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown

        End Sub

        'Private Sub MyApplication_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Me.UnhandledException
        '    Dim request As System.Net.WebRequest = System.Net.WebRequest.Create("https://api.github.com/repos/organization/repo/issues")
        '    request.Method = "POST"
        '    Dim postData As String = "{'title':'testing', 'body':'testing testing'}"
        '    Dim byteArray() As Byte = Encoding.UTF8.GetBytes(postData)
        '    request.ContentLength = byteArray.Length
        '    Dim dataStream As System.IO.Stream = request.GetRequestStream()
        '    dataStream.Write(byteArray, 0, byteArray.Length)
        '    dataStream.Close()
        '    Dim response As System.Net.WebResponse = request.GetResponse()
        '    Console.WriteLine(response.Headers.Get("url"))
        'End Sub
    End Class
End Namespace
