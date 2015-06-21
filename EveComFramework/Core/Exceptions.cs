#pragma warning disable 1591
using System;
using System.IO;
using System.Reflection;

namespace EveComFramework.Core
{
    public class Exceptions
    {
        #region Instantiation

        static Exceptions _Instance;
        public static Exceptions Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Exceptions();
                }
                return _Instance;
            }
        }

        private Exceptions()
        {
        }

        #endregion

        public string file { get; set; }

        public void Post(string title, Exception val)
        {
            string LogDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\logs\\";


            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            if (file == null) file = LogDirectory + title + " - " + DateTime.Now.Ticks + ".txt";
            StreamWriter oWriter = new StreamWriter(file, true);
            oWriter.Write(val.Message + Environment.NewLine + Environment.NewLine + val.StackTrace + Environment.NewLine + Environment.NewLine);
            oWriter.Close();
        }
    }
}
