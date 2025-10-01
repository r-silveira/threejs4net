using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ThreeJs4Net.Core;
using ThreeJs4Net.Properties;

namespace ThreeJs4Net.Math
{
    using Math = System.Math;

    public class Vector4 : IEquatable<Vector4>, INotifyPropertyChanged
    {
        public double X;
        public double Y;
        public double Z;
        public double W;

        public Vector4()
        {
            this.X = this.Y = this.Z = 0;
            this.W = 1;
        }

        public Vector4(double scalar)
        {
            this.X = this.Y = this.Z = this.W = scalar;
        }

        public Vector4(double x, double y, double z, double w = 1)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        /// <summary>
        /// Defines A unit-length Vector4 that points towards the X-axis.
        /// </summary>
        public static Vector4 UnitX()
        {
            return new Vector4(1, 0, 0, 0);
        }

        /// <summary>
        /// Defines A unit-length Vector4 that points towards the Y-axis.
        /// </summary>
        public static Vector4 UnitY()
        {
            return new Vector4(0, 1, 0, 0);
        }

        /// <summary>
        /// Defines A unit-length Vector4 that points towards the Z-axis.
        /// </summary>
        public static Vector4 UnitZ()
        {
            return new Vector4(0, 0, 1, 0);
        }

        /// <summary>
        /// Defines A unit-length Vector4 that points towards the W-axis.
        /// </summary>
        public static Vector4 UnitW()
        {
            return new Vector4(0, 0, 0, 1);
        }

        /// <summary>
        /// Defines A zero-length Vector4.
        /// </summary>
        public static Vector4 Zero()
        {
            return new Vector4(0, 0, 0, 0);
        }

        /// <summary>
        /// Defines an instance with all components Set to 1.
        /// </summary>
        /// <returns></returns>
        public static Vector4 One()
        {
            return new Vector4(1, 1, 1, 1);
        }



        #region --- Already in R116 ---
        public Vector4 Set(double X, double Y, double Z, double W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;

            return this;
        }

        public Vector4 SetScalar(double scalar)
        {
            this.X = scalar;
            this.Y = scalar;
            this.Z = scalar;
            this.W = scalar;

            return this;
        }

        public Vector4 SetX(double x)
        {
            this.X = x;
            return this;
        }

        public Vector4 SetY(double y)
        {
            this.Y = y;
            return this;
        }

        public Vector4 SetZ(double z)
        {
            this.Z = z;
            return this;
        }

        public Vector4 SetW(double w)
        {
            this.W = w;
            return this;
        }

        public void SetComponent(int index, double value)
        {
            switch (index)
            {
                case 0: this.X = value; break;
                case 1: this.Y = value; break;
                case 2: this.Z = value; break;
                case 3: this.W = value; break;
                default: throw new IndexOutOfRangeException($"Index {index} is out of bounds");
            }
        }

        public double GetComponent(int index)
        {
            switch (index)
            {
                case 0: return this.X;
                case 1: return this.Y;
                case 2: return this.Z;
                case 3: return this.W;
                default: throw new IndexOutOfRangeException($"Index {index} is out of bounds");
            }
        }

        public Vector4 Clone()
        {
            return new Vector4(this.X, this.Y, this.Z, this.W);
        }

        public Vector4 Copy(Vector4 vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
            this.W = vector.W;

            return this;
        }

        public Vector4 Add(Vector4 v)
        {
            this.X += v.X;
            this.Y += v.Y;
            this.Z += v.Z;
            this.W += v.W;

            return this;
        }

        public Vector4 AddScalar(double scalar)
        {
            this.X += scalar;
            this.Y += scalar;
            this.Z += scalar;
            this.W += scalar;

            return this;
        }

        public Vector4 AddVectors(Vector4 a, Vector4 b)
        {
            this.X = a.X + b.X;
            this.Y = a.Y + b.Y;
            this.Z = a.Z + b.Z;
            this.W = a.W + b.Z;

            return this;
        }

