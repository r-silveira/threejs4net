using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Extras.Core
{
    public class EllipseCurve : Curve<Vector3>
    {
        public float aX;
        public float aY;
        public float xRadius;
        public float yRadius;
        public float aStartAngle;
        public float aEndAngle;
        public bool aClockwise;
        public float aRotation;

        public EllipseCurve(float? aX = null, float? aY = null, float? xRadius = null, float? yRadius = null, float? aStartAngle = null, float? aEndAngle = null, bool? clockwise = null, float? rotation = null) : base()
        {
            this.aX = aX != null ? aX.Value : 0;
            this.aY = aY != null ? aY.Value : 0;

            this.xRadius = xRadius != null ? xRadius.Value : 1;
            this.yRadius = yRadius != null ? yRadius.Value : 1;

            this.aStartAngle = aStartAngle != null ? aStartAngle.Value : 0;
            this.aEndAngle = aEndAngle != null ? aEndAngle.Value : (float)(2 * System.Math.PI);

            this.aClockwise = clockwise != null ? clockwise.Value : false;

            this.aRotation = rotation != null ? rotation.Value : 0;
        }

        public override Vector3 GetPoint(float t, Vector3 optionalTarget = null)
        {
            Vector3 point = optionalTarget ?? new Vector3();

            var twoPi = Mathf.PI * 2;
            var deltaAngle = this.aEndAngle - this.aStartAngle;
            var samePoints = Mathf.Abs(deltaAngle) < MathUtils.EPS5;

            // ensures that deltaAngle is 0 .. 2 PI
            while (deltaAngle < 0) deltaAngle += twoPi;
            while (deltaAngle > twoPi) deltaAngle -= twoPi;

            if (deltaAngle < MathUtils.EPS5)
            {
                deltaAngle = samePoints ? 0 : twoPi;
            }

            if (this.aClockwise && !samePoints)
            {
                deltaAngle = deltaAngle == twoPi ? -twoPi : deltaAngle - twoPi;
            }

            var angle = this.aStartAngle + t * deltaAngle;
            var x = this.aX + this.xRadius * Mathf.Cos(angle);
            var y = this.aY + this.yRadius * Mathf.Sin(angle);

            if (this.aRotation != 0)
            {
                var cos = Mathf.Cos(this.aRotation);
                var sin = Mathf.Sin(this.aRotation);

                var tx = x - this.aX;
                var ty = y - this.aY;

                // Rotate the point about the center of the ellipse.
                x = tx * cos - ty * sin + this.aX;
                y = tx * sin + ty * cos + this.aY;
            }
            return point.Set(x, y, 0);
        }

        public Curve<Vector3> Copy(EllipseCurve source)
        {
            base.Copy(source);
            this.aX = source.aX;
            this.aY = source.aY;
            this.xRadius = source.xRadius;
            this.yRadius = source.yRadius;
            this.aStartAngle = source.aStartAngle;
            this.aEndAngle = source.aEndAngle;
            this.aClockwise = source.aClockwise;
            this.aRotation = source.aRotation;
            return this;
        }
    }
}
