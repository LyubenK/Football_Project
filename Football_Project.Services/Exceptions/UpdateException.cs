using System;
namespace Football_Project.Services.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation fails during update.
    /// </summary>
    public class UpdateException : ServiceException
    {
        public UpdateException() { }

        public UpdateException(string message) : base(message) { }

        public UpdateException(string message, Exception innerException) : base(message, innerException) { }
    }
}

