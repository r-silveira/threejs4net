using System.Collections;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Materials
{
    public class LineBasicMaterial : Material
    {
        public Color Color;

        public double Linewidth;

        public string Linecap;

        public string Linejoin;

        public bool Fog;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public LineBasicMaterial(Hashtable parameters = null)
        {
            this.Color = Color.ColorName(ColorKeywords.white);

            this.type = "LineBasicMaterial";

            this.Linewidth = 1.0f;
            this.Linecap = "round";
            this.Linejoin = "round";

            this.Fog = true;

            this.SetValues( parameters );
        }

    }
}
