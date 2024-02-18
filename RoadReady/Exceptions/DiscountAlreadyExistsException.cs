namespace RoadReady.Exceptions
{
    public class DiscountAlreadyExistsException : Exception
    {
        string message;
        public DiscountAlreadyExistsException()
        {
            message = "Discount already exists.";
        }
        public override string Message => message;
    }
}
