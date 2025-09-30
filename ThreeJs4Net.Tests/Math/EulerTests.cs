using ThreeJs4Net.Math;
using Xunit;
using Quaternion = ThreeJs4Net.Math.Quaternion;

namespace ThreeJs4Net.Tests.Math
{
    public class EulerTests : BaseTests
    {
        private bool QuatEquals(Quaternion a, Quaternion b, float? tolerance = null)
        {
            tolerance = tolerance ?? 0.0001f;
            var diff = Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y) + Mathf.Abs(a.Z - b.Z) + Mathf.Abs(a.W - b.W);
            return (diff < tolerance);
        }

        
        [Fact()]
        public void InstancingTest()
        {
            var a = new Euler();
            Assert.Equal(eulerZero, a);
            Assert.NotEqual(eulerAxyz, a);
            Assert.NotEqual(eulerAzyx, a);
        }

        [Fact()]
        public void DefaultOrderTest()
        {
            Assert.Equal(Euler.RotationOrder.XYZ, Euler.DefaultOrder);
        }

        [Fact()]
        public void SetTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void XTest()
        {
            var a = new Euler();
            Assert.Equal(0, a.X);

            a = new Euler(1, 2, 3);
            Assert.Equal(1, a.X);

            a = new Euler(4, 5, 6, Euler.RotationOrder.XYZ);
            Assert.Equal(4, a.X);

            a = new Euler(7, 8, 9, Euler.RotationOrder.XYZ);
            a.X = 10;
            Assert.Equal(10, a.X);

            a = new Euler(11, 12, 13, Euler.RotationOrder.XYZ);
            var b = false;
            a.PropertyChanged += (sender, args) => b = true;

            a.X = 14;
            Assert.True(b);
            Assert.Equal(14, a.X);
        }

        [Fact()]
        public void CloneTest()
        {
            var a = eulerAxyz.Clone();
            Assert.Equal(a, eulerAxyz);
            Assert.NotEqual(a, eulerZero);
            Assert.NotEqual(a, eulerAzyx);

            a.Copy(eulerAzyx);
            Assert.Equal(a, eulerAzyx);
            Assert.NotEqual(a, eulerAxyz);
            Assert.NotEqual(a, eulerZero);
        }

        [Fact()]
        public void CopyTest()
        {
            var a = eulerAxyz.Clone();
            a.Copy(eulerAzyx);
            Assert.Equal(a, eulerAzyx);
            Assert.NotEqual(a, eulerAxyz);
            Assert.NotEqual(a, eulerZero);
        }

        [Fact()]
        public void SetFromRotationMatrixTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void SetFromQuaternionTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void SetFromVector3Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void FromArrayTest()
        {
            var a = new Euler();
            var array = new float[] { x, y, z };

            a.FromArray(array);
            Assert.StrictEqual(a.X, x);
            Assert.StrictEqual(a.Y, y);
            Assert.StrictEqual(a.Z, z);
            Assert.StrictEqual(a.Order, Euler.RotationOrder.XYZ);

            a = new Euler();
            array = new float[] { x, y, z, (float)Euler.RotationOrder.ZXY };
            a.FromArray(array);
            Assert.StrictEqual(a.X, x);
            Assert.StrictEqual(a.Y, y);
            Assert.StrictEqual(a.Z, z);
            Assert.StrictEqual(a.Order, Euler.RotationOrder.ZXY);
        }

        [Fact()]
        public void ToArrayTest()
        {
            var order = Euler.RotationOrder.YXZ;
            var a = new Euler(x, y, z, order);

            var array = a.ToArray();
            Assert.StrictEqual(array[0], x);
            Assert.StrictEqual(array[1], y);
            Assert.StrictEqual(array[2], z);
            Assert.StrictEqual(array[3], (int)order);

            array = new float[] { float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, 0};
            a.ToArray(array);
            Assert.StrictEqual(array[0], x);
            Assert.StrictEqual(array[1], y);
            Assert.StrictEqual(array[2], z);
            Assert.StrictEqual(array[3], (int)order);
        }

        [Fact()]
        public void ToVector3Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ReorderTest()
        {
            var testValues = new Euler[] { eulerZero, eulerAxyz, eulerAzyx };
            for (var i = 0; i < testValues.Length; i++)
            {
                var v = testValues[i];
                var q = new Quaternion().SetFromEuler(v);

                v.Reorder(Euler.RotationOrder.YZX);
                var q2 = new Quaternion().SetFromEuler(v);
                Assert.True(QuatEquals(q, q2));

                v.Reorder(Euler.RotationOrder.ZXY);
                var q3 = new Quaternion().SetFromEuler(v);
                Assert.True(QuatEquals(q, q3));
            }
        }

        [Fact()]
        public void EqualsTest()
        {
            var a = eulerAxyz.Clone();
            Assert.True(a.Equals(eulerAxyz));
            Assert.False(a.Equals(eulerZero));
            Assert.False(a.Equals(eulerAzyx));

            a.Copy(eulerAzyx);
            Assert.True(a.Equals(eulerAzyx));
            Assert.False(a.Equals(eulerAxyz));
            Assert.False(a.Equals(eulerZero));
        }
    }
}