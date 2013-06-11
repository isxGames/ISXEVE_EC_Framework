using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;

namespace EveComFramework.Core
{
    /// <summary>
    /// This class is inherited by many ECF modules and EveCom bots to turn them into state queues
    /// </summary>
    public class State
    {
        /// <summary>
        /// Class for items placed in the State queue
        /// </summary>
        public class StateQueue
        {
            internal Func<object[], bool> State { get; set; }
            internal object[] Params { get; set; }
            /// <summary>
            /// The frequency for the state in milliseconds
            /// </summary>
            public int Frequency { get; set; }
            internal StateQueue(Func<object[], bool> State, int Frequency, object[] Params)
            {
                this.State = State;
                this.Params = Params;
                this.Frequency = Frequency;
            }
            /// <summary>
            /// The name of the state
            /// </summary>
            /// <returns>Name</returns>
            public override string ToString()
            {
                return State.Method.Name;
            }
        }

        /// <summary>
        /// The default frequency to use if none is specified
        /// </summary>
        public int DefaultFrequency { get; set; }
        /// <summary>
        /// The DateTime of the next scheduled pulse
        /// </summary>
        public DateTime NextPulse { get; set; }
        /// <summary>
        /// The state queue
        /// </summary>
        public LinkedList<StateQueue> States = new LinkedList<StateQueue>();
        /// <summary>
        /// The current state waiting to be processed
        /// </summary>
        public StateQueue CurState;
        /// <summary>
        /// Returns true if there are no items in the state queue waiting to be processed
        /// </summary>
        public bool Idle { get { return CurState == null; } }

        /// <summary>
        /// Logger for the State class
        /// </summary>
        public Logger StateLog;

        /// <summary>
        /// Constructor
        /// </summary>
        public State()
        {
            StateLog = new Logger("State: " + this.GetType().Name);
            DefaultFrequency = 1000;
            EVEFrame.OnFrame += OnFrame;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~State()
        {
            EVEFrame.OnFrame -= OnFrame;
        }

        /// <summary>
        /// Queue a new state
        /// </summary>
        /// <param name="State">The boolean function to process</param>
        /// <param name="Frequency">The frequency to use in milliseconds: defaults to -1, which uses the currently defined Default Frequency</param>
        /// <param name="Params">Array of objects to pass to the boolean function</param>
        public void QueueState(Func<object[], bool> State, int Frequency = -1, params object[] Params)
        {
            States.AddFirst(new StateQueue(State, ((Frequency == -1) ? DefaultFrequency : Frequency), Params));
            if (CurState == null)
            {
                CurState = States.Last();
                NextPulse = DateTime.Now.AddMilliseconds(CurState.Frequency);
                States.RemoveLast();
            }
        }

        /// <summary>
        /// Insert a new state at the front of the queue
        /// </summary>
        /// <param name="State">The boolean function to process</param>
        /// <param name="Frequency">The frequency to use in milliseconds: defaults to -1, which uses the currently defined Default Frequency</param>
        /// <param name="Params">Array of objects to pass to the boolean function</param>
        public void InsertState(Func<object[], bool> State, int Frequency = -1, params object[] Params)
        {
            States.AddLast(new StateQueue(State, ((Frequency == -1) ? DefaultFrequency : Frequency), Params));
            if (CurState == null)
            {
                CurState = States.Last();
                NextPulse = DateTime.Now.AddMilliseconds(CurState.Frequency);
                States.RemoveLast();
            }
        }

        /// <summary>
        /// Queue a new state, pushing the current state back into the queue
        /// </summary>
        /// <param name="State">The boolean function to process</param>
        /// <param name="Frequency">The frequency to use in milliseconds: defaults to -1, which uses the currently defined Default Frequency</param>
        /// <param name="Params">Array of objects to pass to the boolean function</param>
        public void DislodgeCurState(Func<object[], bool> State, int Frequency = -1, params object[] Params)
        {
            if (CurState != null)
            {
                States.AddLast(CurState);
            }
            CurState = new StateQueue(State, ((Frequency == -1) ? DefaultFrequency : Frequency), Params);
        }

        /// <summary>
        /// Clear the state queue
        /// </summary>
        public void Clear()
        {
            States.Clear();
            CurState = null;
        }

        /// <summary>
        /// Clear the current state (Advances state queue and assigns current state to next item if not empty)
        /// </summary>
        public void ClearCurState()
        {
            CurState = null;
            if (States.Count > 0)
            {
                CurState = States.Last();
                States.RemoveLast();
            }
        }

        internal bool WaitForState(object[] Params)
        {
            switch (Params.Length)
            {
                case 1:
                    if ((int)Params[0] >= 1)
                    {
                        CurState.Params[0] = (int)Params[0] - 1;
                        return false;
                    }
                    break;
                case 2:
                    if ((int)Params[0] >= 1 && !((Func<bool>)Params[1])())
                    {
                        CurState.Params[0] = (int)Params[0] - 1;
                        return false;
                    }
                    break;
                case 4:
                    if ((int)Params[0] >= 1 && !((Func<bool>)Params[1])())
                    {
                        if (((Func<bool>)Params[2])())
                        {
                            CurState.Params[0] = (int)Params[3];
                        }
                        else
                        {
                            CurState.Params[0] = (int)Params[0] - 1;
                        }
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Inserts a wait for a specified time
        /// </summary>
        /// <param name="TimeOut">How long to wait (in seconds)</param>
        /// <param name="Test">A boolean function which will instantly short circuit the timeout if returns true.  ex: () => var == true</param>
        /// <param name="Reset">A boolean function which will instantly reset the timeout to it's max value if returns true.  ex: () => var == true</param>
        public void WaitFor(int TimeOut, Func<bool> Test = null, Func<bool> Reset = null)
        {
            if (Reset != null)
            {
                InsertState(WaitForState, -1, TimeOut, Test, Reset, TimeOut);
            }
            else if (Test != null)
            {
                InsertState(WaitForState, -1, TimeOut, Test);
            }
            else
            {
                InsertState(WaitForState, -1, TimeOut);
            }
        }

        void OnFrame(object sender, EventArgs e)
        {
            if (DateTime.Now > NextPulse)
            {
                if (CurState != null && Session.Safe && Session.NextSessionChange < Session.Now)
                {
                    if (CurState.State(CurState.Params))
                    {
                        if (States.Count > 0)
                        {
                            CurState = States.Last();
                            States.RemoveLast();
                            StateLog.Log("New CurState: {0}", CurState.ToString());
                        }
                        else
                        {
                            CurState = null;
                        }
                    }
                }

                if (CurState == null)
                {
                    NextPulse = DateTime.Now.AddMilliseconds(DefaultFrequency);
                }
                else
                {
                    NextPulse = DateTime.Now.AddMilliseconds(CurState.Frequency);
                }
            }
        }


    }

}
