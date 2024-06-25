using System;
using ThreeJs4Net.Core;
using ThreeJs4Net.Materials;

namespace ThreeJs4Net.Scenes
{
    public class Scene : Object3D
    {
        #region Fields
        public bool AutoUpdate;
        public Material OverrideMaterial;
        public Fog Fog;
#if USE_WINDOWS
        public Texture Environment;
#endif
#endregion

        public Scene()
        {
            this.type = "Scene";

            this.Fog = null;
            this.OverrideMaterial = null;
#if USE_WINDOWS
            this.Environment = null;
#endif
            this.AutoUpdate = true; // checked by the renderer
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}