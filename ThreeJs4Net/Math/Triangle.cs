using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ThreeJs4Net.Properties;

namespace ThreeJs4Net.Math
{
    [DebuggerDisplay("A = {a}, B = {b}, C = {c}")]
    public class Triangle : IEquatable<Triangle>, INotifyPropertyChanged
    {
        public Vector3 a, b, c;

        public Triangle(Vector3 a = null, Vector3 b = null, Vector3 c = null)
        {
            this.a = a ?? new Vector3();
            this.b = b ?? new Vector3();
            this.c = c ?? new Vector3();
        }

        public static Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c, Vector3 target)
        {
            var x0 = c.X - b.X;
            var y0 = c.Y - b.Y;
            var z0 = c.Z - b.Z;
            var x1 = a.X - b.X;
            var y1 = a.Y - b.Y;
            var z1 = a.Z - b.Z;
            var x = y0 * z1 - z0 * y1;
            var y = z0 * x1 - x0 * z1;
            var z = x0 * y1 - y0 * x1;
            var lengthSq = x * x + y * y + z * z;
            if (lengthSq > 0)
            {
                var invLength = 1 / Mathf.Sqrt(lengthSq);
                return target.Set(x * invLength, y * invLength, z * invLength);
            }
            return target.Set(0, 0, 0);
        }

        // The barycentric kernel writes scalar coordinates to avoid temporary vectors
        // in GetBarycoord, ContainsPoint and GetUV.
        private static void Barycentric(Vector3 point, Vector3 a, Vector3 b, Vector3 c,
            out float x, out float y, out float z)
        {
            var v0x = c.X - a.X;
            var v0y = c.Y - a.Y;
            var v0z = c.Z - a.Z;
            var v1x = b.X - a.X;
            var v1y = b.Y - a.Y;
            var v1z = b.Z - a.Z;
            var v2x = point.X - a.X;
            var v2y = point.Y - a.Y;
            var v2z = point.Z - a.Z;

            var dot00 = v0x * v0x + v0y * v0y + v0z * v0z;
            var dot01 = v0x * v1x + v0y * v1y + v0z * v1z;
            var dot02 = v0x * v2x + v0y * v2y + v0z * v2z;
            var dot11 = v1x * v1x + v1y * v1y + v1z * v1z;
            var dot12 = v1x * v2x + v1y * v2y + v1z * v2z;
            var denom = dot00 * dot11 - dot01 * dot01;
            if (denom == 0)
            {
                x = -2;
                y = -1;
                z = -1;
                return;
            }

            var invDenom = 1 / denom;
            var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            var v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            x = 1 - u - v;
            y = v;
            z = u;
        }

        public static Vector3 GetBarycoord(Vector3 point, Vector3 a, Vector3 b, Vector3 c, Vector3 target)
        {
            Barycentric(point, a, b, c, out var x, out var y, out var z);
            return target.Set(x, y, z);
        }

