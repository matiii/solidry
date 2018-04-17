using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Results;

namespace Solidry.Aspects
{
    public abstract class WithAspectAndErrorHandler<TInput, TOutput> : WithAspect<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategy _errorHandlerStrategy;

        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy, IGeneralAspect generalAspect) :
            this(errorHandlerStrategy, generalAspect, null, null)
        {
        }

        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before) : this(errorHandlerStrategy, before, null)
        {
        }

        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) : this(errorHandlerStrategy, null, before, after)
        {
        }

        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy, IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) : base(generalAspect, before, after)
        {
            _errorHandlerStrategy = errorHandlerStrategy;
        }

        /// <summary>
        /// Execute finally logic. It will execute always.
        /// Good spot do dispose objects.
        /// </summary>
        /// <param name="input"></param>
        protected virtual void Finally(TInput input)
        {
            (input as IDisposable)?.Dispose();
        }

        /// <summary>
        /// Execute logic with error handlers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected new Option<TOutput> Invoke(TInput input)
        {
            try
            {
                return Option<TOutput>.Create(base.Invoke(input));
            }
            catch (Exception e)
            {
                if (!_errorHandlerStrategy.TryHandle(e, CurrentOperationId))
                {
                    throw;
                }
            }
            finally
            {
                Finally(input);
            }

            return Option<TOutput>.Empty;
        }
    }
}