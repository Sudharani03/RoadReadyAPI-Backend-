namespace RoadReady.Exceptions
{
    public class ReservationListEmptyException : Exception
    {
        string message;
        public ReservationListEmptyException()
        {
            message = "Reservation List is Empty.";
        }
        public override string Message => message;
    }
}

