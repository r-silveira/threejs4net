﻿using System.Collections.Generic;
using System.Linq;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Extras
{
    class ShapeUtils
    {
        public static double Area(Vector3[] contour)
        {
            var n = contour.Length;
            double a = 0.0;
            var p = n - 1;

            for (var q = 0; q < n; p = q++)
            {
                a += contour[p].X * contour[q].Y - contour[q].X * contour[p].Y;
            }

            return a * 0.5f;
        }

        public static bool IsClockWise(IEnumerable<Vector3> pts)
        {
            return ShapeUtils.Area(pts.ToArray()) < 0;
        }

        public static List<List<int>> TriangulateShape(List<Vector3> contour, List<List<Vector3>> holes)
        {
            var vertices = new List<double>(); // flat array of vertices like [ x0,y0, x1,y1, x2,y2, ... ]
            var holeIndices = new List<int>(); // array of hole indices
            var faces = new List<List<int>>(); // final array of vertex indices like [ [ a,b,d ], [ b,c,d ] ]

            List<Vector2> contour2 = new List<Vector2>();
            List<List<Vector2>> holes2 = new List<List<Vector2>>();


            RemoveDupEndPts(contour);
            AddContour(vertices, contour);

            holes.ForEach(delegate (List<Vector3> e)
            {
                RemoveDupEndPts(e);
            });


            //

            var holeIndex = contour.Count;



            for (var i = 0; i < holes.Count; i++)
            {

                holeIndices.Add(holeIndex);
                holeIndex += holes[i].Count;
                AddContour(vertices, holes[i]);

            }

            //

            var triangles = new Earcut().Triangulate(vertices, holeIndices);

            //

            for (var i = 0; i < triangles.Count; i += 3)
            {

                faces.Add(triangles.GetRange(i, 3));

            }

            return faces;

        }

        public static void RemoveDupEndPts(List<Vector3> points)
        {

            var l = points.Count;

            if (l > 2 && points[l - 1].Equals(points[0]))
            {

                points.RemoveAt(points.Count - 1);

            }

        }

        public static void AddContour(List<double> vertices, List<Vector3> contour)
        {

            for (var i = 0; i < contour.Count; i++)
            {

                vertices.Add(contour[i].X);
                vertices.Add(contour[i].Y);

            }

        }

    }
}