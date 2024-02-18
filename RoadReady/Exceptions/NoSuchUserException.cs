namespace RoadReady.Exceptions
{
    public class NoSuchUserException : Exception
    {
        string message;
        public NoSuchUserException()
        {
            message = "No user with the given id";
        }
        public override string Message => message;
    }
}
