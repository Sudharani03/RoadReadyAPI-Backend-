namespace RoadReady.Exceptions
{
    public class AdminAlreadyExistsException:Exception
    {
        string message;
        public AdminAlreadyExistsException()
        {
            message = "Admin already exists.";
        }
        public override string Message => message;
    }
}
