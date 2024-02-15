using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MojoIndia.Controllers
{  
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public LogWriter(string logMessage, string FileName)
        {
            LogWrite(logMessage, FileName);
        }
        public LogWriter(string logMessage, string FileName, string FolderName)
        {
            LogWrite(logMessage, FileName, FolderName);
        }
        public void LogWrite(string logMessage, string FileName)
        {
            //try
            //{
            using (StreamWriter w = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + FileName + ".txt"))
            {
                Log(logMessage, w);
            }
            //}
            //catch (Exception ex)
            //{
            //}
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
            catch (Exception ex)
            {
            }
        }
        public void Log(string logMessage, TextWriter txtWriter)
        {
            //try
            //{
            //txtWriter.Write("\r\nLog Entry : ");
            //txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            //    DateTime.Now.ToLongDateString());
            //txtWriter.WriteLine("  :");
            txtWriter.WriteLine("{0}", logMessage);
            //txtWriter.WriteLine("-------------------------------");
            //}
            //catch (Exception ex)
            //{
            //}
        }
    }

    public class LogCreater
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
            }
        }
        public static void MoveLogFile(string OldPath, string newPath)
        {
            try
            {
                System.IO.Directory.Move(AppDomain.CurrentDomain.BaseDirectory + OldPath, AppDomain.CurrentDomain.BaseDirectory + newPath);
            }
            catch (Exception ex)
            {
            }
        }
    }
}