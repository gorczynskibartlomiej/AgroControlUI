namespace AgroControlUI.Exceptions
{
    public class UniqueValueException : Exception
    {
        public UniqueValueException(string message) :base(message)
        {
            
        }
        public UniqueValueException() : base("The value must be uniqe.") { }
    }
}