        public static bool ContainsPoint(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
        {
            Barycentric(point, a, b, c, out var x, out var y, out _);
            return (x >= 0) && (y >= 0) && ((x + y) <= 1);
        }

        public static Vector2 GetUV(Vector3 point, Vector3 p1, Vector3 p2, Vector3 p3, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 target)
        {
            Barycentric(point, p1, p2, p3, out var x, out var y, out var z);
            // Preserve the Set + AddScaledVector sequence, including float rounding.
            target.Set(0, 0);
            target.AddScaledVector(uv1, x);
            target.AddScaledVector(uv2, y);
            target.AddScaledVector(uv3, z);
            return target;
        }

        public static bool IsFrontFacing(Vector3 a, Vector3 b, Vector3 c, Vector3 direction)
        {
            var x0 = c.X - b.X;
            var y0 = c.Y - b.Y;
            var z0 = c.Z - b.Z;
            var x1 = a.X - b.X;
            var y1 = a.Y - b.Y;
            var z1 = a.Z - b.Z;
            var nx = y0 * z1 - z0 * y1;
            var ny = z0 * x1 - x0 * z1;
            var nz = x0 * y1 - y0 * x1;
            return nx * direction.X + ny * direction.Y + nz * direction.Z < 0;
        }

        public Triangle Set(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a.Copy(a);
            this.b.Copy(b);
            this.c.Copy(c);

            return this;
        }

        public Triangle SetFromPointsAndIndices(Vector3[] points, int i0, int i1, int i2)
        {
            this.a.Copy(points[i0]);
            this.b.Copy(points[i1]);
            this.c.Copy(points[i2]);

            return this;
        }

        public Triangle Clone()
        {
            return new Triangle(this.a, this.b, this.c);
        }

        public Triangle Copy(Triangle triangle)
        {
            this.a.Copy(triangle.a);
            this.b.Copy(triangle.b);
            this.c.Copy(triangle.c);

            return this;
        }

        public float GetArea()
        {
            var x0 = this.c.X - this.b.X;
            var y0 = this.c.Y - this.b.Y;
            var z0 = this.c.Z - this.b.Z;
            var x1 = this.a.X - this.b.X;
            var y1 = this.a.Y - this.b.Y;
            var z1 = this.a.Z - this.b.Z;
            var x = y0 * z1 - z0 * y1;
            var y = z0 * x1 - x0 * z1;
            var z = x0 * y1 - y0 * x1;
            return Mathf.Sqrt(x * x + y * y + z * z) * 0.5f;
        }

        public Vector3 GetMidpoint(Vector3 target)
        {
            return target.AddVectors(this.a, this.b).Add(this.c).MultiplyScalar((float)1 / (float)3);

        }

        public Vector3 GetNormal(Vector3 target)
        {
            return Triangle.GetNormal(this.a, this.b, this.c, target);
        }

        //!!public Vector3 GetPlane(Plane target)
        //{
        //    return target.setFromCoplanarPoints(this.A, this.B, this.C);
        //}

        public Vector3 GetBarycoord(Vector3 point, Vector3 target)
        {
            return Triangle.GetBarycoord(point, this.a, this.b, this.c, target);
        }

        public Vector2 GetUV(Vector3 point, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 target)
        {
            return Triangle.GetUV(point, this.a, this.b, this.c, uv1, uv2, uv3, target);
        }

        public bool ContainsPoint(Vector3 point)
        {
            return Triangle.ContainsPoint(point, this.a, this.b, this.c);
        }

        public bool IsFrontFacing(Vector3 direction)
        {
            return Triangle.IsFrontFacing(this.a, this.b, this.c, direction);
        }

        //!!public bool IntersectsBox(Box3 box) {
        //    return box.intersectsTriangle( this );
        //}

        public Vector3 ClosestPointToPoint(Vector3 p, Vector3 target)
        {
            var a = this.a;
            var b = this.b;
            var c = this.c;
            // Christer Ericson's closest-point regions, with scalar intermediates.
            var abx = b.X - a.X;
            var aby = b.Y - a.Y;
            var abz = b.Z - a.Z;
            var acx = c.X - a.X;
            var acy = c.Y - a.Y;
            var acz = c.Z - a.Z;
            var apx = p.X - a.X;
            var apy = p.Y - a.Y;
            var apz = p.Z - a.Z;
            var d1 = abx * apx + aby * apy + abz * apz;
            var d2 = acx * apx + acy * apy + acz * apz;
            if (d1 <= 0 && d2 <= 0) return target.Copy(a);

            var bpx = p.X - b.X;
            var bpy = p.Y - b.Y;
            var bpz = p.Z - b.Z;
            var d3 = abx * bpx + aby * bpy + abz * bpz;
            var d4 = acx * bpx + acy * bpy + acz * bpz;
            if (d3 >= 0 && d4 <= d3) return target.Copy(b);

            var vc = d1 * d4 - d3 * d2;
            if (vc <= 0 && d1 >= 0 && d3 <= 0)
            {
                var v = d1 / (d1 - d3);
                return target.Set(a.X + abx * v, a.Y + aby * v, a.Z + abz * v);
            }

            var cpx = p.X - c.X;
            var cpy = p.Y - c.Y;
            var cpz = p.Z - c.Z;
            var d5 = abx * cpx + aby * cpy + abz * cpz;
            var d6 = acx * cpx + acy * cpy + acz * cpz;
            if (d6 >= 0 && d5 <= d6) return target.Copy(c);

            var vb = d5 * d2 - d1 * d6;
            if (vb <= 0 && d2 >= 0 && d6 <= 0)
            {
                var w = d2 / (d2 - d6);
                return target.Set(a.X + acx * w, a.Y + acy * w, a.Z + acz * w);
            }

            var va = d3 * d6 - d5 * d4;
            if (va <= 0 && d4 - d3 >= 0 && d5 - d6 >= 0)
            {
                var w = (d4 - d3) / ((d4 - d3) + (d5 - d6));
                return target.Set(b.X + (c.X - b.X) * w, b.Y + (c.Y - b.Y) * w, b.Z + (c.Z - b.Z) * w);
            }

            var denom = 1 / (va + vb + vc);
            var fv = vb * denom;
            var fw = vc * denom;
            // Sequential adds to preserve the original float rounding.
            var x = a.X + abx * fv;
            var y = a.Y + aby * fv;
            var z = a.Z + abz * fv;
            return target.Set(x + acx * fw, y + acy * fw, z + acz * fw);
        }

        #region Equality members
        public bool Equals(Triangle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(a, other.a) && Equals(b, other.b) && Equals(c, other.c);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Triangle)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (a != null ? a.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (b != null ? b.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (c != null ? c.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}