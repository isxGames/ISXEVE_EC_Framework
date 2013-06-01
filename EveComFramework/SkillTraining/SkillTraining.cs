using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InnerSpaceAPI;
using EveCom;
using EveComFramework.SessionControl;

namespace EveComFramework.SkillTraining
{
    [Serializable]
    public class SkillToTrain
    {
        public SkillToTrain(int id,int level)
        {
            ID = id;
            Level = level;
        }
        public int ID;
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
    /// Sessioncontrol provides interface for logging in and out of Eve and awareness of downtime
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
            Config.SkillQueues.Add("REDACTED", new List<SkillToTrain>());
            Config.SkillQueues["REDACTED"].Add(new SkillToTrain(25863,5));
            QueueState(Monitor);
        }


        #endregion

        #region Variables

        public Logger Log = new Logger("SkillTraining");
        public SkillTrainingGlobalSettings Config = new SkillTrainingGlobalSettings();
        #endregion

        #region Actions
        
        /// <summary>
        /// Opens up the configuration dialog, this is a MODAL dialog and will block the thread!
        /// </summary>
        public void Configure()
        {
            UI.SkillTraining Configuration = new UI.SkillTraining();
            Configuration.ShowDialog();
        }

        #endregion

        #region States

        #region Utility

        bool Blank(object[] Params)
        {
            Log.Log("Finished");
            return true;
        }

        #endregion

        bool Monitor(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            Log.Log("EndOfQueue {0}", SkillQueue.EndOfQueue.ToString("hh:mm:ss.ff"));
            if (SkillQueue.EndOfQueue < Session.Now.AddDays(1))
            {
                QueueState(BeginSkillTransaction);
                QueueState(AddSkillToQueue);
                return true;
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
            if (!SkillQueue.InTransaction)
            {
                foreach (SkillToTrain stt in Config.SkillQueues[Me.Name])
                {
                    //check if the skill is injected
                    Skill injectedSkill = Skill.Get(stt.ID);
                    Log.Log(injectedSkill.SkillLevel.ToString());
                    if (injectedSkill != null)
                    {
                        //check if we should be training it
                        if (injectedSkill.SkillLevel < stt.Level)
                        {
                            Log.Log("peep");
                            injectedSkill.AddToQueue();
                            SkillQueue.CommitTransaction();
                            QueueState(Monitor);
                            return true;
                        }
                    }
                }
                Log.Log("Couldn't find any skills to train and there is a gap in the skillqueue!");
                return true;
            }
            else
            {
                QueueState(BeginSkillTransaction);
                QueueState(AddSkillToQueue);
                return true;
            }
        }
        #endregion
    }


}
