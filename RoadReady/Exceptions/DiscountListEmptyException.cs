namespace RoadReady.Exceptions
{
    public class DiscountListEmptyException : Exception
    {
        string message;
        public DiscountListEmptyException()
        {
            message = "Discount List is Empty.";
        }
        public override string Message => message;
    }
}
