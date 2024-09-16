namespace MRPA
{
    static class ConsoleManager
    {
        private static RichTextBox? _consoleTextBox;

        public static void Initialization(RichTextBox consoleTextBox)
        {
            _consoleTextBox = consoleTextBox;
        }

        public static void LogInfo(string tag, string message, Color? color = null)
        {
            var timeStamp = GetTimeStamp();
            GenerateMessage(tag, message, timeStamp);

            ChangeTextColor(tag, color ?? Color.DodgerBlue);
            ChangeTextColor(timeStamp, Color.Gray);
            MoveCursor();
        }

        public static void LogError(string message)
        {
            var timeStamp = GetTimeStamp();
            GenerateMessage("ERROR", message, timeStamp);

            ChangeTextColor("ERROR", Color.Red);
            ChangeTextColor(timeStamp, Color.Gray);
            MoveCursor();
        }

        private static void GenerateMessage(string tag, string message, string timeStamp)
        {
            if (_consoleTextBox is null) return;
            message += Environment.NewLine;
            _consoleTextBox.AppendText($"[{tag.ToUpper()}] {timeStamp} {message}");
        }

        private static void MoveCursor()
        {
            if (_consoleTextBox is null) return;
            _consoleTextBox.SelectionStart = _consoleTextBox.Text.Length;
            _consoleTextBox.ScrollToCaret();
        }

        private static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss .fff");
        }

        private static void ChangeTextColor(string messageTag, Color color)
        {
            if (_consoleTextBox is null) return;
            int startIndex = _consoleTextBox.Text.LastIndexOf(messageTag);
            if (startIndex == -1) return;
            _consoleTextBox.Select(startIndex, messageTag.Length);
            _consoleTextBox.SelectionColor = color;
            _consoleTextBox.Select(_consoleTextBox.TextLength, 0);
        }

        public static void ClearConsole()
        {
            if (_consoleTextBox is null) return;
            _consoleTextBox.Clear();
        }
    }
}
