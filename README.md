# CareLink™ For Windows

## Download Latest Release from
https://GitHub.com/paul1956/CareLink/releases/

# Description

This application is designed to show all the available pump data in a visual version that mimics the one on iPhone and in tables that can be exported to Microsoft Excel or the Clipboard.

![Same display](https://GitHub.com/paul1956/CareLink/blob/master/Screenshot%202022-10-08%20203350.png?raw=true)

Some data is filtered out by default because I could not see any use for it. You can turn off filters from Options Menu. but performance will suffer.

# Disclaimer And Warning
This project is intended for educational and informational purposes only.
It relies on a series of fragile components and assumptions, any of which may break at any time.
It is not FDA approved and should not be used to make medical decisions.
It is neither affiliated with nor endorsed by Medtronic, and may violate their Terms of Service.
Use of this code is without warranty or formal support of any kind.
Try it out and send feedback.

# Required Settings

A directory MyDocuments/CareLink/Settings contains a Settings File which is initially blank it will contain information about your pump that is not available from CareLink™.

- Pump AIT
- Insulin Type from drop down List, currently limited to 5 popular pump insulin type open issue it yours is missing and one listed isn't close
- A check box to allow selection of AIT decay algorithm (one uses pump value for 770G only and an advanced one that is based on Insulin Type).
- For MiniMed™ 780G it allows setting you pump Target SG for MiniMed™ 770G its fixed at 120
- Lastly is an area where you can enter your Carb Ratio by time.

## Updated
03/30/2025

## What's New
 New in 5.0.0.8
 - Fix Status Bar text color' this is a temporary fix until I can find a better way to handle
   Dark Mode with controls that don't support it like StatusStrip 

 -  New in 5.0.0.7
 - Parse Json data using CurrentCulture
 - Fix processing of export data
 - Add new fault codes
 - Fix TIR Calculation

 New in 5.0.0.5
 - Fixes handling of Manual Mode Suspend

 New in 5.0.0.3
 - Fixes plotting of TIR and other values

 New in 5.0.0.2
 - Supports new V11 Medtronic API

 New in 4.0.2.0
  - Limited dark mode support

# Requires for running
- .NET Core 9.0 or later
- Windows 10 or later

# Requires for development
- Visual Studio 2022 Version 17.14.0 Preview 2.0
- .NET Core 9.0.0 or later
- Windows 10 or later

# License
- MIT

# Technologies
  - Windows Forms
  - dotnet-core

# Topics
- Medtronic CareLink™ data display

## Known Issue for developers only

For the visualization layer I use the open source System.Windows.Forms.DataVisualization library
https://GitHub.com/Kirsan31/WinForms-DataVisualization


- If you get the error below

> "System.Configuration.ConfigurationErrorsException: 'Configuration system failed to initialize'"
> will need to edit CareLink\src\CareLink\bin\Debug\net7.0-windows\CareLink.dll.config and remove the following lines
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
