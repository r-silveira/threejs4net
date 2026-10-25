using System.Collections;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Materials
{
    public class MeshLambertMaterial : Material, IWireframe, IMap, IMorphTargets
    { 
        public Color Color = Color.ColorName(ColorKeywords.white);

        public Color Ambient = Color.ColorName(ColorKeywords.white);

        public Color Emissive = Color.ColorName(ColorKeywords.black);

        public bool WrapAround = false;

        public Vector3 WrapRgb = new Vector3( 1, 1, 1 );


        // IMap

        public object Map { get; set; }

        public object AlphaMap { get; set; }

        public object SpecularMap { get; set; }

        public object NormalMap { get; set; }

        public object BumpMap { get; set; }

        public object LightMap { get; set; }



        public int Combine = Three.MultiplyOperation;

        public float Reflectivity = 1;

        public float RefractionRatio = 0.98f;

        public bool Fog = true;

        public int Shading = Three.SmoothShading;

        // IWireFrameable

        public bool Wireframe { get; set; }

        public float WireframeLinewidth { get; set; }

        //

        public string WireframeLinecap = "round";

        public string WireframeLinejoin = "round";


        public bool Skinning = false;

        public bool MorphTargets { get; set; }

        public bool MorphNormals = false;
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public MeshLambertMaterial(Hashtable parameters = null)
        {
            this.type = "MeshLambertMaterial";

            // IWireFrameable
            this.Wireframe = false;
            this.WireframeLinewidth = 1;
            
            this.SetValues(parameters);
        }
    }
}
