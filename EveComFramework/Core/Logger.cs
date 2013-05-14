using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace EveComFramework.Core
{
    public class LoggerHelper
    {
        #region Instantiation

        static LoggerHelper _Instance;
        public static LoggerHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LoggerHelper();
                }
                return _Instance;
            }
        }

        private LoggerHelper() : base()
        {
            CurrentBackColor = BackColor1;
        }

        #endregion

        public List<Logger> Loggers = new List<Logger>();
        public Color BackColor1 = Color.Black;
        public Color BackColor2 = Color.FromArgb(50, 50, 50);
        public Color DefaultForegroundColor = Color.White;
        internal Color CurrentBackColor;

        public void RichTextboxUpdater(RichTextBox Console, string Module, string Message)
        {
            if (Console.WordWrap) Console.WordWrap = false;
            Console.SelectionColor = DefaultForegroundColor;
            Console.SelectionBackColor = CurrentBackColor;
            if (CurrentBackColor == BackColor1)
            {
                CurrentBackColor = BackColor2;
            }
            else
            {
                CurrentBackColor = BackColor1;
            }
            Console.AppendText(String.Format("[{0}] {1}", DateTime.Now.ToString("hh:mm"), Module.PadRight(12)));
            Queue<char> StringReader = new Queue<char>(Message);
            while (StringReader.Any())
            {
                char a = StringReader.Dequeue();
                if (a == '|')
                {
                    if (StringReader.Peek() == '-')
                    {
                        StringReader.Dequeue();
                        char darkcolor = StringReader.Dequeue();
                        switch (darkcolor)
                        {
                            case 'w':
                                Console.SelectionColor = Color.Gray;
                                break;
                            case 'r':
                                Console.SelectionColor = Color.DarkRed;
                                break;
                            case 'b':
                                Console.SelectionColor = Color.DarkBlue;
                                break;
                            case 'o':
                                Console.SelectionColor = Color.DarkOrange;
                                break;
                            case 'y':
                                Console.SelectionColor = Color.Goldenrod;
                                break;
                            case 'g':
                                Console.SelectionColor = Color.ForestGreen;
                                break;
                        }
                        continue;
                    }
                    char color = StringReader.Dequeue();
                    switch (color)
                    {
                        case 'w':
                            Console.SelectionColor = Color.White;
                            break;
                        case 'r':
                            Console.SelectionColor = Color.Red;
                            break;
                        case 'b':
                            Console.SelectionColor = Color.Blue;
                            break;
                        case 'o':
                            Console.SelectionColor = Color.Orange;
                            break;
                        case 'y':
                            Console.SelectionColor = Color.Yellow;
                            break;
                        case 'g':
                            Console.SelectionColor = Color.Green;
                            break;
                    }
                    continue;
                }
                Console.AppendText(a.ToString());
            }
            Console.AppendText(new string(' ', 1000) + Environment.NewLine);
            if (Console.Lines.Length > 500)
            {
                Console.Select(0, Console.GetFirstCharIndexFromLine(Console.Lines.Length - 500));
                Console.SelectedText = string.Empty;
            }
            Console.SelectionStart = Console.TextLength;
            Console.ScrollToCaret();
        }
    }

    /// <summary>
    /// Handles logging and feedback, allows multiple events to collect feedback info
    /// </summary>
    public class Logger
    {
        public Logger(string Name)
        {
            this.Name = Name;
            LoggerHelper.Instance.Loggers.Add(this);
        }

        public string Name { get; set; }

        /// <summary>
        /// Delegate for log events
        /// </summary>
        /// <param name="Message">Message that is being logged</param>
        public delegate void LogEvent(string Module, string Message);
        public event LogEvent Event;
        /// <summary>
        /// Send a log event
        /// </summary>
        /// <param name="Message">Message, may contain {0} type tokens like string.Format</param>
        /// <param name="Params">Paramters to insert into the message format string</param>
        public void Log(string Message, params object[] Params)
        {
            if (RichEvent != null)
            {
                RichEvent(Name, string.Format(Message, Params));
            }
            if (Event != null)
            {
                Event(Name, string.Format(Regex.Replace(Message, "\\|.", string.Empty), Params));
            }
        }
        /// <summary>
        /// Delegate for rich log events
        /// </summary>
        /// <param name="Message">Message that is being logged</param>
        public delegate void RichLogEvent(string Module, string Message);
        public event RichLogEvent RichEvent;
    }
}
