using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;
using LavishScriptAPI;

namespace EveComFramework.GroupControl
{
     
    public enum Role { Combat, Miner, Hauler , Booster };
    public enum GroupType { Mining, AnomalyCombat };

    #region Persistence Classes
    [Serializable]
    public class MemberSetting
    {
        public Role Role;
        public string Profile;
    }

    [Serializable]
    public class GroupSettings
    {
        public Guid ID;
        public GroupType GroupType;
        public List<MemberSetting> Members;
    }

    public class GroupControlGlobalSettings : EveComFramework.Core.Settings
    {
        public GroupControlGlobalSettings() : base("GroupControl") {}
        public List<GroupSettings> Groups;
    }

    public class GroupControlSettings : EveComFramework.Core.Settings
    {
        public Guid CurrentGroup;
    }
    #endregion

    public class ActiveMember
    {
        public bool Active = false;
        public MemberSetting MemberSettings;
        public int LeadershipValue = 0;
    }

    public class ActiveGroup
    {
        public GroupSettings GroupSettings;
        public List<ActiveMember> ActiveMembers;    
    }

    public class GroupControl : State
    {
        #region Instantiation

        static GroupControl _Instance;
        public static GroupControl Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GroupControl();
                }
                return _Instance;
            }
        }

        private GroupControl()
            : base()
        {
            LavishScript.Commands.AddCommand("UpdateGroupControl", UpdateGroupControl);   
        }
        
        #endregion

        #region LSCommands

        public int UpdateGroupControl(string[] args)
        {
            switch (args[0])
            {
                case "available":
                    string profile = args[1];
                    int value = Convert.ToInt32(args[2]);
                    Log.Log(args[3]);
                    break;
                case "nothing":
                    //do other stuff
                    break;
            }
            return 0;
        }

        #endregion

        public void RelayAll(string Command , params string[] Args)
        {
            string msg = "relay \"all other\" -noredirect UpdateGroupControl \"" + Command + "\" ";
            foreach (string arg in Args)
            {
                msg = msg + "\"" + arg + "\"";
            }
            LavishScriptAPI.LavishScript.ExecuteCommand(msg);
        }

        #region Variables

        public GroupControlGlobalSettings GlobalConfig = new GroupControlGlobalSettings();
        public GroupControlSettings Config = new GroupControlSettings();
        public Logger Log = new Logger("GroupControl");
        public ActiveMember Self;
        public ActiveGroup CurrentGroup;
        string[] GenericLSkills = new string[] { "Leadership", "Wing Command", "Fleet Command", "Warfare Link Specialist" };
        string[] CombatLSkills = new string[] { "Information Warfare", "Armored Warfare", "Siege Warfare", "Skirmish Warfare" };
        string[] MiningLSkills = new string[] { "Mining Director", "Mining Foreman" };
        #endregion      

        #region States

        #region Utility

        bool Blank(object[] Params)
        {
            Log.Log("Finished");
            return true;
        }

        #endregion

        public bool InitializeSelf(object[] Params)
        {
            Self = new ActiveMember();
            Self.LeadershipValue = 0;
            Self.Active = true;
            Self.MemberSettings = GlobalConfig.Groups.First(a => a.ID == Config.CurrentGroup).Members.First(b => b.Profile == Core.Config.Instance.DefaultProfile);
            foreach (string skill in GenericLSkills)
            {
                Skill s = Skill.All.FirstOrDefault(a => a.Type == skill);
                if (s != null)
                {
                    Self.LeadershipValue += s.SkillLevel;
                }
            }
            if (Self.MemberSettings.Role == Role.Combat && CurrentGroup.GroupSettings.GroupType == GroupType.AnomalyCombat)
            {
                foreach (string skill in CombatLSkills)
                {
                    Skill s = Skill.All.FirstOrDefault(a => a.Type == skill);
                    if (s != null)
                    {
                        Self.LeadershipValue += s.SkillLevel;
                    }
                }
            }
            if (Self.MemberSettings.Role == Role.Miner && CurrentGroup.GroupSettings.GroupType == GroupType.Mining)
            {
                foreach (string skill in MiningLSkills)
                {
                    Skill s = Skill.All.FirstOrDefault(a => a.Type == skill);
                    if (s != null)
                    {
                        Self.LeadershipValue += s.SkillLevel;
                    }
                }
            }
            if (Self.MemberSettings.Role == Role.Booster)
            {
                Self.LeadershipValue += 10000;
            }
            return true;
        }
        public bool WaitForMembers(object[] Params)
        {
            //check for group members who haven't checked it, keep waiting if there are
            if (CurrentGroup.ActiveMembers.Any(a => !a.Active))
            {
                RelayAll("available", Core.Config.Instance.DefaultProfile , Self.LeadershipValue.ToString() , Self.MemberSettings.Role.ToString());
                return false;
            }
            return true;
        }

        #endregion
    }

}
