Imports System.IO
Imports System.Text.Json

Public Module [Module]
    Public Function writeJson(jsonobj As JsonElement, name As String) As Boolean
        Dim now As Date = DateTime.Now()
        Dim filename As String = $"{name}-{now.Year}{now.Month}{now.Date}_{now.Hour}{now.Minute}{now.Second}.json"
        Try
            File.WriteAllText(filename, jsonobj.ToStringIndented)
        Catch e As Exception
            Console.WriteLine($"Error saving {filename}: {e}")
            Return False
        End Try
        Return true
    End Function
    Public parser As Object = argparse.ArgumentParser()

    Sub New()
        parser.add_argument("--username", "-u", type:=Str, help:="CareLink username", required:=True)
        parser.add_argument("--password", "-p", type:=Str, help:="CareLink password", required:=True)
        parser.add_argument("--country", "-c", type:=Str, help:="CareLink two letter country code", required:=True)
        parser.add_argument("--repeat", "-r", type:=Int, help:="Repeat request times", required:=False)
        parser.add_argument("--wait", "-w", type:=Int, help:="Wait minutes between repeated calls", required:=False)
        parser.add_argument("--data", "-d", help:="Save recent data", action:="store_true")
        parser.add_argument("--verbose", "-v", help:="Verbose mode", action:="store_true")
        Threading.Thread.sleep(1)
        Threading.Thread.sleep(1)
        Threading.Thread.sleep(wait * 60)
    End Sub
    Public args As String() = parser.parse_args()
    Public username As Object = args.username
    Public password As Object = args.password
    Public country As Object = args.country
    Public repeat As Object = If(args.repeat Is Nothing, 1, args.repeat)
    Public wait As Object = If(args.wait Is Nothing, 5, args.wait)
    Public data As Object = args.data
    Public verbose As Object = args.verbose
    Public client As Object = carelink_client.CareLinkClient(username, password, country)
    Public recentData As Object = client.getRecentData()
End Module
