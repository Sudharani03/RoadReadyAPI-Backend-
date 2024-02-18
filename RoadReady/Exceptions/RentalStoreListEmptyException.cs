namespace RoadReady.Exceptions
{
    public class RentalStoreListEmptyException : Exception
    {
        string message;
        public RentalStoreListEmptyException()
        {
            message = "RentalStore List is Empty.";
        }
        public override string Message => message;
    }
}

