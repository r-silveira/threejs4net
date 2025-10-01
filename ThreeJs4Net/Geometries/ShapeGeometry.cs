using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThreeJs4Net.Core;
using ThreeJs4Net.Extras;
using ThreeJs4Net.Extras.Core;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Geometries
{
    [Serializable]
    public class ShapeGeometry : Geometry
    {
        public Hashtable parameter;

        public ShapeGeometry(List<Shape> shapes, double? curveSegments = null) : base()
        {
            parameter = new Hashtable()
            {
                {"shapes",shapes },
                {"curveSegments",curveSegments }
            };

            this.FromBufferGeometry(new ShapeBufferGeometry(shapes, curveSegments));
            this.MergeVertices();
        }
    }

    [Serializable]
    public class ShapeBufferGeometry : BufferGeometry
    {
        public Hashtable parameter;

        private List<int> indices = new List<int>();
        private List<double> vertices = new List<double>();
        private List<double> normals = new List<double>();
        private List<double> uvs = new List<double>();

        private double CurveSegments;

        int groupStart = 0;

        int groupCount = 0;

        public ShapeBufferGeometry(Shape shape, double? curveSegments = null) : base()
        {
            parameter = new Hashtable()
            {
                {"shapes",shape },
                {"curveSegments",curveSegments }
            };

            CurveSegments = curveSegments != null ? curveSegments.Value : 12;

            AddShape(shape);

            this.SetIndex(indices);

            BufferAttribute<double> positions = new BufferAttribute<double>(vertices.ToArray(), 3);


            this.SetAttribute("position", positions);

            BufferAttribute<double> normalAttributes = new BufferAttribute<double>(normals.ToArray(), 3);

            this.SetAttribute("normal", normalAttributes);

            BufferAttribute<double> uvAttributes = new BufferAttribute<double>(uvs.ToArray(), 2);

            this.SetAttribute("uv", uvAttributes);

        }
        public ShapeBufferGeometry(List<Shape> shapes, double? curveSegments = null) : base()
        {
            parameter = new Hashtable()
            {
                {"shapes",shapes },
                {"curveSegments",curveSegments }
            };

            CurveSegments = curveSegments != null ? curveSegments.Value : 12;


            // helper variables



            if (shapes.Count == 1)
            {
                AddShape(shapes[0]);
            }
            else
            {
                for (int i = 0; i < shapes.Count; i++)
                {
                    AddShape(shapes[i]);
                    this.AddGroup(groupStart, groupCount, i);

                    groupStart += groupCount;
                    groupCount = 0;
                }
            }

            this.SetIndex(indices);

            BufferAttribute<double> positions = new BufferAttribute<double>(vertices.ToArray(), 3);
            //positions.ItemSize = 3;
            //positions.Type = typeof(double);

            this.SetAttribute("position", positions);

            BufferAttribute<double> normalAttributes = new BufferAttribute<double>(normals.ToArray(), 3);
            //normalAttributes.ItemSize = 3;
            //normalAttributes.Type = typeof(double);
            this.SetAttribute("normal", normalAttributes);

            BufferAttribute<double> uvAttributes = new BufferAttribute<double>(uvs.ToArray(), 2);
            //uvAttributes.ItemSize = 2;
            //uvAttributes.Type = typeof(double);
            this.SetAttribute("uv", uvAttributes);
        }
        private void AddShape(Shape shape)
        {
            int i, l;

            List<Vector3> shapeHole = null;

            var indexOffset = vertices.Count / 3;
            var points = shape.ExtractPoints(CurveSegments);

            var shapeVertices = (List<Vector3>)points["shape"];
            var shapeHoles = (List<List<Vector3>>)points["holes"];

            // check direction of vertices

            if (ShapeUtils.IsClockWise(shapeVertices) == false)
            {

                shapeVertices.Reverse();



                for (i = 0, l = shapeHoles.Count; i < l; i++)
                {

                    shapeHole = shapeHoles[i];

                    if (ShapeUtils.IsClockWise(shapeHole) == true)
                    {
                        shapeHole.Reverse();
                        shapeHoles[i] = shapeHole;

                    }

                }
            }
            var faces = ShapeUtils.TriangulateShape(shapeVertices, shapeHoles);

            // join vertices of inner and outer paths to a single array

            for (i = 0, l = shapeHoles.Count; i < l; i++)
            {

                shapeHole = shapeHoles[i];
                shapeVertices = shapeVertices.Concat(shapeHole).ToList();

            }

            // vertices, normals, uvs

            for (i = 0, l = shapeVertices.Count; i < l; i++)
            {

                var vertex = shapeVertices[i];

                vertices.Add(vertex.X, vertex.Y, vertex.Z);
                normals.Add(0, 0, 1);
                uvs.Add(vertex.X, vertex.Y); // world uvs

            }

            // incides

            for (i = 0, l = faces.Count; i < l; i++)
            {

                var face = faces[i];

                var a = face[0] + indexOffset;
                var b = face[1] + indexOffset;
                var c = face[2] + indexOffset;

                indices.Add(a, b, c);
                groupCount += 3;
            }

        }
    }
}
