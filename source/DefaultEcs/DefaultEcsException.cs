using System;

namespace DefaultEcs
{
    /// <summary>An exception stemming from a misuse of the DefaultEcs API</summary>
    /// <remarks>Only thrown in the safe variant of DefaultEcs</remarks>
    public class DefaultEcsException : InvalidOperationException
    {
        /// <inheritdoc/>
        public DefaultEcsException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public DefaultEcsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc/>
        public DefaultEcsException() : this("A misuse of the DefaultEcs API was detected.")
        {
        }
    }
}
