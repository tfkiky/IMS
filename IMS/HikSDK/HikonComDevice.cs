using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using IMS.Common;
using System.Drawing;
using System.Drawing.Imaging;
using FaceDll;

namespace HikSDK
{
    public class HikonComDevice 
    {
        public int lastErrorCode = 0;
        public string lastError = "";
        private Int32 m_lPort = -1;
        private CHCNetSDK.REALDATACALLBACK RealData = null;
        private PlayCtrl.DECCBFUN m_fDisplayFun = null;
        public log4net.ILog log = log4net.LogManager.GetLogger(typeof(HikonComDevice));
    
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }
        
        private Timer _timer = null;
        enum PlayType
        {
            Real,//实时
            Record,//录像
            File//本地文件
        }
        private int userId = -1;
        private Dictionary<string, PlayRecordState> downloadStates = new Dictionary<string, PlayRecordState>();
        private Dictionary<string, PlayRecordState> playRecordStates = new Dictionary<string, PlayRecordState>();
        private Dictionary<string, int> realPlayHandleWinHandle = new Dictionary<string, int>();
        class PlayRecordState
        {
            public bool playState=true;
            public bool stepState = false;//单帧状态
            public float speed=1;
            public bool forward = true;
            public PlayType type = PlayType.Record;
            public DateTime start;
            public DateTime end;
            public DateTime current;
            public string filePath = null;
            public float perHourSize = 1717f;//每小时下载大小M
            public object objlock = new object();
            public int wndHandle = 0;
        }


