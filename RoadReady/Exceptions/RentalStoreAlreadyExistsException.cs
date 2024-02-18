namespace RoadReady.Exceptions
{
    public class RentalStoreAlreadyExistsException : Exception
    {
        string message;
        public RentalStoreAlreadyExistsException()
        {
            message = "RentalStore already exists.";
        }
        public override string Message => message;
    }
}
