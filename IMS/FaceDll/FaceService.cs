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

        [DllImport("face/TaiSDK.dll", EntryPoint = "face_exist", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_exist(string fname);

        [DllImport("face/TaiSDK.dll", EntryPoint = "face_exit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_exit();

        [DllImport("face/TaiSDK.dll", EntryPoint = "face_get_pos", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_get_pos(string fname, ref tagFaceCoord[] pFaceArr);

    }

   public struct tagFaceCoord
    {//能定人脸面积区域的两点C1(x1, y1), C2(x2, y2)
        public int x1;
       public int y1;
       public int x2;
       public int y2;
    };

}