        public Vector4 AddScaledVector(Vector4 v, double s)
        {
            this.X += v.X * s;
            this.Y += v.Y * s;
            this.Z += v.Z * s;
            this.W += v.W * s;

            return this;
        }

        public Vector4 Sub(Vector4 v)
        {
            this.X -= v.X;
            this.Y -= v.Y;
            this.Z -= v.Z;
            this.W -= v.W;

            return this;
        }

        public Vector4 SubScalar(double s)
        {
            this.X -= s;
            this.Y -= s;
            this.Z -= s;
            this.W -= s;

            return this;
        }

        public Vector4 SubVectors(Vector4 a, Vector4 b)
        {
            this.X = a.X - b.X;
            this.Y = a.Y - b.Y;
            this.Z = a.Z - b.Z;
            this.W = a.W - b.W;

            return this;
        }

        public Vector4 ApplyMatrix4(Matrix4 matrix)
        {
            var x = this.X;
            var y = this.Y;
            var z = this.Z;

            var e = matrix.Elements;

            this.X = e[0] * x + e[4] * y + e[8] * z + e[12];
            this.Y = e[1] * x + e[5] * y + e[9] * z + e[13];
            this.Z = e[2] * x + e[6] * y + e[10] * z + e[14];
            return this;
        }

        public Vector4 DivideScalar(double scalar)
        {
            return this.MultiplyScalar(1.0 / scalar);
        }

        public Vector4 SetAxisAngleFromQuaternion(Quaternion q)
        {
            // http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToAngle/index.htm
            // q is assumed to be normalized

            this.W = 2 * Math.Acos(q.W);
            var s = Math.Sqrt(1 - q.W * q.W);

            if (s < 0.0001)
            {
                this.X = 1;
                this.Y = 0;
                this.Z = 0;
            }
            else
            {
                this.X = q.X / s;
                this.Y = q.Y / s;
                this.Z = q.Z / s;
            }

            return this;
        }

        public Vector4 SetAxisAngleFromRotationMatrix(Matrix3 m)
        {
            // http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToAngle/index.htm
            // assumes the upper 3x3 of m is a pure rotation matrix (i.e, unscaled)

            double angle, x, y, z;     // variables for result
            double epsilon = (double)0.01;     // margin to allow for rounding errors
            double epsilon2 = (double)0.1;     // margin to distinguish between 0 and 180 degrees

            var te = m.Elements;

            double m11 = te[0], m12 = te[4], m13 = te[8];
            double m21 = te[1], m22 = te[5], m23 = te[9];
            double m31 = te[2], m32 = te[6], m33 = te[10];

            if ((Math.Abs(m12 - m21) < epsilon) &&
                 (Mathf.Abs(m13 - m31) < epsilon) &&
                 (Math.Abs(m23 - m32) < epsilon))
            {
                // singularity found
                // first check for identity matrix which must have +1 for all terms
                // in leading diagonal and zero in other terms

                if ((Math.Abs(m12 + m21) < epsilon2) &&
                     (Math.Abs(m13 + m31) < epsilon2) &&
                     (Math.Abs(m23 + m32) < epsilon2) &&
                     (Math.Abs(m11 + m22 + m33 - 3) < epsilon2))
                {
                    // this singularity is identity matrix so angle = 0
                    this.Set(1, 0, 0, 0);
                    return this; // zero angle, arbitrary axis
                }

                // otherwise this singularity is angle = 180
                angle = Math.PI;

                var xx = (m11 + 1) / 2;
                var yy = (m22 + 1) / 2;
                var zz = (m33 + 1) / 2;
                var xy = (m12 + m21) / 4;
                var xz = (m13 + m31) / 4;
                var yz = (m23 + m32) / 4;

                if ((xx > yy) && (xx > zz))
                {
                    // m11 is the largest diagonal term
                    if (xx < epsilon)
                    {
                        x = 0;
                        y = (double)0.707106781;
                        z = (double)0.707106781;
                    }
                    else
                    {
                        x = Math.Sqrt(xx);
                        y = xy / x;
                        z = xz / x;
                    }
                }
                else if (yy > zz)
                {
                    // m22 is the largest diagonal term
                    if (yy < epsilon)
                    {
                        x = (double)0.707106781;
                        y = 0;
                        z = (double)0.707106781;
                    }
                    else
                    {
                        y = Math.Sqrt(yy);
                        x = xy / y;
                        z = yz / y;
                    }
                }
                else
                {
                    // m33 is the largest diagonal term so base result on this
                    if (zz < epsilon)
                    {
                        x = (double)0.707106781;
                        y = (double)0.707106781;
                        z = 0;
                    }
                    else
                    {
                        z = Math.Sqrt(zz);
                        x = xz / z;
                        y = yz / z;
                    }

                }

                this.Set(x, y, z, angle);

                return this; // return 180 deg rotation
            }

            // as we have reached here there are no singularities so we can handle normally

            var s = Math.Sqrt((m32 - m23) * (m32 - m23) +
                                     (m13 - m31) * (m13 - m31) +
                                     (m21 - m12) * (m21 - m12)); // used to normalize

            if (Math.Abs(s) < 0.001) s = 1;

            // prevent divide by zero, should not happen if matrix is orthogonal and should be
            // caught by singularity test above, but I've left it in just in case

            this.X = (m32 - m23) / s;
            this.Y = (m13 - m31) / s;
            this.Z = (m21 - m12) / s;
            this.W = Math.Acos((m11 + m22 + m33 - 1) / 2);

            return this;
        }

