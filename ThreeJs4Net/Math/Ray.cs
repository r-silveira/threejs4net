using System;

namespace ThreeJs4Net.Math
{
    public class Ray : IEquatable<Ray>
    {
        public Vector3 Origin = new Vector3();

        public Vector3 Direction = new Vector3(0, 0, -1);

        /// <summary>
        /// 
        /// </summary>
        public Ray()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="direction"></param>
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.Origin.Copy(origin);
            this.Direction.Copy(direction);
        }












        #region --- Already in R116 ---
        public Ray ApplyMatrix4(Matrix4 matrix4)
        {
            this.Origin.ApplyMatrix4(matrix4);
            this.Direction.TransformDirection(matrix4);

            return this;
        }

        public Vector3 At(float t, Vector3 target)
        {
            return target.Copy(this.Direction).MultiplyScalar(t).Add(this.Origin);
        }

        public Ray Clone()
        {
            return new Ray().Copy(this);
        }

        public Ray Copy(Ray another)
        {
            this.Origin.Copy(another.Origin);
            this.Direction.Copy(another.Direction);

            return this;
        }

        public Vector3 ClosestPointToPoint(Vector3 point, Vector3 target)
        {
            target.SubVectors(point, this.Origin);
            var directionDistance = target.Dot(this.Direction);
            if (directionDistance < 0)
            {
                return target.Copy(this.Origin);
            }
            return target.Copy(this.Direction).MultiplyScalar(directionDistance).Add(this.Origin);
        }

        public float DistanceToPoint(Vector3 point)
        {
            return Mathf.Sqrt(this.DistanceSqToPoint(point));
        }

        public float DistanceSqToPoint(Vector3 point)
        {
            // Keep the original projection and squared-distance operations without a temporary Vector3.
            var dx = point.X - this.Origin.X;
            var dy = point.Y - this.Origin.Y;
            var dz = point.Z - this.Origin.Z;
            var directionDistance = dx * this.Direction.X + dy * this.Direction.Y + dz * this.Direction.Z;

            if (directionDistance < 0)
            {
                return this.Origin.DistanceToSquared(point);
            }

            var x = this.Direction.X * directionDistance + this.Origin.X - point.X;
            var y = this.Direction.Y * directionDistance + this.Origin.Y - point.Y;
            var z = this.Direction.Z * directionDistance + this.Origin.Z - point.Z;
            return x * x + y * y + z * z;
        }

        public float DistanceSqToSegment(Vector3 v0, Vector3 v1, Vector3 optionalPointOnRay = null, Vector3 optionalPointOnSegment = null)
        {
            // from http://www.geometrictools.com/LibMathematics/Distance/Wm5DistRay3Segment3.cpp
            // It returns the min distance between the ray and the segment
            // defined by v0 and v1
            // It can also Set two optional targets :
            // - The closest point on the ray
            // - The closest point on the segment

            // Scalar form of the existing geometric-tools algorithm. No temporary vectors,
            // and the optional output vectors are written only after the calculation.
            var centerX = (v0.X + v1.X) * 0.5f;
            var centerY = (v0.Y + v1.Y) * 0.5f;
            var centerZ = (v0.Z + v1.Z) * 0.5f;
            var segX = v1.X - v0.X;
            var segY = v1.Y - v0.Y;
            var segZ = v1.Z - v0.Z;
            var segLength = Mathf.Sqrt(segX * segX + segY * segY + segZ * segZ);
            var invLength = 1 / (segLength == 0 ? 1 : segLength);
            segX *= invLength;
            segY *= invLength;
            segZ *= invLength;
            var diffX = this.Origin.X - centerX;
            var diffY = this.Origin.Y - centerY;
            var diffZ = this.Origin.Z - centerZ;

            var segExtent = v0.DistanceTo(v1) * 0.5f;
            var a01 = -(this.Direction.X * segX + this.Direction.Y * segY + this.Direction.Z * segZ);
            var b0 = diffX * this.Direction.X + diffY * this.Direction.Y + diffZ * this.Direction.Z;
            var b1 = -(diffX * segX + diffY * segY + diffZ * segZ);
            var c = diffX * diffX + diffY * diffY + diffZ * diffZ;
            var det = System.Math.Abs(1 - a01 * a01);

            float s0, s1, sqrDist;

            if (det >= 0)
            {
                // The ray and segment are not parallel.

                s0 = a01 * b1 - b0;
                s1 = a01 * b0 - b1;
                var extDet = segExtent * det;

                if (s0 >= 0)
                {
                    if (s1 >= -extDet)
                    {
                        if (s1 <= extDet)
                        {
                            // region 0
                            // Minimum at interior points of ray and segment.
                            var invDet = 1 / det;
                            s0 *= invDet;
                            s1 *= invDet;
                            sqrDist = s0 * (s0 + a01 * s1 + 2 * b0) + s1 * (a01 * s0 + s1 + 2 * b1) + c;
                        }
                        else
                        {
                            // region 1
                            s1 = segExtent;
                            s0 = System.Math.Max(0, -(a01 * s1 + b0));
                            sqrDist = -s0 * s0 + s1 * (s1 + 2 * b1) + c;
                        }
                    }
                    else
                    {
                        // region 5
                        s1 = -segExtent;
                        s0 = System.Math.Max(0, -(a01 * s1 + b0));
                        sqrDist = -s0 * s0 + s1 * (s1 + 2 * b1) + c;
                    }
                }
                else
                {
                    if (s1 <= -extDet)
                    {
                        // region 4
                        s0 = System.Math.Max(0, -(-a01 * segExtent + b0));
                        s1 = (s0 > 0) ? -segExtent : System.Math.Min(System.Math.Max(-segExtent, -b1), segExtent);
                        sqrDist = -s0 * s0 + s1 * (s1 + 2 * b1) + c;
                    }
                    else if (s1 <= extDet)
                    {
                        // region 3
                        s0 = 0;
                        s1 = System.Math.Min(System.Math.Max(-segExtent, -b1), segExtent);
                        sqrDist = s1 * (s1 + 2 * b1) + c;
                    }
                    else
                    {
                        // region 2
                        s0 = System.Math.Max(0, -(a01 * segExtent + b0));
                        s1 = (s0 > 0) ? segExtent : System.Math.Min(System.Math.Max(-segExtent, -b1), segExtent);
                        sqrDist = -s0 * s0 + s1 * (s1 + 2 * b1) + c;
                    }
                }
            }
            else
            {
                // Ray and segment are parallel.
                s1 = (a01 > 0) ? -segExtent : segExtent;
                s0 = System.Math.Max(0, -(a01 * s1 + b0));
                sqrDist = -s0 * s0 + s1 * (s1 + 2 * b1) + c;
            }

            optionalPointOnRay?.Set(
                this.Direction.X * s0 + this.Origin.X,
                this.Direction.Y * s0 + this.Origin.Y,
                this.Direction.Z * s0 + this.Origin.Z);
            optionalPointOnSegment?.Set(
                segX * s1 + centerX,
                segY * s1 + centerY,
                segZ * s1 + centerZ);

            return sqrDist;
        }

