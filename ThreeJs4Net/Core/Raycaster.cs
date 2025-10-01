using System.Collections.Generic;
using ThreeJs4Net.Math;
using ThreeJs4Net.Objects;

namespace ThreeJs4Net.Core
{
    public class Raycaster
    {
        //public Layers Layers = new Layers();

        public double Precision = 0.0001;
        public double LinePrecision = 1;
        public Ray Ray;
        public double Near = 0;
        public double Far = double.PositiveInfinity;

        public Raycaster()
        {
        }

        public Raycaster(Vector3 origin, Vector3 direction, double near = 0, double? far = null)
        {
            far ??= double.NegativeInfinity;
            this.Ray = new Ray(origin, direction);
            // direction is assumed to be normalized (for accurate distance calculations)
        }

        private void CheckIntersectObject(Object3D object3d, Raycaster raycaster, List<Intersect> intersects, bool recursive)
        {
            //if (object3d.Layers.Test(raycaster.Layers))
            //{
            object3d.Raycast(raycaster, intersects);
            //}

            if (recursive)
            {
                var children = object3d.Children;
                foreach (var child in children)
                {
                    CheckIntersectObject(child, raycaster, intersects, true);
                }
            }
        }

        #region --- Already in R116 ---
        public void Set(Vector3 origin, Vector3 direction)
        {
            // direction is assumed to be normalized (for accurate distance calculations)
            this.Ray.Set(origin, direction);
        }
       
        public List<Intersect> IntersectObject(Object3D object3d, bool recursive = false, List<Intersect> optionalTarget = null)
        {
            var intersects = optionalTarget ?? new List<Intersect>();
            CheckIntersectObject(object3d, this, intersects, recursive);
            intersects.Sort((left, right) => (int)(left.Distance - right.Distance));
            return intersects;
        }

        public List<Intersect> IntersectObjects(IEnumerable<Object3D> object3Ds, bool recursive = false, List<Intersect> optionalTarget = null)
        {
            var intersects = optionalTarget ?? new List<Intersect>();
            
            foreach (var t in object3Ds)
            {
                this.CheckIntersectObject(t, this, intersects, recursive);
            }

            intersects.Sort((left, right) => (int)(left.Distance - right.Distance));
            return intersects;
        }

        #endregion


    }
}
