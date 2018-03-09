using Solidry.Helpers;
using Xunit;

namespace Solidry.Tests.Helpers
{
    public class InstanceHelperTests
    {
        public class DemoClass
        {
            public DemoClass(string arg1)
            {
                Arg1 = arg1;
                Arg2 = 10;
            }

            public DemoClass(string arg1, int arg2)
            {
                Arg1 = arg1;
                Arg2 = arg2;
            }

            public string Arg1 { get; }

            public int Arg2 { get; }
        }

        [Fact]
        public void InstanceOf_ShouldReturn_Instance_With_Correct_Data()
        {
            DemoClass demo = InstanceHelper.InstanceOf<DemoClass>("demo", 11);

            Assert.NotNull(demo);
            Assert.Equal("demo", demo.Arg1);
            Assert.Equal(11, demo.Arg2);
        }
    }
}