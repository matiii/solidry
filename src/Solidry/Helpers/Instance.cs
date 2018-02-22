namespace Solidry.Helpers
{
    public static class Instance
    {
        public static T Create<T>(params object[] arguments)
        {
            return default(T);
        }
    }
}