using System;
using System.Collections.Generic;
using System.Diagnostics;
using ThreeJs4Net.Core;

namespace ThreeJs4Net.Math
{
    [DebuggerDisplay("Min ({Min}), Max ({Max})")]
    public class Box3 : IEquatable<Box3>
    {
        public static Box3 Empty()
        {
            return new Box3(Vector3.Infinity(), Vector3.NegativeInfinity());
        }

        public Vector3 Min = new Vector3();
        public Vector3 Max = new Vector3();

        /// <summary>
        /// 
        /// </summary>
        public Box3()
        {
            // Same values as Copy(Box3.Empty()), without allocating a temporary Box3 and four Vector3.
            this.Min.Set(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            this.Max.Set(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Box3(Vector3 min, Vector3 max)
        {
            this.Min.Copy(min);
            this.Max.Copy(max);
        }

        #region --- Already in R116 ---
        public Box3 SetFromBufferAttribute(BufferAttribute<float> attribute)
        {
            ComputeBounds(attribute.Array, attribute.ItemSize,
                out var minX, out var minY, out var minZ,
                out var maxX, out var maxY, out var maxZ);

            this.Min.Set(minX, minY, minZ);
            this.Max.Set(maxX, maxY, maxZ);

            return this;
        }

        internal static void ComputeBounds(float[] array, int itemSize,
            out float minX, out float minY, out float minZ,
            out float maxX, out float maxY, out float maxZ)
        {
            minX = float.PositiveInfinity;
            minY = float.PositiveInfinity;
            minZ = float.PositiveInfinity;

            maxX = float.NegativeInfinity;
            maxY = float.NegativeInfinity;
            maxZ = float.NegativeInfinity;

            var count = array.Length / itemSize;

            for (int i = 0, o = 0; i < count; i++, o += itemSize)
            {
                var x = array[o];
                var y = array[o + 1];
                var z = array[o + 2];

                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (z < minZ) minZ = z;

                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
                if (z > maxZ) maxZ = z;
            }
        }

        public Box3 ApplyMatrix4(Matrix4 matrix)
        {
            // transform of empty box is an empty box.
            if (this.IsEmpty())
            {
                return this;
            }

            float x0 = Min.X, y0 = Min.Y, z0 = Min.Z;
            float x1 = Max.X, y1 = Max.Y, z1 = Max.Z;

            var e = matrix.Elements;
            float e0 = e[0], e1 = e[1], e2 = e[2];
            float e4 = e[4], e5 = e[5], e6 = e[6];
            float e8 = e[8], e9 = e[9], e10 = e[10];
            float e12 = e[12], e13 = e[13], e14 = e[14];

            float minX = float.PositiveInfinity, minY = float.PositiveInfinity, minZ = float.PositiveInfinity;
            float maxX = float.NegativeInfinity, maxY = float.NegativeInfinity, maxZ = float.NegativeInfinity;

            // NOTE: I am using A binary pattern to specify all 2^3 combinations below
            ExpandByCorner(x0, y0, z0); // 000
            ExpandByCorner(x0, y0, z1); // 001
            ExpandByCorner(x0, y1, z0); // 010
            ExpandByCorner(x0, y1, z1); // 011
            ExpandByCorner(x1, y0, z0); // 100
            ExpandByCorner(x1, y0, z1); // 101
            ExpandByCorner(x1, y1, z0); // 110
            ExpandByCorner(x1, y1, z1); // 111

            this.Min.Set(minX, minY, minZ);
            this.Max.Set(maxX, maxY, maxZ);

            return this;

            void ExpandByCorner(float lX, float lY, float lZ)
            {
                var x = e0 * lX + e4 * lY + e8 * lZ + e12;
                var y = e1 * lX + e5 * lY + e9 * lZ + e13;
                var z = e2 * lX + e6 * lY + e10 * lZ + e14;

                // Vector3.Min / Vector3.Max
                if (minX > x) minX = x;
                if (minY > y) minY = y;
                if (minZ > z) minZ = z;

                if (maxX < x) maxX = x;
                if (maxY < y) maxY = y;
                if (maxZ < z) maxZ = z;
            }
        }

        public Vector3 ClampPoint(Vector3 point, Vector3 target = null)
        {
            if (target == null)
                target = new Vector3();

            return target.Copy(point).Clamp(this.Min, this.Max);
        }

        public Box3 Clone()
        {
            return new Box3().Copy(this);
        }

        public bool ContainsBox(Box3 box)
        {
            return (this.Min.X <= box.Min.X) && (box.Max.X <= this.Max.X) &&
                   (this.Min.Y <= box.Min.Y) && (box.Max.Y <= this.Max.Y) &&
                   (this.Min.Z <= box.Min.Z) && (box.Max.Z <= this.Max.Z);
        }

        public bool ContainsPoint(Vector3 point)
        {
            return !(point.X < this.Min.X || point.X > this.Max.X ||
                     point.Y < this.Min.Y || point.Y > this.Max.Y ||
                     point.Z < this.Min.Z || point.Z > this.Max.Z);
        }

        public Box3 Copy(Box3 box)
        {
            this.Min.Copy(box.Min);
            this.Max.Copy(box.Max);

            return this;
        }

        public double DistanceToPoint(Vector3 point)
        {
            var x = point.X;
            var y = point.Y;
            var z = point.Z;

            // Vector3.Clamp uses comparisons (not Math.Min/Max), including for NaNs.
            if (x < this.Min.X)
            {
                x = this.Min.X;
            }
            else if (x > this.Max.X)
            {
                x = this.Max.X;
            }
            if (y < this.Min.Y)
            {
                y = this.Min.Y;
            }
            else if (y > this.Max.Y)
            {
                y = this.Max.Y;
            }
            if (z < this.Min.Z)
            {
                z = this.Min.Z;
            }
            else if (z > this.Max.Z)
            {
                z = this.Max.Z;
            }

            x -= point.X;
            y -= point.Y;
            z -= point.Z;

            return Mathf.Sqrt(x * x + y * y + z * z);
        }

        public Box3 ExpandByObject(Object3D o)
        {
            // Computes the world-axis-aligned bounding box of an object (including its children),
            // accounting for both the object's, and children's, world transforms
            o.UpdateMatrixWorld(false);   //  object.updateWorldMatrix( false, false );

            var box = Box3.Empty();
            var geometry = o.Geometry;

            if (geometry != null)
            {
                if (geometry.BoundingBox == null || geometry.BoundingBox.IsEmpty())
                {
                    geometry.ComputeBoundingBox();
                }
                box.Copy(geometry.BoundingBox);
                box.ApplyMatrix4(o.MatrixWorld);
                this.Union(box);
            }
            foreach (var child in o.Children)
            {
                this.ExpandByObject(child);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Box3 ExpandByPoint(Vector3 point)
        {
            this.Min.Min(point);
            this.Max.Max(point);

            return this;
        }

        public Box3 ExpandByScalar(float scalar)
        {
            Min.AddScalar(-scalar);
            Max.AddScalar(scalar);

            return this;
        }

        public Box3 ExpandByVector(Vector3 vector)
        {
            Min.Sub(vector);
            Max.Add(vector);

            return this;
        }

        public Sphere GetBoundingSphere(Sphere target)
        {
            this.GetCenter(target.Center);
            target.Radius = this.GetSize(new Vector3()).Length() * (float)0.5;
            return target;
        }

        /// <summary>
        /// Get the center point of the box
        /// </summary>
        /// <param name="target">The result will be copied into this Vector3</param>
        /// <returns>Center point of the box as a Vector3.</returns>
        /// <seealso cref="Vector3"/>
        public Vector3 GetCenter(Vector3 target)
        {
            if (this.IsEmpty())
            {
                target.Set(0, 0, 0);
            }
            else
            {
                target.AddVectors(this.Min, this.Max).MultiplyScalar((float)0.5);
            }

            return target;
        }

        public Vector3 GetParameter(Vector3 point, Vector3 target)
        {
            // This can potentially have A divide by zero if the box
            // has A size dimension of 0.
            var x = (point.X - this.Min.X) / (this.Max.X - this.Min.X);
            var y = (point.Y - this.Min.Y) / (this.Max.Y - this.Min.Y);
            var z = (point.Z - this.Min.Z) / (this.Max.Z - this.Min.Z);

            return target.Set(x, y, z);
        }

        /// <summary>
        /// Get the size (width, height and depth)
        /// </summary>
        /// <param name="target">The result will be copied into this Vector3</param>
        /// <returns>The width, height and depth of this box.</returns>
        public Vector3 GetSize(Vector3 target)
        {
            if (this.IsEmpty())
            {
                target.Set(0, 0, 0);
            }
            else
            {
                target.SubVectors(this.Max, this.Min);
            }

            return target;
        }

        /// <summary>
        /// Computes the intersection of this and box, setting the upper bound of this box to the lesser of
        /// the two boxes' upper bounds and the lower bound of this box to the greater of the two boxes'
        /// lower bounds. If there's no overlap, makes this box empty.
        /// </summary>
        /// <param name="box">Box to intersect with</param>
        /// <returns>Box3 with the intersection or empty if no overlap</returns>
        public Box3 Intersect(Box3 box)
        {
            Min.Max(box.Min);
            Max.Min(box.Max);

            if (this.IsEmpty())
            {
                this.MakeEmpty();
            }

            return this;
        }

        public bool IntersectsBox(Box3 box)
        {
            // using 6 splitting planes to rule out intersections.
            return !(box.Max.X < this.Min.X || box.Min.X > this.Max.X ||
                     box.Max.Y < this.Min.Y || box.Min.Y > this.Max.Y ||
                     box.Max.Z < this.Min.Z || box.Min.Z > this.Max.Z);
        }

        public bool IntersectsSphere(Sphere sphere)
        {
            // Inline the clamp: no temporary Vector3 for a potentially very frequent AABB test.
            var x = sphere.Center.X;
            var y = sphere.Center.Y;
            var z = sphere.Center.Z;

            if (x < this.Min.X)
            {
                x = this.Min.X;
            }
            else if (x > this.Max.X)
            {
                x = this.Max.X;
            }
            if (y < this.Min.Y)
            {
                y = this.Min.Y;
            }
            else if (y > this.Max.Y)
            {
                y = this.Max.Y;
            }
            if (z < this.Min.Z)
            {
                z = this.Min.Z;
            }
            else if (z > this.Max.Z)
            {
                z = this.Max.Z;
            }

            var dx = x - sphere.Center.X;
            var dy = y - sphere.Center.Y;
            var dz = z - sphere.Center.Z;

            return dx * dx + dy * dy + dz * dz <= sphere.Radius * sphere.Radius;
        }


        public bool IntersectsPlane(Plane plane)
        {
            // We compute the minimum and maximum dot product values. If those values
            // are on the same side (back or front) of the plane, then there is no intersection.
            float min, max;

            if (plane.Normal.X > 0)
            {
                min = plane.Normal.X * this.Min.X;
                max = plane.Normal.X * this.Max.X;
            }
            else
            {

                min = plane.Normal.X * this.Max.X;
                max = plane.Normal.X * this.Min.X;
            }

            if (plane.Normal.Y > 0)
            {
                min += plane.Normal.Y * this.Min.Y;
                max += plane.Normal.Y * this.Max.Y;
            }
            else
            {
                min += plane.Normal.Y * this.Max.Y;
                max += plane.Normal.Y * this.Min.Y;
            }

            if (plane.Normal.Z > 0)
            {
                min += plane.Normal.Z * this.Min.Z;
                max += plane.Normal.Z * this.Max.Z;
            }
            else
            {
                min += plane.Normal.Z * this.Max.Z;
                max += plane.Normal.Z * this.Min.Z;
            }

            return (min <= -plane.Constant && max >= -plane.Constant);
        }

        public bool IsEmpty()
        {
            // this is A more robust check for empty than ( volume <= 0 ) because volume can get positive with two negative axes
            return (Max.X < Min.X) || (Max.Y < Min.Y) || (Max.Z < Min.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        public void MakeEmpty()
        {
            this.Min.X = this.Min.Y = this.Min.Z = float.PositiveInfinity;
            this.Max.X = this.Max.Y = this.Max.Z = float.NegativeInfinity;
        }

        public Box3 Set(Vector3 min, Vector3 max)
        {
            this.Min.Copy(min);
            this.Max.Copy(max);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        public Box3 SetFromPoints(List<Vector3> points)
        {
            this.MakeEmpty();

            foreach (Vector3 t in points)
            {
                this.ExpandByPoint(t);
            }

            return this;
        }

        public Box3 SetFromArray(float[] array)
        {
            var minX = float.PositiveInfinity;
            var minY = float.PositiveInfinity;
            var minZ = float.PositiveInfinity;

            var maxX = float.NegativeInfinity;
            var maxY = float.NegativeInfinity;
            var maxZ = float.NegativeInfinity;

            for (int i = 0; i < array.Length; i += 3)
            {
                var x = array[i];
                var y = array[i + 1];
                var z = array[i + 2];

                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (z < minZ) minZ = z;

                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
                if (z > maxZ) maxZ = z;
            }

            this.Min.Set(minX, minY, minZ);
            this.Max.Set(maxX, maxY, maxZ);

            return this;
        }

        public Box3 SetFromPoints(IEnumerable<Vector3> points)
        {
            this.MakeEmpty();

            foreach (var p in points)
            {
                this.ExpandByPoint(p);
            }

            return this;
        }

        public Box3 SetFromCenterAndSize(Vector3 center, Vector3 size)
        {
            var halfSize = new Vector3().Copy(size).MultiplyScalar((float)0.5);

            this.Min.Copy(center).Sub(halfSize);
            this.Max.Copy(center).Add(halfSize);

            return this;
        }

        public Box3 SetFromObject(Object3D o)
        {
            this.MakeEmpty();
            return this.ExpandByObject(o);
        }


        //Returns point as A proportion of this box's width and height.

        public Box3 Translate(Vector3 offset)
        {
            Min.Add(offset);
            Max.Add(offset);

            return this;
        }

        public Box3 Union(Box3 box)
        {
            Min.Min(box.Min);
            Max.Max(box.Max);

            return this;
        }
        #endregion

        #region Equality members
        public bool Equals(Box3 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != this.GetType()) return false;
            return Min.Equals(((Box3)other).Min) && Max.Equals(((Box3)other).Max);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Min != null ? Min.GetHashCode() : 0) * 397) ^ (Max != null ? Max.GetHashCode() : 0);
            }
        }
        #endregion
    }
}
