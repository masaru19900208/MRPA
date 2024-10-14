using System.Text.Json.Serialization;

namespace MRPA
{
    internal class SettingsData
    {
        [JsonPropertyName("lastUpdate")]
        public string LastUpdate { get; set; } = "";

        [JsonPropertyName("exeCommand")]
        public ExeCommand? exeCommand { get; set; }

        [JsonPropertyName("allCommandRepetition")]
        public int AllCommandRepetition { get; set; }

        [JsonPropertyName("commands")]
        public List<Commands> commands { get; set; } = new List<Commands>();

        public class ExeCommand
        {
            [JsonPropertyName("exeCommandStr")]
            public string ExeCommandStr { get; set; } = "";

            [JsonPropertyName("exeCommandByte")]
            public List<byte> ExeCommandByte { get; set; } = new List<byte> { };
        }

        public class Commands
        {
            [JsonPropertyName("command")]
            public string Command { get; set; } = "Shift + Alt + L";

            [JsonPropertyName("delay")]
            public int Delay { get; set; } = 200;

            [JsonPropertyName("repetition")]
            public int Repetition { get; set; } = 1;

            [JsonPropertyName("order")]
            public int Order { get; set; } = 1;

            [JsonPropertyName("cmdByte")]
            public List<byte> CmdByte { get; set; } = new List<byte> { };
        }
    }
}
