using System;
using System.Collections.Generic;
using Solidry.Aspects.Contract;
using Solidry.Aspects.Contract.Factory;
using Solidry.Results;

namespace Solidry.Aspects
{
    /// <inheritdoc />
    /// <summary>
    /// Aspect with error handler.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public abstract class WithAspectAndErrorHandler<TInput, TOutput> : WithAspect<TInput, TOutput>
    {
        private readonly IErrorHandlerStrategy _errorHandlerStrategy;

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy and general aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy, IGeneralAspect generalAspect) :
            this(errorHandlerStrategy, generalAspect, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy and before aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="before"></param>
        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before) : this(errorHandlerStrategy, before, null)
        {
        }

        /// <summary>
        /// Create with error handler strategy, before and after aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) : this(errorHandlerStrategy, null, before, after)
        {
        }

        /// <summary>
        /// Create with factory.
        /// </summary>
        /// <param name="factory"></param>
        protected WithAspectAndErrorHandler(IAspectAndErrorHandlerFactory<TInput, TOutput> factory)
            : this(factory.ErrorHandlerStrategy, factory.GeneralAspect, factory.Before, factory.After)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Create with error handler strategy, general aspect, before and after aspect.
        /// </summary>
        /// <param name="errorHandlerStrategy"></param>
        /// <param name="generalAspect"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        protected WithAspectAndErrorHandler(IErrorHandlerStrategy errorHandlerStrategy, IGeneralAspect generalAspect,
            IReadOnlyList<IBeforeAspect<TInput, TOutput>> before,
            IReadOnlyList<IAfterAspect<TInput, TOutput>> after) : base(generalAspect, before, after)
        {
            _errorHandlerStrategy = errorHandlerStrategy ?? throw new ArgumentNullException("Error handler cannot be null.");
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