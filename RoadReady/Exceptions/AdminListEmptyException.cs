namespace RoadReady.Exceptions
{
    public class AdminListEmptyException:Exception
    {
        string message;
        public AdminListEmptyException()
        {
            message = "Admin List is Empty.";
        }
        public override string Message => message;
    }
}
