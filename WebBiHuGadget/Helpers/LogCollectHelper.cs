using System;
using System.IO;
using System.Text;

namespace WebBiHuGadget.Helpers
{
    public class LogCollectHelper
    {
        public readonly static string logParent = AppDomain.CurrentDomain.BaseDirectory + "Log\\";
        private static object lockObjAsError = new object();
        private static object lockObjAsSuccess = new object();
        private static object lockObjAsInfo = new object();

        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="count">错误内容</param>
        public static void ErrorLog(string count)
        {
            lock (lockObjAsError)
            {
                string errorPath = logParent + "record_[" + DateTime.Now.ToString("yyyyMMdd") + "]" + "_Error.log";
                if (!Directory.Exists(logParent))
                {
                    Directory.CreateDirectory(logParent);
                }
                using (StreamWriter file = new StreamWriter(errorPath, true, Encoding.UTF8))
                {
                    string logMaster = "[{0}]:\r{1}\r";
                    string errorlog = string.Format(logMaster, DateTime.Now.ToString("HH:mm:ss"), count);
                    file.WriteLine(errorlog);
                    file.Close();
                }
            }
        }

        /// <summary>
        /// 成功日志
        /// </summary>
        /// <param name="count">错误内容</param>
        public static void SuccessLog(string count)
        {
            lock (lockObjAsSuccess)
            {
                string successPath = logParent + "record_[" + DateTime.Now.ToString("yyyyMMdd") + "]" + "_Success.log";
                if (!Directory.Exists(logParent))
                {
                    Directory.CreateDirectory(logParent);
                }
                using (StreamWriter file = new StreamWriter(successPath, true, Encoding.UTF8))
                {
                    string logMaster = "[{0}]:\r{1}\r";
                    string successlog = string.Format(logMaster, DateTime.Now.ToString("HH:mm:ss"), count);
                    file.WriteLine(successlog);
                    file.Close();
                }
            }
        }

        public static void InfoLog(string count)
        {
            lock (lockObjAsInfo)
            {
                string infoPath = logParent + "record_[" + DateTime.Now.ToString("yyyyMMdd") + "]" + "_Info.log";
                if (!Directory.Exists(logParent))
                {
                    Directory.CreateDirectory(logParent);
                }
                using (StreamWriter file = new StreamWriter(infoPath, true, Encoding.UTF8))
                {
                    string logMaster = "[{0}]:\r{1}\r";
                    string infolog = string.Format(logMaster, DateTime.Now.ToString("HH:mm:ss"), count);
                    file.WriteLine(infolog);
                    file.Close();
                }
            }
        }

    }
}
