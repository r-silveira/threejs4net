using System.Collections.Generic;
using System.Diagnostics;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Geometries
{
    public class CylinderGeometry : Geometry
    {
        public double RadiusTop { get; }
        public double RadiusBottom { get; }
        public double Height { get; }
        public int RadialSegments { get; }
        public int HeightSegments { get; }
        public bool OpenEnded { get; }
        public double ThetaStart { get; }
        public double ThetaLength { get; }

        public CylinderGeometry(double radiusTop = 20, double radiusBottom = 20, double height = 10, int radialSegments = 8, int heightSegments = 1, bool openEnded = false, double thetaStart = 0, double thetaLength = Mathf.PI * 2)
            : base()
        {
            RadiusTop = radiusTop;
            RadiusBottom = radiusBottom;
            Height = height;

            RadialSegments = radialSegments;
            HeightSegments = heightSegments;

            ThetaLength = thetaLength;
            ThetaStart = thetaStart;

            OpenEnded = openEnded;

            FromBufferGeometry(new CylinderBufferGeometry(radiusTop, radiusBottom, height, radialSegments, heightSegments, openEnded, thetaStart, thetaLength));
            MergeVertices();            
        }
    }

    public class CylinderBufferGeometry : BufferGeometry
    {
        public double RadiusTop { get; }
        public double RadiusBottom { get; }
        public double Height { get; }
        public int RadialSegments { get; }
        public int HeightSegments { get; }
        public bool OpenEnded { get; }
        public double ThetaStart { get; }
        public double ThetaLength { get; }
        private List<uint> indices = new List<uint>();
        private List<double> vertices = new List<double>();
        private List<double> normals = new List<double>();
        private List<double> uvs = new List<double>();
        private List<List<int>> indexArray = new List<List<int>>();
        private double halfHeight;
        private int groupStart;
        private int index;

        public CylinderBufferGeometry(double radiusTop = 20, double radiusBottom = 20, double height = 10, int radialSegments = 8, int heightSegments = 1, bool openEnded = false, double thetaStart = 0, double thetaLength = Mathf.PI * 2)
        {
            RadiusTop = radiusTop;
            RadiusBottom = radiusBottom;
            Height = height != 0 ? height : 1;

            RadialSegments = radialSegments;
            HeightSegments = heightSegments;

            OpenEnded = openEnded;
            ThetaStart = thetaStart;
            ThetaLength = thetaLength;

            halfHeight = height / 2;

            GenerateTorso();

            if (!OpenEnded)
            {
                if (RadiusTop > 0) GenerateCap(true);
                if (RadiusBottom > 0) GenerateCap(false);
            }

            this.SetIndex(new BufferAttribute<uint>(indices.ToArray(), 1));
            SetAttribute("position", new BufferAttribute<double>(vertices.ToArray(), 3));
            SetAttribute("normal", new BufferAttribute<double>(normals.ToArray(), 3));
            SetAttribute("uv", new BufferAttribute<double>(uvs.ToArray(), 2));
        }

        private void GenerateTorso()
        {
            int x, y;
            int groupCount = 0;

            Vector3 normal = Vector3.Zero();
            Vector3 vertex = Vector3.Zero();

            double slope = (RadiusBottom - RadiusTop) / Height;

            for (y = 0; y <= HeightSegments; y++)
            {
                List<int> indexRow = new List<int>();

                double v = y / (double)HeightSegments;

                double radius = v * (RadiusBottom - RadiusTop) + RadiusTop;

                for (x = 0; x <= RadialSegments; x++)
                {
                    double u = x / (double)RadialSegments;
                    double theta = u * ThetaLength + ThetaStart;

                    double sinTheta = (double)System.Math.Sin(theta);
                    double cosTheta = (double)System.Math.Cos(theta);

                    //vertex

                    vertex.X = radius * sinTheta;
                    vertex.Y = -v * Height + halfHeight;
                    vertex.Z = radius * cosTheta;
                    vertices.Add(vertex.X); vertices.Add(vertex.Y); vertices.Add(vertex.Z);

                    //normal

                    normal.Set(sinTheta, slope, cosTheta).Normalize();
                    normals.Add(normal.X); normals.Add(normal.Y); normals.Add(normal.Z);

                    //uv

                    uvs.Add(u); uvs.Add(1 - v);
                    indexRow.Add(index++);
                }
                indexArray.Add(indexRow);
            }

            // generate indices

            for (x = 0; x < RadialSegments; x++)
            {
                for (y = 0; y < HeightSegments; y++)
                {
                    uint a = (uint)indexArray[y][x];
                    uint b = (uint)indexArray[y + 1][x];
                    uint c = (uint)indexArray[y + 1][x + 1];
                    uint d = (uint)indexArray[y][x + 1];

                    indices.Add(a); indices.Add(b); indices.Add(d);
                    indices.Add(b); indices.Add(c); indices.Add(d);

                    groupCount += 6;
                }
            }

            AddGroup(groupStart, groupCount);

            groupStart += groupCount;
        }

        private void GenerateCap(bool top)
        {
            int x, centerIndexStart, centerIndexEnd;

            Vector2 uv = new Vector2();
            Vector3 vertex = new Vector3();

            int groupCount = 0;

            double radius = top ? RadiusTop : RadiusBottom;
            double sign = top ? 1 : -1;

            centerIndexStart = index;

            for (x = 1; x <= RadialSegments; x++)
            {
                vertices.Add(0); vertices.Add(halfHeight * sign); vertices.Add(0);
                normals.Add(0); normals.Add(sign); normals.Add(0);
                uvs.Add(0.5f); uvs.Add(0.5f);
                index++;
            }

            centerIndexEnd = index;

            for (x = 0; x <= RadialSegments; x++)
            {
                double u = x / (double)RadialSegments;
                double theta = u * ThetaLength + ThetaStart;

                double cosTheta = (double)System.Math.Cos(theta);
                double sinTheta = (double)System.Math.Sin(theta);

                //vertex

                vertex.X = radius * sinTheta;
                vertex.Y = halfHeight * sign;
                vertex.Z = radius * cosTheta;

                vertices.Add(vertex.X); vertices.Add(vertex.Y); vertices.Add(vertex.Z);

                //normal
                normals.Add(0); normals.Add(sign); normals.Add(0);

                //uv

                uv.X = (cosTheta * 0.5f) + 0.5f;
                uv.Y = (sinTheta * 0.5f * sign) + 0.5f;
                uvs.Add(uv.X); uvs.Add(uv.Y);

                index++;
            }

            //generate indices

            for (x = 0; x < RadialSegments; x++)
            {
                uint c = (uint)(centerIndexStart + x);
                uint i = (uint)(centerIndexEnd + x);

                if (top)
                {
                    indices.Add(i); indices.Add(i + 1); indices.Add(c);
                }
                else
                {
                    indices.Add(i + 1); indices.Add(i); indices.Add(c);
                }
                groupCount += 3;
            }

            AddGroup(groupStart, groupCount, top ? 1 : 2);

            groupStart += groupCount;
        }

    }
}
