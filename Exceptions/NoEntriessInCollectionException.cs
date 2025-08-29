namespace FastxWebApi.Exceptions
{
    public class NoEntriessInCollectionException:Exception
    {
        public NoEntriessInCollectionException()
        {
        }

        public NoEntriessInCollectionException(string message)
            : base(message)
        {
        }

        public NoEntriessInCollectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
