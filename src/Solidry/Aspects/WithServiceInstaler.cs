using Solidry.Model;

namespace Solidry.Aspects
{
    public abstract class WithServiceInstaler<TContainer, TApplicationType>
    {
        public abstract void Install(TContainer container, TApplicationType applicationType);
    }

    public abstract class WithServiceInstaller<TContainer> : WithServiceInstaler<TContainer, ApplicationType>
    {
    }
}