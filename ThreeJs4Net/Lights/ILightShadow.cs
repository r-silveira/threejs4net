using ThreeJs4Net.Math;

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
        object shadowMap { get; set; }

        //Size shadowMapSize { get; set; }

        //Texture shadowCamera { get; Set; }

        Matrix4 shadowMatrix { get; set; }
    }
}
