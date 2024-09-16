using System.Runtime.InteropServices;

namespace MRPA
{
    internal static partial class MacroManager
    {
        private const int KEYEVENTF_KEYDOWN = 0x0000;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const uint INPUT_KEYBOARD = 1;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int BaseCommandExecutionIntervalMs = 100;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public MouseKeybdHardWareInput mkhi;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MouseKeybdHardWareInput
        {
            [FieldOffset(0)]
            public MouseInput mi;
            [FieldOffset(0)]
            public KeybdInput ki;
            [FieldOffset(0)]
            public HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeybdInput
        {
            public short wVk;
            public short wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [LibraryImport("user32.dll", SetLastError = true)]
        public static partial uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [LibraryImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
        public static partial int MapVirtualKey(int wCode, int wMapType);

        static public async Task StartMacro(List<SettingsData.Commands> commands)
        {
            await _semaphore.WaitAsync();
            try
            {
                await ReadyExecution();
                ConsoleManager.LogInfo("START", "Start Macro Commands.");
                for (var i = 1; i <= commands.Count; i++)
                {
                    var cmd = commands.Where(command => command.Order == i).First();
                    List<byte> cmdsByteList = GetCorrectOrderList(cmd.CmdByte);
                    for (var j = 0; j < cmd.Repetition; j++)
                    {
                        SimulateKey(cmdsByteList, isKeyDown: true);
                        SimulateKey(cmdsByteList, isKeyDown: false);
                        Thread.Sleep(BaseCommandExecutionIntervalMs);
                    }
                    await Task.Delay(cmd.Delay);
                }
                ConsoleManager.LogInfo("FIN", "Completed Macro All Commands.");
            }
            catch (Exception e)
            {
                ConsoleManager.LogError(e.ToString());
            }
            finally { _semaphore.Release(); }
        }

        static private uint SimulateKeyPress(byte keyCode)
        {
            List<INPUT> inputList = [];

            inputList.Add(new INPUT
            {
                type = INPUT_KEYBOARD,
                mkhi = new MouseKeybdHardWareInput
                {
                    ki = new KeybdInput
                    {
                        wVk = (short)keyCode,
                        dwFlags = KEYEVENTF_KEYDOWN | KEYEVENTF_EXTENDEDKEY,
                        dwExtraInfo = 0
                    }
                }
            });
            INPUT[] inputs = inputList.ToArray();
            return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        static private uint SimulateKeyUp(byte keyCode)
        {
            List<INPUT> inputList = [];

            inputList.Add(new INPUT
            {
                type = INPUT_KEYBOARD,
                mkhi = new MouseKeybdHardWareInput
                {
                    ki = new KeybdInput
                    {
                        wVk = (short)keyCode,
                        dwFlags = KEYEVENTF_KEYUP | KEYEVENTF_EXTENDEDKEY,
                        dwExtraInfo = 0
                    }
                }
            });
            INPUT[] inputs = inputList.ToArray();
            return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        static private async Task ReadyExecution()
        {
            int countDownStartNum = 3;
            for (var i = countDownStartNum; i > 0; i--)
            {
                if (i != 1) ConsoleManager.LogInfo("READY", $"{i}", Color.LightCoral);
                else ConsoleManager.LogInfo("GO", $"{i}", Color.MediumAquamarine);
                await Task.Delay(500);
            }
        }

        static private List<byte> GetCorrectOrderList(List<byte> cmdByteList)
        {
            List<byte> correctByteList = cmdByteList.ToList();
            foreach (var cmdByte in cmdByteList)
            {
                if (!IsAttributeKey(cmdByte))
                {
                    correctByteList.Remove(cmdByte);
                    correctByteList.Add(cmdByte);
                }
            }
            return correctByteList;
        }

        static private void SimulateKey(List<byte> cmdByteList, bool isKeyDown)
        {
            uint exeCommandResult;
            foreach (Byte cmdByte in cmdByteList)
            {
                exeCommandResult = isKeyDown ? SimulateKeyPress(cmdByte) : SimulateKeyUp(cmdByte);
                if (exeCommandResult == 0)
                {
                    ConsoleManager.LogError("SendInput failed with error code: " + Marshal.GetLastWin32Error());
                }
            }
        }

        static private bool IsAttributeKey(byte cmd)
        {
            List<byte> attributeKeyByte =
            [
                (byte)Keys.ControlKey,
                (byte)Keys.LControlKey,
                (byte)Keys.RControlKey,
                (byte)Keys.ShiftKey,
                (byte)Keys.LShiftKey,
                (byte)Keys.RShiftKey,
                (byte)Keys.Menu,
                (byte)Keys.LMenu,
                (byte)Keys.RMenu,
                (byte)Keys.Space,
                (byte)Keys.LWin,
                (byte)Keys.RWin,
                (byte)Keys.CapsLock,
                (byte)Keys.Capital,
                (byte)Keys.Tab,
            ];
            List<int> attributeKeyInt = [
                (int)Keys.Shift,
                (int)Keys.Alt,
                (int)Keys.Control,
            ];

            foreach (var attributeKey in attributeKeyByte)
            {
                if (cmd == attributeKey) return true;
            }
            foreach (var attributeKey in attributeKeyInt)
            {
                if ((int)cmd == attributeKey) return true;
            }
            return false;
        }
    }
}
