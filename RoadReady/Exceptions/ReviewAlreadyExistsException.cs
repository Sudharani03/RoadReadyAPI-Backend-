namespace RoadReady.Exceptions
{
    public class ReviewAlreadyExistsException : Exception
    {
        string message;
        public ReviewAlreadyExistsException()
        {
            message = "Review already exists.";
        }
        public override string Message => message;
    }
}
