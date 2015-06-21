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
            RegisterCommands();
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

        public void RegisterCommands()
        {
            LavishScriptAPI.LavishScript.Commands.AddCommand("ECF", Event);
            LavishScriptAPI.LavishScript.Commands.AddCommand("ecf", Event);
        }

        private int Event(string[] args)
        {
            Exceptions exceptions = Exceptions.Instance;
            StringBuilder data = new StringBuilder();

            data.AppendLine("State Report");
            data.AppendLine();
            foreach (State state in States)
            {
                if (state.CurState == null)
                {
                    data.AppendLine(state.GetType().Name + " : Idle");
                }
                else
                {
                    data.AppendLine(String.Format("{0} : {1}", state.GetType().Name, state.CurState));
                }
            }

            if (exceptions.file != null && File.Exists(exceptions.file))
            {
                data.AppendLine();
                data.AppendLine("Exception Report");
                data.AppendLine();
                StreamReader reader = new StreamReader(exceptions.file);
                while (!reader.EndOfStream)
                {
                    data.AppendLine(reader.ReadLine());
                }
            }
            data.AppendLine();

            System.Collections.Specialized.NameValueCollection parm = new System.Collections.Specialized.NameValueCollection();
            parm.Add("paste_content", data.ToString());
            parm.Add("line_numbers", "on");
            parm.Add("expire", "604800");

            HttpWebRequest client = (HttpWebRequest)WebRequest.Create("https://privatepaste.com/save");
            client.ContentType = "application/x-www-form-urlencoded";
            client.Method = "POST";
            Stream s = client.GetRequestStream();
            StringBuilder Parameters = new StringBuilder();
            foreach (string key in parm)
            {
                Parameters.AppendFormat("{0}={1}&", key, Uri.EscapeUriString(parm[key]));
            }
            byte[] ParamBytes = Encoding.ASCII.GetBytes(Parameters.ToString());
            s.Write(ParamBytes, 0, ParamBytes.Length);
            s.Close();
            client.AllowAutoRedirect = false;

            WebResponse response = client.GetResponse();

            EVEFrame.Log("Diagnostic information has been uploaded to https://privatepaste.com" + response.Headers[HttpResponseHeader.Location] + " and the link has been copied to your clipboard.");
            string clip = string.Format("System:SetClipboardText[\"https://privatepaste.com{0}\"]", response.Headers[HttpResponseHeader.Location]);
            LavishScriptAPI.LavishScript.ExecuteCommand(clip);
            return 0;
        }

        #endregion

        public string file { get; set; }
        public string LogDirectory { get; set; }

        public void Post(string message, LogType logtype)
        {
            StreamWriter oWriter = new StreamWriter(file, true);
            oWriter.Write(logtype.ToString() + ":  " + message + Environment.NewLine);
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
