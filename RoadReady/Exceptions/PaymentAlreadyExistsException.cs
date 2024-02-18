namespace RoadReady.Exceptions
{
    public class PaymentAlreadyExistsException : Exception
    {
        string message;
        public PaymentAlreadyExistsException()
        {
            message = "Payment already exists.";
        }
        public override string Message => message;
    }
}
