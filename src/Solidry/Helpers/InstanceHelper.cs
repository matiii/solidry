namespace Solidry.Helpers
{
    public static class InstanceHelper
    {
        public static T Create<T>(params object[] arguments)
        {
            return default(T);
        }
    }
}