        public float? DistanceToPlane(Plane plane)
        {
            var denominator = plane.Normal.Dot(this.Direction);

            if (denominator == 0)
            {
                // line is coplanar, return origin
                if (plane.DistanceToPoint(this.Origin) == 0)
                {
                    return 0;
                }

                // Null is preferable to undefined since undefined means.... it is undefined
                return null;
            }

            var t = -(this.Origin.Dot(plane.Normal) + plane.Constant) / denominator;

            // Return if the ray never intersects the plane
            return t >= 0 ? t : (float?)null;
        }

        public bool Equals(Ray ray)
        {
            return ray != null && ray.Origin.Equals(this.Origin) && ray.Direction.Equals(this.Direction);
        }

        public Vector3 IntersectSphere(Sphere sphere, Vector3 target)
        {
            var x = sphere.Center.X - this.Origin.X;
            var y = sphere.Center.Y - this.Origin.Y;
            var z = sphere.Center.Z - this.Origin.Z;
            var tca = x * this.Direction.X + y * this.Direction.Y + z * this.Direction.Z;
            var d2 = (x * x + y * y + z * z) - tca * tca;
            var radius2 = sphere.Radius * sphere.Radius;

            if (d2 > radius2)
            {
                return null;
            }

            var thc = Mathf.Sqrt(radius2 - d2);

            // t0 = first intersect point - entrance on front of sphere
            var t0 = tca - thc;

            // t1 = second intersect point - exit point on back of sphere
            var t1 = tca + thc;

            // test to see if both t0 and t1 are behind the ray - if so, return null
            if (t0 < 0 && t1 < 0)
            {
                return null;
            }

            // test to see if t0 is behind the ray:
            // if it is, the ray is inside the sphere, so return the second exit point scaled by t1,
            // in order to always return an intersect point that is in front of the ray.
            if (t0 < 0)
            {
                return this.At(t1, target);
            }

            // else t0 is in front of the ray, so return the first collision point scaled by t0
            return this.At(t0, target);
        }

        public bool IntersectsSphere(Sphere sphere)
        {
            return this.DistanceSqToPoint(sphere.Center) <= (sphere.Radius * sphere.Radius);
        }

        public Vector3 IntersectPlane(Plane plane, Vector3 target)
        {
            var t = this.DistanceToPlane(plane);

            if (t == null)
            {
                return null;
            }

            return this.At((float)t, target);
        }

        public bool IntersectsPlane(Plane plane)
        {
            // check if the ray lies on the plane first
            var distToPoint = plane.DistanceToPoint(this.Origin);

            if (distToPoint == 0)
            {
                return true;
            }

            var denominator = plane.Normal.Dot(this.Direction);

            if (denominator * distToPoint < 0)
            {
                return true;
            }

            // ray origin is behind the plane (and is pointing behind it)
            return false;
        }

        public Vector3 IntersectBox(Box3 box, Vector3 target)
        {
            var t = GetBoxIntersectionT(box);
            return t.HasValue ? this.At(t.Value, target) : null;
        }

