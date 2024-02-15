using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LogWriter_New
    {
        private string m_exePath = string.Empty;
        public LogWriter_New(string logMessage, string FileName)
        {
            LogWrite(logMessage, FileName);
        }
        public LogWriter_New(string logMessage, string FileName, string FolderName)
        {
            LogWrite(logMessage, FileName, FolderName);
        }
        public LogWriter_New(string logMessage, string FileName, string FolderName, string HeaderName)
        {
            if (!string.IsNullOrEmpty(HeaderName))
                logMessage = Environment.NewLine + "---------------------------------------------" + HeaderName + "" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------------------------------" + Environment.NewLine + logMessage;
            LogWrite(logMessage, FileName, FolderName);
        }
        public void LogWrite(string logMessage, string FileName)
        {
            using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FileName + ".txt"))
            {
                Log(logMessage, w);
            }
        }
        public void LogWrite(string logMessage, string FileName, string FolderName)
        {
            try
            {
                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FolderName + "\\" + FileName + ".txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch
            {
            }
        }
        public void Log(string logMessage, TextWriter txtWriter)
        {
            txtWriter.WriteLine("  :{0}", logMessage);
        }
        public void bookingLog(ref StringBuilder sbLogger, string requestTitle, string logText)
        {
            sbLogger.Append(Environment.NewLine + "---------------------------------------------" + requestTitle + "" + DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + "---------------------------------------------");
            sbLogger.Append(Environment.NewLine + logText);
            sbLogger.Append(Environment.NewLine + "------------------------------------------------------" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        }
    }
    public class LogCreaterNew
    {
        public static void CreateDirectory(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            catch
            {

            }
        }
        public static void CreateLogFile(string logMessage, string PathPrefix, string dirName, string fileName)
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + dirName))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + dirName);
                }

                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + dirName + "\\" + fileName))
                {
                    w.WriteLine("  :{0}", logMessage);
                }
            }
            catch
            {
            }
        }
        public static void CreateLogFile(string logMessage, string PathPrefix, string fileName)
        {
            try
            {
                using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + PathPrefix + "\\" + fileName))
                {
                    w.WriteLine("  :{0}", logMessage);
                }
            }
            catch
            {
            }
        }
        public static void MoveLogFile(string OldPath, string newPath)
        {
            try
            {
                System.IO.Directory.Move(AppDomain.CurrentDomain.BaseDirectory + OldPath, AppDomain.CurrentDomain.BaseDirectory + newPath);
            }
            catch
            {
            }
        }
    }
}
