using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Solidry.Extensions;
using Solidry.Results;

using Task = System.Threading.Tasks.Task;

namespace Solidry.Aspects
{
    public abstract class WithAspectAsync<TInput, TOutput>
    {
        private readonly List<Func<TInput, Option<TOutput>>> _before = new List<Func<TInput, Option<TOutput>>>();
        private readonly List<Func<TInput, Task<Option<TOutput>>>> _beforeAsync = new List<Func<TInput, Task<Option<TOutput>>>>();

        private readonly List<Action<TInput, TOutput>> _after = new List<Action<TInput, TOutput>>();
        private readonly List<Func<TInput, TOutput, Task>> _afterAsync = new List<Func<TInput, TOutput, Task>>();

        private bool _aspectsWasRegister;

        /// <summary>
        /// Execute asynchronous logic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected abstract Task<TOutput> ExecuteAsync(TInput input);

        /// <summary>
        /// Register aspects.
        /// </summary>
        protected abstract void RegisterAspects();

        /// <summary>
        /// Execute logic with aspects.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected async Task<TOutput> InvokeAsync(TInput input)
        {
            if (!_aspectsWasRegister)
            {
                RegisterAspects();
                _aspectsWasRegister = true;
            }

            for (int i = 0; i < _before.Count; i++)
            {
                Option<TOutput> result = _before[i](input);

                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            for (int i = 0; i < _beforeAsync.Count; i++)
            {
                Option<TOutput> result = await _beforeAsync[i](input);

                if (result.HasValue)
                {
                    return result.Value;
                }
            }

            TOutput output = await ExecuteAsync(input);

            for (int i = 0; i < _afterAsync.Count; i++)
            {
                await _afterAsync[i](input, output);
            }

            _after.Each(x => x(input, output));

            return output;
        }

        /// <summary>
        /// Add synchronous aspect before. Will be executed before asynchronous before aspect.
        /// </summary>
        /// <param name="before"></param>
        protected void AddBefore(Func<TInput, Option<TOutput>> before)
        {
            _before.Add(before);
        }

        /// <summary>
        /// Add asynchronous aspect before. Will be executed after synchronous before aspect.
        /// </summary>
        /// <param name="before"></param>
        protected void AddBefore(Func<TInput, Task<Option<TOutput>>> before)
        {
            _beforeAsync.Add(before);
        }

        /// <summary>
        /// Add synchronous aspect after. Will be executed after asynchronous after aspect.
        /// </summary>
        /// <param name="after"></param>
        protected void AddAfter(Action<TInput, TOutput> after)
        {
            _after.Add(after);
        }

        /// <summary>
        /// Add asynchronous aspect after. Will be executed before synchronous after aspect. 
        /// </summary>
        /// <param name="after"></param>
        protected void AddAfter(Func<TInput, TOutput, Task> after)
        {
            _afterAsync.Add(after);
        }
    }
}