namespace Common.Models
{
    public class MarsException
    {
        public string Message { get; private set; }

        public MarsException()
        {
            Message = "";
        }

        public MarsException(string msg)
        {
            Message = msg;
        }
    }
}