        /// <summary>
        /// 重新初始化日志类
        /// </summary>
        /// <param name="typeOfClass">类型</param>
        public virtual void ReInitLog(Type typeOfClass)
        {
            log = log4net.LogManager.GetLogger(typeOfClass);
        }
        //初始化定时器，回放和录像的进度递增
        private void InitTimer()
        {
            lock (this)
            {
                if (_timer == null)
                {
                    _timer = new Timer(new TimerCallback(TimerCall), null, 1000, 1000);
                }
            }
        }
        //删除定时器
        private void DeleteTimer()
        {
            lock (this)
            {
                if (_timer!=null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }
        //定时回调函数
        private void TimerCall(object state)
        {
            try
            {
                lock (obj)
                {
                    if (downloadStates.Count == 0 && playRecordStates.Count == 0)
                    {
                        DeleteTimer();
                    }
                    foreach (var item in downloadStates)//下载进度
                    {
                        if (item.Value.playState)
                        {
                            if (System.IO.File.Exists(item.Value.filePath))
                            {
                                System.IO.FileInfo fi = new System.IO.FileInfo(item.Value.filePath);
                                float downSize=fi.Length/1024f/1024f;
                                item.Value.current=item.Value.start.AddHours(downSize / item.Value.perHourSize);
                                if (item.Value.current>item.Value.end)
                                {
                                    item.Value.current = item.Value.end;
                                }
                            }
                        }
                    }
                    foreach (var item in playRecordStates)
                    {
                        if (item.Value.playState)
                        {
                            item.Value.current = item.Value.current.AddSeconds(item.Value.speed);
                        }
                        if (item.Value.current<item.Value.start)
                        {
                            item.Value.current = item.Value.start.AddSeconds(1);
                        }
                        else if (item.Value.current > item.Value.end)
                        {
                            item.Value.current = item.Value.end.AddSeconds(-1);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        //更新错误信息
        private void UpdateError(bool ret)
        {
            if (!ret)
            {
                lastErrorCode = (int)CHCNetSDK.NET_DVR_GetLastError();
                lastError = CHCNetSDK.NET_DVR_GetErrorMsg(ref lastErrorCode);
            }
            else
            {
                lastErrorCode = 0;
                lastError = "";
            }
        }

        public bool Login(string ip, int port, string user, string pwd)
        {
            if (userId>=0)
            {
                return true;
            }
            ReInitLog(this.GetType());
            CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo=new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
            userId=CHCNetSDK.NET_DVR_Login_V30(ip, port, user, pwd, ref  deviceInfo);
            bool ret= userId >= 0;
            UpdateError(ret);
            CHCNetSDK.NET_DVR_SetAudioMode(2);
            return ret;
        }

        public void Logout()
        {
            if (userId>=0)
            {
                CHCNetSDK.NET_DVR_Logout_V30(userId);
                userId = -1;
            }
        }

      
        //实时播放
        public string RealPlay(string cameraCode, int controlHandle, int streamType,int showType, int protocol)
        {
            int ichannel = -1;
            if (!int.TryParse(cameraCode,out ichannel))
            {
                lastErrorCode = 1;
                lastError = "参数cameraCode非整数！cameraCode=" + cameraCode;
                return null;
            }
            CHCNetSDK.NET_DVR_PREVIEWINFO clientInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            clientInfo.hPlayWnd = new IntPtr(controlHandle);
            clientInfo.lChannel = ichannel+1;
            clientInfo.bBlocked = false;
            clientInfo.dwStreamType = (uint)(streamType - 1);
            clientInfo.dwLinkMode = (uint)(2-protocol);
            clientInfo.byPreviewMode = 0;
            int phandle=CHCNetSDK.NET_DVR_RealPlay_V40(userId, ref clientInfo, null, IntPtr.Zero);
            bool ret = phandle >= 0;
            UpdateError(ret);
            string str = phandle.ToString();
            if (!ret)
            {
                str = null;
            }
            else
            {
                realPlayHandleWinHandle.Add(str, controlHandle);
            }
            return str;
        }

        private string RealPlayCallBack()
        {
            if (userId < 0)
            {
                return null;
            }

            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.hPlayWnd = IntPtr.Zero;//预览窗口 live view window
            lpPreviewInfo.lChannel = 1;//预览的设备通道 the device channel number
            lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
            lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
            lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
            lpPreviewInfo.dwDisplayBufNum = 15; //播放库显示缓冲区最大帧数

            IntPtr pUser = IntPtr.Zero;//用户数据 user data 


            lpPreviewInfo.hPlayWnd = IntPtr.Zero;//预览窗口 live view window
            RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数 real-time stream callback function 
            int phandle = CHCNetSDK.NET_DVR_RealPlay_V40(userId, ref lpPreviewInfo, RealData, pUser);

            bool ret = phandle >= 0;
            UpdateError(ret);
            return phandle.ToString();
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            //下面数据处理建议使用委托的方式
            switch (dwDataType)
            {
                case CHCNetSDK.NET_DVR_SYSHEAD:     // sys head
                    if (dwBufSize > 0)
                    {
                        if (m_lPort >= 0)
                        {
                            return; //同一路码流不需要多次调用开流接口
                        }

                        //获取播放句柄 Get the port to play
                        if (!PlayCtrl.PlayM4_GetPort(ref m_lPort))
                        {
                            lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, PlayCtrl.STREAME_REALTIME))
                        {
                            lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 2 * 1024 * 1024))
                        {
                            lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                            break;
                        }


                        //设置显示缓冲区个数 Set the display buffer number
                        if (!PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                        }

                        //设置显示模式 Set the display mode
                        if (!PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                        }

                        //设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data
                        m_fDisplayFun = new PlayCtrl.DECCBFUN(DecCallbackFUN);
                        if (!PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, m_fDisplayFun, IntPtr.Zero, 0))
                        {
                        }

                        //开始解码 Start to play                       
                        if (!PlayCtrl.PlayM4_Play(m_lPort, IntPtr.Zero))
                        {
                            lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                            break;
                        }
                    }
                    break;
                case CHCNetSDK.NET_DVR_STREAMDATA:     // video stream data
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        for (int i = 0; i < 999; i++)
                        {
                            //送入码流数据进行解码 Input the stream data to decode
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                                Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        //送入其他数据 Input the other data
                        for (int i = 0; i < 999; i++)
                        {
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                lastErrorCode = (int)PlayCtrl.PlayM4_GetLastError(m_lPort);
                                Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        private void DecCallbackFUN(int nPort, IntPtr pBuf, int nSize, ref PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1, int nReserved2)
        {
            if (pFrameInfo.nType == 3) //#define T_YV12	3
            {
                byte[] byteBuf = new byte[nSize];
                Marshal.Copy(pBuf, byteBuf, 0, nSize);

                byte[] rgbbuff = new byte[pFrameInfo.nWidth * pFrameInfo.nHeight * 3];

                YV12ToRGB yvRgb = new YV12ToRGB(pFrameInfo.nWidth, pFrameInfo.nHeight);
                yvRgb.Convert(byteBuf, ref rgbbuff);

                byte[] feature1 = new byte[3000], feature2 = new byte[3000];

                System.Drawing.Imaging.PixelFormat pFmt = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                Bitmap bitmap = new Bitmap(pFrameInfo.nWidth, pFrameInfo.nHeight, pFmt);
                BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pFrameInfo.nWidth, pFrameInfo.nHeight), ImageLockMode.WriteOnly, pFmt);   //// 获取图像参数

                string picFile = @".\pic\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                System.Runtime.InteropServices.Marshal.Copy(rgbbuff, 0, bmpData.Scan0, rgbbuff.Length);
                bitmap.UnlockBits(bmpData);
                bitmap.Save(picFile, System.Drawing.Imaging.ImageFormat.Jpeg);

                int f3 = FaceService.face_get_feature_from_image(picFile, feature1);

                //string base64bmp = Convert.ToBase64String(rgbbuff);
                //int f3 = FaceService.face_get_feature(base64bmp, feature1, null);

                int f2 = FaceService.face_get_feature_from_image("E:\\查验系统\\2.jpg", feature2);

                int result2 = FaceService.face_comp_feature(feature1, feature2);
            }
        }

        //停止实时播放
        public void RealStop(string playHandle)
        {
            int ihandle;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "摄像头编码无效,非数字";
            }
            bool ret=CHCNetSDK.NET_DVR_StopRealPlay(ihandle);
            UpdateError(ret);
            realPlayHandleWinHandle.Remove(playHandle);
        }

        public bool LocalSnapshot(string playHandle, string fileNamePath)
        {
            int ihandle;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "摄像头编码无效,非数字";
                return false;
            }
            PlayType type = PlayType.Record;
            if (playRecordStates.ContainsKey(playHandle))
            {
                type = playRecordStates[playHandle].type;
            }
            bool ret = false;

            if (type== PlayType.Real)
            {
                ret = CHCNetSDK.NET_DVR_CapturePicture(ihandle, fileNamePath);
            }
            else if (type== PlayType.Record)
            {
                ret = CHCNetSDK.NET_DVR_PlayBackCaptureFile(ihandle, fileNamePath);
            }
            UpdateError(ret);
            return ret;
        }


        public bool CallPreset(string cameraCode, int presetIndex)
        {
            return true;
        }

        public bool AddPreset(string cameraCode, string presetName)
        {
            return true;
        }

        public bool ModPreset(string cameraCode, int presetIndex, string presetName)
        {
            return true;
        }

        public bool DelPreset(string cameraCode, int presetIndex)
        {
            return true;
        }

        public bool GetPresetList(string cameraCode, out Dictionary<int, string> presetList)
        {
            presetList = new Dictionary<int, string>();
            return true;
        }


        public bool SetDisplayScale(string playHandle, int displayScale)
        {
            return true;
        }

        private Dictionary<string, bool> mutelist = new Dictionary<string, bool>();
        public bool PlaySound(string playHandle)
        {
            int ihandle;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "摄像头编码无效,非数字";
                return false;
            }
            PlayType type = PlayType.Record;
            if (playRecordStates.ContainsKey(playHandle))
            {
                type = playRecordStates[playHandle].type;
            }
            bool ret = false;
            if (type== PlayType.Real)
            {
                ret = CHCNetSDK.NET_DVR_OpenSoundShare(ihandle);
            }
            else if (type == PlayType.Record)
            {
                uint o = 0 ;
                ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, CHCNetSDK.NET_DVR_PLAYSTARTAUDIO, IntPtr.Zero, 0, IntPtr.Zero, ref o);
            }
            try
            {
                if (mutelist.ContainsKey(playHandle))
                {
                    mutelist[playHandle] = ret;
                }
                else
                {
                    mutelist.Add(playHandle, ret);
                }
            }
            catch (Exception)
            {
            }
           
            UpdateError(ret);
            return ret;
        }

        public bool StopSound(string playHandle)
        {
            int ihandle;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "摄像头编码无效,非数字";
                return false;
            }
            PlayType type = PlayType.Record;
            if (playRecordStates.ContainsKey(playHandle))
            {
                type = playRecordStates[playHandle].type;
            }
            bool ret = false;
            if (type == PlayType.Real)
            {
                ret = CHCNetSDK.NET_DVR_CloseSoundShare(ihandle);
            }
            else if (type== PlayType.Record)
            {
                uint o = 0;
                ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, CHCNetSDK.NET_DVR_PLAYSTOPAUDIO, IntPtr.Zero, 0, IntPtr.Zero, ref o);
            }
            try
            {
                if (ret)
                {
                    if (mutelist.ContainsKey(playHandle))
                    {
                        mutelist[playHandle] = false;
                    }
                }
            }
            catch (Exception)
            {
            }
           
            UpdateError(ret);
            return ret;
        }
        private Dictionary<string, ushort> vodlist = new Dictionary<string, ushort>();
        public bool SetVolume(string playHandle, int volume)
        {
            int ihandle;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "摄像头编码无效,非数字";
                return false;
            }
            ushort sh = (ushort)(0xffff * (volume / 100f));
            PlayType type = PlayType.Record;
            if (playRecordStates.ContainsKey(playHandle))
            {
                type = playRecordStates[playHandle].type;
            }
            bool ret = false;
            if (type== PlayType.Real)
            {
                ret = CHCNetSDK.NET_DVR_Volume(ihandle, sh);
            }
            else if (type== PlayType.Record)
            {
                uint o = 0;
                ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, CHCNetSDK.NET_DVR_PLAYAUDIOVOLUME, IntPtr.Zero, sh, IntPtr.Zero, ref o);
            }
            
            try
            {
                if (ret)
                {
                    if (vodlist.ContainsKey(playHandle))
                    {
                        vodlist[playHandle] = sh;
                    }
                    else
                    {
                        vodlist.Add(playHandle, sh);
                    }
                }
            }
            catch (Exception)
            {
            }
            
            UpdateError(ret);
            return ret;
        }

        private object obj = new object();

        public bool SetPlayBackTime(string playHandle, DateTime startTime, DateTime endTime, DateTime setTime)
        {
            lock (obj)
            {
                int ihandle = -1;
                if (!int.TryParse(playHandle, out ihandle))
                {
                    lastErrorCode = -1;
                    lastError = "播放句柄无效,非数字";
                    return false;
                }
                uint o = 0;
                CHCNetSDK.NET_DVR_TIME time = new CHCNetSDK.NET_DVR_TIME();
                time.dwYear = (uint)setTime.Year;
                time.dwMonth = (uint)setTime.Month;
                time.dwDay = (uint)setTime.Day;
                time.dwHour = (uint)setTime.Hour;
                time.dwMinute = (uint)setTime.Minute;
                time.dwSecond = (uint)setTime.Second;
                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(time));
                Marshal.StructureToPtr(time, ptr, true);
                bool ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, CHCNetSDK.NET_DVR_PLAYSETTIME, ptr, 0, IntPtr.Zero, ref o);
                Marshal.FreeHGlobal(ptr);
                if (ret)
                {
                    PlayRecordState state = null;
                    if (playRecordStates.TryGetValue(playHandle, out state))
                    {
                        state.current = setTime;
                    }
                }
                UpdateError(ret);
                return ret;
            }
           
        }

        public bool GetPlayBackTime(string playHandle,DateTime startTime,DateTime endTime, ref DateTime dateTime)
        {
            lock (obj)
            {
                int ihandle = -1;
                if (!int.TryParse(playHandle, out ihandle))
                {
                    lastErrorCode = -1;
                    lastError = "播放句柄无效,非数字";
                    return false;
                }
                uint o = 0;
                uint percent = 0;
                IntPtr intptr = Marshal.AllocHGlobal(Marshal.SizeOf(percent));

                bool ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, CHCNetSDK.NET_DVR_PLAYGETPOS, IntPtr.Zero, 0, intptr, ref o);
                UpdateError(ret);
                if (ret)
                {
                    percent = (uint)Marshal.ReadInt32(intptr);
                    if (percent<100)
                    {
                        if (percent<=0)
                        {
                            PlayRecordState state = null;
                            if (playRecordStates.TryGetValue(playHandle, out state))
                            {
                                dateTime = state.current;
                            }
                        }
                        else
                        {
                            dateTime = startTime.AddSeconds((endTime - startTime).TotalSeconds * percent / 100);
                        }
                    }
                    else
                    {
                        bool bset = false;
                        if (percent==100)
                        {
                            PlayRecordState state = null;
                            if (playRecordStates.TryGetValue(playHandle, out state))
                            {
                                if (!state.forward)
                                {
                                    dateTime = state.current;
                                    bset = true;
                                }
                            }
                        }
                        if (!bset)
                        {
                            dateTime = startTime.AddSeconds((endTime - startTime).TotalSeconds * percent / 100);
                        }
                       
                    }
                }
                Marshal.FreeHGlobal(intptr);
                return ret;
            }
        }

        public bool GetVolume(string playHandle, ref int volume)
        {
            ushort us=0;
            vodlist.TryGetValue(playHandle,out us);
            if (us<=0)
            {
                us = 0;
            }
            volume = (int)(((us * 0.1f) / 0xffff) * 100);
            return true;
        }

        public bool GetMute(string playHandle, ref bool mute)
        {
            mute=false;
            mutelist.TryGetValue(playHandle, out mute);
            return true;
        }

        public string StartDownloadRecord(string cameraCode, DateTime dateTimeStart, DateTime dateTimeEnd, string path, string fileName, params object[] args)
        {
            int channel = -1;
            if (!int.TryParse(cameraCode, out channel))
            {
                lastErrorCode = -1;
                lastError = "摄像头编码无效,非数字";
                return null;
            }

            CHCNetSDK.NET_DVR_PLAYCOND struDownPara = new CHCNetSDK.NET_DVR_PLAYCOND();
            struDownPara.dwChannel = (uint)channel+1; //通道号 Channel number  

            //设置下载的开始时间 Set the starting time
            struDownPara.struStartTime.dwYear = (uint)dateTimeStart.Year;
            struDownPara.struStartTime.dwMonth = (uint)dateTimeStart.Month;
            struDownPara.struStartTime.dwDay = (uint)dateTimeStart.Day;
            struDownPara.struStartTime.dwHour = (uint)dateTimeStart.Hour;
            struDownPara.struStartTime.dwMinute = (uint)dateTimeStart.Minute;
            struDownPara.struStartTime.dwSecond = (uint)dateTimeStart.Second;

            //设置下载的结束时间 Set the stopping time
            struDownPara.struStopTime.dwYear = (uint)dateTimeEnd.Year;
            struDownPara.struStopTime.dwMonth = (uint)dateTimeEnd.Month;
            struDownPara.struStopTime.dwDay = (uint)dateTimeEnd.Day;
            struDownPara.struStopTime.dwHour = (uint)dateTimeEnd.Hour;
            struDownPara.struStopTime.dwMinute = (uint)dateTimeEnd.Minute;
            struDownPara.struStopTime.dwSecond = (uint)dateTimeEnd.Second;

            string sVideoFileName = System.IO.Path.Combine(path, fileName);//录像文件保存路径和文件名 the path and file name to save  

            if (!sVideoFileName.EndsWith(".mp4",StringComparison.CurrentCultureIgnoreCase))
            {
                sVideoFileName += ".mp4";
            }

            log.InfoFormat("开始下载录像：开始时间={0},结束时间={1},录像名称={2}", dateTimeStart, dateTimeEnd, sVideoFileName);

            //按时间下载 Download by time
            int m_lDownHandle = CHCNetSDK.NET_DVR_GetFileByTime_V40(userId, sVideoFileName, ref struDownPara);
            if (m_lDownHandle < 0)
            {
                UpdateError(false);
                return null;
            }

            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(m_lDownHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                UpdateError(false);
                return null;
            }
            string temp=m_lDownHandle.ToString();
            downloadStates.Add(temp, new PlayRecordState()
            {
                start=dateTimeStart,
                end=dateTimeEnd,
                current=dateTimeStart,
                playState=true,
                filePath = sVideoFileName
            });
            InitTimer();
            return temp;
        }
        //录像播放与下载控制
        internal bool DoRecordPlayCmd(string downloadHandle, uint cmd)
        {
            int ihandle = -1;
            if (!int.TryParse(downloadHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "下载句柄无效,非数字";
                return false;
            }
            uint iout = 0;
            bool ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, cmd, IntPtr.Zero, 0, IntPtr.Zero, ref iout);
            UpdateError(ret);
            return ret;
        }

        public bool GetDownloadProgress(string downloadHandle, ref int progress)
        {
            int ihandle = -1;
            if (!int.TryParse(downloadHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "下载句柄无效,非数字";
                return false;
            }
            if (!downloadStates.ContainsKey(downloadHandle))
            {
                progress = 100;
                return true;
            }
            uint o = 0;
            uint percent = 0;
            IntPtr intptr = Marshal.AllocHGlobal(Marshal.SizeOf(percent));

            bool ret = CHCNetSDK.NET_DVR_PlayBackControl_V40(ihandle, CHCNetSDK.NET_DVR_PLAYGETPOS, IntPtr.Zero, 0, intptr, ref o);
            UpdateError(ret);
            if (ret)
            {
                percent = (uint)Marshal.ReadIntPtr(intptr);
            }
            Marshal.FreeHGlobal(intptr);

            lock (obj)
            {
                //获取下载进度
                //int iPos = CHCNetSDK.NET_DVR_GetDownloadPos(ihandle);
                if (percent < 100)
                {
                    progress = (int)percent;
                    if (progress==0)
                    {
                        PlayRecordState state = null;
                        downloadStates.TryGetValue(downloadHandle, out state);
                        if (state != null)
                        {
                            double d = (state.current - state.start).TotalSeconds / (state.end - state.start).TotalSeconds;
                            progress = (int)(d * 100);
                            if (progress>=100)
                            {
                                progress = 99;
                            }
                        }
                    }
                }
                else if (percent == 100)
                {
                    progress = (int)percent;
                    CHCNetSDK.NET_DVR_StopGetFile(ihandle);
                    downloadStates.Remove(downloadHandle);
                }
                else if (percent == 200)//网络异常
                {
                    UpdateError(false);
                    CHCNetSDK.NET_DVR_StopGetFile(ihandle);
                    downloadStates.Remove(downloadHandle);
                    return false;
                }
            }
           
            return true;
        }


        public bool PauseDownloadRecord(string downloadHandle)
        {
            if (!downloadStates.ContainsKey(downloadHandle))
            {
                //DoRecordPlayCmd(downloadHandle, CHCNetSDK.NET_DVR_PLAYPAUSE);
                return true;
            }
            bool ret = DoRecordPlayCmd(downloadHandle, CHCNetSDK.NET_DVR_PLAYPAUSE);
            if (ret)
            {
                downloadStates[downloadHandle].playState = false;
            }
            return ret;
        }


        public bool ResumeDownloadRecord(string downloadHandle)
        {
            if (!downloadStates.ContainsKey(downloadHandle))
            {
                //DoRecordPlayCmd(downloadHandle, CHCNetSDK.NET_DVR_PLAYRESTART);
                return true;
            }
            bool ret = DoRecordPlayCmd(downloadHandle, CHCNetSDK.NET_DVR_PLAYRESTART);
            if (ret)
            {
                downloadStates[downloadHandle].playState = true;
            }
            return ret;
        }


        public bool StopDownloadRecord(string downloadHandle)
        {
            int ihandle = -1;
            if (!int.TryParse(downloadHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "下载句柄无效,非数字";
                return false;
            }
            if (!downloadStates.ContainsKey(downloadHandle))
            {
                //DoRecordPlayCmd(downloadHandle, CHCNetSDK.NET_DVR_PLAYRESTART);
                return true;
            }
            bool ret = CHCNetSDK.NET_DVR_StopGetFile(ihandle);
            UpdateError(ret);
            if (!ret)
            {
                log.ErrorFormat("停止下载录像失败：ErrorCode={0},ErrorMsg={1},重新停止一遍....", lastErrorCode, lastError);
                Thread.Sleep(100);
                ret = CHCNetSDK.NET_DVR_StopGetFile(ihandle);
                if (!ret)
                {
                    log.ErrorFormat("重新停止下载录像失败：ErrorCode={0},ErrorMsg={1}。", lastErrorCode, lastError);
                }
            }
            lock (obj)
            {
                downloadStates.Remove(downloadHandle);
            }
            return ret;
        }


        public bool IsDownloading(string downloadHandle, ref bool isDownloading)
        {
            PlayRecordState state = null;
            downloadStates.TryGetValue(downloadHandle, out state);
            if (state!=null)
            {
                isDownloading = state.playState;
            }
            else isDownloading = false;
            return true;
        }


        public bool StopPlayPlatformRecord(string playHandle)
        {
            int ihandle = -1;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "播放句柄无效,非数字";
                return false;
            }
            bool ret = CHCNetSDK.NET_DVR_StopPlayBack(ihandle);
            UpdateError(ret);
            if (!ret)
            {
                log.ErrorFormat("停止录像失败：ErrorCode={0},ErrorMsg={1},重新停止一遍...", lastErrorCode, lastError);
                ret = CHCNetSDK.NET_DVR_StopPlayBack(ihandle);
                UpdateError(ret);
                if (!ret)
                {
                    log.ErrorFormat("重新停止录像失败：ErrorCode={0},ErrorMsg={1}。", lastErrorCode, lastError);
                }
            }
            lock (obj)
            {
                playRecordStates.Remove(playHandle); 
            }
            vodlist.Remove(playHandle);
            mutelist.Remove(playHandle);
            return ret;
        }

        public bool PausePlayPlatformRecord(string playHandle)
        {
            int ihandle = -1;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "播放句柄无效,非数字";
                return false;
            }
            bool ret = true;
            PlayRecordState state;
            if (playRecordStates.TryGetValue(playHandle, out state))
            {
                lock (state.objlock)
                {
                    if (state.stepState)
                    {
                        ret = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYNORMAL);
                        UpdateError(ret);
                        if (ret)
                        {
                            state.stepState = false;
                            state.speed = 1;
                        }
                    }
                    if (!state.stepState)
                    {
                        ret = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYPAUSE);
                        UpdateError(ret);
                        state.playState = !ret;
                    }
                }
            }
            return ret;
        }

        public bool ResumePlayPlatformRecord(string playHandle)
        {
            int ihandle = -1;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "播放句柄无效,非数字";
                return false;
            }
            bool ret = true;
            PlayRecordState state;
            if (playRecordStates.TryGetValue(playHandle, out state))
            {
                lock (state.objlock)
                {
                    if (state.stepState)
                    {
                        ret = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYNORMAL);
                        UpdateError(ret);
                        if (ret)
                        {
                            state.stepState = false;
                            state.speed = 1;
                        }
                    }
                    if (!state.stepState&&!state.playState)
                    {
                        ret = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYRESTART);
                        UpdateError(ret);
                        //DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYNORMAL);
                        if (ret)
                        {
                            state.playState = true;
                        }
                    }
                }
            }
            return ret;
        }

        public bool SetPlayBackStep(string playHandle)
        {
            int ihandle = -1;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "播放句柄无效,非数字";
                return false;
            }

            PlayRecordState state;
            if (playRecordStates.TryGetValue(playHandle, out state))
            {
                lock (state.objlock)
                {
                    if (state.stepState)
                    {
                        return true;
                    }
                    else
                    {
                        bool ret = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYFRAME);
                        UpdateError(ret);
                        if (ret)
                        {
                            state.stepState = true;
                            state.speed = 0;
                        }
                        return ret;
                    }
                }
            }
            return false;
        }

        public bool SetPlayBackStepBackward(string playHandle)
        {
            int ihandle = -1;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "播放句柄无效,非数字";
                return false;
            }

            PlayRecordState state;
            if (playRecordStates.TryGetValue(playHandle, out state))
            {
                if (state.forward)
                {
                   bool ret= SetPlayBackSpeed(playHandle, -1);
                   state.forward = !ret;
                   UpdateError(ret);
                }
                if (state.forward)
                {
                    return false;
                }
                if (state.stepState)
                {
                    return true;
                }
                else
                {
                    bool ret = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYFRAME);
                    UpdateError(ret);
                    state.stepState = ret;
                    return ret;
                }
            }
            return false;
        }

        internal int GetSpeedLevel(float speed)
        {
            int level = 0;
            if (speed == 1/8f||speed==0)
            {
                level = 0;
            }
            else if (speed == 0.25)
            {
                level = 1;
            }
            else if (speed == 0.5)
            {
                level = 2;
            }
            else if (speed == 1)
            {
                level = 3;
            }
            else if (speed == 2)
            {
                level = 4;
            }
            else if (speed == 4)
            {
                level = 5;
            }
            else if (speed == 8)
            {
                level = 6;
            }
            return level;
        }

        public bool SetPlayBackSpeed(string playHandle, float speed)
        {
            int ihandle = -1;
            if (!int.TryParse(playHandle, out ihandle))
            {
                lastErrorCode = -1;
                lastError = "播放句柄无效,非数字";
                return false;
            }

            PlayRecordState state;
            if (playRecordStates.TryGetValue(playHandle, out state))
            {
                if (speed == 1)
                {
                    state.forward = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAYNORMAL);
                    if (state.forward)
                    {
                        state.speed = 1;
                    }
                    state.stepState = false;
                    return state.forward;
                }
               

                if (state.speed==speed)
                {
                    return true;
                }
                if (state.forward && speed<0)
                {
                    state.forward = !DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAY_REVERSE);
                    if (state.forward)
                    {
                        return false;
                    }
                    else
                    {
                        state.speed = -1;
                        return true;
                    }
                }
                else if (!state.forward&&speed>0)
                {
                    state.forward = DoRecordPlayCmd(playHandle, CHCNetSDK.NET_DVR_PLAY_FORWARD);
                    if (state.forward)
                    {
                        state.speed = 1;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                int lev1 = GetSpeedLevel(Math.Abs(state.speed));
                int lev2 = GetSpeedLevel(Math.Abs(speed));

                int c = lev2 - lev1;
                uint cmd = CHCNetSDK.NET_DVR_PLAYFAST;
                if (c<0)
                {
                    cmd = CHCNetSDK.NET_DVR_PLAYSLOW;
                }
                for (int i = 0; i < Math.Abs(c); i++)
                {
                    bool ret = DoRecordPlayCmd(playHandle, cmd);
                    if (!ret)
                    {
                        return false;
                    }
                    if (c<0&&ret)
                    {
                        state.speed = state.speed/2;
                    }
                    else if (c>0&&ret)
                    {
                        state.speed = state.speed * 2;
                    }
                    System.Threading.Thread.Sleep(50);
                }
                state.speed = speed;
                return true;
            }
            return false;
        }


        public bool GetPlayBackSpeed(string playHandle, ref float speed)
        {
            PlayRecordState state;
            if (playRecordStates.TryGetValue(playHandle, out state))
            {
                speed = state.speed;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetPlayBackStatus(string playHandle, ref bool status)
        {
            if (playRecordStates.ContainsKey(playHandle))
            {
                status= playRecordStates[playHandle].playState;
            }
            else status= false;
            return true;
        }

        public string StartRealToLocal(string playHandle,string filePathName)
        {
            int ihandle = -1;
            if (playHandle != null)
            {
                if (!int.TryParse(playHandle, out ihandle))
                {
                    lastErrorCode = -1;
                    lastError = "播放句柄无效,非数字";
                    return null;
                }
            }
            bool ret = CHCNetSDK.NET_DVR_SaveRealData(ihandle, filePathName);
            UpdateError(ret);
            if (ret)
            {
                return ihandle.ToString();
            }
            else return null;
        }
        public bool StopRealToLocal(string recordHandle)
        {
            int ihandle = -1;
            if (recordHandle != null)
            {
                if (!int.TryParse(recordHandle, out ihandle))
                {
                    lastErrorCode = -1;
                    lastError = "本地录像句柄无效,非数字";
                    return false;
                }
            }
            bool ret = CHCNetSDK.NET_DVR_StopSaveRealData(ihandle);
            UpdateError(ret);
            return ret;
        }
    }
}
