namespace RoadReady.Exceptions
{
    public class InvalidValidationException : Exception
    {
        string message;
        public InvalidValidationException()
        {
            message = "Invalid username or password";
        }

        public override string Message => message;
    
    }
}
