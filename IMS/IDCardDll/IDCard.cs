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
        [DllImport("./IDCard/termb.dll", EntryPoint = "InitComm", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitComm(int port);

        [DllImport("./IDCard/termb.dll", EntryPoint = "InitCommExt", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitCommExt();

        [DllImport("./IDCard/termb.dll", EntryPoint = "CloseComm", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CloseComm();

        [DllImport("./IDCard/termb.dll", EntryPoint = "Authenticate", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Authenticate();

        [DllImport("./IDCard/termb.dll", EntryPoint = "Read_Content", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Read_Content(int active);

        [DllImport("./IDCard/termb.dll", EntryPoint = "Read_Content_Path", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Read_Content_Path(string path,int active);

        [DllImport("./IDCard/termb.dll", EntryPoint = "GetDeviceID", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetDeviceID(string pmsg);

        [DllImport("./IDCard/termb.dll", EntryPoint = "GetSAMID", CallingConvention = CallingConvention.Cdecl)]
        public static extern string GetSAMID();

        [DllImport("./IDCard/termb.dll", EntryPoint = "GetPhoto", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetPhoto(string filepath);

        [DllImport("./IDCard/termb.dll", EntryPoint = "MfrInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MfrInfo(string deviceType,string deviceCato,string deviceName,string mfr);
    }
}
