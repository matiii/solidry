using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Solidry.Helpers
{
    public delegate T InstanceActivator<out T>(params object[] args);

    public static class InstanceHelper
    {
        private static readonly Dictionary<ConstructorInfo, Func<object[],object>> Activators = new Dictionary<ConstructorInfo, Func<object[],object>>();

        /// <summary>
        /// Create instance from cached activator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments"></param>
        /// <returns>Instance of object</returns>
        public static T InstanceOf<T>(params object[] arguments)
        {
            var ctor = typeof(T).GetConstructor(arguments.Select(x => x.GetType()).ToArray());

            if (Activators.ContainsKey(ctor))
            {
                return (T) Activators[ctor](arguments);
            }

            InstanceActivator<T> activator = GetActivator<T>(ctor);

            Activators.Add(ctor, args => activator);

            return activator(arguments);
        }

        /// <summary>
        /// Get activator of type
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="constructorInfo"></param>
        /// <returns>Method to create</returns>
        public static InstanceActivator<TInstance> GetActivator<TInstance>(ConstructorInfo constructorInfo)
        {
            ParameterInfo[] parametersInfo = constructorInfo.GetParameters();

            ParameterExpression parameters =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argumentsExpression =
                new Expression[parametersInfo.Length];

            for (int i = 0; i < parametersInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type parameterType = parametersInfo[i].ParameterType;

                Expression parameterExpression =
                    Expression.ArrayIndex(parameters, index);

                Expression parameterCastExpression =
                    Expression.Convert(parameterExpression, parameterType);

                argumentsExpression[i] = parameterCastExpression;
            }

            NewExpression newExpression = Expression.New(constructorInfo, argumentsExpression);

            LambdaExpression lambda =
                Expression.Lambda(typeof(InstanceActivator<TInstance>), newExpression, parameters);

            return (InstanceActivator<TInstance>) lambda.Compile();
        }
    }
}