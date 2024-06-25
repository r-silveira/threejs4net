using ThreeJs4Net.Math;
using ThreeJs4Net.Textures;

namespace ThreeJs4Net.Lights
{
    public interface ILightShadow
    {
        bool onlyShadow { get; set; }

        float shadowCameraNear { get; set; }

        float shadowCameraFar { get; set; }

        float shadowCameraFov { get; set; }

        bool shadowCameraVisible { get; set; }

        float shadowBias { get; set; }

        float shadowDarkness { get; set; }

        float shadowMapWidth { get; set; }

        float shadowMapHeight { get; set; }
#if USE_WINDOWS
        Texture shadowMap { get; set; }
#else
        object shadowMap { get; set; }
#endif

        //Size shadowMapSize { get; set; }

        //Texture shadowCamera { get; Set; }

        Matrix4 shadowMatrix { get; set; }
    }
}
