using System;

namespace Solidry.Aspects.Contract
{
    public interface IRetryStrategy
    {
        /// <summary>
        /// Check if retry logic.
        /// </summary>
        /// <param name="operationId">Id of operation</param>
        /// <param name="ex">Exception threw by logic.</param>
        /// <param name="attempt">Number of attempts executing logic.</param>
        /// <param name="currentDelay">Current delay.</param>
        /// <param name="setNewDelay">Method to set new delay in method scope.</param>
        /// <returns>If true then retry.</returns>
        bool Retry(Guid operationId, Exception ex, int attempt, int currentDelay, Action<int> setNewDelay);
    }
}