using System;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;
using Xunit;

namespace ThreeJs4Net.Tests.Math
{
    public class Vector3Tests : BaseTests
    {
        [Fact()]
        public void Instancing()
        {
            var a = new Vector3();
            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);

            a = new Vector3(x, y, z);
            Assert.True(a.X == x);
            Assert.True(a.Y == y);
            Assert.True(a.Z == z);

        }

        [Fact()]
        public void OpDivTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);
            var c = new Vector3(x*2, y*3, 0);

            var r1 = c / a;
            var r2 = a / c;
            var r3 = b / c;

            Assert.Equal(r1.X, 2);
            Assert.Equal(r1.Y, 3);
            Assert.Equal(r1.Z, 0);

            Assert.Equal(r2.X, 0.5);
            Assert.True(r2.Y - 0.33333 <= MathUtils.EPS);
            Assert.True(double.IsInfinity(r2.Z));

            Assert.Equal(r3.X, -0.5);
            Assert.True(r3.Y - (-0.33333) <= MathUtils.EPS);
            Assert.True(double.IsInfinity(r3.Z));
        }

        [Fact()]
        public void OpSubTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);
            var c = new Vector3(x * 2, y * 3, 0);

            var r1 = c - a;
            var r2 = a - b;
            var r3 = b - c;

            Assert.Equal(r1.X, 2f);
            Assert.Equal(r1.Y, 6f);
            Assert.Equal(r1.Z, -4f);

            Assert.Equal(r2.X, 4f);
            Assert.Equal(r2.Y, 6f);
            Assert.Equal(r2.Z, 8f);

            Assert.Equal(r3.X, -6f);
            Assert.Equal(r3.Y, -12f);
            Assert.Equal(r3.Z, -4f);
        }

        [Fact()]
        public void OpAddTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);
            var c = new Vector3(x * 2, y * 3, 0);

            var r1 = c + a;
            var r2 = a + b;
            var r3 = b + c;

            Assert.Equal(r1.X, 6f);
            Assert.Equal(r1.Y, 12f);
            Assert.Equal(r1.Z, 4f);

            Assert.Equal(r2.X, 0f);
            Assert.Equal(r2.Y, 0f);
            Assert.Equal(r2.Z, 0f);

            Assert.Equal(r3.X, 2f);
            Assert.Equal(r3.Y, 6f);
            Assert.Equal(r3.Z, -4f);
        }

        [Fact()]
        public void OpMulTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);
            var c = new Vector3(x * 2, y * 3, 0);

            var r1 = c * a;
            var r2 = a * b;
            var r3 = b * c;

            Assert.Equal(r1.X, 8);
            Assert.Equal(r1.Y, 27);
            Assert.Equal(r1.Z, 0);

            Assert.Equal(r2.X, -4f);
            Assert.Equal(r2.Y, -9f);
            Assert.Equal(r2.Z, -16f);

            Assert.Equal(r3.X, -8f);
            Assert.Equal(r3.Y, -27f);
            Assert.Equal(r3.Z, 0f);
        }

        [Fact()]
        public void ToVector2Test()
        {
            var a = new Vector3(5, 4, -6);
            var b = a.ToVector2();

            Assert.True(b.X == a.X);
            Assert.True(b.Y == a.Y);
        }

        [Fact()]
        public void ToVector4Test()
        {
            var a = new Vector3(5, 4, -6);
            var b = a.ToVector4();

            Assert.True(b.X == a.X);
            Assert.True(b.Y == a.Y);
            Assert.True(b.Z == a.Z);
            Assert.True(b.W == 1);

            b = a.ToVector4(4);
            Assert.True(b.X == a.X);
            Assert.True(b.Y == a.Y);
            Assert.True(b.Z == a.Z);
            Assert.True(b.W == 4);
        }

        [Fact()]
        public void IndexedAccessTest()
        {
            var a = new Vector3(5, 4, -6);

            Assert.True(a[0] == a.X);
            Assert.True(a[1] == a.Y);
            Assert.True(a[2] == a.Z);

            a[0] = 17;
            a[1] = 16;
            a[2] = 15;

            Assert.True(a.X == 17);
            Assert.True(a.Y == 16);
            Assert.True(a.Z == 15);
        }

        [Fact()]
        public void IndexedAccessExceptionTest()
        {
            var a = new Vector3(5, 4, -6);

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                a[5] = 10;
            });

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var n = a[5];
            });
        }

        [Fact()]
        public void UnitXTest()
        {
            var a = Vector3.UnitX();

            Assert.True(a.X == 1);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);
        }

        [Fact()]
        public void UnitYTest()
        {
            var a = Vector3.UnitY();

            Assert.True(a.X == 0);
            Assert.True(a.Y == 1);
            Assert.True(a.Z == 0);
        }

        [Fact()]
        public void UnitZTest()
        {
            var a = Vector3.UnitZ();

            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 1);
        }

        [Fact()]
        public void ZeroTest()
        {
            var a = Vector3.Zero();

            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);
        }

        [Fact()]
        public void OneTest()
        {
            var a = Vector3.One();

            Assert.True(a.X == 1);
            Assert.True(a.Y == 1);
            Assert.True(a.Z == 1);
        }

        [Fact()]
        public void InfinityTest()
        {
            var a = Vector3.Infinity();

            Assert.True(a.X == double.PositiveInfinity);
            Assert.True(a.Y == double.PositiveInfinity);
            Assert.True(a.Z == double.PositiveInfinity);
        }

        [Fact()]
        public void NegativeInfinityTest()
        {
            var a = Vector3.NegativeInfinity();

            Assert.True(a.X == double.NegativeInfinity);
            Assert.True(a.Y == double.NegativeInfinity);
            Assert.True(a.Z == double.NegativeInfinity);
        }

        [Fact()]
        public void SetLengthTest()
        {
            var a = new Vector3(x, 0, 0);

            Assert.True(a.Length() == x);
            a.SetLength(y);
            Assert.True(a.Length() == y);

            a = new Vector3(0, 0, 0);
            Assert.True(a.Length() == 0);
            a.SetLength(y);
            Assert.True(a.Length() == 0);
            //A.SetLength();
            //Assert.True(isNaN(A.Length()));
        }

        [Fact()]
        public void SetScalarAddScalarSubScalarTest()
        {
            var a = new Vector3();
            var s = 3;

            a.SetScalar(s);
            Assert.StrictEqual(a.X, s);
            Assert.StrictEqual(a.Y, s);
            Assert.StrictEqual(a.Z, s);

            a.AddScalar(s);
            Assert.StrictEqual(a.X, 2 * s);
            Assert.StrictEqual(a.Y, 2 * s);
            Assert.StrictEqual(a.Z, 2 * s);

            a.SubScalar(2 * s);
            Assert.StrictEqual(a.X, 0);
            Assert.StrictEqual(a.Y, 0);
            Assert.StrictEqual(a.Z, 0);
        }

        [Fact()]
        public void SetXSetYSetZTest()
        {
            var a = new Vector3();
            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);

            a.SetX(x);
            a.SetY(y);
            a.SetZ(z);

            Assert.True(a.X == x);
            Assert.True(a.Y == y);
            Assert.True(a.Z == z);
        }


        [Fact()]
        public void LengthLengthSqTest()
        {
            var a = new Vector3(x, 0, 0);
            var b = new Vector3(0, -y, 0);
            var c = new Vector3(0, 0, z);
            var d = new Vector3();

            Assert.True(a.Length() == x);
            Assert.True(a.LengthSq() == x * x);
            Assert.True(b.Length() == y);
            Assert.True(b.LengthSq() == y * y);
            Assert.True(c.Length() == z);
            Assert.True(c.LengthSq() == z * z);
            Assert.True(d.Length() == 0);
            Assert.True(d.LengthSq() == 0);

            a.Set(x, y, z);
            Assert.True(a.Length() == Mathf.Sqrt(x * x + y * y + z * z));
            Assert.True(a.LengthSq() == (x * x + y * y + z * z));
        }


        [Fact()]
        public void SetTest()
        {
            var a = new Vector3();
            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);

            a.Set(x, y, z);
            Assert.True(a.X == x);
            Assert.True(a.Y == y);
            Assert.True(a.Z == z);
        }

        [Fact()]
        public void AddTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);

            a.Add(b);
            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);

            var c = new Vector3().AddVectors(b, b);
            Assert.True(c.X == -2 * x);
            Assert.True(c.Y == -2 * y);
            Assert.True(c.Z == -2 * z);
        }

        [Fact()]
        public void SubTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);

            a.Sub(b);
            Assert.True(a.X == 2 * x);
            Assert.True(a.Y == 2 * y);
            Assert.True(a.Z == 2 * z);

            var c = new Vector3().SubVectors(a, a);
            Assert.True(c.X == 0);
            Assert.True(c.Y == 0);
            Assert.True(c.Z == 0);
        }

        [Fact()]
        public void SetComponentGetComponentTest()
        {
            var a = new Vector3();
            Assert.True(a.X == 0);
            Assert.True(a.Y == 0);
            Assert.True(a.Z == 0);

            a.SetComponent(0, 1);
            a.SetComponent(1, 2);
            a.SetComponent(2, 3);
            Assert.True(a.GetComponent(0) == 1);
            Assert.True(a.GetComponent(1) == 2);
            Assert.True(a.GetComponent(2) == 3);
        }

        [Fact()]
        public void SetGetComponentExceptionTest()
        {
            var a = new Vector3();
            a.SetComponent(0, 1);
            a.SetComponent(1, 2);

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                a.SetComponent(5, 4);
            });

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                a.GetComponent(5);
            });
        }


        [Fact()]
        public void CopyTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3().Copy(a);
            Assert.True(b.X == x);
            Assert.True(b.Y == y);
            Assert.True(b.Z == z);

            // ensure that it is A true copy
            a.X = 0;
            a.Y = -1;
            a.Z = -2;
            Assert.True(b.X == x);
            Assert.True(b.Y == y);
            Assert.True(b.Z == z);
        }

        [Fact()]
        public void AddScaledVectorTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2, 3, 4);
            var s = 3;

            a.AddScaledVector(b, s);
            Assert.StrictEqual(a.X, x + b.X * s);
            Assert.StrictEqual(a.Y, y + b.Y * s);
            Assert.StrictEqual(a.Z, z + b.Z * s);
        }

        [Fact()]
        public void Vector3PropAccessTest()
        {
            var a = new Vector3();

            a.X = 1;
            a.Y = 2;
            a.Z = 3;

            Assert.Equal(a.X, 1);
            Assert.Equal(a.Y, 2);
            Assert.Equal(a.Z, 3);
        }

        [Fact()]
        public void SubVectorsTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2 * x, 2 * y, 2 * z);
            var c = new Vector3(4 * x, 4 * y, 4 * z);

            a.SubVectors(a, b);

            Assert.StrictEqual(a.X, -2);
            Assert.StrictEqual(a.Y, -3);
            Assert.StrictEqual(a.Z, -4);

            a = new Vector3(x, y, z);
            c.SubVectors(c, a);

            Assert.StrictEqual(c.X, 6);
            Assert.StrictEqual(c.Y, 9);
            Assert.StrictEqual(c.Z, 12);
        }

        [Fact()]
        public void SubtractVectorsTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2 * x, 2 * y, 2 * z);
            var c = new Vector3(4 * x, 4 * y, 4 * z);

            var s1 = Vector3.SubtractVectors(a, b);
            var s2 = Vector3.SubtractVectors(c, a);

            Assert.StrictEqual(s1.X, -2);
            Assert.StrictEqual(s1.Y, -3);
            Assert.StrictEqual(s1.Z, -4);

            Assert.StrictEqual(s2.X, 6);
            Assert.StrictEqual(s2.Y, 9);
            Assert.StrictEqual(s2.Z, 12);
        }

        [Fact()]
        public void MultiplyDivideTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2 * x, 2 * y, 2 * z);
            var c = new Vector3(4 * x, 4 * y, 4 * z);

            a.Multiply(b);
            Assert.StrictEqual(a.X, x * b.X);
            Assert.StrictEqual(a.Y, y * b.Y);
            Assert.StrictEqual(a.Z, z * b.Z);

            b.Divide(c);
            Assert.True(Mathf.Abs(b.X - (double)0.5) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(b.Y - (double)0.5) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(b.Z - (double)0.5) <= MathUtils.EPS);
        }

        [Fact()]
        public void MultiplyDivideTest2()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);

            a.MultiplyScalar(-2);
            Assert.True(a.X == x * -2);
            Assert.True(a.Y == y * -2);
            Assert.True(a.Z == z * -2);

            b.MultiplyScalar(-2);
            Assert.True(b.X == 2 * x);
            Assert.True(b.Y == 2 * y);
            Assert.True(b.Z == 2 * z);

            a.DivideScalar(-2);
            Assert.True(a.X == x);
            Assert.True(a.Y == y);
            Assert.True(a.Z == z);

            b.DivideScalar(-2);
            Assert.True(b.X == -x);
            Assert.True(b.Y == -y);
            Assert.True(b.Z == -z);
        }


        [Fact()]
        public void MultiplyVectorsTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2, 3, -5);

            var c = new Vector3().MultiplyVectors(a, b);
            Assert.StrictEqual(c.X, x * 2);
            Assert.StrictEqual(c.Y, y * 3);
            Assert.StrictEqual(c.Z, z * -5);
        }

        [Fact()]
        public void ApplyMatrix3Test()
        {
            var a = new Vector3(x, y, z);
            var m = new Matrix3().Set(2, 3, 5, 7, 11, 13, 17, 19, 23);

            a.ApplyMatrix3(m);
            Assert.StrictEqual(a.X, 33);
            Assert.StrictEqual(a.Y, 99);
            Assert.StrictEqual(a.Z, 183);
        }

        [Fact()]
        public void ApplyNormalMatrixTest()
        {
            var a = new Vector3(x, y, z);
            var m = new Matrix3().Set(2, 3, 5, 7, 11, 13, 17, 19, 23);

            a.ApplyNormalMatrix(m);
            Assert.True(a.X - 0.156664 <= MathUtils.EPS);
            Assert.True(a.Y - 0.469944 <= MathUtils.EPS);
            Assert.True(a.Z - 0.868685 <= MathUtils.EPS);
        }

        [Fact()]
        public void ApplyMatrix4Test()
        {
            var A = new Vector3(x, y, z);
            var B = new Vector4(x, y, z, 1);

            var m1 = new Matrix4().MakeRotationX(Mathf.PI);
            A.ApplyMatrix4(m1);
            B.ApplyMatrix4(m1);
            Assert.True(A.X == B.X / B.W);
            Assert.True(A.Y == B.Y / B.W);
            Assert.True(A.Z == B.Z / B.W);

            var m2 = new Matrix4().MakeTranslation(3, 2, 1);
            A.ApplyMatrix4(m2);
            B.ApplyMatrix4(m2);
            Assert.True(A.X == B.X / B.W);
            Assert.True(A.Y == B.Y / B.W);
            Assert.True(A.Z == B.Z / B.W);

            var m3 = new Matrix4().Set(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 1, 0
            );
            A.ApplyMatrix4(m3);
            B.ApplyMatrix4(m3);
            Assert.True(A.X == B.X / B.W);
            Assert.True(A.Y == B.Y / B.W);
            Assert.True(A.Z == B.Z / B.W);
        }

        [Fact()]
        public void ApplyQuaternionTest()
        {
            var a = new Vector3(x, y, z);

            a.ApplyQuaternion(new Quaternion());
            Assert.StrictEqual(a.X, x);
            Assert.StrictEqual(a.Y, y);
            Assert.StrictEqual(a.Z, z);

            a.ApplyQuaternion(new Quaternion(x, y, z, w));
            Assert.StrictEqual(a.X, 108);
            Assert.StrictEqual(a.Y, 162);
            Assert.StrictEqual(a.Z, 216);
        }

        [Fact()]
        public void ReflectTest()
        {
            var a = new Vector3();
            var normal = new Vector3(0, 1, 0);
            var b = new Vector3();

            a.Set(0, -1, 0);
            Assert.True(b.Copy(a).Reflect(normal).Equals(new Vector3(0, 1, 0)));

            a.Set(1, -1, 0);
            Assert.True(b.Copy(a).Reflect(normal).Equals(new Vector3(1, 1, 0)));

            a.Set(1, -1, 0);
            normal.Set(0, -1, 0);
            Assert.True(b.Copy(a).Reflect(normal).Equals(new Vector3(1, 1, 0)));
        }

        [Fact()]
        public void ProjectUnprojectTest()
        {
            //var A = new Vector3( x, y, z );
            //var camera = new PerspectiveCamera( 75, 16 / 9, (float)0.1, (float)300.0 );
            //var projected = new Vector3( (float)(-0.36653213611158914), (float)(-0.9774190296309043), (float)1.0506835611870624 );

            //A.Project( camera );
            //Assert.True( MathF.Abs( A.X - projected.X ) <= eps, "project: check x" );
            //Assert.True( MathF.Abs( A.Y - projected.Y ) <= eps, "project: check y" );
            //Assert.True( MathF.Abs( A.Z - projected.Z ) <= eps, "project: check z" );

            //A.Unproject( camera );
            //Assert.True( MathF.Abs( A.X - x ) <= eps, "unproject: check x" );
            //Assert.True( MathF.Abs( A.Y - y ) <= eps, "unproject: check y" );
            //Assert.True( MathF.Abs( A.Z - z ) <= eps, "unproject: check z" );
        }

        [Fact()]
        public void TransformDirectionTest()
        {
            var a = new Vector3(x, y, z);
            var m = new Matrix4();
            var transformed = new Vector3((double)0.3713906763541037, (double)0.5570860145311556, (double)0.7427813527082074);

            a.TransformDirection(m);
            Assert.True(Mathf.Abs(a.X - transformed.X) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Y - transformed.Y) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Z - transformed.Z) <= MathUtils.EPS);
        }

        [Fact()]
        public void MinMaxClampTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);
            var c = new Vector3();

            c.Copy(a).Min(b);
            Assert.True(c.X == -x);
            Assert.True(c.Y == -y);
            Assert.True(c.Z == -z);

            c.Copy(a).Max(b);
            Assert.True(c.X == x);
            Assert.True(c.Y == y);
            Assert.True(c.Z == z);

            c.Set(-2 * x, 2 * y, -2 * z);
            c.Clamp(b, a);
            Assert.True(c.X == -x);
            Assert.True(c.Y == y);
            Assert.True(c.Z == -z);
        }

        [Fact()]
        public void ClampLengthTest()
        {
            var a = new Vector3((double)-0.01, (double)10.5, (double)7.5);

            var clamp = a.ClampLength(1, 3);

            Assert.True(clamp.X - (-0.002324) <= MathUtils.EPS);
            Assert.True(clamp.Y - (2.441199) <= MathUtils.EPS);
            Assert.True(clamp.Z - (1.743714) <= MathUtils.EPS);
        }

        [Fact()]
        public void ClampScalarTest()
        {
            var a = new Vector3((double)-0.01, (double)0.5, (double)1.5);
            var clamped = new Vector3((double)0.1, (double)0.5, (double)1.0);

            a.ClampScalar((double)0.1, (double)1.0);
            Assert.True(Mathf.Abs(a.X - clamped.X) <= 0.001);
            Assert.True(Mathf.Abs(a.Y - clamped.Y) <= 0.001);
            Assert.True(Mathf.Abs(a.Z - clamped.Z) <= 0.001);

        }

        [Fact()]
        public void FloorTest()
        {
            var a = new Vector3(0.3f, 0.5f, 0.7f);
            var b = new Vector3(-0.9f, 2.3f, 3.6f);

            var ceil1 = a.Floor();
            var ceil2 = b.Floor();

            Assert.Equal(ceil1.X, 0f);
            Assert.Equal(ceil1.Y, 0f);
            Assert.Equal(ceil1.Z, 0f);

            Assert.Equal(ceil2.X, -1f);
            Assert.Equal(ceil2.Y, 2f);
            Assert.Equal(ceil2.Z, 3f);
        }

        [Fact()]
        public void CeilTest()
        {
            var a = new Vector3(0.3f, 0.5f, 0.7f);
            var b = new Vector3(-0.9f, 2.3f, 3.6f);

            var ceil1 = a.Ceil();
            var ceil2 = b.Ceil();

            Assert.Equal(ceil1.X, 1);
            Assert.Equal(ceil1.Y, 1);
            Assert.Equal(ceil1.Z, 1);

            Assert.Equal(ceil2.X, 0);
            Assert.Equal(ceil2.Y, 3);
            Assert.Equal(ceil2.Z, 4);

        }

        //[Fact()]
        //public void RoundTest()
        //{

        //    //NOTE: Needs more tests. It looks like rounding in javascript is different from C#

        //    //var a = new Vector3((float)-0.01, (float)0.5, (float)1.5);
        //    //a.Round();

        //    //Assert.Equal(System.Math.Round((float)0), a.X);
        //    //Assert.Equal(System.Math.Round((float)0), a.Y);
        //    //Assert.Equal(System.Math.Round((float)2), a.Z);

        //    //a.SetX((float)0.6);
        //    //a.SetY((float)-0.6);
        //    //a.Round();
        //    //Assert.Equal(System.Math.Round((float)1), a.X);
        //    //Assert.Equal(System.Math.Round((float)-1), a.Y);

        //    Assert.True(false, "This test needs an implementation");
        //}

        [Fact()]
        public void RoundToZeroTest()
        {
            var a = new Vector3(0.1f, 2f, 0.9f);

            var round = a.RoundToZero();
            
            Assert.True(round.X == 0);
            Assert.True(round.Y == 2);
            Assert.True(round.Z == 0);

            var b = new Vector3(-0.1f, -2f, -0.9f);

            round = b.RoundToZero();

            Assert.True(round.X == 0);
            Assert.True(round.Y == -2);
            Assert.True(round.Z == 0);
        }

        [Fact()]
        public void NegateTest()
        {
            var a = new Vector3(x, y, z);

            a.Negate();
            Assert.True(a.X == -x);
            Assert.True(a.Y == -y);
            Assert.True(a.Z == -z);

        }

        [Fact()]
        public void DotTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(-x, -y, -z);
            var c = new Vector3();

            var result = a.Dot(b);
            Assert.True(result == (-x * x - y * y - z * z));

            result = a.Dot(c);
            Assert.True(result == 0);
        }


        [Fact()]
        public void ManhattanLengthTest()
        {
            var a = new Vector3(x, 0, 0);
            var b = new Vector3(0, -y, 0);
            var c = new Vector3(0, 0, z);
            var d = new Vector3();

            Assert.True(a.ManhattanLength() == x, "Positive x");
            Assert.True(b.ManhattanLength() == y, "Negative y");
            Assert.True(c.ManhattanLength() == z, "Positive z");
            Assert.True(d.ManhattanLength() == 0, "Empty initialization");

            a.Set(x, y, z);
            Assert.True(a.ManhattanLength() == Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z), "All components");
        }

        [Fact()]
        public void ManhattanDistanceToTest()
        {
            var a = new Vector3(x, 0, 0);
            var b = new Vector3(0, -y, 0);
            var c = new Vector3(0, 0, z);
            var d = new Vector3();

            Assert.Equal(a.ManhattanDistanceTo(b), 5);
            Assert.Equal(a.ManhattanDistanceTo(c), 6);
            Assert.Equal(b.ManhattanDistanceTo(a), 5);
            Assert.Equal(c.ManhattanDistanceTo(b), 7);
        }

        [Fact()]
        public void NormalizeTest()
        {
            var a = new Vector3(x, 0, 0);
            var b = new Vector3(0, -y, 0);
            var c = new Vector3(0, 0, z);

            a.Normalize();
            Assert.True(a.Length() == 1);
            Assert.True(a.X == 1);

            b.Normalize();
            Assert.True(b.Length() == 1);
            Assert.True(b.Y == -1);

            c.Normalize();
            Assert.True(c.Length() == 1);
            Assert.True(c.Z == 1);

        }

        [Fact()]
        public void LerpCloneTest()
        {
            var a = new Vector3(x, 0, z);
            var b = new Vector3(0, -y, 0);

            Assert.True(a.Lerp(a, 0).Equals(a.Lerp(a, (double)0.5)));
            Assert.True(a.Lerp(a, 0).Equals(a.Lerp(a, 1)));

            Assert.True(a.Clone().Lerp(b, 0).Equals(a));

            Assert.True(a.Clone().Lerp(b, (double)0.5).X == x * 0.5);
            Assert.True(a.Clone().Lerp(b, (double)0.5).Y == -y * 0.5);
            Assert.True(a.Clone().Lerp(b, (double)0.5).Z == z * 0.5);

            Assert.True(a.Clone().Lerp(b, 1).Equals(b));
        }

        [Fact()]
        public void LerpVectorsTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2 * x, -y, (double)0.5 * z);

            var l1 = a.LerpVectors(a, b, 0.5f);
            var l2 = a.LerpVectors(b, a, 1.5f);

            Assert.True(l1.X == 3);
            Assert.True(l1.Y == 0);
            Assert.True(l1.Z == 3);

            Assert.True(l2.X == 1);
            Assert.True(l2.Y == 6);
            Assert.True(l2.Z == 5);
        }

        [Fact()]
        public void CrossTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(2 * x, -y, (double)0.5 * z);
            var crossed = new Vector3(18, 12, -18);

            a.Cross(b);
            Assert.True(Mathf.Abs(a.X - crossed.X) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Y - crossed.Y) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Z - crossed.Z) <= MathUtils.EPS);
        }

        [Fact()]
        public void CrossVectorsTest()
        {
            var a = new Vector3(x, y, z);
            var b = new Vector3(x, -y, z);
            var c = new Vector3();
            var crossed = new Vector3(24, 0, -12);

            c.CrossVectors(a, b);
            Assert.True(Mathf.Abs(c.X - crossed.X) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(c.Y - crossed.Y) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(c.Z - crossed.Z) <= MathUtils.EPS);
        }

        [Fact()]
        public void AngleToTest()
        {
            var a = new Vector3(0, (double)-0.18851655680720186, (double)0.9820700116639124);
            var b = new Vector3(0, (double)0.18851655680720186, (double)-0.9820700116639124);

            Assert.Equal(a.AngleTo(a), 0);
            Assert.Equal(a.AngleTo(b), Mathf.PI);

            var x = new Vector3(1, 0, 0);
            var y = new Vector3(0, 1, 0);
            var z = new Vector3(0, 0, 1);

            Assert.Equal(x.AngleTo(y), Mathf.PI / 2);
            Assert.Equal(x.AngleTo(z), Mathf.PI / 2);
            Assert.Equal(z.AngleTo(x), Mathf.PI / 2);

            Assert.True(Mathf.Abs(x.AngleTo(new Vector3(1, 1, 0)) - (Mathf.PI / 4)) < 0.0000001);

        }

        [Fact()]
        public void DistanceToDistanceToSqTest()
        {
            var a = new Vector3(x, 0, 0);
            var b = new Vector3(0, -y, 0);
            var c = new Vector3(0, 0, z);
            var d = new Vector3();

            Assert.True(a.DistanceTo(d) == x);
            Assert.True(a.DistanceToSquared(d) == x * x);

            Assert.True(b.DistanceTo(d) == y);
            Assert.True(b.DistanceToSquared(d) == y * y);

            Assert.True(c.DistanceTo(d) == z);
            Assert.True(c.DistanceToSquared(d) == z * z);
        }


        [Fact()]
        public void SetFromMatrixPositionTest()
        {
            var a = new Vector3();
            var m = new Matrix4().Set(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53);

            a.SetFromMatrixPosition(m);
            Assert.StrictEqual(a.X, 7);
            Assert.StrictEqual(a.Y, 19);
            Assert.StrictEqual(a.Z, 37);
        }

        [Fact()]
        public void SetFromMatrixScaleTest()
        {
            var a = new Vector3();
            var m = new Matrix4().Set(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53);
            var expected = new Vector3((double)25.573423705088842, (double)31.921779399024736, (double)35.70714214271425);

            a.SetFromMatrixScale(m);
            Assert.True(Mathf.Abs(a.X - expected.X) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Y - expected.Y) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Z - expected.Z) <= MathUtils.EPS);
        }

        [Fact()]
        public void SetFromMatrixColumnTest()
        {
            var a = new Vector3();
            var m = new Matrix4().Set(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53);

            a.SetFromMatrixColumn(m, 0);
            Assert.StrictEqual(a.X, 2);
            Assert.StrictEqual(a.Y, 11);
            Assert.StrictEqual(a.Z, 23);

            a.SetFromMatrixColumn(m, 2);
            Assert.StrictEqual(a.X, 5);
            Assert.StrictEqual(a.Y, 17);
            Assert.StrictEqual(a.Z, 31);
        }

        [Fact()]
        public void SetFromMatrix3ColumnTest()
        {
            var a = new Vector3();
            var m = new Matrix3().Set(1, 2, 3, 4, 5, 6, 7, 8, 9);

            a.SetFromMatrix3Column(m, 0);
            Assert.StrictEqual(a.X, 1);
            Assert.StrictEqual(a.Y, 4);
            Assert.StrictEqual(a.Z, 7);

            a.SetFromMatrix3Column(m, 1);
            Assert.StrictEqual(a.X, 2);
            Assert.StrictEqual(a.Y, 5);
            Assert.StrictEqual(a.Z, 8);

            a.SetFromMatrix3Column(m, 2);
            Assert.StrictEqual(a.X, 3);
            Assert.StrictEqual(a.Y, 6);
            Assert.StrictEqual(a.Z, 9);
        }

        [Fact()]
        public void EqualsTest()
        {
            var a = new Vector3(x, 0, z);
            var b = new Vector3(0, -y, 0);

            Assert.True(a.X != b.X);
            Assert.True(a.Y != b.Y);
            Assert.True(a.Z != b.Z);

            Assert.True(!a.Equals(b));
            Assert.True(!b.Equals(a));

            a.Copy(b);
            Assert.True(a.X == b.X);
            Assert.True(a.Y == b.Y);
            Assert.True(a.Z == b.Z);

            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
        }

        [Fact()]
        public void FromArrayTest()
        {
            var a = new Vector3();
            var array = new double[] { 1, 2, 3, 4, 5, 6 };

            a.FromArray(array);
            Assert.StrictEqual(a.X, 1);
            Assert.StrictEqual(a.Y, 2);
            Assert.StrictEqual(a.Z, 3);

            a.FromArray(array, 3);
            Assert.StrictEqual(a.X, 4);
            Assert.StrictEqual(a.Y, 5);
            Assert.StrictEqual(a.Z, 6);
        }

        [Fact()]
        public void ToArrayTest()
        {
            var a = new Vector3(x, y, z);

            var array = a.ToArray();
            Assert.StrictEqual(array[0], x);
            Assert.StrictEqual(array[1], y);
            Assert.StrictEqual(array[2], z);

            array = new double[] { };
            a.ToArray(ref array);
            Assert.StrictEqual(array[0], x);
            Assert.StrictEqual(array[1], y);
            Assert.StrictEqual(array[2], z);

            array = new double[] { };
            a.ToArray(ref array, 1);
            //Assert.StrictEqual(array[0], null);
            Assert.StrictEqual(array[1], x);
            Assert.StrictEqual(array[2], y);
            Assert.StrictEqual(array[3], z);

            double[] arr = null;
            a.ToArray(ref arr, 1);
            Assert.StrictEqual(array[1], x);
            Assert.StrictEqual(array[2], y);
            Assert.StrictEqual(array[3], z);

        }

        [Fact()]
        public void ProjectOnVectorTest()
        {
            var a = new Vector3(1, 0, 0);
            var b = new Vector3();
            var normal = new Vector3(10, 0, 0);

            Assert.True(b.Copy(a).ProjectOnVector(normal).Equals(new Vector3(1, 0, 0)));

            a.Set(0, 1, 0);
            Assert.True(b.Copy(a).ProjectOnVector(normal).Equals(new Vector3(0, 0, 0)));

            a.Set(0, 0, -1);
            Assert.True(b.Copy(a).ProjectOnVector(normal).Equals(new Vector3(0, 0, 0)));

            a.Set(-1, 0, 0);
            Assert.True(b.Copy(a).ProjectOnVector(normal).Equals(new Vector3(-1, 0, 0)));

            a.Set(3, 4, 5);
            Assert.True(b.Copy(a).ProjectOnVector(new Vector3(0,0,0)).Equals(new Vector3(0, 0, 0)));
        }

        [Fact()]
        public void ApplyEulerTest()
        {
            var a = new Vector3(x, y, z);
            var euler = new Euler(90, -45, 0);
            var expected = new Vector3((double)-2.352970120501014, (double)(-4.7441750936226645), (double)0.9779234597246458);

            a.ApplyEuler(euler);
            Assert.True(Mathf.Abs(a.X - expected.X) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Y - expected.Y) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Z - expected.Z) <= MathUtils.EPS);

        }

        [Fact()]
        public void ApplyAxisAngleTest()
        {
            var a = new Vector3(x, y, z);
            var axis = new Vector3(0, 1, 0);
            var angle = Mathf.PI / (double)4.0;
            var expected = new Vector3(3 * Mathf.Sqrt(2), 3, Mathf.Sqrt(2));

            a.ApplyAxisAngle(axis, angle);
            Assert.True(Mathf.Abs(a.X - expected.X) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Y - expected.Y) <= MathUtils.EPS);
            Assert.True(Mathf.Abs(a.Z - expected.Z) <= MathUtils.EPS);
        }

        [Fact()]
        public void ProjectOnPlaneTest()
        {
            var a = new Vector3(1, 0, 0);
            var b = new Vector3();
            var normal = new Vector3(1, 0, 0);

            Assert.True(b.Copy(a).ProjectOnPlane(normal).Equals(new Vector3(0, 0, 0)));

            a.Set(0, 1, 0);
            Assert.True(b.Copy(a).ProjectOnPlane(normal).Equals(new Vector3(0, 1, 0)));

            a.Set(0, 0, -1);
            Assert.True(b.Copy(a).ProjectOnPlane(normal).Equals(new Vector3(0, 0, -1)));

            a.Set(-1, 0, 0);
            Assert.True(b.Copy(a).ProjectOnPlane(normal).Equals(new Vector3(0, 0, 0)));

        }

        [Fact()]
        public void RandomTest()
        {
            var a = new Vector3();
            a.Random();
        }

        [Fact()]
        public void Vector3FromVector2Test()
        {
            var v2 = new Vector2(2, 3);
            var a = new Vector3(v2, 10);

            Assert.Equal(a.X, v2.X);
            Assert.Equal(a.Y, v2.Y);
            Assert.Equal(a.Z, 10);
        }

        [Fact()]
        public void Vector3FromScalarTest()
        {
            var a = new Vector3(5);

            Assert.Equal(a.X, 5f);
            Assert.Equal(a.Y, 5f);
            Assert.Equal(a.Z, 5f);
        }

        [Fact()]
        public void FromBufferAttributeTest()
        {
            var a = new Vector3();
            var attr = new BufferAttribute<double>(new double[] { 1, 2, 3, 4, 5, 6 }, 3);

            a.FromBufferAttribute(attr, 0);
            Assert.Equal(1, a.X);
            Assert.Equal(2, a.Y);
            Assert.Equal(3, a.Z);

            a.FromBufferAttribute(attr, 1);
            Assert.Equal(4, a.X);
            Assert.Equal(5, a.Y);
            Assert.Equal(6, a.Z);
        }

    }
}