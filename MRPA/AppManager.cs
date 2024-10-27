namespace MRPA
{
    internal class AppManager
    {
        public AppManager(MainForm mainForm, FlowLayoutPanel macroPanel, RichTextBox consoleTextBox)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MRPA");
            string jsonFilePath = Path.Combine(folderPath, "settings.json");
            DataManager dataManager = new DataManager(jsonFilePath);
            ConsoleManager.Initialization(consoleTextBox);

            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            if (!File.Exists(jsonFilePath)) dataManager.BuildSettingsJsonFile();
            _ = new InputAreaView(dataManager, mainForm, macroPanel);
        }
    }
}
