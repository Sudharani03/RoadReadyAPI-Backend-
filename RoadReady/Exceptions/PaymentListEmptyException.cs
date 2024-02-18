namespace RoadReady.Exceptions
{
    public class PaymentListEmptyException : Exception
    {
        string message;
        public PaymentListEmptyException()
        {
            message = "Payment List is Empty.";
        }
        public override string Message => message;
    }
}
