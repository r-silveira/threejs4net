using System;
using System.Collections.Generic;

namespace ThreeJs4Net.Math
{
    public static class Mathf
    {
        public const double Tau = 6.2831853071795864769252;
        public const double PI = Tau / 2.0;
        public const double NaturalLog2 = 0.69314718056;

        public static double Sign(double value) { return (double)System.Math.Sign(value); }
        public static double Cos(double value) { return (double)System.Math.Cos(value); }
        public static double Sin(double value) { return (double)System.Math.Sin(value); }
        public static double Tan(double value) { return (double)System.Math.Tan(value); }

        public static double Acos(double value) { return (double)System.Math.Acos(value); }
        public static double Asin(double value) { return (double)System.Math.Asin(value); }
        public static double Atan(double value) { return (double)System.Math.Atan(value); }
        public static double Atan2(double y, double x) { return (double)System.Math.Atan2(y, x); }

        public static double Clamp(double x, double a, double b) { return (x < a) ? a : ((x > b) ? b : x); }

        public static double DegreesToRadians(double degree) { return degree * Tau / 360; }
        public static double RadiansToDegrees(double radians) { return radians * 360 / Tau; }

        public static int Abs(int value) { return System.Math.Abs(value); }
        public static double Abs(double value) { return System.Math.Abs(value); }

        public static double Sqrt(double value) { return (double)System.Math.Sqrt(value); }

        public static int Min(int a, int b) { return System.Math.Min(a, b); }
        public static double Min(double a, double b) { return System.Math.Min(a, b); }
        public static int Max(int a, int b) { return System.Math.Max(a, b); }
        public static double Max(double a, double b) { return System.Math.Max(a, b); }


        public static bool IsPowerOfTwo(int value) { return (value & (value - 1)) == 0 && value != 0; }

        internal static int Pow(int x, int y) { return (int)System.Math.Pow(x, y); }
        internal static double Pow(double x, double y) { return (double)System.Math.Pow(x, y); }

        public static int Round(double value, MidpointRounding midpoint = MidpointRounding.ToEven) { return (int)System.Math.Round(value, midpoint); }
        public static int Round(double value, int decimals) { return (int)System.Math.Round(value, decimals); }

        internal static int Floor(double value) { return (int)System.Math.Floor(value); }
        internal static int Ceiling(double value) { return (int)System.Math.Ceiling(value); }

        internal static double Log(double value) { return (double)System.Math.Log(value); }

        public static Random Random = new Random();

        public static double RandomF(double a, double b)
        {
            return (double)(a + Random.NextDouble() * (b - a));
        }

        public static double RandomF(double a)
        {
            return RandomF() * a;
        }

        public static double RandomF()
        {
            return (double)Random.NextDouble();
        }

        public static bool RandomBool => RandomF() > 0.5f;

        public static List<Vector3> Hilbert3D(Vector3 center, double size = 10, int iterations = 1, int v0 = 0, int v1 = 1, int v2 = 2, int v3 = 3, int v4 = 4, int v5 = 5, int v6 = 6, int v7 = 7)
        {
            var half = size / 2;

            var s = new Vector3[]{
                new Vector3( center.X - half, center.Y + half, center.Z - half ),
                new Vector3( center.X - half, center.Y + half, center.Z + half ),
                new Vector3( center.X - half, center.Y - half, center.Z + half ),
                new Vector3( center.X - half, center.Y - half, center.Z - half ),
                new Vector3( center.X + half, center.Y - half, center.Z - half ),
                new Vector3( center.X + half, center.Y - half, center.Z + half ),
                new Vector3( center.X + half, center.Y + half, center.Z + half ),
                new Vector3( center.X + half, center.Y + half, center.Z - half )
            };

            var vec = new List<Vector3>()
            {
                s[ v0 ],
                s[ v1 ],
                s[ v2 ],
                s[ v3 ],
                s[ v4 ],
                s[ v5 ],
                s[ v6 ],
                s[ v7 ]
            };

            // Recurse iterations
            if (--iterations >= 0)
            {
                var tmp = new List<Vector3>();
                tmp.AddRange(Hilbert3D(vec[0], half, iterations, v0, v3, v4, v7, v6, v5, v2, v1));
                tmp.AddRange(Hilbert3D(vec[1], half, iterations, v0, v7, v6, v1, v2, v5, v4, v3));
                tmp.AddRange(Hilbert3D(vec[2], half, iterations, v0, v7, v6, v1, v2, v5, v4, v3));
                tmp.AddRange(Hilbert3D(vec[3], half, iterations, v2, v3, v0, v1, v6, v7, v4, v5));
                tmp.AddRange(Hilbert3D(vec[4], half, iterations, v2, v3, v0, v1, v6, v7, v4, v5));
                tmp.AddRange(Hilbert3D(vec[5], half, iterations, v4, v3, v2, v5, v6, v1, v0, v7));
                tmp.AddRange(Hilbert3D(vec[6], half, iterations, v4, v3, v2, v5, v6, v1, v0, v7));
                tmp.AddRange(Hilbert3D(vec[7], half, iterations, v6, v5, v2, v1, v0, v3, v4, v7));
                return tmp; // Return recursive call
            }

            return vec; // Return complete Hilbert Curve.
        }

        public static double Fit(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            if (oldMin > oldMax)
            {
                var tmp = oldMin;
                oldMin = oldMax;
                oldMax = tmp;
            }

            return (((newMax - newMin) * (value - oldMin)) / (oldMax - oldMin)) + newMin;
        }
    }
}
