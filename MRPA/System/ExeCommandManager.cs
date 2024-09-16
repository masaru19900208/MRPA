using System.Runtime.InteropServices;

namespace MRPA
{
    internal partial class ExeCommandManager
    {
        //private readonly IExeCommandEventArgs? _exeCommandEventArgs;
        private const int WM_HOTKEY = 0x0312;
        private readonly int _hotkeyId;
        private IntPtr _windowHandle;
        private readonly Form _mainForm;
        private readonly IExeCommandEventArgs _exeListener;
        private readonly NativeWindow _window;

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool UnregisterHotKey(IntPtr hWnd, int id);

        public ExeCommandManager(int hotKeyId, Form mainForm, IExeCommandEventArgs exeListener)
        {
            _hotkeyId = hotKeyId;
            _mainForm = mainForm;
            _exeListener = exeListener;
            _window = new MessageWindow(this, _exeListener);
            _window.AssignHandle(_mainForm.Handle);
        }

        public void RegisterExeCommand(SettingsData.ExeCommand exeCommand)
        {
            uint mergedAttributeKey = GetKeyUint(exeCommand.ExeCommandByte, isMergeAttributeKeys: true);
            uint notAttributeKey = GetKeyUint(exeCommand.ExeCommandByte, isMergeAttributeKeys: false);
            RegisterHotKey(mergedAttributeKey, notAttributeKey);
        }

        public void RegisterHotKey(uint registerModifiers, uint registerKey)
        {
            _windowHandle = IntPtr.Zero;
            RegisterHotKey(_window.Handle, _hotkeyId, registerModifiers, registerKey);
        }

        public void UnregisterHotKey()
        {
            UnregisterHotKey(_window.Handle, _hotkeyId);
            _window.ReleaseHandle();
        }

        private class MessageWindow(ExeCommandManager manager, IExeCommandEventArgs exeListener) : NativeWindow
        {
            private readonly ExeCommandManager _manager = manager;
            private readonly IExeCommandEventArgs _exeListener = exeListener;

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    int id = m.WParam.ToInt32();
                    if (id == _manager._hotkeyId)
                    {
                        _exeListener.OnExeCommandOccur(this, EventArgs.Empty);
                    }
                }
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Create and return values required for hot key registration
        /// </summary>
        /// <param name="keyList">List instance containing commands for macro firing</param>
        /// <param name="isMergeAttributeKeys">True to return a merge for the modifier key, false otherwise</param>
        /// <returns>MergeAttributeKeys is true, it is a uint value to register for the hot key; False, it is a merged uint value of type Keys</returns>
        /// <remarks>Note that isMergeAttributeKeys changes the source type of the returned value.</remarks>
        private uint GetKeyUint(List<byte> keyList, bool isMergeAttributeKeys)
        {
            uint resultKey = 0;
            Dictionary<byte, int> attributeKeyByteDict =
            new Dictionary<byte, int>(){
                {(byte)Keys.ControlKey, 0x0002},
                {(byte)Keys.LControlKey, 0x0002},
                {(byte)Keys.RControlKey, 0x0002},
                {(byte)Keys.ShiftKey, 0x0004},
                {(byte)Keys.LShiftKey, 0x0004},
                {(byte)Keys.RShiftKey, 0x0004},
                {(byte)Keys.Menu, 0x0001},
                {(byte)Keys.LMenu, 0x0001},
                {(byte)Keys.RMenu, 0x0001},
                {(byte)Keys.LWin, 0x0008},
                {(byte)Keys.RWin, 0x0008},
            };
            Dictionary<int, int> attributeKeyIntDict =
            new Dictionary<int, int>()
            {
                { (int)Keys.Shift,0x0004},
                { (int)Keys.Alt,0x0001},
                { (int)Keys.Control, 0x0002},
            };

            List<uint> pastResult = [];
            foreach (var inputKey in keyList)
            {
                bool isAttributeKey = false;
                foreach (var attributeKey in attributeKeyByteDict)
                {
                    if (inputKey == attributeKey.Key)
                    {
                        resultKey |= (uint)attributeKey.Value;
                        isAttributeKey = true;
                    }
                }
                if (!isAttributeKey) pastResult.Add((uint)inputKey);
            }
            foreach (var inputKey in pastResult)
            {
                foreach (var attributeKey in attributeKeyIntDict)
                {
                    if (inputKey == attributeKey.Key)
                    {
                        resultKey |= (uint)attributeKey.Value;
                        pastResult.Remove(inputKey);
                    }
                }
            }
            if (!isMergeAttributeKeys)
            {
                // It is not AttributeKeys, reply with the Keys type.
                resultKey = pastResult
                    .Select(keyByte => (uint)keyByte)
                    .Aggregate((acc, next) => acc | next);
            }
            return resultKey;
        }

    }
}
