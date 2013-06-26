using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InnerSpaceAPI;
using EveCom;
using EveComFramework.SessionControl;
using System.Windows.Forms;

namespace EveComFramework.SkillTraining
{
    [Serializable]
    public class SkillToTrain
    {       
        public string Type;
        public int Level;
    }


    public class SkillTrainingGlobalSettings : Settings
    {
        public SkillTrainingGlobalSettings() : base("SkillTraining") { }
        /// <summary>
        /// Available skillqueues, keyed by the character name
        /// </summary>
        public SerializableDictionary<string, List<SkillToTrain>> SkillQueues = new SerializableDictionary<string,List<SkillToTrain>>();
    }


    /// <summary>
    /// SkillTraining will manage a skill queue for the active character, can act event based or full auto
    ///
    /// </summary>
    public class SkillTraining : State
    {
        #region Instantiation

        static SkillTraining _Instance;
        public static SkillTraining Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SkillTraining();
                }
                return _Instance;
            }
        }

        private SkillTraining()
            : base()
        {
            QueueState(GetName);
        }


        #endregion

        #region Variables

        public Logger Log = new Logger("SkillTraining");
        public SkillTrainingGlobalSettings Config = new SkillTrainingGlobalSettings();
        public string CharName;
        #endregion

        #region Events
        /// <summary>
        /// Fires when there is space in the skill queue
        /// </summary>
        public event Action SpaceInQueue;

        public event Action SkillQueued;

        #endregion

        #region Actions

        /// <summary>
        /// State in passive event based mode, will fire an event when it thinks you should queue a skill
        /// </summary>
        public void StartWatch()
        {
            if (Idle || CurState.ToString() != "EventMonitor")
            {
                QueueState(EventMonitor);
            }  
        }
        /// <summary>
        /// Handles everything and queues skills as it can.
        /// </summary>
        public void StartAuto()
        {
            if (Idle || CurState.ToString() != "Monitor")
            {
                QueueState(Monitor);
            }            
        }

        public void Stop()
        {
            Clear();
        }

        /// <summary>
        /// Add stuff to the skill queue
        /// </summary>
        public void DoSkillQueue()
        {
            InsertState(BeginSkillTransaction);
            InsertState(AddSkillToQueue);
        }

        /// <summary>
        /// Opens up the configuration dialog, this is a MODAL dialog and will block the thread! Won't work when not logged in
        /// </summary>
        public void Configure()
        {
            if (CharName != null)
            {
                UI.SkillTraining Configuration;
                if (Config.SkillQueues.ContainsKey(CharName))
                {
                    Configuration = new UI.SkillTraining(Config.SkillQueues[CharName]);
                }
                else
                {
                    Config.SkillQueues.Add(CharName, new List<SkillToTrain>());
                    Configuration = new UI.SkillTraining(Config.SkillQueues[CharName]);
                }
                Configuration.ShowDialog();
                Config.SkillQueues[CharName] = Configuration.SkillPlan;
                Config.Save();
            }
            else
            {
                MessageBox.Show("Can't configure right now , you must be logged in");
            }
        }

        #endregion

        #region States

        #region Utility

        bool Blank(object[] Params)
        {
            Log.Log("Finished");
            return true;
        }

        bool GetName(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            CharName = Me.Name;
            return true;
        }

        #endregion

        bool EventMonitor(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            if (CharName == null)
            {
                CharName = Me.Name;
            }
            if (!SkillQueue.ServiceStarted)
            {
                SkillQueue.StartService();
                return false;
            }
            if (SkillQueue.EndOfQueue < Session.Now.AddDays(1))
            {
                if (Config.SkillQueues.ContainsKey(Me.Name))
                {
                    if (SpaceInQueue != null)
                    {
                        SpaceInQueue();
                    }
                    return true;
                }
            }
            return false;
        }

        bool Monitor(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            if (CharName == null)
            {
                CharName = Me.Name;
            }
            if (!SkillQueue.ServiceStarted)
            {
                SkillQueue.StartService();
                return false;
            }
            if (SkillQueue.EndOfQueue < Session.Now.AddDays(1))
            {
                if (Config.SkillQueues.ContainsKey(Me.Name))
                {
                    QueueState(BeginSkillTransaction);
                    QueueState(AddSkillToQueue);
                    QueueState(Monitor);
                    return true;
                }
            }
            return false;
        }

        bool BeginSkillTransaction(object[] Params)
        {
            SkillQueue.BeginTransaction();
            InsertState(Blank, 1000);
            return true;
        }

        bool AddSkillToQueue(object[] Params)
        {
            DefaultFrequency = 1000;
            if (SkillQueue.InTransaction)
            {               
                foreach (SkillToTrain stt in Config.SkillQueues[Me.Name])
                {
                    //check if the skill is injected
                    Skill injectedSkill = Skill.All.FirstOrDefault(a => a.Type == stt.Type);
                    if (injectedSkill != null)
                    {
                        //check if we should be training it
                        if (injectedSkill.SkillLevel < stt.Level)
                        {
                            //check if this entry is not already in the queue
                            if (!SkillQueue.Skills.Any(a => a.ToLevel == stt.Level && a.Skill.Type == stt.Type))
                            {
                                Log.Log("Queueing new skill up , Skill {0} , Level {1}", stt.Type, stt.Level);
                                injectedSkill.AddToQueue();
                                SkillQueue.CommitTransaction();
                                InsertState(Blank, 1000);
                                if (SkillQueued != null)
                                {
                                    SkillQueued();
                                }
                                return true;
                            }
                        }
                    }
                }
                Log.Log("Couldn't find any skills to train and there is a gap in the skillqueue!");
                return true;
            }
            else
            {
                Log.Log("Not in skill transaction , retrying");
                InsertState(AddSkillToQueue);
                InsertState(BeginSkillTransaction);
                return true;
            }
        }
        #endregion
    }


}
