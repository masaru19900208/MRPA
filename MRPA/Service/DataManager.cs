
using System.Text.Json;

namespace MRPA
{
    internal class DataManager
    {
        private readonly string _jsonFilePath = "";
        private readonly SettingsData _settingsData;
        public string lastUpdate = "";
        public SettingsData.ExeCommand exeCommand;
        public List<SettingsData.Commands>? commands;

        public DataManager(string filePath, SettingsData settingsData)
        {
            _jsonFilePath = filePath;
            _settingsData = settingsData;
            exeCommand = new SettingsData.ExeCommand
            {
                ExeCommandStr = "",
                ExeCommandByte = [],
            };
        }

        public string GetJsonDataString()
        {
            return File.ReadAllText(_jsonFilePath);
        }

        public void BuildSettingsJsonFile()
        {
            string jsonString = JsonSerializer.Serialize(_settingsData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, jsonString);
        }

        public void LoadSettingsData()
        {
            SettingsData? settingsData = JsonSerializer.Deserialize<SettingsData>(GetJsonDataString());
            if (settingsData is not null && settingsData.exeCommand is not null)
            {
                this.lastUpdate = settingsData.LastUpdate;
                this.exeCommand.ExeCommandStr = settingsData.exeCommand.ExeCommandStr;
                this.exeCommand.ExeCommandByte = settingsData.exeCommand.ExeCommandByte;
                this.commands = settingsData.commands;
            }
        }

        public bool UpdateJsonData(SettingsData.ExeCommand? exeCommand = null, List<SettingsData.Commands>? commands = null)
        {
            try
            {
                this.lastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                if (exeCommand is not null && exeCommand.ExeCommandStr is not null && exeCommand.ExeCommandByte is not null)
                {
                    this.exeCommand.ExeCommandStr = exeCommand.ExeCommandStr;
                    this.exeCommand.ExeCommandByte = exeCommand.ExeCommandByte;
                }

                if (commands is not null)
                {
                    UpdateThisCommands(commands);
                }
                string jsonString = JsonSerializer.Serialize(new { this.lastUpdate, this.exeCommand, this.commands });
                File.WriteAllText(_jsonFilePath, jsonString);
                LoadSettingsData();
                return true;
            }
            catch (Exception ex)
            {
                ConsoleManager.LogError(ex.ToString());
                return false;
            }
        }

        private void UpdateThisCommands(List<SettingsData.Commands> argCommands)
        {
            try
            {
                bool isNewData = true;
                foreach (var command in argCommands)
                {
                    foreach (var currentCommand in this.commands!)
                    {
                        if (currentCommand.Order == command.Order)
                        {
                            currentCommand.Command = command.Command;
                            currentCommand.Repetition = command.Repetition;
                            currentCommand.Delay = command.Delay;
                            currentCommand.CmdByte = command.CmdByte;
                            isNewData = false;
                        }
                    }
                    if (isNewData) this.commands.Add(command);
                }
                return;
            }
            catch (Exception ex)
            {
                ConsoleManager.LogError(ex.ToString());
                return;
            }
        }

        public string GetExistingCommandByOrder(int order)
        {
            if (this.commands is null) return "";
            string currentCommand = "";
            foreach (var command in this.commands)
            {
                if (command.Order == order) currentCommand = command.Command;
            }
            return currentCommand;
        }

        // TODO もうちょい共通化するリファクタリング
        public int GetExistingRepetitionByOrder(int order)
        {
            if (this.commands is null) return 1;
            int currentRepetition = 1;
            foreach (var command in this.commands)
            {
                if (command.Order == order) currentRepetition = command.Repetition;
            }
            return currentRepetition;
        }

        // TODO もうちょい共通化するリファクタリング
        public int GetExistingDelayByOrder(int order)
        {
            if (this.commands is null) return 1;
            int currentDelay = 1;
            foreach (var command in this.commands)
            {
                if (command.Order == order) currentDelay = command.Delay;
            }
            return currentDelay;
        }

        public List<Byte> GetExistingCmdByteByOrder(int order)
        {
            if (this.commands is null) return [];
            List<Byte> currentCmdByte = [];
            foreach (var command in this.commands)
            {
                if (command.Order == order) currentCmdByte = command.CmdByte;
            }
            return currentCmdByte;
        }
    }
}
