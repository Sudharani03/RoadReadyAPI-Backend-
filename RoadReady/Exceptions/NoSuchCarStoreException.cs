namespace RoadReady.Exceptions
{
    public class NoSuchCarStoreException : Exception
    {
        string message;
        public NoSuchCarStoreException()
        {
            message = "No CarStore with the given id";
        }
        public override string Message => message;
    }
}
