using System.Collections.Generic;
using Solidry.Model;

namespace Solidry.Aspects
{
    /// <summary>
    /// Define module with container configuration.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TApplicationType"></typeparam>
    public abstract class WithContainerConfiguration<TContainer, TApplicationType>
    {

        /// <summary>
        /// Create with container.
        /// </summary>
        /// <param name="container"></param>
        protected WithContainerConfiguration(TContainer container)
        {
            Kernel = container;
        }

        /// <summary>
        /// Get container.
        /// </summary>
        public TContainer Kernel { get; }

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

    /// <inheritdoc />
    /// <summary>
    /// Define module with container cconfiguration per application type.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    public abstract class WithContainerConfiguration<TContainer> : WithContainerConfiguration<TContainer, ApplicationType>
    {

        /// <inheritdoc />
        /// <summary>
        /// Create with container. 
        /// </summary>
        /// <param name="container"></param>
        protected WithContainerConfiguration(TContainer container): base(container) { }
    }
}