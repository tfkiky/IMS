using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FaceDll
{
    public class FaceService 
    {
        [DllImport("face/TaiSDK.dll", EntryPoint = "face_comp_feature", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_comp_feature(byte[] feature1, byte[] feature2);

        [DllImport("face/TaiSDK.dll", EntryPoint = "face_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_init();

        [DllImport("face/TaiSDK.dll", EntryPoint = "face_get_feature", CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("TaiSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_get_feature(string pic_sec64, byte[] feature, string savePic);


        [DllImport("face/TaiSDK.dll", EntryPoint = "face_get_feature_from_image", CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("TaiSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_get_feature_from_image(string savePic, byte[] feature);

    }
}
