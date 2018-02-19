using System.Collections.Generic;
using Solidry.Extensions;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <summary>
    /// Process recursion in object oriented way
    /// </summary>
    public abstract class Processor<TInput, TResult>
    {
        private readonly List<TResult> _accumulator = new List<TResult>();

        /// <summary>
        /// History of results
        /// </summary>
        protected IReadOnlyList<TResult> Accumulator => _accumulator;

        /// <summary>
        /// Check if finish execution of method
        /// </summary>
        /// <param name="context"></param>
        /// <returns>If true return</returns>
        protected abstract bool FinishLoop(TInput context);

        /// <summary>
        /// Body of method
        /// </summary>
        /// <param name="context"></param>
        /// <param name="accumulator"></param>
        /// <returns>Return next result</returns>
        protected abstract TResult Process(TInput context);

        /// <summary>
        /// Invoke processing
        /// </summary>
        /// <param name="context">Input parameter</param>
        /// <returns></returns>
        public Option<TResult> Invoke(TInput context)
        {
            while (!FinishLoop(context))
            {
                TResult result = Process(context);

                _accumulator.Add(result);
            }

            if (_accumulator.Count == 0)
            {
                return Option<TResult>.Empty;
            }

            return Option<TResult>.Create(_accumulator.Last());
        }
    }
}