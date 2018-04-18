using System;
using System.Threading;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Internal;

namespace Solidry.Aspects
{
    /// <summary>
    /// Define logic with retry aspect. 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithRetry<TInput, TOutput>
    {
        private readonly IRetryStrategy _retryStrategy;
        private readonly int _delayMiliseconds;

        /// <summary>
        /// Retry with dealy miliseconds.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delayMiliseconds"></param>
        protected WithRetry(IRetryStrategy retryStrategy, int delayMiliseconds)
        {
            _retryStrategy = retryStrategy;
            _delayMiliseconds = delayMiliseconds;
        }

        /// <inheritdoc />
        /// <summary>
        /// Retry with delay.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delay"></param>
        protected WithRetry(IRetryStrategy retryStrategy, TimeSpan delay) : this(retryStrategy, (int)delay.TotalMilliseconds)
        {
            
        }

        /// <inheritdoc />
        /// <summary>
        /// Retry with 20 miliseconds.
        /// </summary>
        /// <param name="retryStrategy"></param>
        protected WithRetry(IRetryStrategy retryStrategy) : this(retryStrategy, Constant.DefaultDelay)
        {
        }

        /// <summary>
        /// Execute logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract TOutput Execute(TInput input);

        /// <summary>
        /// Current operation id
        /// </summary>
        protected Guid OperationId { get; private set; }

        /// <summary>
        /// Invoke logic with retry.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected TOutput Invoke(TInput input)
        {
            OperationId = Guid.NewGuid();
            int attempt = 0;
            int delayMiliseconds = _delayMiliseconds;

            while (true)
            {
                attempt++;

                try
                {
                    return Execute(input);
                }
                catch (Exception e)
                {
                    if (!_retryStrategy.Retry(OperationId, e, attempt, delayMiliseconds, x => { delayMiliseconds = x; } ))
                    {
                        throw;
                    }
                }
                
                SpinWait.SpinUntil(() => false, delayMiliseconds);
            }
        }

    }
}