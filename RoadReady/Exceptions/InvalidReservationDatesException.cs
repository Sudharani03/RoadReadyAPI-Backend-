namespace RoadReady.Exceptions
{
    public class InvalidReservationDatesException:Exception
    {
        string message;
        public InvalidReservationDatesException()
        {
            message = "Invalid Dates ";
        }
        public override string Message => message;
    }
}
