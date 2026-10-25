using System.Collections;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Materials
{
    public class PointCloudMaterial : Material, IMap
    {
        public Color Color = Color.ColorName(ColorKeywords.white);
        public float Size = 1;
        public bool SizeAttenuation = true;
        public bool Fog = true;

        public object Map { get; set; }
        public object AlphaMap { get; set; }
        public object SpecularMap { get; set; }
        public object NormalMap { get; set; }
        public object BumpMap { get; set; }
        public object LightMap { get; set; }

        public PointCloudMaterial(Hashtable parameters = null)
        {
            this.type = "PointCloudMaterial";

            this.SetValues(parameters);
        }
    }
}
