using System.Collections.Generic;
using Solidry.Model;

namespace Solidry.Aspects
{
    public abstract class WithContainerConfiguration<TContainer, TApplicationType>
    {
        private readonly TContainer _container;

        protected WithContainerConfiguration(TContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Get container.
        /// </summary>
        public TContainer Kernel => _container;

        /// <summary>
        /// Install all services.
        /// </summary>
        public virtual void Configure()
        {
            foreach (var service in GetServiceInstallers())
            {
                service.Install(Kernel, CurrentApplicationType);
            }
        }

        /// <summary>
        /// Get current application type.
        /// </summary>
        public abstract TApplicationType CurrentApplicationType { get; }

        /// <summary>
        /// Get all installers to install.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<WithServiceInstaler<TContainer, TApplicationType>> GetServiceInstallers();
    }

    public abstract class WithContainerConfiguration<TContainer> : WithContainerConfiguration<TContainer, ApplicationType>
    {
        protected WithContainerConfiguration(TContainer container): base(container) { }
    }
}