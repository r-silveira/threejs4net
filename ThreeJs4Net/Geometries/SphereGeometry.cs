using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Geometries
{
    public class SphereGeometry : Geometry
    {
        public Hashtable parameters;

        public SphereGeometry(float radius, float? widthSegments = null, float? heightSegments = null, float? phiStart = null, float? phiLength = null, float? thetaStart = null, float? thetaLength = null)
            : base()
        {
            parameters = new Hashtable()
            {
                {"radius", radius},
                {"withSegments", widthSegments},
                {"heightSegments",heightSegments},
                {"phiStart", phiStart != null ? (float)phiStart : 0},
                {"phiLength", phiLength != null ? (float)phiLength : 2 * (float)Mathf.PI},
                {"thetaStart", thetaStart!=null ? (float)thetaStart : 0},
                {"thetaLength", thetaLength!=null ? (float)thetaLength : (float)Mathf.PI}
            };

            this.FromBufferGeometry(new SphereBufferGeometry(radius, widthSegments, heightSegments, phiStart, phiLength, thetaStart, thetaLength));
            this.MergeVertices();
        }
    }

    public class SphereBufferGeometry : BufferGeometry
    {
        public Hashtable parameters;

        public SphereBufferGeometry(float radius, float? widthSegments = null, float? heightSegments = null, float? phiStart = null, float? phiLength = null, float? thetaStart = null, float? thetaLength = null)
            : base()
        {
            radius = radius != 0 ? radius : 1;

            if (widthSegments == null) widthSegments = 8;
            if (heightSegments == null) heightSegments = 6;

            widthSegments = (float)Mathf.Max(3, Mathf.Floor(widthSegments.Value));

            heightSegments = (float)Mathf.Max(2, Mathf.Floor(heightSegments.Value));

            phiStart = phiStart != null ? (float)phiStart : 0;
            phiLength = phiLength != null ? (float)phiLength : 2 * (float)Mathf.PI; ;
            thetaStart = thetaStart != null ? (float)thetaStart : 0;
            thetaLength = thetaLength != null ? (float)thetaLength : (float)Mathf.PI;

            parameters = new Hashtable()
            {
                {"radius", radius},
                {"withSegments", widthSegments},
                {"heightSegments",heightSegments},
                {"phiStart", phiStart != null ? (float)phiStart : 0},
                {"phiLength", phiLength != null ? (float)phiLength : 2 * (float)Mathf.PI},
                {"thetaStart", thetaStart!=null ? (float)thetaStart : 0},
                {"thetaLength", thetaLength!=null ? (float)thetaLength : (float)Mathf.PI}
            };

            var thetaEnd = (float)Mathf.Min((float)(thetaStart + thetaLength), Mathf.PI);

            List<uint> indices = new List<uint>();
            List<float> vertices = new List<float>();
            List<float> normals = new List<float>();
            List<float> uvs = new List<float>();
            List<List<int>> grid = new List<List<int>>();

            int index = 0;

            Vector3 vertex = new Vector3();
            Vector3 normal = new Vector3();

            for (int iy = 0; iy <= heightSegments; iy++)
            {

                List<int> verticesRow = new List<int>();

                float v = iy / (float)heightSegments;

                // special case for the poles

                var uOffset = 0.0f;

                if (iy == 0 && thetaStart == 0)
                {

                    uOffset = 0.5f / (float)widthSegments;

                }
                else if (iy == heightSegments && thetaEnd == (float)Mathf.PI)
                {

                    uOffset = -0.5f / (float)widthSegments;

                }

                for (int ix = 0; ix <= widthSegments; ix++)
                {

                    float u = ix / (float)widthSegments;

                    // vertex

                    vertex.X = (float)(-radius * Mathf.Cos((float)phiStart + u * (float)phiLength) * Mathf.Sin((float)thetaStart + v * (float)thetaLength));
                    vertex.Y = (float)(radius * Mathf.Cos((float)thetaStart + v * (float)thetaLength));
                    vertex.Z = (float)(radius * Mathf.Sin((float)phiStart + u * (float)phiLength) * Mathf.Sin((float)thetaStart + v * (float)thetaLength));

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
                    if (iy != (int)(heightSegments - 1) || thetaEnd < (float)Mathf.PI)
                    {
                        indices.Add((uint)b);
                        indices.Add((uint)c);
                        indices.Add((uint)d);
                    }
                }
            }

            // build geometry

            this.SetIndex(new BufferAttribute<uint>(indices.ToArray(), 1));
            this.SetAttribute("position", new BufferAttribute<float>(vertices.ToArray(), 3));
            this.SetAttribute("normal", new BufferAttribute<float>(normals.ToArray(), 3));
            this.SetAttribute("uv", new BufferAttribute<float>(uvs.ToArray(), 2));
        }
    }
}
