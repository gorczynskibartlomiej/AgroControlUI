namespace AgroControlUI.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message):base(message)
        {
            
        }
        public BadRequestException():base("The ID in the route does not match the ID in the provided data.")
        {
            
        }
    }
}
