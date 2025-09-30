using System.Collections;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Materials
{
    public class PointsMaterial : Material
    {
        public Color Color { get; set; }
#if USE_WINDOWS
        public Texture Map { get; set; }
        public Texture AlphaMap { get; set; }
#else   
        public object Map { get; set; }
        public object AlphaMap { get; set; }
#endif

        public float Size { get; set; }
        public bool SizeAttenuation { get; set; }
        public bool MorphTargets { get; set; }

        public PointsMaterial(Hashtable parameters = null)
        {
            this.Color = Color.ColorName(ColorKeywords.white);
            this.Map = null;
            this.AlphaMap = null;
            this.Size = 1;
            this.SizeAttenuation = true;
            this.MorphTargets = false;
            this.SetValues(parameters);

            this.SetValues(parameters);
        }
    }
}
