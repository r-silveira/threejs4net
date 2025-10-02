using ThreeJs4Net.Examples.Jsm.Math;
using ThreeJs4Net.Math;
using Xunit;

namespace ThreeJs4Net.Tests.Examples.Jsm.Math
{
    public class OBBTests : BaseTests
    {
        public static void AssertEqualWithTolerance(double[] expected, double[] actual, double tolerance = 1e-6)
        {
            Assert.Equal(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.True(System.Math.Abs(expected[i] - actual[i]) <= tolerance,
                    $"Expected {expected[i]} but got {actual[i]} at index {i}");
            }
        }

        [Fact()]
        public void OBBTest()
        {
            var box = new Box3(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
            var obb = new OBB();
            var matrix4 = new Matrix4(new double[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 });

            AssertEqualWithTolerance(obb.Center.ToArray(), new double[] { 0, 0, 0 });
            obb.FromBox3(box);
            AssertEqualWithTolerance(obb.Center.ToArray(), new double[] { 5, 5, 5 });
            AssertEqualWithTolerance(obb.Rotation.ToArray(), new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 });

            matrix4 = matrix4.MakeRotationX((double)0.5);
            obb.ApplyMatrix4(matrix4);

            AssertEqualWithTolerance(obb.Rotation.ToArray(), new double[]
            {
                1, 0, 0,
                0, (double)0.8775825618903728, (double)0.479425538604203,
                0, (double)-0.479425538604203, (double)0.8775825618903728
            });


            matrix4 = matrix4.MakeRotationY((double)0.3);
            obb.ApplyMatrix4(matrix4);

            AssertEqualWithTolerance(obb.Rotation.ToArray(), new double[]
            {
                (double) 0.9553365, 
                (double) 0.141679943, 
                (double) -0.2593434,
                (double) 0, 
                (double) 0.87758255, 
                (double) 0.47942555,
                (double) 0.295520216, 
                (double) -0.45801273, 
                (double) 0.838386655
            });
        }

        [Fact()]
        public void OBBTest1()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void SetTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void CopyTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void CloneTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetSizeTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ContainsPointTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void IntersectsBox3Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ApplyMatrix4Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ClampPointTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void IntersectsRayTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void FromBox3Test()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void IntersectsSphereTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void EqualsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void IntersectRayTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void IntersectsPlaneTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void IntersectsOBBTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}