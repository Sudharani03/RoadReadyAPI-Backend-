namespace RoadReady.Exceptions
{
    public class NoSuchPaymentException : Exception
    {
        string message;
        public NoSuchPaymentException()
        {
            message = "No payment with the given id";
        }
        public override string Message => message;
    }
}
