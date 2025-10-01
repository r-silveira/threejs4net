﻿using ThreeJs4Net.Math;
using Xunit;

namespace ThreeJs4Net.Tests.Math
{
    public class PlaneTests : BaseTests
    {

        private bool comparePlane(Plane a, Plane b, double threshold = 0.0001)
        {
            return (a.Normal.DistanceTo(b.Normal) < threshold &&
                     Mathf.Abs(a.Constant - b.Constant) < threshold);
        }

        [Fact()]
        public void InstancingTest()
        {
            var a = new Plane();
            Assert.True(a.Normal.X == 1);
            Assert.True(a.Normal.Y == 0);
            Assert.True(a.Normal.Z == 0);
            Assert.True(a.Constant == 0);

            a = new Plane(one3.Clone(), 0);
            Assert.True(a.Normal.X == 1);
            Assert.True(a.Normal.Y == 1);
            Assert.True(a.Normal.Z == 1);
            Assert.True(a.Constant == 0);

            a = new Plane(one3.Clone(), 1);
            Assert.True(a.Normal.X == 1);
            Assert.True(a.Normal.Y == 1);
            Assert.True(a.Normal.Z == 1);
            Assert.True(a.Constant == 1);

        }

        [Fact()]
        public void CloneTest()
        {
            var a = new Plane(new Vector3(2.0, 0.5, 0.25));
            var b = a.Clone();

            Assert.True(a.Equals(b), "clones are equal");

        }

        [Fact()]
        public void CopyTest()
        {
            var a = new Plane(new Vector3(x, y, z), w);
            var b = new Plane().Copy(a);
            Assert.Equal(x, b.Normal.X);
            Assert.Equal(y, b.Normal.Y);
            Assert.Equal(z, b.Normal.Z);
            Assert.Equal(w, b.Constant);

            // ensure that it is a true copy
            a.Normal.X = 0;
            a.Normal.Y = -1;
            a.Normal.Z = -2;
            a.Constant = -3;
            Assert.Equal(x, b.Normal.X);
            Assert.Equal(y, b.Normal.Y);
            Assert.Equal(z, b.Normal.Z);
            Assert.Equal(w, b.Constant);
        }

        [Fact()]
        public void SetTest()
        {
            var a = new Plane();
            Assert.True(a.Normal.X == 1);
            Assert.True(a.Normal.Y == 0);
            Assert.True(a.Normal.Z == 0);
            Assert.True(a.Constant == 0);

            var b = a.Clone().Set(new Vector3(x, y, z), w);
            Assert.True(b.Normal.X == x);
            Assert.True(b.Normal.Y == y);
            Assert.True(b.Normal.Z == z);
            Assert.True(b.Constant == w);
        }

        [Fact()]
        public void SetComponentsTest()
        {
            var a = new Plane();
            Assert.True(a.Normal.X == 1);
            Assert.True(a.Normal.Y == 0);
            Assert.True(a.Normal.Z == 0);
            Assert.True(a.Constant == 0);

            var b = a.Clone().SetComponents(x, y, z, w);
            Assert.True(b.Normal.X == x);
            Assert.True(b.Normal.Y == y);
            Assert.True(b.Normal.Z == z);
            Assert.True(b.Constant == w);
        }

        [Fact()]
        public void SetFromNormalAndCoplanarPointTest()
        {
            var normal = one3.Clone().Normalize();
            var a = new Plane().SetFromNormalAndCoplanarPoint(normal, zero3);

            Assert.True(a.Normal.Equals(normal));
            Assert.True(a.Constant == 0);
        }

        [Fact()]
        public void SetFromCoplanarPointsTest()
        {
            var a = new Plane();
            var v1 = new Vector3(2.0, 0.5, 0.25);
            var v2 = new Vector3(2.0, -0.5, 1.25);
            var v3 = new Vector3(2.0, -3.5, 2.2);
            var normal = new Vector3(1, 0, 0);
            var constant = -2;

            a.SetFromCoplanarPoints(v1, v2, v3);

            Assert.True(a.Normal.Equals(normal), "Check normal");
            Assert.Equal(constant, a.Constant);
        }

        [Fact()]
        public void NormalizeTest()
        {
            var a = new Plane(new Vector3(2, 0, 0), 2);

            a.Normalize();
            Assert.True(a.Normal.Length() == 1);
            Assert.True(a.Normal.Equals(new Vector3(1, 0, 0)));
            Assert.True(a.Constant == 1);

        }

        [Fact()]
        public void NegateTest()
        {
            var a = new Plane(new Vector3(2, 0, 0), -2);

            a.Normalize();
            Assert.True(a.DistanceToPoint(new Vector3(4, 0, 0)) == 3);
            Assert.True(a.DistanceToPoint(new Vector3(1, 0, 0)) == 0);

            a.Negate();
            Assert.True(a.DistanceToPoint(new Vector3(4, 0, 0)) == -3);
            Assert.True(a.DistanceToPoint(new Vector3(1, 0, 0)) == 0);

        }

        [Fact()]
        public void DistanceToPointTest()
        {
            var a = new Plane(new Vector3(2, 0, 0), -2);
            var point = new Vector3();

            a.Normalize().ProjectPoint(zero3.Clone(), point);
            Assert.True(a.DistanceToPoint(point) == 0);
            Assert.True(a.DistanceToPoint(new Vector3(4, 0, 0)) == 3);
        }

