namespace RoadReady.Exceptions
{
    public class NoSuchRentalStoreException : Exception
    {
        string message;
        public NoSuchRentalStoreException()
        {
            message = "No RentalStore with the given id";
        }
        public override string Message => message;
    }
}
