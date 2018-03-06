using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solidry.Results;

namespace Solidry.Aspects
{
    public abstract class WithSagaAsync<TInput, TOutput> : WithAspectAndRetryAsync<TInput, TOutput>
    {
        private readonly Dictionary<Type, List<Func<Exception, bool>>> _handlers =
            new Dictionary<Type, List<Func<Exception, bool>>>();

        private readonly Dictionary<Type, List<Func<Exception, Task<bool>>>> _handlersAsync =
            new Dictionary<Type, List<Func<Exception, Task<bool>>>>();

        private readonly bool _shouldBreak;

        private bool _errorHandlersRegistered;

        /// <summary>
        /// Create error handler.
        /// </summary>
        /// <param name="shouldBreak">If true, only one handler handle exception</param>
        protected WithSagaAsync(bool shouldBreak)
        {
            _shouldBreak = shouldBreak;
        }

        /// <summary>
        /// Create error handler with shouldBreak = true.
        /// </summary>
        protected WithSagaAsync() : this(true)
        {
        }

        /// <summary>
        /// Register error handler.
        /// </summary>
        protected abstract void RegisterErrorHandlers();

        /// <summary>
        /// Execute finally logic.
        /// Good place to dispose objects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual Task FinallyAsync(TInput input)
        {
            (input as IDisposable)?.Dispose();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Add synchronous error handler. It will be executed before asynchronous error handlers.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="handler"></param>
        protected void AddErrorHandler<TException>(Func<TException, bool> handler) where TException : Exception
        {
            Type typeException = typeof(TException);

            if (_handlers.ContainsKey(typeException))
            {
                _handlers[typeException].Add(e => handler((TException)e));
            }
            else
            {
                _handlers.Add(typeException, new List<Func<Exception, bool>> { e => handler((TException)e) });
            }
        }

        /// <summary>
        /// Add asynchronous error handler. It will be executed after synchronous error handlers.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="handler"></param>
        protected void AddErrorHandler<TException>(Func<TException, Task<bool>> handler) where TException : Exception
        {
            Type typeException = typeof(TException);

            if (_handlersAsync.ContainsKey(typeException))
            {
                _handlersAsync[typeException].Add(e => handler((TException)e));
            }
            else
            {
                _handlersAsync.Add(typeException, new List<Func<Exception, Task<bool>>> { e => handler((TException)e) });
            }
        }

        /// <summary>
        /// Execute logic with error handler.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected new async Task<Option<TOutput>> InvokeAsync(TInput input)
        {
            if (!_errorHandlersRegistered)
            {
                _errorHandlersRegistered = true;
                RegisterErrorHandlers();
            }

            try
            {
                return Option<TOutput>.Create(await base.InvokeAsync(input));
            }
            catch (Exception e)
            {
                Type type = e.GetType();
                bool isHandled = false;

                if (_handlers.ContainsKey(type))
                {
                    for (int i = 0; i < _handlers[type].Count; i++)
                    {
                        if (_handlers[type][i](e))
                        {
                            isHandled = true;

                            if (_shouldBreak)
                            {
                                break;
                            }
                        }
                    }
                }

                if (!isHandled && _handlersAsync.ContainsKey(type))
                {
                    for (int i = 0; i < _handlersAsync[type].Count; i++)
                    {
                        if (await _handlersAsync[type][i](e))
                        {
                            isHandled = true;

                            if (_shouldBreak)
                            {
                                break;
                            }
                        }
                    }
                }

                if (!isHandled)
                {
                    throw;
                }
            }
            finally
            {
                await FinallyAsync(input);
            }

            return Option<TOutput>.Empty;
        }
    }
}