namespace RoadReady.Exceptions
{
    public class NoPendingReservationsException : Exception
    {
        string message;
        public NoPendingReservationsException ()
        {
            message = "No Pending Reservations Found.";
        }
        public override string Message => message;
    }
}
