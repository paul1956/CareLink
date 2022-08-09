# CareLink

## Download Latest Release from
https://github.com/paul1956/CareLink/releases/
# Requires for running
- .NET Core 6.0.6 or later
- Windows 10 or later

## Requires for development
- Visual Studio 2022 Version 17.3.0 Preview 2.0 or later
- .NET Core 6.0.6 or later
- Windows 10 or later

## License
- MIT

## Technologies
  - Windows Forms
  - dotnet-core

## Topics
- Medtronic CareLink data display

## Updated
- 8/08/2022

## What's New in this release
  New in 3.5.0.4
  Improve date handling
  Add current update time to UI

  New in 3.5.0.3
  Fix crash when pump is disconnected from phone

  New in 3.5.0.2
  Update pump battery images and text

  New in 3.5.0.1
  Add back pump model number
  Update version

  New in 3.5.0.0
  Complete rewrite of time handling
  Improved support for mmol/L with all values scaled
  Added better TimeZone support

  Another contribution from Chris Deprez https://github.com/Krikke99
  - shows SG on System Tray with dynamic background color:
    - Green normal
    - Orange low
    - Red high
  
![image](https://user-images.githubusercontent.com/4416348/182358729-dfcf680b-4127-4505-9e59-be7250b25383.png)

- on hover info:
    - Time last update
    - Last SG
    - SG Trend
    - Active insulin
    
![image](https://user-images.githubusercontent.com/4416348/182360137-ff7b0075-eb88-4a6b-bb92-18d4d4117e16.png)

- Notification when SG is out of range.

![image](https://user-images.githubusercontent.com/4416348/182361568-2a3d91c2-94a7-4275-911c-187150f5319d.png)

  Complete rewrite of date handling
  
  New in 3.4.2.7
  Fix from Krikke99 to allow Mini Display to be not topmost
  
  New in 3.4.2.6
  Use NameOf in Charts to prevent errors on renaming
  Improve Error Report naming to prevent overwriting reports.

  New in 3.4.2.5
  Support loading of error reports, displaying the exceptions and rerunning them

  New in 3.4.2.4
  Fix removal of 1 culture every time ParseDate is called

  New in 3.4.2.3
  Handle multiple pump formats for mmol/L and more languages.

  New in 3.4.2.2
  Remove all CDate and replace with DateParse

  New in 3.4.2.1
  Add support for creating human readable Error File for reporting issue on GitHub
  Modify DateParse to try many cultures to convert dates.

  New in 3.4.0.7
  Add additional messages
  Improve app on scaled displays
  Improve crash resilience

  New in 3.4.0.6
  Improve crash reporting
  Better support for new pumps and manual mode
  Remove Watchdog timer

  New in 3.4.0.5
  More scaling and internationalization improvements

  New in 3.4.0.3
  Fixes to date conversion for better international support
  Added new method to do AIT decay, you can select new method under options

  New in 3.2.0.0
  First public release supporting 770G and 780 in North America and Europe
  Scale displays have limited support the app works best at 100%
  Issue reporting and check for new release have been added to Help menu.

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
