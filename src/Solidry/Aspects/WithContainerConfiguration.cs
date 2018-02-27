using System;
using System.Collections.Generic;
using Solidry.Model;

namespace Solidry.Aspects
{
    public abstract class WithContainerConfiguration<TContainer, TApplicationType> where TContainer : IServiceProvider
    {
        private readonly TContainer _container;

        protected WithContainerConfiguration(TContainer container)
        {
            _container = container;
        }

        public TContainer Kernel => _container;

        public virtual void Configure()
        {
            foreach (var service in GetServiceInstallers())
            {
                service.Install(Kernel, CurrentApplicationType);
            }
        }

        public abstract TApplicationType CurrentApplicationType { get; }

        public abstract IEnumerable<WithServiceInstaler<TContainer, TApplicationType>> GetServiceInstallers();
    }

    public abstract class WithContainerConfiguration<TContainer> : WithContainerConfiguration<TContainer, ApplicationType>
        where TContainer : IServiceProvider
    {
        protected WithContainerConfiguration(TContainer container): base(container) { }
    }
}