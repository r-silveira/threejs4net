﻿using ThreeJs4Net.Geometries;
using ThreeJs4Net.Math;
using ThreeJs4Net.Objects;
using Xunit;

namespace ThreeJs4Net.Tests.Math
{
    public class FrustumTests : BaseTests
    {
        [Fact()]
        public void InstancingTest()
        {
            var a = new Frustum();

            Assert.NotNull(a.Planes);
            Assert.Equal(6, a.Planes.Length);

            var pDefault = new Plane();
            for (var i = 0; i < 6; i++)
            {

                Assert.True(a.Planes[i].Equals(pDefault));

            }

            var p0 = new Plane(unit3, -1);
            var p1 = new Plane(unit3, 1);
            var p2 = new Plane(unit3, 2);
            var p3 = new Plane(unit3, 3);
            var p4 = new Plane(unit3, 4);
            var p5 = new Plane(unit3, 5);

            a = new Frustum(p0, p1, p2, p3, p4, p5);
            Assert.True(a.Planes[0].Equals(p0));
            Assert.True(a.Planes[1].Equals(p1));
            Assert.True(a.Planes[2].Equals(p2));
            Assert.True(a.Planes[3].Equals(p3));
            Assert.True(a.Planes[4].Equals(p4));
            Assert.True(a.Planes[5].Equals(p5));
        }

        [Fact()]
        public void SetTest()
        {
            var a = new Frustum();
            var p0 = new Plane(unit3, -1);
            var p1 = new Plane(unit3, 1);
            var p2 = new Plane(unit3, 2);
            var p3 = new Plane(unit3, 3);
            var p4 = new Plane(unit3, 4);
            var p5 = new Plane(unit3, 5);

            a.Set(p0, p1, p2, p3, p4, p5);

            Assert.True(a.Planes[0].Equals(p0), "Check plane #0");
            Assert.True(a.Planes[1].Equals(p1), "Check plane #1");
            Assert.True(a.Planes[2].Equals(p2), "Check plane #2");
            Assert.True(a.Planes[3].Equals(p3), "Check plane #3");
            Assert.True(a.Planes[4].Equals(p4), "Check plane #4");
            Assert.True(a.Planes[5].Equals(p5), "Check plane #5");
        }

        [Fact()]
        public void CloneTest()
        {
            var p0 = new Plane(unit3, -1);
            var p1 = new Plane(unit3, 1);
            var p2 = new Plane(unit3, 2);
            var p3 = new Plane(unit3, 3);
            var p4 = new Plane(unit3, 4);
            var p5 = new Plane(unit3, 5);

            var b = new Frustum(p0, p1, p2, p3, p4, p5);
            var a = b.Clone();
            Assert.True(a.Planes[0].Equals(p0));
            Assert.True(a.Planes[1].Equals(p1));
            Assert.True(a.Planes[2].Equals(p2));
            Assert.True(a.Planes[3].Equals(p3));
            Assert.True(a.Planes[4].Equals(p4));
            Assert.True(a.Planes[5].Equals(p5));

            // ensure it is a true copy by modifying source
            a.Planes[0].Copy(p1);
            Assert.True(b.Planes[0].Equals(p0));
        }

        [Fact()]
        public void CopyTest()
        {

            var p0 = new Plane(unit3, -1);
            var p1 = new Plane(unit3, 1);
            var p2 = new Plane(unit3, 2);
            var p3 = new Plane(unit3, 3);
            var p4 = new Plane(unit3, 4);
            var p5 = new Plane(unit3, 5);

            var b = new Frustum(p0, p1, p2, p3, p4, p5);
            var a = new Frustum().Copy(b);
            Assert.True(a.Planes[0].Equals(p0));
            Assert.True(a.Planes[1].Equals(p1));
            Assert.True(a.Planes[2].Equals(p2));
            Assert.True(a.Planes[3].Equals(p3));
            Assert.True(a.Planes[4].Equals(p4));
            Assert.True(a.Planes[5].Equals(p5));

            // ensure it is a true copy by modifying source
            b.Planes[0] = p1;
            Assert.True(a.Planes[0].Equals(p0));

        }

        [Fact()]
        public void SetFromProjectionMatrixOrthogonalTest()
        {
            var m = new Matrix4().MakeOrthographic(-1, 1, -1, 1, 1, 100);
            var a = new Frustum().SetFromProjectionMatrix(m);

            Assert.True(!a.ContainsPoint(new Vector3(0, 0, 0)));
            Assert.True(a.ContainsPoint(new Vector3(0, 0, -50)));
            Assert.True(a.ContainsPoint(new Vector3(0, 0, (double)-1.001)));
            Assert.True(a.ContainsPoint(new Vector3(-1, -1, (double)-1.001)));
            Assert.True(!a.ContainsPoint(new Vector3((double)-1.1, (double)-1.1, (double)-1.001)));
            Assert.True(a.ContainsPoint(new Vector3(1, 1, (double)-1.001)));
            Assert.True(!a.ContainsPoint(new Vector3((double)1.1, (double)1.1, (double)-1.001)));
            Assert.True(a.ContainsPoint(new Vector3(0, 0, -100)));
            Assert.True(a.ContainsPoint(new Vector3(-1, -1, -100)));
            Assert.True(!a.ContainsPoint(new Vector3((double)-1.1, (double)-1.1, (double)-100.1)));
            Assert.True(a.ContainsPoint(new Vector3(1, 1, -100)));
            Assert.True(!a.ContainsPoint(new Vector3((double)1.1, (double)1.1, (double)-100.1)));
            Assert.True(!a.ContainsPoint(new Vector3(0, 0, -101)));
        }

