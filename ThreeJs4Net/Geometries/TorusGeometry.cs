using System.Collections;
using System.Collections.Generic;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Geometries
{
    public class TorusGeometry : Geometry
    {
        public Hashtable parameters;
        public TorusGeometry(float? radius = null, double? tube = null, double? radialSegments = null, double? tubularSegments = null, double? arc = null) : base()
        {
            parameters = new Hashtable()
            {
                {"radius",radius },
                {"tube",radius },
                {"radialSegments",radius },
                {"tubularSegments",radius },
                {"arc",radius },
            };

            this.FromBufferGeometry(new TorusBufferGeometry(radius, tube, radialSegments, tubularSegments, arc));
            this.MergeVertices();
        }

    }

    public class TorusBufferGeometry : BufferGeometry
    {
        public Hashtable parameters;

        public TorusBufferGeometry(double? radius = null, double? tube = null, double? radialSegments = null, double? tubularSegments = null, double? arc = null) : base()
        {
            radius = radius != null ? radius : 1;
            tube = tube != null ? tube : 1;
            radialSegments = radialSegments != null ? (double)Mathf.Floor(radialSegments.Value) : 8;
            tubularSegments = tubularSegments != null ? (double)Mathf.Floor(tubularSegments.Value) : 6;
            arc = arc != null ? arc : (double)Mathf.PI * 2;
            parameters = new Hashtable()
            {
                {"radius",radius },
                {"tube",radius },
                {"radialSegments",radius },
                {"tubularSegments",radius },
                {"arc",radius },
            };

            List<uint> indices = new List<uint>();
            List<double> vertices = new List<double>();
            List<double> normals = new List<double>();
            List<double> uvs = new List<double>();

            // helper variables

            var center = new Vector3();
            var vertex = new Vector3();
            var normal = new Vector3();

            int j, i;

            // generate vertices, normals and uvs

            for (j = 0; j <= radialSegments; j++)
            {

                for (i = 0; i <= tubularSegments; i++)
                {

                    double u = i / (double)tubularSegments * (double)arc;
                    double v = j / (double)radialSegments * (double)Mathf.PI * 2;

                    // vertex

                    vertex.X = (double)((radius + tube * Mathf.Cos(v)) * Mathf.Cos(u));
                    vertex.Y = (double)((radius + tube * Mathf.Cos(v)) * Mathf.Sin(u));
                    vertex.Z = (double)(tube * Mathf.Sin(v));

                    vertices.Add(vertex.X, vertex.Y, vertex.Z);

                    // normal

                    center.X = radius.Value * (double)Mathf.Cos(u);
                    center.Y = radius.Value * (double)Mathf.Sin(u);
                    normal.SubVectors(vertex, center).Normalize();

                    normals.Add(normal.X, normal.Y, normal.Z);

                    // uv

                    uvs.Add(i / tubularSegments.Value);
                    uvs.Add(j / radialSegments.Value);

                }

            }

            // generate indices

            for (j = 1; j <= radialSegments; j++)
            {

                for (i = 1; i <= tubularSegments; i++)
                {

                    // indices

                    int a = ((int)tubularSegments + 1) * j + i - 1;
                    int b = ((int)tubularSegments + 1) * (j - 1) + i - 1;
                    int c = ((int)tubularSegments + 1) * (j - 1) + i;
                    int d = ((int)tubularSegments + 1) * j + i;

                    // faces

                    indices.Add((uint)a);
                    indices.Add((uint)b);
                    indices.Add((uint)d);
                    indices.Add((uint)b);
                    indices.Add((uint)c);
                    indices.Add((uint)d);
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
