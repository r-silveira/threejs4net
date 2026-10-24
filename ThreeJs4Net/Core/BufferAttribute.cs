using System;
using System.Collections.Generic;
using System.Linq;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Core
{
    public class BufferAttribute<T> : Renderers.Shaders.Attribute, IBufferAttribute
    {
        // Cached boxes so that setting needsUpdate/Normalized does not allocate a new boxed bool on every call.
        private static readonly object BoxedTrue = true;
        private static readonly object BoxedFalse = false;

        public T[] Array
        {
            get => (T[])this["array"];
            set => this["array"] = value;
        }

        public Type Type
        {
            get => (Type)this["type"];
            set => this["type"] = value;
        }

        public int ItemSize
        {
            get => (int)this["itemSize"];
            set => this["itemSize"] = value;
        }

        public int buffer
        {
            get => (int)this["buffer"];
            set => this["buffer"] = value;
        }

        public bool needsUpdate
        {
            get => (bool)this["needsUpdate"];
            set => this["needsUpdate"] = value ? BoxedTrue : BoxedFalse;
        }

        public bool Normalized
        {
            get => (bool)this["normalized"];
            set => this["normalized"] = value ? BoxedTrue : BoxedFalse;
        }

        public int Usage
        {
            get => (int)this["usage"];
            set => this["usage"] = value;
        }

        public int length => this.Array.Length;

        // We are hiding the Count implementation for Dictionary in here.
        // If needed we can use base.Count to use it again
        public new int Count => this.Array.Length / this.ItemSize;

        public BufferAttribute()
        {
            this.Add("array", null);
            this.Add("itemSize", -1);
            this.Add("buffer", -1);
            this.Add("needsUpdate", false);
            this.Add("type", null);
            this.Add("normalized", false);
            this.Add("usage", Three.StaticDrawUsage);
        }

        public BufferAttribute(T[] array, int itemSize, bool normalized = false) : this()
        {
            this.Array = array;
            this.ItemSize = itemSize;
            this.Type = typeof(T);
            this.Normalized = normalized;
        }



        #region --- Already in R116 ---

        public T GetX(int index)
        {
            return this.Array[index * this.ItemSize];
        }

        public T GetY(int index)

        {
            return this.Array[index * this.ItemSize + 1];
        }

        public T GetZ(int index)
        {
            return this.Array[index * this.ItemSize + 2];
        }

        public T GetW(int index)
        {
            return this.Array[index * this.ItemSize + 3];
        }


        public BufferAttribute<T> Set(T[] source, int offset = 0)
        {
            int newSize;
            if (offset > this.Array.Length)
            {
                newSize = offset + source.Length;
            }
            else
            {
                newSize = this.Array.Length - offset + source.Length;
            }

            if (this.Array.Length < newSize)
            {
                var tempArray = new T[newSize];
                System.Array.Copy(this.Array, tempArray, this.Array.Length);
                this.Array = tempArray;
            }

            System.Array.Copy(source, 0, this.Array, offset, source.Length);

            return this;
        }

        public BufferAttribute<T> SetX(int index, T x)
        {
            this.Array[index * this.ItemSize] = x;
            return this;
        }

        public BufferAttribute<T> SetY(int index, T y)
        {
            this.Array[index * this.ItemSize + 1] = y;
            return this;
        }

        public BufferAttribute<T> SetZ(int index, T z)
        {
            this.Array[index * this.ItemSize + 2] = z;
            return this;
        }

        public BufferAttribute<T> SetW(int index, T w)
        {
            this.Array[index * this.ItemSize + 3] = w;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public BufferAttribute<T> SetXY(int index, T x, T y)
        {
            index *= this.ItemSize;

            this.Array[index] = x;
            this.Array[index + 1] = y;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public BufferAttribute<T> SetXYZ(int index, T x, T y, T z)
        {
            index *= this.ItemSize;

            this.Array[index] = x;
            this.Array[index + 1] = y;
            this.Array[index + 2] = z;

            return this;
        }

        public BufferAttribute<T> SetXYZW(int index, T x, T y, T z, T w)
        {
            index *= this.ItemSize;

            this.Array[index] = x;
            this.Array[index + 1] = y;
            this.Array[index + 2] = z;
            this.Array[index + 3] = w;

            return this;
        }


        public BufferAttribute<T> SetUsage(int value)
        {
            this.Usage = value;
            return this;
        }

        public BufferAttribute<T> Copy(BufferAttribute<T> source)
        {
            //!!this.Name = source.name;
            this.Array = (new T[] { }).Concat(source.Array).ToArray();
            this.ItemSize = source.ItemSize;
            //!!this.Count = source.Count;
            this.Normalized = source.Normalized;
            this.Usage = source.Usage;

            return this;
        }

        public BufferAttribute<T> Clone()
        {
            return new BufferAttribute<T>(this.Array, this.ItemSize).Copy(this);
        }

        public BufferAttribute<T> CopyAt(int index1, BufferAttribute<T> attribute, int index2)
        {
            index1 *= this.ItemSize;
            index2 *= attribute.ItemSize;

            for (int i = 0; i < this.ItemSize; i++)
            {
                this.Array[index1 + i] = attribute.Array[index2 + i];
            }

            return this;
        }

        public BufferAttribute<T> CopyArray(T[] array)
        {
            this.Array = array;
            return this;
        }

        public BufferAttribute<T> CopyVector4sArray(IEnumerable<Vector4> vectors)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't copy Vector4 into it");
            }

            int targetLength = vectors.Count() * 4;
            var tempArray = new T[targetLength];
            int offset = 0;

            foreach (var vector in vectors)
            {
                tempArray[offset++] = (T)(object)vector.X;
                tempArray[offset++] = (T)(object)vector.Y;
                tempArray[offset++] = (T)(object)vector.Z;
                tempArray[offset++] = (T)(object)vector.W;
            }

            this.Array = tempArray;

            return this;
        }

        public BufferAttribute<T> CopyVector3sArray(IEnumerable<Vector3> vectors)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't copy Vector3 into it");
            }

            int targetLength = vectors.Count() * 3;
            var tempArray = new T[targetLength];
            int offset = 0;

            foreach (var vector in vectors)
            {
                tempArray[offset++] = (T)(object)vector.X;
                tempArray[offset++] = (T)(object)vector.Y;
                tempArray[offset++] = (T)(object)vector.Z;
            }

            this.Array = tempArray;

            return this;
        }

        public BufferAttribute<T> CopyVector2sArray(IEnumerable<Vector2> vectors)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't copy Vector2 into it");
            }

            int targetLength = vectors.Count() * 2;
            var tempArray = new T[targetLength];
            int offset = 0;

            foreach (var vector in vectors)
            {
                tempArray[offset++] = (T)(object)vector.X;
                tempArray[offset++] = (T)(object)vector.Y;
            }

            this.Array = tempArray;

            return this;
        }

        // The transforms below operate directly on the backing float[]: Array and ItemSize are read once
        // (each is a dictionary lookup) and no Vector3 is allocated per vertex. The arithmetic is the same,
        // in the same order, as Vector3.ApplyMatrix3/ApplyMatrix4/TransformDirection/Normalize.

        public BufferAttribute<T> ApplyMatrix3(Matrix3 m)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't apply Matrix3 to it");
            }

            ApplyMatrix3(m, normalize: false);

            return this;
        }

        public BufferAttribute<T> ApplyMatrix4(Matrix4 m)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't apply Matrix4 to it");
            }

            var array = (float[])(object)this.Array;
            var itemSize = this.ItemSize;
            var count = array.Length / itemSize;

            var e = m.Elements;
            float e0 = e[0], e1 = e[1], e2 = e[2];
            float e4 = e[4], e5 = e[5], e6 = e[6];
            float e8 = e[8], e9 = e[9], e10 = e[10];
            float e12 = e[12], e13 = e[13], e14 = e[14];

            for (int i = 0, o = 0; i < count; i++, o += itemSize)
            {
                var lX = array[o];
                var lY = array[o + 1];
                var lZ = array[o + 2];

                array[o] = e0 * lX + e4 * lY + e8 * lZ + e12;
                array[o + 1] = e1 * lX + e5 * lY + e9 * lZ + e13;
                array[o + 2] = e2 * lX + e6 * lY + e10 * lZ + e14;
            }

            return this;
        }

        public BufferAttribute<T> ApplyNormalMatrix(Matrix3 m)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't apply Normal Matrix to it");
            }

            ApplyMatrix3(m, normalize: true);

            return this;
        }

        public BufferAttribute<T> TransformDirection(Matrix4 m)
        {
            if (typeof(T) != typeof(float))
            {
                throw new Exception("BufferAttribute is not a float buffer. Can't apply Normal Matrix to it");
            }

            var array = (float[])(object)this.Array;
            var itemSize = this.ItemSize;
            var count = array.Length / itemSize;

            var e = m.Elements;
            float e0 = e[0], e1 = e[1], e2 = e[2];
            float e4 = e[4], e5 = e[5], e6 = e[6];
            float e8 = e[8], e9 = e[9], e10 = e[10];

            for (int i = 0, o = 0; i < count; i++, o += itemSize)
            {
                var lX = array[o];
                var lY = array[o + 1];
                var lZ = array[o + 2];

                var x = e0 * lX + e4 * lY + e8 * lZ;
                var y = e1 * lX + e5 * lY + e9 * lZ;
                var z = e2 * lX + e6 * lY + e10 * lZ;

                Normalize(ref x, ref y, ref z);

                array[o] = x;
                array[o + 1] = y;
                array[o + 2] = z;
            }

            return this;
        }

        private void ApplyMatrix3(Matrix3 m, bool normalize)
        {
            var array = (float[])(object)this.Array;
            var itemSize = this.ItemSize;
            var count = array.Length / itemSize;

            var e = m.Elements;
            float e0 = e[0], e1 = e[1], e2 = e[2];
            float e3 = e[3], e4 = e[4], e5 = e[5];
            float e6 = e[6], e7 = e[7], e8 = e[8];

            for (int i = 0, o = 0; i < count; i++, o += itemSize)
            {
                var lX = array[o];
                var lY = array[o + 1];
                var lZ = array[o + 2];

                var x = e0 * lX + e3 * lY + e6 * lZ;
                var y = e1 * lX + e4 * lY + e7 * lZ;
                var z = e2 * lX + e5 * lY + e8 * lZ;

                if (normalize)
                {
                    Normalize(ref x, ref y, ref z);
                }

                array[o] = x;
                array[o + 1] = y;
                array[o + 2] = z;
            }
        }

        /// <summary>Same arithmetic as <see cref="Vector3.Normalize"/> (zero-length vectors stay unchanged).</summary>
        private static void Normalize(ref float x, ref float y, ref float z)
        {
            var length = (float)System.Math.Sqrt(x * x + y * y + z * z);
            var inverse = 1 / (length == 0 ? 1 : length);

            x *= inverse;
            y *= inverse;
            z *= inverse;
        }
        #endregion

    }
}

