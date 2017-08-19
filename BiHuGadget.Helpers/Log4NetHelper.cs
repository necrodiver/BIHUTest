using System.Diagnostics;
using System.Linq;
using log4net;
using System;
using log4net.Core;
using System.Text;
using BiHuGadget.Models;

namespace BiHuGadget.Helpers
{
    public class Log4NetHelper
    {
        /// <summary>
        /// 生成日志信息——Fatal(致命错误)
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Fatal(string message)
        {
            ILog log = LogManager.GetLogger("Fatal");
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Fatal(致命错误)
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="message">日志内容</param>
        public static void Fatal(string name, string message)
        {
            ILog log = LogManager.GetLogger(name);
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
            log = null;
        }


        /// <summary>
        /// 生成日志信息——Error（一般错误）
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(string message)
        {
            ILog log = LogManager.GetLogger("Error");
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Error（一般错误）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="message">日志内容</param>
        public static void Error(string name, string message)
        {
            ILog log = LogManager.GetLogger(name);
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Warn（警告）
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warn(string message)
        {
            ILog log = LogManager.GetLogger("Warn");
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Warn（警告）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="message">日志内容</param>
        public static void Warn(string name, string message)
        {
            ILog log = LogManager.GetLogger(name);
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Info（一般信息）
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            ILog log = LogManager.GetLogger("Info");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Info（一般信息）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="message">日志内容</param>
        public static void Info(string name, string message)
        {
            ILog log = LogManager.GetLogger(name);
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Debug（调试信息）
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(string message)
        {
            ILog log = LogManager.GetLogger("Debug");
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
            log = null;
        }

        /// <summary>
        /// 生成日志信息——Debug（调试信息）
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="message">日志内容</param>
        public static void Debug(string name, string message)
        {
            ILog log = LogManager.GetLogger(name);
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
            log = null;
        }
    }
}