        public Vector4 Min(Vector4 v)
        {
            this.X = Math.Min(this.X, v.X);
            this.Y = Math.Min(this.Y, v.Y);
            this.Z = Math.Min(this.Z, v.Z);
            this.W = Math.Min(this.W, v.W);

            return this;
        }

        public Vector4 Max(Vector4 v)
        {
            this.X = Math.Max(this.X, v.X);
            this.Y = Math.Max(this.Y, v.Y);
            this.Z = Math.Max(this.Z, v.Z);
            this.W = Math.Max(this.W, v.W);

            return this;
        }

        public Vector4 Clamp(Vector4 min, Vector4 max)
        {
            // assumes min < max, componentwise
            this.X = Math.Max(min.X, Math.Min(max.X, this.X));
            this.Y = Math.Max(min.Y, Math.Min(max.Y, this.Y));
            this.Z = Math.Max(min.Z, Math.Min(max.Z, this.Z));
            this.W = Math.Max(min.W, Math.Min(max.W, this.W));

            return this;
        }

        public Vector4 ClampScalar(double minVal, double maxVal)
        {
            this.X = Math.Max(minVal, Math.Min(maxVal, this.X));
            this.Y = Math.Max(minVal, Math.Min(maxVal, this.Y));
            this.Z = Math.Max(minVal, Math.Min(maxVal, this.Z));
            this.W = Math.Max(minVal, Math.Min(maxVal, this.W));

            return this;
        }

        public Vector4 ClampLength(double min, double max)
        {
            var length = this.Length();
            return this.DivideScalar(length == 0 ? 1 : length).MultiplyScalar(Mathf.Max(min, Mathf.Min(max, length)));
        }

        public double Length()
        {
            return Mathf.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
        }

        public double LengthSq()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
        }

        public double ManhattanLength()
        {
            return Mathf.Abs(this.X) + Mathf.Abs(this.Y) + Mathf.Abs(this.Z) + Mathf.Abs(this.W);
        }

        public Vector4 Normalize()
        {
            var length = this.Length();
            return this.DivideScalar(length == 0 ? 1 : length);
        }

        public Vector4 SetLength(double length)
        {
            return this.Normalize().MultiplyScalar(length);
        }

        public Vector4 Lerp(Vector4 vector, double alpha)
        {
            this.X += (vector.X - this.X) * alpha;
            this.Y += (vector.Y - this.Y) * alpha;
            this.Z += (vector.Z - this.Z) * alpha;
            this.W += (vector.W - this.W) * alpha;

            return this;
        }

