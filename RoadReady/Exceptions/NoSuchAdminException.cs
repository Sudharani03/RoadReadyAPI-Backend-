namespace RoadReady.Exceptions
{
    public class NoSuchAdminException:Exception
    {
        string message;
        public NoSuchAdminException()
        {
            message = "No admin with the given id";
        }
        public override string Message => message;
    }
}
