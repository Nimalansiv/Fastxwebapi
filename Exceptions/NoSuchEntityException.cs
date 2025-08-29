namespace FastxWebApi.Exceptions
{
    public class NoSuchEntityException:Exception
    {

        public NoSuchEntityException()
        {
        }

        public NoSuchEntityException(string message)
            : base(message)
        {
        }

        public NoSuchEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