        // Shared slab test avoids allocating a throwaway Vector3 in IntersectsBox.
        private float? GetBoxIntersectionT(Box3 box)
        {
            float tmin, tmax, tymin, tymax, tzmin, tzmax;

            float invdirx = 1 / this.Direction.X,
                invdiry = 1 / this.Direction.Y,
                invdirz = 1 / this.Direction.Z;

            var origin = this.Origin;

            if (invdirx >= 0)
            {
                tmin = (box.Min.X - origin.X) * invdirx;
                tmax = (box.Max.X - origin.X) * invdirx;
            }
            else
            {
                tmin = (box.Max.X - origin.X) * invdirx;
                tmax = (box.Min.X - origin.X) * invdirx;
            }

            if (invdiry >= 0)
            {
                tymin = (box.Min.Y - origin.Y) * invdiry;
                tymax = (box.Max.Y - origin.Y) * invdiry;
            }
            else
            {
                tymin = (box.Max.Y - origin.Y) * invdiry;
                tymax = (box.Min.Y - origin.Y) * invdiry;
            }

            if ((tmin > tymax) || (tymin > tmax))
            {
                return null;
            }

            // These lines also handle the case where tmin or tmax is NaN
            // (result of 0 * Infinity). x !== x returns true if x is NaN

            if (tymin > tmin || tmin != tmin)
            {
                tmin = tymin;
            }

            if (tymax < tmax || tmax != tmax)
            {
                tmax = tymax;
            }

            if (invdirz >= 0)
            {
                tzmin = (box.Min.Z - origin.Z) * invdirz;
                tzmax = (box.Max.Z - origin.Z) * invdirz;
            }
            else
            {
                tzmin = (box.Max.Z - origin.Z) * invdirz;
                tzmax = (box.Min.Z - origin.Z) * invdirz;
            }

            if ((tmin > tzmax) || (tzmin > tmax))
            {
                return null;
            }

            if (tzmin > tmin || tmin != tmin)
            {
                tmin = tzmin;
            }

            if (tzmax < tmax || tmax != tmax)
            {
                tmax = tzmax;
            }

            //return point closest to the ray (positive side)
            if (tmax < 0)
            {
                return null;
            }

            return tmin >= 0 ? tmin : tmax;
        }


        public bool IntersectsBox(Box3 box)
        {
            return GetBoxIntersectionT(box).HasValue;
        }

        public Vector3 IntersectTriangle(Vector3 a, Vector3 b, Vector3 c, bool backfaceCulling, Vector3 target)
        {
            // Scalar equivalent of the original GTEngine test. Preserve face-culling,
            // degenerate triangles, boundary hits and the supplied output target.
            var e1x = b.X - a.X;
            var e1y = b.Y - a.Y;
            var e1z = b.Z - a.Z;
            var e2x = c.X - a.X;
            var e2y = c.Y - a.Y;
            var e2z = c.Z - a.Z;
            var nx = e1y * e2z - e1z * e2y;
            var ny = e1z * e2x - e1x * e2z;
            var nz = e1x * e2y - e1y * e2x;

            var dx = this.Direction.X;
            var dy = this.Direction.Y;
            var dz = this.Direction.Z;
            var ddn = dx * nx + dy * ny + dz * nz;
            float sign;
            if (ddn > 0)
            {
                if (backfaceCulling) return null;
                sign = 1;
            }
            else if (ddn < 0)
            {
                sign = -1;
                ddn = -ddn;
            }
            else
            {
                return null;
            }

            var qx = this.Origin.X - a.X;
            var qy = this.Origin.Y - a.Y;
            var qz = this.Origin.Z - a.Z;
            var qxe2x = qy * e2z - qz * e2y;
            var qxe2y = qz * e2x - qx * e2z;
            var qxe2z = qx * e2y - qy * e2x;
            var ddqxe2 = sign * (dx * qxe2x + dy * qxe2y + dz * qxe2z);
            if (ddqxe2 < 0) return null;

            var e1xqx = e1y * qz - e1z * qy;
            var e1xqy = e1z * qx - e1x * qz;
            var e1xqz = e1x * qy - e1y * qx;
            var dde1xq = sign * (dx * e1xqx + dy * e1xqy + dz * e1xqz);
            if (dde1xq < 0) return null;
            if (ddqxe2 + dde1xq > ddn) return null;

            var qdn = -sign * (qx * nx + qy * ny + qz * nz);
            if (qdn < 0) return null;

            return this.At(qdn / ddn, target);
        }

        public Ray LookAt(Vector3 target)
        {
            this.Direction.Copy(target).Sub(this.Origin).Normalize();
            return this;
        }

        public Ray Recast(float t)
        {
            var v1 = new Vector3();
            this.Origin.Copy(this.At(t, v1));
            return this;
        }

        public Ray Set(Vector3 origin, Vector3 direction)
        {
            this.Origin.Copy(origin);
            this.Direction.Copy(direction);

            return this;
        }
        #endregion
    }
}
