namespace MRPA
{
    internal class InputAreaView
    {
        private readonly DataManager _dataManager;
        private readonly MainForm _mainForm;
        private readonly FlowLayoutPanel _macroPanel;
        private readonly InputAreaViewModel _viewModel;
        private readonly InputAreaModel _model;
        private readonly Color backColor = Color.FromArgb(36, 36, 36);
        private readonly Font baseFont = new Font("Microsoft JhengHei", 10F);
        private readonly Font labelFont = new Font("Microsoft JhengHei", 12F);
        private readonly Color foreColor = Color.AliceBlue;
        private readonly BorderStyle borderStyle = BorderStyle.FixedSingle;

        public InputAreaView(DataManager dataManager, MainForm mainForm, FlowLayoutPanel macroPanel)
        {
            _dataManager = dataManager;
            _mainForm = mainForm;
            _macroPanel = macroPanel;
            _model = new InputAreaModel();
            _dataManager.LoadSettingsData();
            _viewModel = new InputAreaViewModel(_dataManager, _mainForm);
            try
            {
                SyncDataView();
                _mainForm.Controls.Add(GenerateExecutionTextBox());
                _mainForm.Controls.Add(GenerateAllCommandRepetitionComboBox(_dataManager.allCommandRepetition));
            }
            catch (Exception ex)
            {
                ConsoleManager.LogError(ex.ToString());
            }
            _mainForm.FormClosing += OnCloseForm;
        }

        private void SyncDataView()
        {
            if (_dataManager.commands!.Count <= 1)
            {
                AddNewLine(1);
            }
            else
            {
                for (var i = 1; i <= _dataManager.commands!.Count; i++)
                {
                    AddNewLine(i, _dataManager.commands.Where(cmd => cmd.Order == i).First());
                }
            }
        }

        private void AddNewLine(int order, SettingsData.Commands? command = null)
        {
            command ??= _viewModel.GenerateNewLineData(order);
            Panel panel = new Panel
            {
                TabStop = false,
                Location = new Point(3, 3),
                Name = "panelCommand" + order,
                Size = new Size(500, 100),
                Tag = order
            };
            panel.Controls.Add(GenerateTextBox(order, command.Command));
            panel.Controls.Add(GenerateComboBox(InputAreaModel.inputAreaItem.delay, order, command.Delay));
            panel.Controls.Add(GenerateComboBox(InputAreaModel.inputAreaItem.repetition, order, command.Repetition));
            panel.Controls.Add(GenerateMacroCommandLabel(order));
            panel.Controls.Add(GenerateRepeatLabel(order));
            panel.Controls.Add(GenerateDelayLabel(order));
            panel.Controls.Add(GenerateTrashPictureBox(order));
            panel.Controls.Add(GeneratePlusPictureBox(order));
            _macroPanel.Controls.Add(panel);
        }

        public void ClearMacroPanel()
        {
            _macroPanel.Controls.Clear();
        }

        #region Label

        private Label GenerateMacroCommandLabel(int order)
        {
            Label macroCommandLabel = new Label
            {
                Font = labelFont,
                ForeColor = foreColor,
                Location = new Point(10, 3),
                Size = new Size(200, 25),
                TabStop = false,
                Text = "Macro Command: " + order,
                Name = "labelMacroCommand" + order
            };
            return macroCommandLabel;
        }

        private Label GenerateRepeatLabel(int order)
        {
            Label repeatLabel = new Label
            {
                Font = labelFont,
                ForeColor = foreColor,
                Location = new Point(400, 3),
                Size = new Size(78, 25),
                TabStop = false,
                Text = "Repeat",
                Name = "labelRepeat" + order
            };

            return repeatLabel;
        }


        private Label GenerateDelayLabel(int order)
        {
            Label delayLabel = new Label
            {
                Font = labelFont,
                ForeColor = foreColor,
                Location = new Point(166, 65),
                Name = "delayDelay" + order,
                Size = new Size(64, 25),
                Text = "Delay",
                TabStop = false
            };
            return delayLabel;
        }

        #endregion

        private PictureBox GenerateTrashPictureBox(int order)
        {
            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(25, 25),
                BackgroundImage = Properties.Resources.trash,
                Location = new Point(440, 70),
                Name = "trashButton" + order,
                TabStop = false,
                Tag = order
            };
            pictureBox.Click += OnClickTrash;
            return pictureBox;
        }

