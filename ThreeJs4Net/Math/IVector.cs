namespace ThreeJs4Net.Math
{
    public interface IVector<T>
    {
        T SetComponent(int index, double value);
        public double GetComponent(int index);
        T Copy(T v);
        T Add(T v);

        //      set( ...args: number[] ): this;

        T SetScalar(double scalar);
        T AddVectors(T v1, T v2);
        T AddScaledVector(T v, double s);
        T AddScalar(double scalar);
        T Sub(T v);
        T SubVectors(T a, T b);
        T MultiplyScalar(double scalar);
        T DivideScalar(double scalar);
        T Negate();
        double Dot(T v);
        double LengthSq();
        double Length();
        T Normalize();
        //* NOTE: Vector4 doesn't have the property.
        public double DistanceTo(T vector);
        //* NOTE: Vector4 doesn't have the property.
        public double DistanceToSquared(T vector);
        T SetLength(double length);
        T Lerp(T v, double alpha);
        bool Equals(T v);
        T Clone();
    }
}
