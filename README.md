# CareLink

## Download Latest Release
https://github.com/paul1956/CareLink/releases/download/3.0.0.0/CareLinkx64V3.0.0.0.zip



## Requires
- Visual Studio 2022
- .NET Core 6.0

## License
- MIT

## Technologies
  - Windows Forms
  - dotnet-core

## Topics
- Medtronic CareLink data display

## Updated
- 6/29/2022

## Description

Repo for CareLink Windows App
This is a Visual Basic application that provides a UI to view Medtronic 670G and 770G (possibly 780G but that has not been tested).
This shows all available data and is not in any way supported by Medtronic, it was created from publicly available data.
Some data was filtered out because I could not see any use for it. You can turn off filters but performance will suffer.

For the visualization layer I use the open source NuGet package.
Maikebing.System.Windows.Forms.DataVisualization
https://www.nuget.org/packages/Maikebing.System.Windows.Forms.DataVisualization/5.0.1?_src=template

Try it out and send feedback.
This update has a UI to show all the available data and a visual version that mimics the one on iPhone.
![Same display](https://github.com/paul1956/CareLink/blob/master/Screenshot%202021-05-16%20050718.png?raw=true)

#Known Issue
If you get a "System.Configuration.ConfigurationErrorsException: 'Configuration system failed to initialize'"
You will need to edit CareLink\src\CareLink\bin\Debug\net6.0-windows\CareLink.dll.config and remove the following lines
```
<system.diagnostics>
    <sources>
        <!-- This section defines the logging configuration for My.Application.Log -->
        <source name="DefaultSource" switchName="DefaultSwitch">
            <listeners>
                <add name="FileLog"/>
                <!-- Uncomment the below section to write to the Application Event Log -->
                <!--<add name="EventLog"/>-->
            </listeners>
        </source>
    </sources>
    <switches>
        <add name="DefaultSwitch" value="Information" />
    </switches>
    <sharedListeners>
        <add name="FileLog"
                type="Microsoft.VisualBasic.Logging.FileLogTraceListener, Microsoft.VisualBasic, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, processorArchitecture=MSIL"
                initializeData="FileLogWriter"/>
        <!-- Uncomment the below section and replace APPLICATION_NAME with the name of your application to write to the Application Event Log -->
        <!--<add name="EventLog" type="System.Diagnostics.EventLogTraceListener" initializeData="APPLICATION_NAME"/> -->
    </sharedListeners>
</system.diagnostics>
```
