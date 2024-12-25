namespace Football_Project.Services.Exceptions
{
    /// <summary>
    /// Exception thrown when an operation fails during retrieval.
    /// </summary>
    public class RetrievalException : ServiceException
    {
        public RetrievalException() { }

        public RetrievalException(string message) : base(message) { }

        public RetrievalException(string message, Exception innerException) : base(message, innerException) { }
    }
}

