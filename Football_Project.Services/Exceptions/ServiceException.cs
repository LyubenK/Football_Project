namespace Football_Project.Services.Exceptions
{
    /// <summary>
    /// Base class for service-related exceptions.
    /// </summary>
    public class ServiceException : Exception
    {
        public ServiceException() { }

        public ServiceException(string message) : base(message) { }

        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}

