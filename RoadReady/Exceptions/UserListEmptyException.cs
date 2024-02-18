namespace RoadReady.Exceptions
{
    public class UserListEmptyException : Exception
    {
        string message;
        public UserListEmptyException()
        {
            message = "User List is Empty";
        }
        public override string Message => message;
    }
}