        [Fact()]
        public void DistanceToSphereTest()
        {
            var a = new Plane(new Vector3(1, 0, 0), 0);

            var b = new Sphere(new Vector3(2, 0, 0), 1);

            Assert.True(a.DistanceToSphere(b) == 1);

            a.Set(new Vector3(1, 0, 0), 2);
            Assert.True(a.DistanceToSphere(b) == 3);
            a.Set(new Vector3(1, 0, 0), -2);
            Assert.True(a.DistanceToSphere(b) == -1);

        }

        [Fact()]
        public void ProjectPointTest()
        {
            var a = new Plane(new Vector3(1, 0, 0), 0);
            var point = new Vector3();

            a.ProjectPoint(new Vector3(10, 0, 0), point);
            Assert.True(point.Equals(zero3));
            a.ProjectPoint(new Vector3(-10, 0, 0), point);
            Assert.True(point.Equals(zero3));

            a = new Plane(new Vector3(0, 1, 0), -1);
            a.ProjectPoint(new Vector3(0, 0, 0), point);
            Assert.True(point.Equals(new Vector3(0, 1, 0)));
            a.ProjectPoint(new Vector3(0, 1, 0), point);
            Assert.True(point.Equals(new Vector3(0, 1, 0)));
        }

        [Fact()]
        public void IntersectLineTest()
        {
            var a = new Plane(new Vector3(1, 0, 0), 0);
            var point = new Vector3();

            var l1 = new Line3(new Vector3(-10, 0, 0), new Vector3(10, 0, 0));
            a.IntersectLine(l1, point);
            Assert.True(point.Equals(new Vector3(0, 0, 0)));

            a = new Plane(new Vector3(1, 0, 0), -3);
            a.IntersectLine(l1, point);
            Assert.True(point.Equals(new Vector3(3, 0, 0)));

        }

        [Fact()]
        public void IntersectsBoxTest()
        {
            var a = new Box3(zero3.Clone(), one3.Clone());
            var b = new Plane(new Vector3(0, 1, 0), 1);
            var c = new Plane(new Vector3(0, 1, 0), 1.25);
            var d = new Plane(new Vector3(0, -1, 0), 1.25);
            var e = new Plane(new Vector3(0, 1, 0), 0.25);
            var f = new Plane(new Vector3(0, 1, 0), -0.25);
            var g = new Plane(new Vector3(0, 1, 0), -0.75);
            var h = new Plane(new Vector3(0, 1, 0), -1);
            var i = new Plane(new Vector3(1, 1, 1).Normalize(), -1.732);
            var j = new Plane(new Vector3(1, 1, 1).Normalize(), -1.733);

            Assert.True(!b.IntersectsBox(a));
            Assert.True(!c.IntersectsBox(a));
            Assert.True(!d.IntersectsBox(a));
            Assert.True(!e.IntersectsBox(a));
            Assert.True(f.IntersectsBox(a));
            Assert.True(g.IntersectsBox(a));
            Assert.True(h.IntersectsBox(a));
            Assert.True(i.IntersectsBox(a));
            Assert.True(!j.IntersectsBox(a));
        }

        [Fact()]
        public void IntersectsSphereTest()
        {
            var a = new Sphere(zero3.Clone(), 1);
            var b = new Plane(new Vector3(0, 1, 0), 1);
            var c = new Plane(new Vector3(0, 1, 0), 1.25);
            var d = new Plane(new Vector3(0, -1, 0), 1.25);

            Assert.True(b.IntersectsSphere(a));
            Assert.True(!c.IntersectsSphere(a));
            Assert.True(!d.IntersectsSphere(a));

        }

        [Fact()]
        public void CoplanarPointTest()
        {
            var point = new Vector3();

            var a = new Plane(new Vector3(1, 0, 0), 0);
            a.CoplanarPoint(point);
            Assert.True(a.DistanceToPoint(point) == 0);

            a = new Plane(new Vector3(0, 1, 0), -1);
            a.CoplanarPoint(point);
            Assert.True(a.DistanceToPoint(point) == 0);

        }

        [Fact()]
        public void ApplyMatrix4Test()
        {
            var a = new Plane(new Vector3(1, 0, 0), 0);

            var m = new Matrix4();
            m.MakeRotationZ(Mathf.PI * 0.5);

            Assert.True(comparePlane(a.Clone().ApplyMatrix4(m), new Plane(new Vector3(0, 1, 0), 0)));

            a = new Plane(new Vector3(0, 1, 0), -1);
            Assert.True(comparePlane(a.Clone().ApplyMatrix4(m), new Plane(new Vector3(-1, 0, 0), -1)));

            m.MakeTranslation(1, 1, 1);
            Assert.True(comparePlane(a.Clone().ApplyMatrix4(m), a.Clone().Translate(new Vector3(1, 1, 1))));
        }

        [Fact()]
        public void EqualsTest()
        {
            var a = new Plane(new Vector3(1, 0, 0), 0);
            var b = new Plane(new Vector3(1, 0, 0), 1);
            var c = new Plane(new Vector3(0, 1, 0), 0);

            Assert.True(a.Normal.Equals(b.Normal), "Normals: equal");
            Assert.False(a.Normal.Equals(c.Normal), "Normals: not equal");

            Assert.NotEqual(a.Constant, b.Constant);
            Assert.Equal(a.Constant, c.Constant);

            Assert.False(a.Equals(b), "Planes: not equal");
            Assert.False(a.Equals(c), "Planes: not equal");

            a.Copy(b);
            Assert.True(a.Normal.Equals(b.Normal));
            Assert.Equal(a.Constant, b.Constant);
            Assert.True(a.Equals(b));

        }
    }
}