using System;
using System.Collections.Generic;
using Solidry.Results;

namespace Solidry.Aspects
{
    public abstract class WithErrorHandler<TInput, TOutput>
    {
        private readonly Dictionary<Type, List<Func<Exception, bool>>> _handlers =
            new Dictionary<Type, List<Func<Exception, bool>>>();

        private readonly bool _shouldBreak;

        private bool _errorHandlersRegistered;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shouldBreak">If true, only one handler handle exception</param>
        protected WithErrorHandler(bool shouldBreak)
        {
            _shouldBreak = shouldBreak;
        }

        protected WithErrorHandler() : this(true)
        {
        }

        /// <summary>
        /// Try execute logic
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract TOutput Try(TInput input);

        protected abstract void RegisterErrorhandlers();

        protected virtual void Finally(TInput input)
        {
            (input as IDisposable)?.Dispose();
        }

        protected void AddErrorHandler<TException>(Func<TException, bool> handler) where TException : Exception
        {
            Type typeException = typeof(TException);

            if (_handlers.ContainsKey(typeException))
            {
                _handlers[typeException].Add(e => handler((TException)e));
            }
            else
            {
                _handlers.Add(typeException, new List<Func<Exception, bool>> {e => handler((TException)e)});
            }
        }

        protected Option<TOutput> Invoke(TInput input)
        {
            if (!_errorHandlersRegistered)
            {
                _errorHandlersRegistered = true;
                RegisterErrorhandlers();
            }

            try
            {
                return Option<TOutput>.Create(Try(input));
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

                if (!isHandled)
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