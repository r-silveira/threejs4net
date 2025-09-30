namespace ThreeJs4Net.Math
{
    public class MathUtils
    {
        public static double DEG2RAD = (double)(System.Math.PI / 180);
        public static double RAD2DEG = (double)(180 / System.Math.PI);
        public static double EPS = (double)0.0001;
        public static double EPS3 = (double)0.001;
        public static double EPS5 = (double)0.00001;

        public static double Clamp(double value, double min, double max)
        {
            return System.Math.Max(min, System.Math.Min(max, value));
        }
    }
}
