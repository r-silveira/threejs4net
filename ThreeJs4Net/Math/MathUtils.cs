using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeJs4Net.Math
{
    public class MathUtils
    {
        public static float DEG2RAD = (float)(System.Math.PI / 180);
        public static float RAD2DEG = (float)(180 / System.Math.PI);
        public static float EPS = (float)0.0001;
        public static float EPS3 = (float)0.001;
        public static float EPS5 = (float)0.00001;

        public static float Clamp(float value, float min, float max)
        {
            return System.Math.Max(min, System.Math.Min(max, value));
        }

        /// <summary>
        /// Random (version 4, RFC 4122 variant) Guid for object ids, like three.js MathUtils.generateUUID,
        /// which is also based on a non-cryptographic generator.
        /// <para>
        /// Guid.NewGuid() asks the OS for cryptographically secure bytes on every call, which dominated the
        /// cost of constructing (and therefore cloning/merging) small geometries. The ids only need to be
        /// unique, not unpredictable; 122 random bits from the thread-safe shared generator give the same
        /// collision odds as Guid.NewGuid().
        /// </para>
        /// </summary>
        internal static Guid GenerateUuid()
        {
            Span<byte> bytes = stackalloc byte[16];
            Random.Shared.NextBytes(bytes);

            // Guid byte layout is little-endian for the first three fields: the version nibble is the
            // high nibble of byte 7, the variant bits are the two high bits of byte 8.
            bytes[7] = (byte)((bytes[7] & 0x0F) | 0x40);
            bytes[8] = (byte)((bytes[8] & 0x3F) | 0x80);

            return new Guid(bytes);
        }
    }
}
