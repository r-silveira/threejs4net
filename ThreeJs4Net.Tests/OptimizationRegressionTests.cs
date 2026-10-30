using System.Collections.Generic;
using ThreeJs4Net;
using ThreeJs4Net.Core;
using ThreeJs4Net.Materials;
using ThreeJs4Net.Math;
using ThreeJs4Net.Objects;
using Xunit;

namespace ThreeJs4Net.Tests
{
    public class OptimizationRegressionTests
    {
        [Fact]
        public void IntersectTriangleReusesTargetAndDoesNotChangeItOnMiss()
        {
            var ray = new Ray(new Vector3(0.25f, 0.25f, 1), new Vector3(0, 0, -1));
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(1, 0, 0);
            var c = new Vector3(0, 1, 0);
            var target = new Vector3(7, 8, 9);

            Assert.Same(target, ray.IntersectTriangle(a, b, c, true, target));
            Assert.True(target.DistanceTo(new Vector3(0.25f, 0.25f, 0)) < 0.00001f);

            target.Set(7, 8, 9);
            Assert.Null(ray.IntersectTriangle(c, b, a, true, target));
            Assert.Equal(7f, target.X);
            Assert.Equal(8f, target.Y);
            Assert.Equal(9f, target.Z);
        }

        [Fact]
        public void RayBoxAndSpherePredicatesHaveExpectedBoundaryBehavior()
        {
            var box = new Box3(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
            var toward = new Ray(new Vector3(0, 0, 3), new Vector3(0, 0, -1));
            var away = new Ray(new Vector3(0, 0, 3), new Vector3(0, 0, 1));
            Assert.True(toward.IntersectsBox(box));
            Assert.False(away.IntersectsBox(box));
            Assert.True(box.IntersectsSphere(new Sphere(new Vector3(2, 0, 0), 1)));
            Assert.False(box.IntersectsSphere(new Sphere(new Vector3(3, 0, 0), 1)));
        }

        [Fact]
        public void TriangleBarycentricsAndClosestPointStayConsistent()
        {
            var a = new Vector3(0, 0, 0);
            var b = new Vector3(1, 0, 0);
            var c = new Vector3(0, 1, 0);
            var tri = new Triangle(a, b, c);
            var inside = new Vector3(0.25f, 0.25f, 0);
            var bary = tri.GetBarycoord(inside, new Vector3());
            Assert.True(bary.DistanceTo(new Vector3(0.5f, 0.25f, 0.25f)) < 0.00001f);
            Assert.True(tri.ContainsPoint(inside));
            var uv = tri.GetUV(inside, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2());
            Assert.Equal(0.25f, uv.X);
            Assert.Equal(0.25f, uv.Y);
            Assert.True(tri.ClosestPointToPoint(new Vector3(2, 0, 0), new Vector3())
                .DistanceTo(b) < 0.00001f);
        }

        [Fact]
        public void BufferGeometryRaycastKeepsHitPointsIndependent()
        {
            var geometry = new BufferGeometry();
            geometry.SetAttribute("position", new BufferAttribute<float>(new float[]
            {
                0,0,0, 1,0,0, 0,1,0,
                0,0,-2, 1,0,-2, 0,1,-2
            }, 3));
            var mesh = new Mesh(geometry, new MeshBasicMaterial { Side = Three.DoubleSide });
            var raycaster = new Raycaster(new Vector3(0.25f, 0.25f, 3), new Vector3(0, 0, -1));
            var hits = new List<Intersect>();
            mesh.Raycast(raycaster, hits);
            Assert.Equal(2, hits.Count);
            Assert.NotSame(hits[0].Point, hits[1].Point);
            Assert.Equal(0f, hits[0].Point.Z);
            Assert.Equal(-2f, hits[1].Point.Z);
        }

        private static IEnumerable<Vector3> OneShotVectors()
        {
            yield return new Vector3(1, 2, 3);
            yield return new Vector3(4, 5, 6);
        }

        [Fact]
        public void CopyVectorsHandlesSinglePassEnumerables()
        {
            var attribute = new BufferAttribute<float>(new float[0], 3);
            attribute.CopyVector3sArray(OneShotVectors());
            Assert.Equal(new float[] { 1, 2, 3, 4, 5, 6 }, attribute.Array);
            var geometry = new BufferGeometry().SetFromPoints(OneShotVectors());
            Assert.Equal(new float[] { 1, 2, 3, 4, 5, 6 }, geometry.GetAttribute<float>("position").Array);
        }
    }
}
