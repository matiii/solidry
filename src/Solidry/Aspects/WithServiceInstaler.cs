using System;
using Solidry.Model;

namespace Solidry.Aspects
{
    public abstract class WithServiceInstaler<TContainer, TApplicationType> where TContainer : IServiceProvider
    {
        public abstract void Install(TContainer container, TApplicationType applicationType);
    }

    public abstract class WithServiceInstaller<TContainer> : WithServiceInstaler<TContainer, ApplicationType>
        where TContainer : IServiceProvider
    {
    }
}