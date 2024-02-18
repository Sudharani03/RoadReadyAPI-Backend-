namespace RoadReady.Exceptions
{
    public class NoSuchDiscountException : Exception
    {
        string message;
        public NoSuchDiscountException()
        {
            message = "No Discount with the given id";
        }
        public override string Message => message;
    }
}
