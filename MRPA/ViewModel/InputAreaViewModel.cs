namespace MRPA
{
    internal class InputAreaViewModel : IExeCommandEventArgs
    {
        private readonly DataManager _dataManager;
        private readonly InputAreaModel _inputAreaModel;
        private readonly ExeCommandManager _exeCommandManager;

        public InputAreaViewModel(DataManager dataManager, InputAreaModel inputAreaModel, Form mainForm)
        {
            _inputAreaModel = inputAreaModel;
            _dataManager = dataManager;
            _exeCommandManager = new ExeCommandManager(1, mainForm, this);
        }

        public int GetMaxOrder()
        {
            return _dataManager.commands!.Max(x => x.Order);
        }

        public void OnChangeComboBox(ComboBox comboBox, EventArgs e)
        {
            if (comboBox.SelectedItem is null) return;
            if (int.TryParse(comboBox.SelectedItem.ToString(), out int selectedItem))
            {
                if (comboBox.Tag is null) return;
                if (comboBox.Name.Contains("delay"))
                {
                    _dataManager.UpdateJsonData(commands: [GenerateExistingData((int)comboBox.Tag!, delay: selectedItem)]);
                }
                else
                {
                    _dataManager.UpdateJsonData(commands: [GenerateExistingData((int)comboBox.Tag!, repetition: selectedItem)]);
                }
            }
        }

        public void OnChangeAllCommandRepetitionComboBox(ComboBox comboBox, EventArgs e)
        {
            if (comboBox.SelectedItem is null) return;
            if (int.TryParse(comboBox.SelectedItem.ToString(), out int selectedAllCommandRepetition))
            {
                _dataManager.UpdateJsonData(allCommandRepetition: selectedAllCommandRepetition);
            }
        }

        private readonly List<string> keysStr = [];
        private readonly List<Byte> keysCodeByte = [];

        public void OnKeyDownTextBox(object? sender, KeyEventArgs e)
        {
            string currentKeyStr;

            if (sender is null) return;
            TextBox textBox = (TextBox)sender;

            if (e.KeyCode == Keys.ControlKey) currentKeyStr = "Ctrl";
            else if (e.KeyCode == Keys.ShiftKey) currentKeyStr = "Shift";
            else if (e.KeyCode == Keys.Menu) currentKeyStr = "Alt";
            else if (e.KeyCode == Keys.Space) currentKeyStr = "Space";
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) currentKeyStr = "Win";
            else currentKeyStr = e.KeyCode.ToString();

            if (!keysCodeByte.Contains((Byte)e.KeyCode))
            {
                keysStr.Add(currentKeyStr);
                textBox.Text = string.Join(" + ", keysStr);
                keysCodeByte.Add((Byte)e.KeyCode);
            }

            textBox.SelectionStart = textBox.Text.Length;
            e.SuppressKeyPress = true;
        }

        public void OnKeyUpTextBox(object? sender, KeyEventArgs e)
        {
            if (sender is null) return;
            TextBox textBox = (TextBox)sender;
            if (!int.TryParse(textBox.Tag?.ToString(), out int tagNumber))
            {
                ConsoleManager.LogError("The tag contains a value other than an integer.");
                return;
            }
            if (_dataManager.commands is null)
            {
                ConsoleManager.LogError("Json data is null.");
                return;
            }
            if (keysCodeByte.Count > 0)
            {
                if (!textBox.Name.Contains("exe"))
                {
                    _dataManager.UpdateJsonData(commands: [GenerateExistingData(tagNumber, cmdByte: keysCodeByte, command: textBox.Text)]);
                    if (!_dataManager.commands.Any(data => data.Order == tagNumber + 1))
                    {
                        _dataManager.UpdateJsonData(commands: [GenerateNewLineData(tagNumber + 1)]);
                    }
                }
                else
                {
                    var currentExeCommand = new SettingsData.ExeCommand
                    {
                        ExeCommandByte = keysCodeByte,
                        ExeCommandStr = textBox.Text,
                    };
                    _dataManager.UpdateJsonData(exeCommand: currentExeCommand);
                    _exeCommandManager.RegisterExeCommand(currentExeCommand);
                }
            }
            keysCodeByte.Clear();
            keysStr.Clear();
        }

        public SettingsData.Commands GenerateNewLineData(int order)
        {
            return new SettingsData.Commands
            {
                Command = "",
                Repetition = 1,
                Delay = 200,
                Order = order
            };
        }

        public SettingsData.Commands GenerateExistingData(int order, string? command = null, int? repetition = null, int? delay = null, List<Byte>? cmdByte = null)
        {
            string? pastCommand = command;
            pastCommand ??= _dataManager.GetExistingCommandByOrder(order);
            int pastDelay = delay ?? _dataManager.GetExistingDataByOrder(order, isRepetition: false);
            int pastRepetition = repetition ?? _dataManager.GetExistingDataByOrder(order, isRepetition: true);
            List<Byte> pastCmdByte = cmdByte ?? _dataManager.GetExistingCmdByteByOrder(order);
            return new SettingsData.Commands()
            {
                Command = pastCommand,
                Repetition = pastRepetition,
                Delay = pastDelay,
                CmdByte = pastCmdByte,
                Order = order
            };
        }

        public void OnCloseForm(object? sender, EventArgs e)
        {
            _exeCommandManager.UnregisterHotKey();
        }

        public void InitRegisterExeCommand(SettingsData.ExeCommand exeCommand)
        {
            _exeCommandManager.RegisterExeCommand(exeCommand);
        }

        public async void OnExeCommandOccur(object? sender, EventArgs e)
        {
            if (_dataManager.commands is null) return;
            await MacroManager.StartMacro(_dataManager.commands, _dataManager.allCommandRepetition);
        }

        public void OnClickTrash(int clickedOrder)
        {
            DeleteDataWithSequence(clickedOrder);
            _dataManager.UpdateJsonData();
        }

        public void OnClickPlus(int clickedOrder)
        {
            AddDataWithSequence(clickedOrder);
            _dataManager.UpdateJsonData(commands: [GenerateNewLineData(clickedOrder + 1)]);
        }

        private void DeleteDataWithSequence(int deleteOrder)
        {
            List<SettingsData.Commands> changeOrderCommands = [];
            // Reset the order after the clicked column.
            for (int i = deleteOrder + 1; i <= _dataManager.commands?.Count; i++)
            {
                changeOrderCommands.Add(_dataManager.commands.Where(command => command.Order == i).First());
            }
            _dataManager.commands?.Remove(_dataManager.commands.Where(command => command.Order == deleteOrder).First());
            foreach (var i in changeOrderCommands) i.Order--;
        }

        private void AddDataWithSequence(int addOrder)
        {
            List<SettingsData.Commands> changeOrderCommands = [];
            // Reset the order after the clicked column.
            for (int i = addOrder + 1; i <= _dataManager.commands?.Count; i++)
            {
                changeOrderCommands.Add(_dataManager.commands.Where(command => command.Order == i).First());
            }
            foreach (var i in changeOrderCommands) i.Order++;
        }
    }
}

