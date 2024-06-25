using System.Collections;
using ThreeJs4Net.Math;
using ThreeJs4Net.Textures;

namespace ThreeJs4Net.Materials
{
    public class PointCloudMaterial : Material, IMap
    {
        public Color Color = Color.ColorName(ColorKeywords.white);
        public float Size = 1;
        public bool SizeAttenuation = true;
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
        public object AlphaMap { get; set; }
        public object SpecularMap { get; set; }
        public object NormalMap { get; set; }
        public object BumpMap { get; set; }
        public object LightMap { get; set; }

#endif
        public PointCloudMaterial(Hashtable parameters = null)
        {
            this.type = "PointCloudMaterial";

            this.SetValues(parameters);
        }
    }
}
