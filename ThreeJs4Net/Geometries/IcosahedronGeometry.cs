using System.Collections.Generic;
using ThreeJs4Net.Core;

namespace ThreeJs4Net.Geometries
{
    public class IcosahedronGeometry : Geometry
    {
        public IcosahedronGeometry(double radius = 1, double detail = 0)
        {
            FromBufferGeometry(new IcosahedronBufferGeometry(radius, detail));
            MergeVertices();
        }
    }

    public class IcosahedronBufferGeometry : PolyhedronBufferGeometry
    {
        static double t = (1 + (double)System.Math.Sqrt(5)) / 2;

        private new static List<double> Vertices { get; } = new List<double>
        {
            - 1, t, 0,  1, t, 0,    - 1, - t, 0,    1, - t, 0,
            0, - 1, t,     0, 1, t,    0, - 1, - t,    0, 1, - t,
            t, 0, - 1,     t, 0, 1,    - t, 0, - 1,    - t, 0, 1
        };


        private new static List<int> Indices { get; } = new List<int>
        {
            0, 11, 5,   0, 5, 1,    0, 1, 7,    0, 7, 10,   0, 10, 11,
            1, 5, 9,   5, 11, 4,   11, 10, 2,  10, 7, 6,   7, 1, 8,
            3, 9, 4,   3, 4, 2,    3, 2, 6,    3, 6, 8,    3, 8, 9,
            4, 9, 5,   2, 4, 11,   6, 2, 10,   8, 6, 7,    9, 8, 1
        };

        public IcosahedronBufferGeometry(double radius = 1, double detail = 0) : base(Vertices, Indices, radius, detail)
        {
        }
    }
}
