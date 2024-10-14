
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
        public int allCommandRepetition;

        public DataManager(string filePath)
        {
            _jsonFilePath = filePath;
            _settingsData = new SettingsData();
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
                this.allCommandRepetition = settingsData.AllCommandRepetition;
            }
        }

        public bool UpdateJsonData(SettingsData.ExeCommand? exeCommand = null, List<SettingsData.Commands>? commands = null, int? allCommandRepetition = null)
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

                if (allCommandRepetition is not null)
                {
                    this.allCommandRepetition = (int)allCommandRepetition;
                }

                string jsonString = JsonSerializer.Serialize(new { this.lastUpdate, this.exeCommand, this.allCommandRepetition, this.commands });
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

        /// <summary>
        /// Get the repetition or delay of the order number data.
        /// </summary>
        /// <param name="order">Integer for order number</param>
        /// <param name="isRepetition">True for repetition, false for delay</param>
        /// <returns></returns>
        public int GetExistingDataByOrder(int order, bool isRepetition)
        {
            if (this.commands is null) return 1;
            int currentData = 1;
            if (isRepetition)
            {
                foreach (var command in this.commands)
                {
                    if (command.Order == order) currentData = command.Repetition;
                }
            }
            else
            {
                foreach (var command in this.commands)
                {
                    if (command.Order == order) currentData = command.Delay;
                }
            }
            return currentData;
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
