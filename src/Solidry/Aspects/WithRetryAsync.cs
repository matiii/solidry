using System;
using System.Threading.Tasks;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Internal;

namespace Solidry.Aspects
{
    /// <summary>
    /// Aspect with retry.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithRetryAsync<TInput, TOutput>
    {
        private readonly IRetryStrategy _retryStrategy;
        private readonly int _delayMiliseconds;

        /// <summary>
        /// Retry with delay miliseconds.
        /// </summary>
        /// <param name="retryStrategy"></param>
        /// <param name="delayMiliseconds"></param>
        protected WithRetryAsync(IRetryStrategy retryStrategy, int delayMiliseconds)
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
        protected WithRetryAsync(IRetryStrategy retryStrategy, TimeSpan delay) : this(retryStrategy, (int)delay.TotalMilliseconds)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Retry with 20 miliseconds.
        /// </summary>
        /// <param name="retryStrategy"></param>
        protected WithRetryAsync(IRetryStrategy retryStrategy) : this(retryStrategy, Constant.DefaultDelay)
        {
        }

        /// <summary>
        /// Execute logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract Task<TOutput> ExecuteAsync(TInput input);

        /// <summary>
        /// Current operation id
        /// </summary>
        protected Guid OperationId { get; private set; }

        /// <summary>
        /// Invoke logic with retry.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected async Task<TOutput> InvokeAsync(TInput input)
        {
            OperationId = Guid.NewGuid();
            int attempt = 0;
            int delayMiliseconds = _delayMiliseconds;

            while (true)
            {
                attempt++;

                try
                {
                    return await ExecuteAsync(input).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    if (!_retryStrategy.Retry(OperationId, e, attempt, delayMiliseconds, x => { delayMiliseconds = x; }))
                    {
                        throw;
                    }
                }

                await Task.Delay(delayMiliseconds).ConfigureAwait(false);
            }
        }
    }
}