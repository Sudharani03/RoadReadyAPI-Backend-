namespace RoadReady.Exceptions
{
    public class CarStoreAlreadyExistsException : Exception
    {
        string message;
        public CarStoreAlreadyExistsException()
        {
            message = "CarStore already exists.";
        }
        public override string Message => message;
    }
}
