namespace RoadReady.Exceptions
{
    public class CarListEmptyException : Exception
    {
        string message;
        public CarListEmptyException()
        {
            message = "Car List is Empty.";
        }
        public override string Message => message;
    }
}
