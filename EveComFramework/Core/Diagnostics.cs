#pragma warning disable 1591
using System;
using System.Collections.Generic;
using System.Text;
using EveCom;
using System.Net;
using System.IO;
using System.Reflection;

namespace EveComFramework.Core
{
    public class Diagnostics
    {
        #region Instantiation

        static Diagnostics _Instance;
        public static Diagnostics Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Diagnostics();
                }
                return _Instance;
            }
        }

        private Diagnostics()
        {
            LogDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\logs\\";

            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            if (file == null)
            {
                file = LogDirectory + DateTime.Now.Ticks + ".txt";
            }

            StreamWriter oWriter = new StreamWriter(file, true);
            oWriter.Write("Diagnostics log started: {0}" + Environment.NewLine + Environment.NewLine, DateTime.Now);
            oWriter.Close();

        }

        public List<State> States = new List<State>();

        #endregion

        public string file { get; set; }
        public string LogDirectory { get; set; }

        public void Post(string message, LogType logtype, string Module="")
        {
            StreamWriter oWriter = new StreamWriter(file, true);
            oWriter.Write("{0}\t{1}\t{2}: {3}"+Environment.NewLine, DateTime.Now.ToString("HH:mm"), logtype, Module, message);
            oWriter.Close();
        }

        public bool Upload(string uploadFile)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "binary/octet-stream");
            Byte[] result = client.UploadFile("http://api.eve-com.com/log.php", "POST", uploadFile);
            EVEFrame.Log(Encoding.UTF8.GetString(result, 0, result.Length));
            if (Encoding.UTF8.GetString(result, 0, result.Length) == "uploaded") return true;
            return false;
        }
    }
}
