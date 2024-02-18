namespace RoadReady.Exceptions
{
    public class ReviewListEmptyException : Exception
    {
        string message;
        public ReviewListEmptyException()
        {
            message = "Review List is Empty.";
        }
        public override string Message => message;
    }
}

