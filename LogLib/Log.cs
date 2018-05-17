using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Net.Mail;
using System.Net;

namespace LogLib
{
    public class MYLog
    {
        ILog log = LogManager.GetLogger("mylogger");

        public void write()
        {
            if (log.IsDebugEnabled)
            {
                log.Debug("aaaaaa");
            }
        }
    }
    /// <summary>
    /// 统一的日志记录对象。使用Log4Net技术。
    /// </summary>
    public class Log
    {
        #region Constructors

        protected Log(ILog logger)
        {
            _logger = logger;
        }

        #endregion

        #region Fields

        /// <summary>
        /// 实际的Log4Net日志记录对象。
        /// </summary>
        private readonly ILog _logger;

        /// <summary>
        /// 用于存储所有已生成的日志记录对象的字典。
        /// </summary>
        private readonly static Dictionary<string, Log> LoggerTable
            = new Dictionary<string, Log>();

        #endregion

        #region Properties

        #endregion

        #region Delegates & Events

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取日志记录器对象。
        /// </summary>
        /// <param name="loggerName">日志记录器的名称，该名称关系到日志的级别。</param>
        /// <returns>平台统一日志对象。</returns>
        public static Log GetLogger(string loggerName)
        {
            if (LoggerTable.ContainsKey(loggerName))
                return LoggerTable[loggerName];

            return new Log(LogManager.GetLogger(loggerName));
        }

        /// <summary>
        /// 获取日志记录器对象
        /// </summary>
        /// <param name="owner">日志记录器的拥有者，将用该拥有者的类型名称来作为日志对象的名称。</param>
        /// <returns>平台统一日志对象。</returns>
        public static Log GetLogger(object owner)
        {
            string loggerName = owner == null
                          ? "<<Unknown Module>>"
                          : string.Format("<<{0}>>", owner.GetType().Name);

            return GetLogger(loggerName);
        }

        #region Fatals
        /// <summary>
        /// 记录致命（Fatal）级别的日志信息。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="exception">日志所对应的异常对象，默认为null。</param>
        public void Fatal(object message, Exception exception = null)
        {
            if (!_logger.IsFatalEnabled)
                return;

            if (exception == null)
                _logger.Fatal(message);
            else
                _logger.Fatal(message, exception);
        }

        /// <summary>
        /// 使用格式化字符串记录致命（Fatal）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        public void FatalFormat(string format, object arg0)
        {
            if (!_logger.IsFatalEnabled)
                return;

            _logger.FatalFormat(format, arg0);
        }

        /// <summary>
        /// 使用格式化字符串记录致命（Fatal）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (!_logger.IsFatalEnabled)
                return;

            _logger.FatalFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 使用格式化字符串记录致命（Fatal）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        /// <param name="arg2">格式化参数2</param>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!_logger.IsFatalEnabled)
                return;

            _logger.FatalFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 使用格式化字符串记录致命（Fatal）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">格式化参数数组</param>
        public void FatalFormat(string format, params object[] args)
        {
            if (!_logger.IsFatalEnabled)
                return;

            _logger.FatalFormat(format, args);
        }
        #endregion

        #region Errors
        /// <summary>
        /// 记录错误（Error）级别的日志信息。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="exception">日志所对应的异常对象，默认为null。</param>
        public void Error(object message, Exception exception = null)
        {
            if (!_logger.IsErrorEnabled)
                return;

            if (exception == null)
                _logger.Error(message);
            else
                _logger.Error(message, exception);
        }

        /// <summary>
        /// 使用格式化字符串记录错误（Error）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        public void ErrorFormat(string format, object arg0)
        {
            if (!_logger.IsErrorEnabled)
                return;

            _logger.ErrorFormat(format, arg0);
        }

        /// <summary>
        /// 使用格式化字符串记录错误（Error）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (!_logger.IsErrorEnabled)
                return;

            _logger.ErrorFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 使用格式化字符串记录错误（Error）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        /// <param name="arg2">格式化参数2</param>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!_logger.IsErrorEnabled)
                return;

            _logger.ErrorFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 使用格式化字符串记录错误（Error）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">格式化参数数组</param>
        public void ErrorFormat(string format, params object[] args)
        {
            if (!_logger.IsErrorEnabled)
                return;

            _logger.ErrorFormat(format, args);
        }
        #endregion

        #region Warns
        /// <summary>
        /// 记录警告（Warn）级别的日志信息。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="exception">日志所对应的异常对象，默认为null。</param>
        public void Warn(object message, Exception exception = null)
        {
            if (!_logger.IsWarnEnabled)
                return;

            if (exception == null)
                _logger.Warn(message);
            else
                _logger.Warn(message, exception);
        }

        /// <summary>
        /// 使用格式化字符串记录警告（Warn）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        public void WarnFormat(string format, object arg0)
        {
            if (!_logger.IsWarnEnabled)
                return;

            _logger.WarnFormat(format, arg0);
        }

        /// <summary>
        /// 使用格式化字符串记录警告（Warn）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (!_logger.IsWarnEnabled)
                return;

            _logger.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 使用格式化字符串记录警告（Warn）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        /// <param name="arg2">格式化参数2</param>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!_logger.IsWarnEnabled)
                return;

            _logger.WarnFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 使用格式化字符串记录警告（Warn）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">格式化参数数组</param>
        public void WarnFormat(string format, params object[] args)
        {
            if (!_logger.IsWarnEnabled)
                return;

            _logger.WarnFormat(format, args);
        }
        #endregion

