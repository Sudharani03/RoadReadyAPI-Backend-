namespace RoadReady.Exceptions
{
    public class ReservationConflictException:Exception
    {
        string message;
        public ReservationConflictException()
        {
            message = "Sorry !!! The Car is already Reserved on the above dates";
        }
        public override string Message => message;
    }
}
