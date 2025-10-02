using Xunit;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Extras.Core.Tests
{
    public class SplineCurveTests
    {
        [Fact()]
        public void SplineCurveTest()
        {
            var _curve = new SplineCurve(new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 5, 5, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 5, - 5, 0 ),
                new Vector3( 10, 0, 0 )
            });

            var expectedPoints = new Vector2[] {
                        new Vector2( - 10, 0 ),
                        new Vector2( - 6.08, 4.56 ),
                        new Vector2( - 2, 2.48 ),
                        new Vector2( 2, - 2.48 ),
                        new Vector2( 6.08, - 4.56 ),
                        new Vector2( 10, 0 )
                };

            var curve = _curve;
            var points = curve.GetPoints(5);
            Assert.Equal(expectedPoints.Length, points.Count);

            for (int i = 0; i < points.Count; i++)
            {
                Assert.True(expectedPoints[i].X - points[i].X <= MathUtils.EPS);
                Assert.True(expectedPoints[i].Y - points[i].Y <= MathUtils.EPS);
            }

            points = curve.GetPoints(4);
            Assert.Equal(curve.points, points);
        }

        [Fact()]
        public void SplineCurve_GetLength()
        {
            var _curve = new SplineCurve(new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 5, 5, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 5, - 5, 0 ),
                new Vector3( 10, 0, 0 )
            });

            var expectedPoints = new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 6.08, 4.56, 0 ),
                new Vector3( - 2, 2.48, 0 ),
                new Vector3( 2, - 2.48, 0 ),
                new Vector3( 6.08, - 4.56, 0 ),
                new Vector3( 10, 0, 0 )
            };

            var curve = _curve;

            var length = curve.GetLength();
            var expectedLength = 28.876950901868135;

            Assert.True(expectedLength - length <= MathUtils.EPS);

            var expectedLengths = new double[] { 0.0, Mathf.Sqrt(50), Mathf.Sqrt(200), Mathf.Sqrt(450), Mathf.Sqrt(800) };

            var lengths = curve.GetLengths(4);

            Assert.Equal(expectedLengths, lengths);

        }

        [Fact()]
        public void SplineCurve_GetPointAt()
        {
            var _curve = new SplineCurve(new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 5, 5, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 5, - 5, 0 ),
                new Vector3( 10, 0, 0 )
            });

            var expectedPoints = new Vector2[] {
                new Vector2( - 10, 0 ),
                new Vector2( - 6.08, 4.56 ),
                new Vector2( - 2, 2.48 ),
                new Vector2( 2, - 2.48 ),
                new Vector2( 6.08, - 4.56 ),
                new Vector2( 10, 0 )
            };

            var curve = _curve;
            var point = new Vector3();

            Assert.True(curve.GetPointAt(0, point).Equals(curve.points[0]));
            Assert.True(curve.GetPointAt(1, point).Equals(curve.points[4]));

            curve.GetPointAt(0.5f, point);

            Assert.True(0.0 - point.X <= MathUtils.EPS);
            Assert.True(0.0 - point.Y <= MathUtils.EPS);
        }

        [Fact()]
        public void SplineCurve_GetTangent()
        {
            var _curve = new SplineCurve(new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 5, 5, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 5, - 5, 0 ),
                new Vector3( 10, 0, 0 )
            });

            var expectedPoints = new Vector2[] {
                new Vector2( - 10, 0 ),
                new Vector2( - 6.08, 4.56 ),
                new Vector2( - 2, 2.48 ),
                new Vector2( 2, - 2.48 ),
                new Vector2( 6.08, - 4.56 ),
                new Vector2( 10, 0 )
            };

            var curve = _curve;
            var expectedTangent = new Vector2[] {
                    new Vector2( 0.7068243340243188, 0.7073891155729485 ), // 0
                    new Vector2( 0.7069654305325396, - 0.7072481035902046 ), // 0.5
                    new Vector2( 0.7068243340245123, 0.7073891155727552 ) // 1
                };

            var tangents = new Vector3[] {
                curve.GetTangent( 0, new Vector3() ),
                curve.GetTangent( 0.5, new Vector3() ),
                curve.GetTangent( 1, new Vector3() )
                };

            for (int i = 0; i < tangents.Length; i++)
            {
                Assert.True(expectedTangent[i].X - tangents[i].X <= MathUtils.EPS3);
                Assert.True(expectedTangent[i].Y - tangents[i].Y <= MathUtils.EPS3);

            }
        }

        [Fact()]
        public void SplineCurve_GetUtoTmapping()
        {
            var _curve = new SplineCurve(new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 5, 5, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 5, - 5, 0 ),
                new Vector3( 10, 0, 0 )
            });

            var curve = _curve;

            var start = curve.GetUtoTmapping(0, 0);
            var end = curve.GetUtoTmapping(0, curve.GetLength());
            var middle = curve.GetUtoTmapping(0.5, 0);

            Assert.Equal(0, start);
            Assert.Equal(1, end);
            Assert.True(0.5-middle <= MathUtils.EPS5);


        }


        [Fact()]
        public void SplineCurve_GetSpacedPoints()
        {
            var _curve = new SplineCurve(new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 5, 5, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 5, - 5, 0 ),
                new Vector3( 10, 0, 0 )
            });

            var expectedPoints = new Vector3[] {
                new Vector3( - 10, 0, 0 ),
                new Vector3( - 4.996509634683014, 4.999995128640857, 0 ),
                new Vector3( 0, 0, 0 ),
                new Vector3( 4.996509634683006, - 4.999995128640857, 0 ),
                new Vector3( 10, 0, 0 )
            };

            var curve = _curve;

            var points = curve.GetSpacedPoints(4);

            Assert.Equal(expectedPoints.Length, points.Count);

            for (int i = 0; i < points.Count; i++)
            {
                //Assert.Equal(expectedPoints[i].X, points[i].X);
                //Assert.Equal(expectedPoints[i].Y, points[i].Y);

                Assert.True(System.Math.Abs(expectedPoints[i].X - points[i].X) <= MathUtils.EPS3);
                Assert.True(System.Math.Abs(expectedPoints[i].Y - points[i].Y) <= MathUtils.EPS3);

            }
        }
    }
}
