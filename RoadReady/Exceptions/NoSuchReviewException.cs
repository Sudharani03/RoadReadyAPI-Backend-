namespace RoadReady.Exceptions
{
    public class NoSuchReviewException : Exception
    {
        string message;
        public NoSuchReviewException()
        {
            message = "No review Found...";
        }
        public override string Message => message;
    }
}
