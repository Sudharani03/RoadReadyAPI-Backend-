namespace RoadReady.Exceptions
{
    public class FailedReviewException:Exception
    {
        string message;
        public FailedReviewException()
        {
            message = "Failed to add review ";
        }
        public override string Message => message;
    }
}
