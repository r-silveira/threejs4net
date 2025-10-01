using System;
using System.Collections.Generic;

namespace ThreeJs4Net.Math
{
    public static class Mat
    {
        private static readonly Random random = new Random();

        public static double RadToDeg(double rad)
        {
            return (rad * 180.0 / System.Math.PI);
        }

        public static double DegToRad(double deg)
        {
            return (System.Math.PI * deg / 180.0);
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static double Clamp(double val, double min, double max) 
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static double Random()
        {
            return (double)random.NextDouble();
        }

        public static Color Random(this Color value)
        {
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="values"></param>
        public static void Add(this List<double> list, params double[] values)
        {
            list.AddRange(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static double hue2rgb (double p, double q, double t )
        {
		    if ( t < 0.0f ) t += 1;
		    if ( t > 1.0f ) t -= 1;
		    if ( t < 1.0f / 6.0f ) return p + ( q - p ) * 6 * t;
		    if ( t < 1.0f / 2.0f ) return q;
		    if ( t < 2.0f / 3.0f ) return p + ( q - p ) * 6 * ( 2 / 3.0f - t );

		    return p;
		}

        public static double Lerp(this double x, double y, double t)
        {
            return (1 - t) * x + t * y;

        }

        public static List<T> Add<T>(this List<T> source, T x, T y, T z)
        {
            source.Add(x);
            source.Add(y);
            source.Add(z);

            return source;
        }
    }
}
