using System.Collections;
using System.Collections.Generic;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Geometries
{
    public class SphereGeometry : Geometry
    {
        public Hashtable parameters;

        public SphereGeometry(double radius, double? widthSegments = null, double? heightSegments = null, double? phiStart = null, double? phiLength = null, double? thetaStart = null, double? thetaLength = null)
            : base()
        {
            parameters = new Hashtable()
            {
                {"radius", radius},
                {"withSegments", widthSegments},
                {"heightSegments",heightSegments},
                {"phiStart", phiStart != null ? (double)phiStart : 0},
                {"phiLength", phiLength != null ? (double)phiLength : 2 * (double)Mathf.PI},
                {"thetaStart", thetaStart!=null ? (double)thetaStart : 0},
                {"thetaLength", thetaLength!=null ? (double)thetaLength : (double)Mathf.PI}
            };

            this.FromBufferGeometry(new SphereBufferGeometry(radius, widthSegments, heightSegments, phiStart, phiLength, thetaStart, thetaLength));
            this.MergeVertices();
        }
    }

    public class SphereBufferGeometry : BufferGeometry
    {
        public Hashtable parameters;

        public SphereBufferGeometry(double radius, double? widthSegments = null, double? heightSegments = null, double? phiStart = null, double? phiLength = null, double? thetaStart = null, double? thetaLength = null)
            : base()
        {
            radius = radius != 0 ? radius : 1;

            if (widthSegments == null) widthSegments = 8;
            if (heightSegments == null) heightSegments = 6;

            widthSegments = (double)Mathf.Max(3, Mathf.Floor(widthSegments.Value));

            heightSegments = (double)Mathf.Max(2, Mathf.Floor(heightSegments.Value));

            phiStart = phiStart != null ? (double)phiStart : 0;
            phiLength = phiLength != null ? (double)phiLength : 2 * (double)Mathf.PI; ;
            thetaStart = thetaStart != null ? (double)thetaStart : 0;
            thetaLength = thetaLength != null ? (double)thetaLength : (double)Mathf.PI;

            parameters = new Hashtable()
            {
                {"radius", radius},
                {"withSegments", widthSegments},
                {"heightSegments",heightSegments},
                {"phiStart", phiStart != null ? (double)phiStart : 0},
                {"phiLength", phiLength != null ? (double)phiLength : 2 * (double)Mathf.PI},
                {"thetaStart", thetaStart!=null ? (double)thetaStart : 0},
                {"thetaLength", thetaLength!=null ? (double)thetaLength : (double)Mathf.PI}
            };

            var thetaEnd = (double)Mathf.Min((double)(thetaStart + thetaLength), Mathf.PI);

            List<uint> indices = new List<uint>();
            List<double> vertices = new List<double>();
            List<double> normals = new List<double>();
            List<double> uvs = new List<double>();
            List<List<int>> grid = new List<List<int>>();

            int index = 0;

            Vector3 vertex = new Vector3();
            Vector3 normal = new Vector3();

            for (int iy = 0; iy <= heightSegments; iy++)
            {

                List<int> verticesRow = new List<int>();

                double v = iy / (double)heightSegments;

                // special case for the poles

                var uOffset = 0.0;

                if (iy == 0 && thetaStart == 0)
                {

                    uOffset = 0.5 / (double)widthSegments;

                }
                else if (iy == heightSegments && thetaEnd == (double)Mathf.PI)
                {

                    uOffset = -0.5 / (double)widthSegments;

                }

                for (int ix = 0; ix <= widthSegments; ix++)
                {

                    double u = ix / (double)widthSegments;

                    // vertex

                    vertex.X = (double)(-radius * Mathf.Cos((double)phiStart + u * (double)phiLength) * Mathf.Sin((double)thetaStart + v * (double)thetaLength));
                    vertex.Y = (double)(radius * Mathf.Cos((double)thetaStart + v * (double)thetaLength));
                    vertex.Z = (double)(radius * Mathf.Sin((double)phiStart + u * (double)phiLength) * Mathf.Sin((double)thetaStart + v * (double)thetaLength));

                    vertices.Add(vertex.X);
                    vertices.Add(vertex.Y);
                    vertices.Add(vertex.Z);

                    // normal

                    normal.Copy(vertex).Normalize();
                    normals.Add(normal.X);
                    normals.Add(normal.Y);
                    normals.Add(normal.Z);

                    // uv

                    uvs.Add(u + uOffset);
                    uvs.Add(1 - v);

                    verticesRow.Add(index++);

                }

                grid.Add(verticesRow);
            }

            // indices

            for (int iy = 0; iy < (int)heightSegments; iy++)
            {

                for (int ix = 0; ix < (int)widthSegments; ix++)
                {

                    var a = grid[iy][ix + 1];
                    var b = grid[iy][ix];
                    var c = grid[iy + 1][ix];
                    var d = grid[iy + 1][ix + 1];

                    if (iy != 0 || thetaStart > 0)
                    {
                        indices.Add((uint)a);
                        indices.Add((uint)b);
                        indices.Add((uint)d);
                    }
                    if (iy != (int)(heightSegments - 1) || thetaEnd < (double)Mathf.PI)
                    {
                        indices.Add((uint)b);
                        indices.Add((uint)c);
                        indices.Add((uint)d);
                    }
                }
            }

            // build geometry

            this.SetIndex(new BufferAttribute<uint>(indices.ToArray(), 1));
            this.SetAttribute("position", new BufferAttribute<double>(vertices.ToArray(), 3));
            this.SetAttribute("normal", new BufferAttribute<double>(normals.ToArray(), 3));
            this.SetAttribute("uv", new BufferAttribute<double>(uvs.ToArray(), 2));
        }
    }
}
