using ThreeJs4Net.Core;
using Xunit;

namespace ThreeJs4Net.Tests.Core
{
    public class LayersTests
    {
        [Fact()]
        public void SetTest()
        {
            var a = new Layers();

            a.Set(0);
            Assert.Equal(1, a.Mask);

            a.Set(1);
            Assert.Equal(2, a.Mask);

            a.Set(2);
            Assert.Equal(4, a.Mask);

        }

        [Fact()]
        public void EnableTest()
        {
            var a = new Layers();

            a.Set(0);
            a.Enable(0);
            Assert.Equal(1, a.Mask);

            a.Set(0);
            a.Enable(1);
            Assert.Equal(3, a.Mask);

            a.Set(1);
            a.Enable(0);
            Assert.Equal(3, a.Mask);

            a.Set(1);
            a.Enable(1);
            Assert.Equal(2, a.Mask);
        }

        [Fact()]
        public void EnableAllTest()
        {
            var a = new Layers();

            a.EnableAll();
            Assert.Equal(4294967295, a.Mask);
           
        }

        [Fact()]
        public void ToggleTest()
        {
            var a = new Layers();

            a.Set(0);
            a.Toggle(0);
            Assert.Equal(0, a.Mask);

            a.Set(0);
            a.Toggle(1);
            Assert.Equal(3, a.Mask);

            a.Set(1);
            a.Toggle(0);
            Assert.Equal(3, a.Mask);

            a.Set(1);
            a.Toggle(1);
            Assert.Equal(0, a.Mask);
        }

        [Fact()]
        public void DisableTest()
        {
            var a = new Layers();

            a.Set(0);
            a.Disable(0);
            Assert.Equal(0, a.Mask);

            a.Set(0);
            a.Disable(1);
            Assert.Equal(1, a.Mask);

            a.Set(1);
            a.Disable(0);
            Assert.Equal(2, a.Mask);

            a.Set(1);
            a.Disable(1);
            Assert.Equal(0, a.Mask);

        }

        [Fact()]
        public void DisableAllTest()
        {
            var a = new Layers();

            a.Set(0);
            a.Enable(0);
            a.Set(1);
            a.Enable(1);
            a.Set(2);
            a.Enable(2);
            a.Set(3);
            a.Enable(3);

            a.DisableAll();

            Assert.Equal(0, a.Mask);
        }

        [Fact()]
        public void TestTest()
        {
            var a = new Layers();
            var b = new Layers();

            // Both start with only channel 0 enabled, so they share a channel (same as three.js: "Start out true").
            Assert.True(a.Test(b), "Start out true");

            a.Set(1);
            Assert.False(a.Test(b), "Set channel 1 in a and fail the test");

            b.Toggle(1);
            Assert.True(a.Test(b), "Toggle channel 1 in b and pass again");
        }
    }
}