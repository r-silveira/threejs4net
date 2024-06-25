using System.Collections;
using ThreeJs4Net.Math;
using ThreeJs4Net.Textures;

namespace ThreeJs4Net.Materials
{
    public class MeshPhongMaterial : Material, IWireframe, IMap, IMorphTargets
    {
        public Color Color = Color.ColorName(ColorKeywords.white);

        public Color Ambient = Color.ColorName(ColorKeywords.white);

        public Color Emissive = Color.ColorName(ColorKeywords.black);

        public Color Specular = Color.ColorName(ColorKeywords.darkslateblue);

        public float Shininess = 30;

        public bool Metal = false;

        public bool WrapAround = false;

        public Vector3 WrapRgb = new Vector3( 1, 1, 1 );

        // IMap
#if USE_WINDOWS
        public Texture Map  { get; set; }

        public Texture AlphaMap { get; set; }

        public Texture SpecularMap { get; set; }

        public Texture NormalMap { get; set; }

        public Texture BumpMap { get; set; }

        public Texture LightMap { get; set; }
#else
        public object Map  { get; set; }

        public object AlphaMap { get; set; }

        public object SpecularMap { get; set; }

        public object NormalMap { get; set; }

        public object BumpMap { get; set; }

        public object LightMap { get; set; }

#endif

        //


        public float BumpScale = 1;

        public Vector2 NormalScale = new Vector2( 1, 1 );

  //      public Texture EnvMap = null;

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
        public MeshPhongMaterial(Hashtable parameters = null)
        {
            this.type = "MeshPhongMaterial";

            // IWireFrameable
            this.Wireframe = false;
            this.WireframeLinewidth = 1;

            this.SetValues(parameters);
        }
}
}
