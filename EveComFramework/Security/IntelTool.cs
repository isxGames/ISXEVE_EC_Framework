#pragma warning disable 1591
using System;
using System.Linq;
using System.Net;
using System.Text;
using EveCom;
using EveComFramework.Core;

namespace EveComFramework.Security
{
    #region Settings

    internal class IntelToolSettings : Settings
    {
        public bool Enabled = false;
        public int Interval = 5;
        public string URL = "http://inteltool/report/:solarSystem/";
        public string PostData = "local=:pilotList";
    }

    #endregion

    public class IntelTool : State
    {
        #region Instantiation

        static IntelTool _Instance;
        internal static IntelTool Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new IntelTool();
                }
                return _Instance;
            }
        }

        private IntelTool()
        {
            Enable(true);
        }

        internal void Enable(bool val)
        {
            if (val && Config.Enabled)
            {
                if (Idle)
                {
                    DefaultFrequency = Config.Interval*1000;
                    QueueState(Control);
                }
            }
            else
            {
                if (!Idle)
                {
                    Clear();
                }
            }

        }

        #endregion

        #region Variables
        internal readonly IntelToolSettings Config = new IntelToolSettings();
        internal readonly Logger Log = new Logger("Intel");
        #endregion

        #region States

        private String ApplyArgs(String s)
        {
            s = s.Replace(":solarSystem", Session.SolarSystemID.ToString());
            s = s.Replace(":characterID", Me.CharID.ToString());
            s = s.Replace(":pilotList", String.Join(",", Local.Pilots.Select(a => a.Name)));
            return s;
        }

        bool Control(object[] Params)
        {
            if (!Session.InSpace && !Session.InStation) return false;

            String url = ApplyArgs(Config.URL);
            string postData = ApplyArgs(Config.PostData);
            byte[] postDataBytes = Encoding.ASCII.GetBytes(postData);

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = postData.Length;

                using (var stream = req.GetRequestStream())
                {
                    stream.Write(postDataBytes, 0, postDataBytes.Length);
                }

                req.GetResponse();
            }
            catch (Exception ex)
            {
                Log.Log("Exception: " + ex);
            }

            return false;
        }

        #endregion
    }

}
