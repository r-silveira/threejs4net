using System.Collections;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Materials
{
    public class PointCloudMaterial : Material
    {
        public Color Color = Color.ColorName(ColorKeywords.white);
        public float Size = 1;
        public bool Fog = true;

#if USE_WINDOWS
        public Texture Map { get; set; }
        public Texture AlphaMap { get; set; }
        public Texture SpecularMap { get; set; }
        public Texture NormalMap { get; set; } // TODO: not in ThreeJs, just to be an IMap. Must be NULL
        public Texture BumpMap { get; set; } // TODO: not in ThreeJs, just to be an IMap.  Must be NULL
        public Texture LightMap { get; set; }
#else
        public object Map { get; set; }

#endif
        public PointCloudMaterial(Hashtable parameters = null)
        {
            this.type = "PointCloudMaterial";

            this.SetValues(parameters);
        }
    }
}
