namespace RoadReady.Exceptions
{
    public class CarStoreListEmptyException : Exception
    {
        string message;
        public CarStoreListEmptyException()
        {
            message = "CarStore List is Empty.";
        }
        public override string Message => message;
    }
}
