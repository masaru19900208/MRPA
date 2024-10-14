<h1 align="center">
  <br>
  <a href=""><img src=".img/MRPA_Logo_v1.png" alt="" width="200"></a>
</h1>

![ScreenShot](.img/ScreenShot.png)

## What is MRPA

- Abbreviation for Mini Robotic Process Automation.
- This application allows you to automate operations at any desired timing, similar to RPA, by pre-programming keyboard commands.
- As long as this application is running, keyboard commands can be executed in any scenario.
- Note that some keyboard commands are not supported, so caution is required.

## How To Use

1. Install (**Prepared in the future.**)
1. Enter any keyboard command in the `Input macro` text box.
    - There are some keystrokes that are not supported, such as “Alt + Tag”.
1. Execute the command `Macro execution command`.

## Download

- You can download an installable version **in the future**.

## Class diagram

I created a class diagram after implementing the code and realized that I hadn’t followed the MVVM pattern. I'm quite shocked.

```mermaid
---
title: MRPA Class Diagram
---
classDiagram
note "Private items have been omitted."
  class Program
  class AppManager
  namespace class SettingsData {
    class SettingsData{
      +string LastUpdate
      +ExeCommand exeCommand
      +int AllCommandRepetition
      +List~Commands~ commands
    }
    class ExeCommand{
      +string ExeCommandStr
      +List~byte~ ExeCommandByte
    }
    class Commands{
      +string Command
      +int Delay
      +int Repetition
      +int Order
      +List~byte~ CmdByte
    }
  }
  class DataManager{
    -SettingsData _settingsData
    +string lastUpdate
    +SettingsData.ExeCommand exeCommand
    +List~SettingsData.Commands~ commands
    +int allCommandRepetition
    +GetJsonDataString()
    +BuildSettingsJsonFile()
    +LoadSettingsData()
    +UpdateJsonData()
    +UpdateThisCommands()
    +GetExistingCommandByOrder()
    +GetExistingDataByOrder()
    +GetExistingCmdByteByOrder() List~byte~
  }
  class InputAreaView{
    +ClearMacroPanel()
  }
  class InputAreaModel{
    +string baseTextBoxName
    +string baseDelayComboBoxName
    +string baseRepetitionComboBox
    +List~int~ delayTimeList
    +List~int~ repetitionCountList
    +enum inputAreaItem
  }
  class InputAreaViewModel{
    +EventHandler~ExeCommandEventArgs~ OnExeCommandOccurred
    +GetMaxOrder() int
    +OnChangeComboBox()
    +OnChangeAllCommandRepetitionComboBox()
    +OnKeyDownTextBox()
    +OnKeyUpTextBox()
    +GenerateNewLineData() SettingsData.Commands
    +GenerateExistingData() SettingsData.Commands
    +OnCloseForm()
    +InitRegisterExeCommand()
    +OnExeCommandOccur()
    +OnClickTrash()
    +OnClickPlus()
  }
  class MacroManager{
    +StartMacro() Task
  }
  class ExeCommandManager{
    +EventHandler~ExeCommandEventArgs~ OnExeCommandOccurred
    +Instance() ExeCommandManager
    +Initialize()
    +RegisterExeCommand()
    +RegisterHotKey()
    +UnregisterHotKey()
  }
  class ExeCommandEventArgs{
    +string Message
    +ExeCommandEventArgs()
  }
  class ConsoleManager{
    +Initialization() 
    +LogInfo()
    +LogError()
    +ClearConsole()
  }

  Program --|> AppManager
  AppManager o-- DataManager
  DataManager --|> SettingsData
  AppManager --|> InputAreaView
  InputAreaView --|> InputAreaModel
  InputAreaView --|> InputAreaViewModel
  InputAreaViewModel --|> MacroManager
  InputAreaViewModel --|> ExeCommandManager
  EventArgs ..|> ExeCommandEventArgs


```

## Goals of this project

- To give form to the results of my training.
- To deepen my understanding by materializing what I have learned.
- To create an application that can be completed within the remaining training period.
- To fully utilize the knowledge and skills acquired during the training.

## Not a goal of this project

- To use this application in business after the training ends.
- To acquire users for the application.
- To create an application with a focus on ease of use and excellent UX.

##
