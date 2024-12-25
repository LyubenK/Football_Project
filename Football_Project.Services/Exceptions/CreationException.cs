namespace Football_Project.Services.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation fails during creation.
    /// </summary>
    public class CreationException : ServiceException
    {
        public CreationException() { }

        public CreationException(string message) : base(message) { }

        public CreationException(string message, Exception innerException) : base(message, innerException) { }
    }
}

