using System.Collections.Generic;
using Solidry.Extensions;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// Process recursion in object oriented way.
    /// </summary>
    public abstract class WithProcessor<TInput, TResult>
    {
        private readonly List<TResult> _accumulator;

        private TInput _input;

        /// <summary>
        /// Create processor.
        /// </summary>
        protected WithProcessor()
        {
            _accumulator = new List<TResult>();
        }

        /// <summary>
        /// Create processor.
        /// </summary>
        /// <param name="capacity">Initial capacity of accumulator</param>
        protected WithProcessor(int capacity)
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
        protected abstract TResult Process(TInput context);

        /// <summary>
        /// Invoke processing.
        /// </summary>
        /// <param name="context">Input parameter</param>
        /// <returns></returns>
        protected Option<TResult> Invoke(TInput context)
        {
            _accumulator.Clear();
            _input = context;

            while (!FinishLoop(_input))
            {
                TResult result = Process(_input);

                _accumulator.Add(result);
            }

            if (_accumulator.Count == 0)
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