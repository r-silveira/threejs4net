using System.Collections.Generic;
using System.Linq;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Extras.Core
{
    public class SplineCurve : Curve<Vector3>
    {
        public List<Vector3> points;

        public SplineCurve(IEnumerable<Vector3> points)
        {
            this.points = points.ToList() ?? new List<Vector3>();
        }

        public override Vector3 GetPoint(float t, Vector3 optionalTarget)
        {
            Vector3 point = optionalTarget ?? new Vector3();

            var lPoints = this.points;
            var p = (lPoints.Count - 1) * t;

            var intPoint = Mathf.Floor(p);
            var weight = p - intPoint;

            var p0 = lPoints[intPoint == 0 ? intPoint : intPoint - 1];
            var p1 = lPoints[intPoint];
            var p2 = lPoints[intPoint > lPoints.Count - 2 ? lPoints.Count - 1 : intPoint + 1];
            var p3 = lPoints[intPoint > lPoints.Count - 3 ? lPoints.Count - 1 : intPoint + 2];

            point.Set(
                Interpolation.CatmullRom(weight, p0.X, p1.X, p2.X, p3.X),
                Interpolation.CatmullRom(weight, p0.Y, p1.Y, p2.Y, p3.Y),
                0
            );

            return point;
        }

        public Curve<Vector3> Copy(SplineCurve source)
        {
            base.Copy(source);
            this.points = new List<Vector3>();
            for (var i = 0; i < source.points.Count; i++)
            {
                var point = source.points[i];
                this.points.Add(point.Clone());
            }
            return this;
        }
    }
}
