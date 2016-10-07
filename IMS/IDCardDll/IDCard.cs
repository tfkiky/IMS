using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IDCardDll
{
    public class IDCard
    {
        [DllImport("termb.dll", EntryPoint = "InitComm", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitComm(int port);

        [DllImport("termb.dll", EntryPoint = "InitCommExt", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitCommExt();

        [DllImport("termb.dll", EntryPoint = "CloseComm", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CloseComm();

        [DllImport("termb.dll", EntryPoint = "Authenticate", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Authenticate();

        [DllImport("termb.dll", EntryPoint = "Read_Content", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Read_Content(int active);

        [DllImport("termb.dll", EntryPoint = "Read_Content_Path", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Read_Content_Path(string path,int active);

        [DllImport("termb.dll", EntryPoint = "GetDeviceID", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceID(string pmsg);

        [DllImport("termb.dll", EntryPoint = "GetSAMID", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetSAMID();

        [DllImport("termb.dll", EntryPoint = "GetPhoto", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPhoto(string filepath);

        [DllImport("termb.dll", EntryPoint = "MfrInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MfrInfo(string deviceType,string deviceCato,string deviceName,string mfr);
    }
}
