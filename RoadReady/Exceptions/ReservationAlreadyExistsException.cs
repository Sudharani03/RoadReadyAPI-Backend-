namespace RoadReady.Exceptions
{
    public class ReservationAlreadyExistsException : Exception
    {
        string message;
        public ReservationAlreadyExistsException()
        {
            message = "Reservation already exists.";
        }
        public override string Message => message;
    }
}
