using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace FaceDll
{
    public class FaceService 
    {
        #region TaiSDK引用
		 
        [DllImport("TaiSDK.dll", EntryPoint = "face_comp_feature", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_comp_feature(byte[] feature1, byte[] feature2);

        [DllImport("TaiSDK.dll", EntryPoint = "face_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_init();

        [DllImport("TaiSDK.dll", EntryPoint = "face_get_feature", CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("TaiSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_get_feature(string pic_sec64, byte[] feature, string savePic);


        [DllImport("TaiSDK.dll", EntryPoint = "face_get_feature_from_image", CallingConvention = CallingConvention.Cdecl)]
        //[DllImport("TaiSDK.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_get_feature_from_image(string savePic, byte[] feature);

        [DllImport("TaiSDK.dll", EntryPoint = "face_exist", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_exist(string fname);

        [DllImport("TaiSDK.dll", EntryPoint = "face_exit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_exit();

        [DllImport("TaiSDK.dll", EntryPoint = "face_get_pos", CallingConvention = CallingConvention.Cdecl)]
        public static extern int face_get_pos(string fname, ref FacePos[] pFaceArr);
	    #endregion

        #region FaceMatcherDll引用

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FD_CreateIns", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FD_CreateIns(IntPtr pIns, string strLog);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FD_DestroyIns", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FD_DestroyIns(IntPtr pIns);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FD_SetParams", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FD_SetParams(IntPtr ins, double dMinDetWidthRatio, double dFaceRectExpandRatio);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FD_DetectFace_BGRBuf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FD_DetectFace_BGRBuf(IntPtr ins, byte[] pBGRBuf, int nW, int nH);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FD_GetFaceNum", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FD_GetFaceNum(IntPtr ins, ref int pnFaceNum);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FM_GetFeatureLen", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FM_GetFeatureLen(IntPtr ins);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FM_ExtractFeature_BGRBuf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FM_ExtractFeature_BGRBuf(IntPtr ins, ref float[] pfFeature, byte[] pBGRBuf, int nW, int nH, int* pnStatus, FM_Rect pRect);

        [DllImport("FaceMatcherDll.dll", EntryPoint = "FM_ExtractFeature_BGRBuf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FM_Match(ref float[] pfFeature1, ref float[] pfFeature2, int nLen,ref float score);


        public struct FM_Rect
        {
            public int left;       // 矩形框左上角x坐标
            public int top;        // 矩形框左上角y坐标
            public int right;      // 矩形框右下角x坐标
            public int bottom;     // 矩形框右下角y坐标
            public float fConf;		// confidence
            public float fRotAngle;	// rotation angle
        };

        #endregion


        public static bool FaceInit()
        {

            return true;
        }

        public static bool FaceExist(string strFile)
        {
            bool bRet=false;

            return bRet;
        }

        public static int FaceCompare(string strFile1, string strFile2)
        {
            int iValue = 0;

            return iValue;
        }

        public static int FacePosition(string strFile , FacePos[] facePos)
        {
            int iRet = 0;

            return iRet;
        }

    }

   public struct FacePos
    {//能定人脸面积区域的两点C1(x1, y1), C2(x2, y2)
        public int x1;
       public int y1;
       public int x2;
       public int y2;
    };

}
