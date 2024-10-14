namespace MRPA
{
    public class ExeCommandEventArgs : EventArgs
    {
        public string Message { get; }

        public ExeCommandEventArgs(string message)
        {
            Message = message;
        }
    }
}
