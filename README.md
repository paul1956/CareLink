# CareLink

## Download Latest Release from
https://GitHub.com/paul1956/CareLink/releases/

# Requires for running
- .NET Core 6.0.6 or later
- Windows 10 or later

Try it out and send feedback.
This update has a UI to show all the available data and a visual version that mimics the one on iPhone.
![Same display](https://GitHub.com/paul1956/CareLink/blob/master/Screenshot%202022-10-08%20203350.png?raw=true)

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
06/01/2023

## What's New in this release
New in 3.8.0.11
- Fixed serious error in calculation for MaxBasal per hour for 780G
- Check for updates more frequently in background. Pause a day between checks if user doesn't want to update immediate.
- Add additional messages for when pump gets soft error.
- Update to support new European server data format

New in 3.8.0.7
- Add support for Irish Standard Time
- Hotfix for V6 CareLink caregiver Protocol

New in 3.8.0.6
- Improve display of DataGridView values

New in 3.8.0.5
- Limits is now available in 3 columns Native, mm/Dl and mmol/L

New in 3.8.0.4
- SG data is now available in 3 columns Native, mm/Dl and mmol/L

New in 3.8.0.3
- Add support for cgmInfo
- Clean up formatting of medicalDeviceInformation
- Add support for Obsolete values

New in 3.8.0.1
- Improve formatting and display of new CareLink server information

New in 3.8.0.0
- Update to support new CareLink server protocol
- Added additional summary records that provide pump information

New in 3.7.0.1
- Minor fix for carb ratio support of values larger then 25
- Correction to Temp Basal for mmol/L

New in 3.7.0.0
- Fixes for 780G Pump names
- Export of important tables to Excel with formatting
- Many fixes for mmol/L

New in 3.6.2.1
- Added discoverability to table Export features 

New in 3.6.2.0
- Add label to active insulin chart
- Fix times for file based displays
- Separate TimeZone in Status Bar
- Fix crash in some tables, in non-English language versions of Windows

New in 3.6.1.3
- Remove duplicate BasalRecords (only effects MM)

New 3.6.1.1
- Fixed conversion issue for languages that use comma as a decimal separator
- Form1 cleanup

New in 3.6.1.0
- It is now possible to export 4 of the tables to the Clipboard with and without headers and to Excel with all formatting intact.
    - Insulin
    - Meal
    - Sensor Glucose (SG's)
    - Auto Bolus 
- To export right-click anywhere within the 4 listed tables and choose the type of export for Excel the program will prompt for a destination to save.

** If there is interest in other tables, open an issue on GitHub.

New in 3.6.0.4
- Update Browser handling code

New in 3.6.0.3
- Prevent Crash with setting up new users

New in 3.6.0.2
- Improve overlap of Callouts, if you don't like it hovering over a blocked callout will bring it to front.

New in 3.6.0.1
- All Graphs have Legends and line colors are editable
- All files associated with application are moved under MyDocuments/CareLink, the first install will move old files to new location. You can delete anything in that directory and if it's still needed it will be recreated. If you had old error files you should probably delete them.

A new Directory MyDocuments/CareLink/Settings contains a Settings File which is initially blank it will contain information about your pump that is not available from CareLink.

- Pump AIT
- Insulin Type from drop down List, currently limited to 5 popular pump insulin type open issue it yours is missing and one listed isn't close
- A check box to allow selection of AIT decay algorithm (one uses pump value and an advanced one that is based on Insulin Type.
- For 780 it allows setting you pump Target SG for 770G its fixed at 120
- Lastly is an area where you can enter your Carb Ratio by time.

## Description

Repo for CareLink Windows App
This is a Visual Basic application that provides a UI to view Medtronic 670G and 770G and 780G.

This shows all available data and is not in any way supported by Medtronic, it was created from publicly available data.
Some data was filtered out because I could not see any use for it. You can turn off filters but performance will suffer.

For the visualization layer I use the open source
System.Windows.Forms.DataVisualization
https://GitHub.com/Kirsan31/WinForms-DataVisualization

#Known Issue
- If you get a 
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
