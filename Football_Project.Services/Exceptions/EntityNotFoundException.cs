﻿using System;
namespace Football_Project.Services.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found.
    /// </summary>
    public class EntityNotFoundException : ServiceException
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}

