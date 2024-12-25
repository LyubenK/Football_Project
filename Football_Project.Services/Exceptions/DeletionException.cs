using System;
namespace Football_Project.Services.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation fails during deletion.
    /// </summary>
    public class DeletionException : ServiceException
    {
        public DeletionException() { }

        public DeletionException(string message) : base(message) { }

        public DeletionException(string message, Exception innerException) : base(message, innerException) { }
    }
}

