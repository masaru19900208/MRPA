namespace MRPA
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            macroPanel = new FlowLayoutPanel();
            LabelInputMacro = new Label();
            LabelMacroExecutionCommand = new Label();
            LabelConsole = new Label();
            clearBtn = new Button();
            consoleTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // macroPanel
            // 
            macroPanel.AutoScroll = true;
            macroPanel.FlowDirection = FlowDirection.TopDown;
            macroPanel.Location = new Point(600, 51);
            macroPanel.Name = "macroPanel";
            macroPanel.Size = new Size(510, 579);
            macroPanel.TabIndex = 0;
            macroPanel.WrapContents = false;
            // 
            // LabelInputMacro
            // 
            LabelInputMacro.AutoSize = true;
            LabelInputMacro.Font = new Font("Microsoft JhengHei", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LabelInputMacro.ForeColor = Color.AliceBlue;
            LabelInputMacro.Location = new Point(590, 14);
            LabelInputMacro.Name = "LabelInputMacro";
            LabelInputMacro.Size = new Size(175, 35);
            LabelInputMacro.TabIndex = 1;
            LabelInputMacro.Text = "Input macro";
            // 
            // LabelMacroExecutionCommand
            // 
            LabelMacroExecutionCommand.AutoSize = true;
            LabelMacroExecutionCommand.Font = new Font("Microsoft JhengHei", 16.2F);
            LabelMacroExecutionCommand.ForeColor = Color.AliceBlue;
            LabelMacroExecutionCommand.Location = new Point(7, 14);
            LabelMacroExecutionCommand.Name = "LabelMacroExecutionCommand";
            LabelMacroExecutionCommand.Size = new Size(376, 35);
            LabelMacroExecutionCommand.TabIndex = 2;
            LabelMacroExecutionCommand.Text = "Macro execution command";
            // 
            // LabelConsole
            // 
            LabelConsole.AutoSize = true;
            LabelConsole.Font = new Font("Microsoft JhengHei", 16.2F);
            LabelConsole.ForeColor = Color.AliceBlue;
            LabelConsole.Location = new Point(7, 101);
            LabelConsole.Name = "LabelConsole";
            LabelConsole.Size = new Size(123, 35);
            LabelConsole.TabIndex = 4;
            LabelConsole.Text = "Console";
            // 
            // clearBtn
            // 
            clearBtn.Location = new Point(430, 110);
            clearBtn.Name = "clearBtn";
            clearBtn.Size = new Size(94, 29);
            clearBtn.TabIndex = 0;
            clearBtn.TabStop = false;
            clearBtn.Text = "Clear";
            clearBtn.UseVisualStyleBackColor = true;
            clearBtn.Click += ClearBtn_Click;
            // 
            // consoleTextBox
            // 
            consoleTextBox.BackColor = Color.FromArgb(36, 36, 36);
            consoleTextBox.ForeColor = Color.LightCyan;
            consoleTextBox.Location = new Point(33, 145);
            consoleTextBox.Name = "consoleTextBox";
            consoleTextBox.ReadOnly = true;
            consoleTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            consoleTextBox.Size = new Size(491, 485);
            consoleTextBox.TabIndex = 5;
            consoleTextBox.TabStop = false;
            consoleTextBox.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(41, 41, 41);
            ClientSize = new Size(1182, 684);
            Controls.Add(consoleTextBox);
            Controls.Add(clearBtn);
            Controls.Add(LabelConsole);
            Controls.Add(LabelMacroExecutionCommand);
            Controls.Add(LabelInputMacro);
            Controls.Add(macroPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "MRPA";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel macroPanel;
        private Label LabelInputMacro;
        private Label LabelMacroExecutionCommand;
        private Label LabelConsole;
        private Button clearBtn;
        private RichTextBox consoleTextBox;
    }
}
