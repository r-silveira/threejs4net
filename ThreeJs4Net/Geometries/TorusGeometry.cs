using System.Collections;
using System.Collections.Generic;
using ThreeJs4Net.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Geometries
{
    public class TorusGeometry : Geometry
    {
        public Hashtable parameters;
        public TorusGeometry(float? radius = null, float? tube = null, float? radialSegments = null, float? tubularSegments = null, float? arc = null) : base()
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

        public TorusBufferGeometry(float? radius = null, float? tube = null, float? radialSegments = null, float? tubularSegments = null, float? arc = null) : base()
        {
            radius = radius != null ? radius : 1;
            tube = tube != null ? tube : 1;
            radialSegments = radialSegments != null ? (float)Mathf.Floor(radialSegments.Value) : 8;
            tubularSegments = tubularSegments != null ? (float)Mathf.Floor(tubularSegments.Value) : 6;
            arc = arc != null ? arc : (float)Mathf.PI * 2;
            parameters = new Hashtable()
            {
                {"radius",radius },
                {"tube",radius },
                {"radialSegments",radius },
                {"tubularSegments",radius },
                {"arc",radius },
            };

            List<uint> indices = new List<uint>();
            List<float> vertices = new List<float>();
            List<float> normals = new List<float>();
            List<float> uvs = new List<float>();

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

                    float u = i / (float)tubularSegments * (float)arc;
                    float v = j / (float)radialSegments * (float)Mathf.PI * 2;

                    // vertex

                    vertex.X = (float)((radius + tube * Mathf.Cos(v)) * Mathf.Cos(u));
                    vertex.Y = (float)((radius + tube * Mathf.Cos(v)) * Mathf.Sin(u));
                    vertex.Z = (float)(tube * Mathf.Sin(v));

                    vertices.Add(vertex.X, vertex.Y, vertex.Z);

                    // normal

                    center.X = radius.Value * (float)Mathf.Cos(u);
                    center.Y = radius.Value * (float)Mathf.Sin(u);
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

            this.SetAttribute("position", new BufferAttribute<float>(vertices.ToArray(), 3));

            this.SetAttribute("normal", new BufferAttribute<float>(normals.ToArray(), 3));

            this.SetAttribute("uv", new BufferAttribute<float>(uvs.ToArray(), 2));
        }
    }
}
