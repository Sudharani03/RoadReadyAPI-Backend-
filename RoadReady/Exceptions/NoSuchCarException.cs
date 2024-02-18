namespace RoadReady.Exceptions
{
    public class NoSuchCarException : Exception
    {
        string message;
        public NoSuchCarException()
        {
            message = "No car with the given id";
        }
        public override string Message => message;
    
    }
}
