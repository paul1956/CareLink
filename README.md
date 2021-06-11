# CareLink
## Requires
- Visual Studio 2019 Version 16.9
- .NET Core 5.0

## License
- MIT

## Technologies
  - Windows Forms
  - dotnet-core

## Topics
- Medtronic CareLink data display

## Updated
- 5/21/2021

## Description

Repo for CareLink Windows App
This is a Visual Basic application that provides a UI to view Medtronic 670G and 770G (possibly 780G but that has not been tested).
This shows all available data and is not in any way supported by Medtronic, it was created from publicly available data.
Some data was filtered out because I could not see any use for it. You can turn off filters but performance will suffer.

For the visualization layer I use the open source package by Angelo Cresta.
https://github.com/AngeloCresta/winforms-datavisualization-net5. Clone this repository also as a peer to this CareLink Repository, then "dotnet run"

Try it out and send feedback.
This update has a simple UI to show all the available data and a visual version that mimics the one on iPhone.
![Same display](https://github.com/paul1956/CareLink/blob/master/Screenshot%202021-05-16%20050718.png?raw=true)
