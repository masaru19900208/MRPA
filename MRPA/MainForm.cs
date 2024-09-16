namespace MRPA
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _ = new AppManager(this, macroPanel, consoleTextBox);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ConsoleManager.ClearConsole();
        }
    }
}
