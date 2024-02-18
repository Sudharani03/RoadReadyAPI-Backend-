namespace RoadReady.Exceptions
{
    public class DiscountNotAssignedToCarException : Exception
    {
        string message;
        public DiscountNotAssignedToCarException()
        {
            message = "The car does not have the specified discount";
        }
        public override string Message => message;
    }
}
