using System.Collections.Generic;
using System.Threading.Tasks;
using Solidry.Extensions;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// Process asynchronous logic.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class WithProcessorAsync<TInput, TResult>
    {
        private readonly List<TResult> _accumulator;

        private TInput _input;

        /// <summary>
        /// Create processor.
        /// </summary>
        protected WithProcessorAsync()
        {
            _accumulator = new List<TResult>();
        }

        /// <summary>
        /// Create processor.
        /// </summary>
        /// <param name="capacity"></param>
        protected WithProcessorAsync(int capacity)
        {
            _accumulator = new List<TResult>(capacity);
        }

        /// <summary>
        /// History of results.
        /// </summary>
        protected IReadOnlyList<TResult> Accumulator => _accumulator;

        /// <summary>
        /// Check if finish execution of method.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>If true return</returns>
        protected abstract bool FinishLoop(TInput context);

        /// <summary>
        /// Body of method.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Return next result</returns>
        protected abstract Task<TResult> ProcessAsync(TInput context);

        /// <summary>
        /// Invoke processing.
        /// </summary>
        /// <param name="context">Input parameter</param>
        /// <returns></returns>
        protected async Task<Option<TResult>> InvokeAsync(TInput context)
        {
            _accumulator.Clear();
            _input = context;

            while (!FinishLoop(_input))
            {
                TResult result = await ProcessAsync(_input).ConfigureAwait(false);

                _accumulator.Add(result);
            }

            if (_accumulator.IsNullOrEmpty())
            {
                return Option<TResult>.Empty;
            }

            return Option<TResult>.Create(_accumulator.Last());
        }

        /// <summary>
        /// Set input argument to be processed.
        /// </summary>
        /// <param name="context"></param>
        protected void SetInput(TInput context)
        {
            _input = context;
        }
    }
}