        #region Infos
        /// <summary>
        /// 记录信息（Info）级别的日志信息。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="exception">日志所对应的异常对象，默认为null。</param>
        public void Info(object message, Exception exception = null)
        {
            //if (!_logger.IsInfoEnabled)
            //    return;

            if (exception == null)
                _logger.Info(message);
            else
                _logger.Info(message, exception);
        }

        /// <summary>
        /// 使用格式化字符串记录信息（Info）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        public void InfoFormat(string format, object arg0)
        {
            if (!_logger.IsInfoEnabled)
                return;

            _logger.InfoFormat(format, arg0);
        }

        /// <summary>
        /// 使用格式化字符串记录信息（Info）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (!_logger.IsInfoEnabled)
                return;

            _logger.InfoFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 使用格式化字符串记录信息（Info）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        /// <param name="arg2">格式化参数2</param>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!_logger.IsInfoEnabled)
                return;

            _logger.InfoFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 使用格式化字符串记录信息（Info）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">格式化参数数组</param>
        public void InfoFormat(string format, params object[] args)
        {
            if (!_logger.IsInfoEnabled)
                return;

            _logger.InfoFormat(format, args);
        }
        #endregion

        #region Debugs
        /// <summary>
        /// 记录调试（Debug）级别的日志信息。
        /// </summary>
        /// <param name="message">日志信息。</param>
        /// <param name="exception">日志所对应的异常对象，默认为null。</param>
        public void Debug(object message, Exception exception = null)
        {
            if (!_logger.IsDebugEnabled)
                return;

            if (exception == null)
                _logger.Debug(message);
            else
                _logger.Debug(message, exception);
        }

        /// <summary>
        /// 使用格式化字符串记录调试（Debug）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        public void DebugFormat(string format, object arg0)
        {
            if (!_logger.IsDebugEnabled)
                return;

            _logger.DebugFormat(format, arg0);
        }

        /// <summary>
        /// 使用格式化字符串记录调试（Debug）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (!_logger.IsDebugEnabled)
                return;

            _logger.DebugFormat(format, arg0, arg1);
        }

        /// <summary>
        /// 使用格式化字符串记录调试（Debug）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="arg0">格式化参数0</param>
        /// <param name="arg1">格式化参数1</param>
        /// <param name="arg2">格式化参数2</param>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!_logger.IsDebugEnabled)
                return;

            _logger.DebugFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// 使用格式化字符串记录调试（Debug）级别的日志信息。
        /// </summary>
        /// <param name="format">格式化文本</param>
        /// <param name="args">格式化参数数组</param>
        public void DebugFormat(string format, params object[] args)
        {
            if (!_logger.IsDebugEnabled)
                return;

            _logger.DebugFormat(format, args);
        }
        #endregion

        #endregion

        #region static Methods
        private static string sysName = "<<iQuant>>";

        public static void FatalLog(object message, Exception exception = null)
        {
            Log log = GetLogger(sysName);
            log.Fatal(message, exception);
        }

        public static void FatalLog(string message, params object[] args)
        {
            Log log = GetLogger(sysName);
            log.FatalFormat(message, args);
        }

        public static void WarnLog(object message, Exception exception = null)
        {
            Log log = GetLogger(sysName);
            log.Warn(message, exception);
        }

        public static void WarnLog(string message, params object[] args)
        {
            Log log = GetLogger(sysName);
            log.WarnFormat(message, args);
        }

        public static void InfoLog(object message, Exception exception = null)
        {
            Log log = GetLogger(sysName);
            log.Info(message, exception);
        }

        public static void InfoLog(string message, params object[] args)
        {
            Log log = GetLogger(sysName);
            log.InfoFormat(message, args);
        }

        public static void DebugLog(object message, Exception exception = null)
        {
            Log log = GetLogger(sysName);
            log.Debug(message, exception);
        }

        public static void DebugLog(string message, params object[] args)
        {
            Log log = GetLogger(sysName);
            log.DebugFormat(message, args);
        }

        public static void Write(string message, LogType type = LogType.Info, Exception exception = null)
        {
            Log log = GetLogger(sysName);
            switch (type)
            {
                case LogType.Fatal:
                    log.Fatal(message, exception);
                    break;
                case LogType.Warn:
                    log.Warn(message, exception);
                    break;
                case LogType.Info:
                    log.Info(message, exception);
                    break;
                case LogType.Debug:
                    log.Debug(message, exception);
                    break;
                case LogType.Error:
                    log.Error(message, exception);
                    break;
                default:
                    log.Info(message, exception);
                    break;
            }
        }

        public static void Write(string message, LogType type = LogType.Info, params object[] args)
        {
            Log log = GetLogger(sysName);
            switch (type)
            {
                case LogType.Fatal:
                    log.FatalFormat(message, args);
                    break;
                case LogType.Warn:
                    log.WarnFormat(message, args);
                    break;
                case LogType.Info:
                    log.InfoFormat(message, args);
                    break;
                case LogType.Debug:
                    log.DebugFormat(message, args);
                    break;
                case LogType.Error:
                    log.ErrorFormat(message, args);
                    break;
                default:
                    log.InfoFormat(message, args);
                    break;
            }
        }
        #endregion
    }
    public enum LogType
    {
        Fatal,
        Warn,
        Info,
        Debug,
        Error
    }
}

