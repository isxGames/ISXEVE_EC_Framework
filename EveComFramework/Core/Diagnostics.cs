using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using System.Windows.Forms;

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

        private Diagnostics() : base()
        {
            RegisterCommands();
        }

        public List<State> States = new List<State>();

        public void RegisterCommands()
        {
            LavishScriptAPI.LavishScript.Events.RegisterEvent("ECF");
            LavishScriptAPI.LavishScript.Events.AttachEventTarget("ECF", Event);
        }

        void Help()
        {
            EVEFrame.Log("EveComFramework Diagnostics Command List");
            EVEFrame.Log("");
            EVEFrame.Log("?          : This menu");
            EVEFrame.Log("State      : Get current state for every object");
            //EVEFrame.Log("           : ");
        }

        void Output(string text)
        {
            EVEFrame.Log(text);
            ClipQueue = ClipQueue + text + Environment.NewLine;
        }

        string ClipQueue;

        void Event(object sender, LavishScriptAPI.LSEventArgs args)
        {
            ClipQueue = "";
            if (args.Args.Length == 0)
            {
                Help();
                return;
            }
            switch (args.Args[0])
            {
                case "?": 
                    Help();
                    break;
                case "State":
                    Output("EveComFramework State Status");
                    Output(" ");
                    foreach (State s in States)
                    {
                        if (s.CurState == null)
                        {
                            Output(s.GetType().Name + " : Idle");
                        }
                        else
                        {
                            Output(s.GetType().Name + " : " + s.CurState.ToString());
                        }
                    }
                    Clipboard.SetText(ClipQueue);
                    Output(" ");
                    Output("This output has been copied to your clipboard");
                    break;

            }
        }

        #endregion
    }
}
