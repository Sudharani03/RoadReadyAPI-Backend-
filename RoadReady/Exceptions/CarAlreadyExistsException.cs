namespace RoadReady.Exceptions
{
    public class CarAlreadyExistsException : Exception
    {
        string message;
        public CarAlreadyExistsException()
        {
            message = "Car already exists.";
        }
        public override string Message => message;
    }
}
