namespace ThreeJs4Net
{
    public class Three
    {
        public static int LineStrip = 0;

        // MATERIAL CONSTANTS

        // side

        public static int FrontSide = 0;
        public static int BackSide = 1;
        public static int DoubleSide = 2;

        // shading
        
        public static int SmoothShading = 2;

        // colors

        public static int NoColors = 0;
        public static int FaceColors = 1;

        // blending modes

        public static int NormalBlending = 1;

        public static int OneFactor = 201;
        // TEXTURE CONSTANTS

        public static int MultiplyOperation = 0;
       
        public static int ByteType = 1010;
        public static int IntType = 1013;
        public static int FloatType = 1015;

        /*
        // Potential future PVRTC compressed texture formats
        public static int RGB_PVRTC_4BPPV1_Format = 2100;
        public static int RGB_PVRTC_2BPPV1_Format = 2101;
        public static int RGBA_PVRTC_4BPPV1_Format = 2102;
        public static int RGBA_PVRTC_2BPPV1_Format = 2103;
        */

        //public static LoadingManager DefaultLoadingManager = null;

        public static int StaticDrawUsage = 35044;
    }
}