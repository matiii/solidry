using Solidry.Model;

namespace Solidry.Aspects
{
    /// <summary>
    /// Define module with service installer.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TApplicationType"></typeparam>
    public abstract class WithServiceInstaller<TContainer, TApplicationType>
    {
        /// <summary>
        /// Install services.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="applicationType"></param>
        public abstract void Install(TContainer container, TApplicationType applicationType);
    }

    /// <inheritdoc />
    /// <summary>
    /// Define module with service installer per application type.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    public abstract class WithServiceInstaller<TContainer> : WithServiceInstaller<TContainer, ApplicationType>
    {
    }
}