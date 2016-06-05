#pragma warning disable 1591
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using EVE.ISXEVE;
using EveComFramework.Core;

namespace EveComFramework.Security
{
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
            if (val && Config.IntelToolEnabled)
            {
                if (Idle)
                {
                    DefaultFrequency = Config.IntelToolInterval*1000;
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
        internal readonly SecuritySettings Config = Security.Instance.Config;
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

            String url = ApplyArgs(Config.IntelToolURL);
            string postData = ApplyArgs(Config.IntelToolPostData);
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
                // HttpWebResponse response = (HttpWebResponse) req.GetResponse();
                // Log.Log(new StreamReader(response.GetResponseStream()).ReadToEnd());
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
