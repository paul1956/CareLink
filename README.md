# CareLink™ For Windows

## Download Latest Release from
https://GitHub.com/paul1956/CareLink/releases/

# Description

This application is designed to show all the available pump data in a visual version that mimics the one on iPhone and in tables that can be exported to Microsoft Excel or the Clipboard.

![Same display](https://GitHub.com/paul1956/CareLink/blob/master/Screenshot%202022-10-08%20203350.png?raw=true)

This application is not in any way supported by Medtronic, it was created from publicly available data.
Some data is filtered out by default because I could not see any use for it. You can turn off filters from Options Menu. but performance will suffer.

Try it out and send feedback.

# Required Settings

A directory MyDocuments/CareLink/Settings contains a Settings File which is initially blank it will contain information about your pump that is not available from CareLink™.

- Pump AIT
- Insulin Type from drop down List, currently limited to 5 popular pump insulin type open issue it yours is missing and one listed isn't close
- A check box to allow selection of AIT decay algorithm (one uses pump value for 770G only and an advanced one that is based on Insulin Type).
- For MiniMed™ 780G it allows setting you pump Target SG for MiniMed™ 770G its fixed at 120
- Lastly is an area where you can enter your Carb Ratio by time.

## Updated
07/15/2023

## What's New
New in 3.8.3.2
- Voice recognition and speech
- You can now ask "What is my BG" and other questions using your voice
- Tab navigation is voice enabled say "Show [tab name]" for example "Show Treatment Details"
- Improve label and calculations for TIR Compliance measurement
- Improved fonts and images
- Fix announcement for out of range blood glucose in mmol/L
- Add support for US 780G new features

New in 3.8.3.1
- Improve display of Sg values in mmol/M
- Make Tray Icon larger
- Add Icon to Update Message in StatusBar
- Add additional messages
- Correct spelling errors in variable names
- Align Sensor messages
- Redo Standard Deviations Handling

New in 3.8.3.0
- Correctly handle CareLink Authorization Server redirect
- Correct application version display
- Update help messages for copy of tables in DataGridViews
- Handle CareLink Partners without Patient ID
- Fix highlighting in StatusBar TimeZone field
- Fix TimeZone display in status bar to provide additional information and handle Daylight Savings Time
- Temp Target display for mmol/L
- Fix parsing of command line parameters /Safe will clear Auto Login
- Update formatting on home page for easier viewing
- Improve Sensor error message display when missing and add missing messages

# Requires for running
- .NET Core 7.0.7 or later
- Windows 10 or later

# Requires for development
- Visual Studio 2022 Version 17.6.4 later
- .NET Core 7.0.7 or later
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
