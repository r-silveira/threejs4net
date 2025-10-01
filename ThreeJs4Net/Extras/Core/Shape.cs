using System;
using System.Collections;
using System.Collections.Generic;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Extras.Core
{
    [Serializable]
    public class Shape : Path
    {
        public Guid Uuid = Guid.NewGuid();

        public List<Path> Holes;

        public Shape(List<Vector3> points = null) : base(points)
        {
            Holes = new List<Path>();
        }

        protected Shape(Shape source)
        {
            Holes = new List<Path>();
            for (int i = 0; i < source.Holes.Count; i++)
            {

                var hole = source.Holes[i];

                this.Holes.Add(hole.Clone() as Path);

            }

        }

        public new object Clone()
        {
            return new Shape(this);
        }
        public List<List<Vector3>> GetPointsHoles(double divisions)
        {
            var holePts = new List<List<Vector3>>();

            for (int i = 0; i < this.Holes.Count; i++)
            {
                if (this.Holes[i] == null) continue;
                holePts.Add(this.Holes[i].GetPoints((int)divisions));
            }

            return holePts;
        }

        public Hashtable ExtractPoints(double divisions)
        {
            return new Hashtable()
            {
                {"shape",this.GetPoints((int)divisions) },
                {"holes",this.GetPointsHoles(divisions) }
            };
        }
    }
}
