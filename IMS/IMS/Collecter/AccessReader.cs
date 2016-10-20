using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace IMS.Collecter
{
    /// <summary>
    /// 刷卡数据列表
    /// </summary>
    /// <param name="dataWapper">数据获取结果</param>
    public delegate void DataReadHandler(Maticsoft.Model.SMT_CARD_RECORDS dataWapper);

    /// <summary>
    /// 定时读取报警数据
    /// </summary>
    public class AccessReader
    {

        /// <summary>
        /// 读取数据线程
        /// </summary>
        public Thread readThread = null;

        /// <summary>
        /// 数据读取委托
        /// </summary>
        private DataReadHandler onDataRead;

        Maticsoft.BLL.SMT_CARD_RECORDS recordBll = new Maticsoft.BLL.SMT_CARD_RECORDS();

        /// <summary>
        /// 数据读取事件
        /// </summary>
        public event DataReadHandler DataRead
        {
            add { onDataRead += new DataReadHandler(value); }
            remove { onDataRead -= new DataReadHandler(value); }
        }

        /// <summary>
        /// 开始线程
        /// </summary>
        public void start()
        {
            try
            {
                this.readThread = new Thread(new ThreadStart(ReadCaller));
                this.readThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 终止线程
        /// </summary>
        public void stop()
        {
            if (null != this.readThread)
            {
                try
                {
                    this.readThread.Abort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("终止线程错误："+ ex.Message);
                }
                finally
                {
                    this.readThread = null;
                }
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        protected void ReadCaller()
        {
            Boolean start = true;
            while (start == true)
            {
                //Console.WriteLine("监测中................");
                sendData(start);
            }
        }

        private void sendData(Boolean start)
        {
            Thread.Sleep(500);
            try
            {
                List<Maticsoft.Model.SMT_CARD_RECORDS> recordList = recordBll.GetModelList(" ORDER BY  RECORD_INDEX DESC ");
                if (recordList.Count > 0 && recordList.Exists(record=>record.RECORD_INDEX.Value>AccessCollect.LastAccessIndex))
                {
                    //Console.WriteLine("有新数据............."+alarmEventList.Count.ToString());
                    onDataRead(recordList.Find(record => record.RECORD_INDEX.Value > AccessCollect.LastAccessIndex));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("循环获取门禁记录错误：" + ex.Message);
            }
        }
    }
}