        private PictureBox GeneratePlusPictureBox(int order)
        {
            PictureBox pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(25, 25),
                BackgroundImage = Properties.Resources.plus,
                Location = new Point(400, 70),
                Name = "trashButton" + order,
                TabStop = false,
                Tag = order
            };
            pictureBox.Click += OnClickPlus;
            return pictureBox;
        }


        private TextBox GenerateTextBox(int order, string command)
        {
            TextBox textBox = new TextBox
            {
                BackColor = backColor,
                Font = baseFont,
                ForeColor = foreColor,
                BorderStyle = borderStyle,
                Location = new Point(10, 30),
                Size = new Size(380, 30),
                TabStop = false,
                Tag = order,
                Text = command
            };
            textBox.TextChanged += OnChangeTextBox;
            textBox.KeyDown += _viewModel.OnKeyDownTextBox;
            textBox.KeyUp += _viewModel.OnKeyUpTextBox;
            textBox.Name = _model.baseTextBoxName + order;
            return textBox;
        }

        private ComboBox GenerateComboBox(InputAreaModel.inputAreaItem inputAreaItem, int order, int initValue)
        {
            ComboBox comboBox = new ComboBox
            {
                BackColor = backColor,
                Font = baseFont,
                ForeColor = foreColor,
                FormattingEnabled = true,
                Size = new Size(70, 30),
                TabStop = false,
                Tag = order,
            };

            if (inputAreaItem is InputAreaModel.inputAreaItem.repetition)
            {
                comboBox.DataSource = new List<int>(_model.repetitionCountList);
                comboBox.Location = new Point(400, 30);
                comboBox.Name = _model.baseRepetitionComboBox + order;
                comboBox.SelectedValueChanged += OnChengeDelayOrRepetitionComboBox;
            }
            else
            {
                comboBox.DataSource = new List<int>(_model.delayTimeList);
                comboBox.Location = new Point(230, 65);
                comboBox.Name = _model.baseDelayComboBoxName + order;
                comboBox.SelectedValueChanged += OnChengeDelayOrRepetitionComboBox;
            }

            comboBox.BindingContextChanged += (sender, e) =>
            {
                if (comboBox.DataSource is not null)
                {
                    comboBox.SelectedItem = initValue;
                }
            };
            return comboBox;
        }

        private ComboBox GenerateAllCommandRepetitionComboBox(int ExistingAllCommandRepetition)
        {
            ComboBox comboBox = new ComboBox
            {
                BackColor = backColor,
                Font = baseFont,
                ForeColor = foreColor,
                FormattingEnabled = true,
                Size = new Size(70, 30),
                TabStop = false,
                DataSource = new List<int>(_model.repetitionCountList),
                Location = new Point(800, 20),
                Name = "allCommandRepetitionComboBox",
            };
            comboBox.BindingContextChanged += (sender, e) =>
            {
                if (comboBox.DataSource is not null)
                {
                    comboBox.SelectedItem = ExistingAllCommandRepetition;
                }
            };
            comboBox.SelectedValueChanged += OnChengeAllCommandRepetitionComboBox;

            return comboBox;
        }

        private TextBox GenerateExecutionTextBox()
        {
            TextBox executionTextBox = new TextBox
            {
                BackColor = backColor,
                BorderStyle = borderStyle,
                Cursor = Cursors.IBeam,
                Font = baseFont,
                ForeColor = foreColor,
                Location = new Point(30, 50),
                Name = "executionTextBox",
                Size = new Size(490, 35),
                Tag = 0,
                TabStop = false,
                Text = _dataManager.exeCommand.ExeCommandStr
            };
            executionTextBox.TextChanged += OnChangeTextBox;
            executionTextBox.KeyDown += _viewModel.OnKeyDownTextBox;
            executionTextBox.KeyUp += _viewModel.OnKeyUpTextBox;
            _viewModel.InitRegisterExeCommand(_dataManager.exeCommand);
            return executionTextBox;
        }

        private void OnChangeTextBox(object? sender, EventArgs e)
        {
            int nextNewOrder = 0;
            if (sender is null) return;
            TextBox textBox = (TextBox)sender;
            if (textBox.Name.Contains("exe")) return;
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
            if (!_dataManager.commands.Any(data => data.Order == tagNumber + 1))
            {
                if (int.TryParse(textBox.Tag!.ToString(), out nextNewOrder)) nextNewOrder++;
                AddNewLine(nextNewOrder);
            }
        }

        private void OnChengeDelayOrRepetitionComboBox(object? sender, EventArgs e)
        {
            if (sender is null) return;
            ComboBox comboBox = (ComboBox)sender;
            _viewModel.OnChangeComboBox(comboBox, e);
        }

        private void OnChengeAllCommandRepetitionComboBox(object? sender, EventArgs e)
        {
            if (sender is null) return;
            ComboBox comboBox = (ComboBox)sender;
            _viewModel.OnChangeAllCommandRepetitionComboBox(comboBox, e);
        }

        private void OnClickTrash(object? sender, EventArgs e)
        {
            if (sender is null) return;
            PictureBox pictureBox = (PictureBox)sender;
            if (pictureBox.Parent is null) return;
            Panel parentPanel = (Panel)pictureBox.Parent;

            if (int.TryParse(parentPanel.Tag?.ToString(), out int clickedOrder))
            {
                _viewModel.OnClickTrash(clickedOrder);
                ClearMacroPanel();
                SyncDataView();
            }
        }

        private void OnClickPlus(object? sender, EventArgs e)
        {
            if (sender is null) return;
            PictureBox pictureBox = (PictureBox)sender;
            if (pictureBox.Parent is null) return;
            Panel parentPanel = (Panel)pictureBox.Parent;

            if (int.TryParse(parentPanel.Tag?.ToString(), out int clickedOrder))
            {
                _viewModel.OnClickPlus(clickedOrder);
                ClearMacroPanel();
                SyncDataView();
            }
        }

        private void OnCloseForm(object? sender, EventArgs e)
        {
            _viewModel.OnCloseForm(sender, e);
        }
    }
}
