namespace FastxWebApi.Exceptions
{
    public class InvalidActionException:Exception
    {
        public InvalidActionException()
        {
        }

        public InvalidActionException(string message)
            : base(message)
        {
        }

        public InvalidActionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
