namespace MRPA
{
    internal class AppManager
    {
        public AppManager(MainForm mainForm, FlowLayoutPanel macroPanel, RichTextBox consoleTextBox)
        {
            string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "settings.json");
            DataManager dataManager = new DataManager(jsonFilePath);
            ConsoleManager.Initialization(consoleTextBox);

            if (!File.Exists(jsonFilePath))
            {
                dataManager.BuildSettingsJsonFile();
            }
            _ = new InputAreaView(dataManager, mainForm, macroPanel);
        }
    }
}