        public Vector4 LerpVectors(Vector4 v1, Vector4 v2, double alpha)
        {
            return this.SubVectors(v2, v1).MultiplyScalar(alpha).Add(v1);
        }

        public Vector4 Floor()
        {
            this.X = Mathf.Floor(this.X);
            this.Y = Mathf.Floor(this.Y);
            this.Z = Mathf.Floor(this.Z);
            this.W = Mathf.Floor(this.W);

            return this;
        }

        public Vector4 Ceil()
        {
            this.X = Mathf.Ceiling(this.X);
            this.Y = Mathf.Ceiling(this.Y);
            this.Z = Mathf.Ceiling(this.Z);
            this.W = Mathf.Ceiling(this.W);

            return this;
        }

        public Vector4 Round()
        {
            this.X = Mathf.Round(this.X);
            this.Y = Mathf.Round(this.Y);
            this.Z = Mathf.Round(this.Z);
            this.W = Mathf.Round(this.W);

            return this;
        }

        public Vector4 RoundToZero()
        {
            this.X = (this.X < 0) ? Mathf.Ceiling(this.X) : Mathf.Floor(this.X);
            this.Y = (this.Y < 0) ? Mathf.Ceiling(this.Y) : Mathf.Floor(this.Y);
            this.Z = (this.Z < 0) ? Mathf.Ceiling(this.Z) : Mathf.Floor(this.Z);
            this.W = (this.W < 0) ? Mathf.Ceiling(this.W) : Mathf.Floor(this.W);

            return this;
        }

        public Vector4 Negate()
        {
            this.X = -this.X;
            this.Y = -this.Y;
            this.Z = -this.Z;
            this.W = -this.W;

            return this;
        }

        public double Dot(Vector4 vector)
        {
            return this.X * vector.X + this.Y * vector.Y + this.Z * vector.Z + this.W * vector.W;
        }

        public bool Equals(Vector4 vector)
        {
            return ((vector.X == this.X) && (vector.Y == this.Y) && (vector.Z == this.Z) && (vector.W == this.W));
        }

        public Vector4 FromArray(double[] array, int offset = 0)
        {
            this.X = array[offset];
            this.Y = array[offset + 1];
            this.Z = array[offset + 2];
            this.Z = array[offset + 3];

            return this;
        }

        public double[] ToArray(ref double[] array, int offset)
        {
            if (array == null)
            {
                array = new double[4];
            }
            if (array.Length < offset + 4)
            {
                Array.Resize(ref array, offset + 4);
            }

            array[offset] = this.X;
            array[offset + 1] = this.Y;
            array[offset + 2] = this.Z;
            array[offset + 3] = this.W;

            return array;
        }

        public Vector4 MultiplyScalar(double scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
            this.Z *= scalar;
            this.W *= scalar;

            return this;
        }

        public Vector4 Random()
        {
            this.X = Mathf.RandomF();
            this.Y = Mathf.RandomF();
            this.Z = Mathf.RandomF();
            this.W = Mathf.RandomF();

            return this;
        }

        public Vector4 FromBufferAttribute(BufferAttribute<double> attribute, int index)
        {
            this.X = attribute.GetX(index);
            this.Y = attribute.GetY(index);
            this.Z = attribute.GetZ(index);
            this.W = attribute.GetW(index);

            return this;
        }

        #endregion

        #region Operator override

        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            v1.X += v2.X;
            v1.Y += v2.Y;
            v1.Z += v2.Z;
            v1.W += v2.W;

            return v1;
        }

        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            v1.Z -= v2.Z;
            v1.W -= v2.W;

            return v1;
        }

        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            v1.X *= v2.X;
            v1.Y *= v2.Y;
            v1.Z *= v2.Z;
            v1.W *= v2.W;

            return v1;
        }

        public static Vector4 operator /(Vector4 v1, Vector4 v2)
        {
            v1.X /= v2.X;
            v1.Y /= v2.Y;
            v1.Z /= v2.Z;
            return v1;
        }

        #endregion




        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}