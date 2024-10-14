namespace MRPA
{
    internal class InputAreaModel
    {

        public string baseTextBoxName { get; private set; } = "commandTextBox";

        public string baseDelayComboBoxName { get; private set; } = "delayComboBox";

        public string baseRepetitionComboBox { get; private set; } = "repetitionComboBox";

        public List<int> delayTimeList { get; private set; } =
            Enumerable.Range(1, 20).Select(x => x * 100).ToList();

        public List<int> repetitionCountList { get; private set; } =
            Enumerable.Range(1, 10).Select(x => x).ToList();

        public enum inputAreaItem
        {
            command,
            repetition,
            delay
        }

        public InputAreaModel() { }
    }
}
