namespace RoadReady.Exceptions
{
    public class NoSuchReservationException : Exception
    {
        string message;
        public NoSuchReservationException()
        {
            message = "No Reservation with the given id";
        }
        public override string Message => message;
    }
}