        [Fact()]
        public void SetFromProjectionMatrixPerspectiveTest()
        {
            var m = new Matrix4().MakePerspective(-1, 1, 1, -1, 1, 100);
            var a = new Frustum().SetFromProjectionMatrix(m);

            Assert.True(!a.ContainsPoint(new Vector3(0, 0, 0)));
            Assert.True(a.ContainsPoint(new Vector3(0, 0, -50)));
            Assert.True(a.ContainsPoint(new Vector3(0, 0, (double)-1.001)));
            Assert.True(a.ContainsPoint(new Vector3(-1, -1, (double)-1.001)));
            Assert.True(!a.ContainsPoint(new Vector3((double)-1.1, (double)-1.1, (double)-1.001)));
            Assert.True(a.ContainsPoint(new Vector3(1, 1, (double)-1.001)));
            Assert.True(!a.ContainsPoint(new Vector3((double)1.1, (double)1.1, (double)-1.001)));
            Assert.True(a.ContainsPoint(new Vector3(0, 0, (double)-99.999)));
            Assert.True(a.ContainsPoint(new Vector3((double)-99.999, (double)-99.999, (double)-99.999)));
            Assert.True(!a.ContainsPoint(new Vector3((double)-100.1, (double)-100.1, (double)-100.1)));
            Assert.True(a.ContainsPoint(new Vector3((double)99.999, (double)99.999, (double)-99.999)));
            Assert.True(!a.ContainsPoint(new Vector3((double)100.1, (double)100.1, (double)-100.1)));
            Assert.True(!a.ContainsPoint(new Vector3(0, 0, -101)));
        }

        [Fact()]
        public void IntersectsObjectTest()
        {
            var m = new Matrix4().MakePerspective(-1, 1, 1, -1, 1, 100);
            var a = new Frustum().SetFromProjectionMatrix(m);
            var object3d = new Mesh(new BoxGeometry(1, 1, 1));

            bool intersects = a.IntersectsObject(object3d);
            Assert.False(intersects);

            object3d.Position.Set(-1, -1, -1);
            object3d.UpdateMatrixWorld();

            intersects = a.IntersectsObject(object3d);
            Assert.True(intersects, "Successful intersection");

            object3d.Position.Set(1, 1, 1);
            object3d.UpdateMatrixWorld();

            intersects = a.IntersectsObject(object3d);
            Assert.False(intersects);
        }

        [Fact()]
        public void IntersectsObjectTest2()
        {
            var m = new Matrix4().MakePerspective(-1, 1, 1, -1, 1, 100);
            var a = new Frustum().SetFromProjectionMatrix(m);

            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3(0, 0, 0), 0)));
            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3(0, 0, 0), (double)0.9)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(0, 0, 0), (double)1.1)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(0, 0, -50), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(0, 0, (double)-1.001), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(-1, -1, (double)-1.001), 0)));
            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3((double)-1.1, (double)-1.1, (double)-1.001), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3((double)-1.1, (double)-1.1, (double)-1.001), (double)0.5)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(1, 1, (double)-1.001), 0)));
            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3((double)1.1, (double)1.1, (double)-1.001), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3((double)1.1, (double)1.1, (double)-1.001), (double)0.5)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(0, 0, (double)-99.999), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3((double)-99.999, (double)-99.999, (double)-99.999), 0)));
            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3((double)-100.1, (double)-100.1, (double)-100.1), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3((double)-100.1, (double)-100.1, (double)-100.1), (double)0.5)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3((double)99.999, (double)99.999, (double)-99.999), 0)));
            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3((double)100.1, (double)100.1, (double)-100.1), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3((double)100.1, (double)100.1, (double)-100.1), (double)0.2)));
            Assert.True(!a.IntersectsSphere(new Sphere(new Vector3(0, 0, -101), 0)));
            Assert.True(a.IntersectsSphere(new Sphere(new Vector3(0, 0, -101), (double)1.1)));
        }

        [Fact()]
        public void IntersectsBoxTest()
        {
            var m = new Matrix4().MakePerspective(-1, 1, 1, -1, 1, 100);
            var a = new Frustum().SetFromProjectionMatrix(m);
            var box = new Box3(zero3.Clone(), one3.Clone());
            bool intersects = a.IntersectsBox(box);
            Assert.False(intersects);

            // add eps so that we prevent box touching the frustum, which might intersect depending on floating point numerics
            box.Translate(new Vector3(-1 - MathUtils.EPS5, -1 - MathUtils.EPS5, -1 - MathUtils.EPS5));

            intersects = a.IntersectsBox(box);
            Assert.True(intersects, "Successful intersection");
        }
    }
}