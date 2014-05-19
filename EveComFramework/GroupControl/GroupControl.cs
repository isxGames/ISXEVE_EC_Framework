using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveCom;
using EveComFramework.Core;
using LavishScriptAPI;
using System.Windows.Forms;

namespace EveComFramework.GroupControl
{
     
    public enum Role { Combat, Miner, Hauler, Booster };
    public enum GroupType { Mining, AnomalyCombat };

    #region Persistence Classes

    [Serializable]
    public class GroupSettings
    {
        public string FriendlyName;
        public Guid ID = Guid.NewGuid();
        public GroupType GroupType;
        public List<string> MemberCharacternames = new List<string>();
    }

    public class MemberSettings
    {
        public Role Role = Role.Miner;
        public Guid CurrentGroup;
    }
    public class GroupControlGlobalSettings : EveComFramework.Core.Settings
    {
        public GroupControlGlobalSettings() : base("GroupControl") {}
        public List<GroupSettings> Groups = new List<GroupSettings>();
        public SerializableDictionary<string,MemberSettings> KnownCharacters = new SerializableDictionary<string,MemberSettings>();
    }

    #endregion

    public class ActiveMember
    {
        public bool Active = false;
        public bool Available = false;
        public string CharacterName;
        public int LeadershipValue = 0;
        public bool InFleet = false;
        public Role Role;
    }

    public class ActiveGroup
    {
        public GroupSettings GroupSettings;
        public List<ActiveMember> ActiveMembers;    
    }

    public class GroupControl : State
    {
        #region Variables

        public bool FinishedCycle = false;
        public GroupControlGlobalSettings GlobalConfig = new GroupControlGlobalSettings();
        public Logger Log = new Logger("GroupControl");
        public ActiveMember Self = new ActiveMember();
        public ActiveGroup CurrentGroup;
        public ActiveMember Leader;
        string[] GenericLSkills = new string[] { "Leadership", "Wing Command", "Fleet Command", "Warfare Link Specialist" };
        string[] CombatLSkills = new string[] { "Information Warfare", "Armored Warfare", "Siege Warfare", "Skirmish Warfare" };
        string[] MiningLSkills = new string[] { "Mining Director", "Mining Foreman" };
        public bool LoadedSettings = false;
        #endregion  
    
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
            DefaultFrequency = 2000;
            LavishScriptAPI.LavishScript.Events.RegisterEvent("UpdateGroupControl");
            LavishScriptAPI.LavishScript.Events.AttachEventTarget("UpdateGroupControl", UpdateGroupControl);
            QueueState(InitializeSelf);
        }
        
        #endregion

        #region LSCommands

        void UpdateGroupControl(object sender, LavishScriptAPI.LSEventArgs args)
        {
            try
            {
                switch (args.Args[0])
                {
                    case "active":
                        if (CurrentGroup != null)
                        {
                            ActiveMember activeMember = CurrentGroup.ActiveMembers.FirstOrDefault(a => a.CharacterName == args.Args[1]);
                            if (activeMember != null)
                            {
                                if (!activeMember.Active)
                                {
                                    activeMember.Active = true;
                                }
                                if (activeMember.LeadershipValue != Convert.ToInt32(args.Args[2]))
                                {
                                    activeMember.LeadershipValue = Convert.ToInt32(args.Args[2]);
                                }
                                if (activeMember.Role != (Role)Enum.Parse(typeof(Role), args.Args[3]))
                                {
                                    activeMember.Role = (Role)Enum.Parse(typeof(Role), args.Args[3]);
                                }
                            }
                        }
                        break;
                    case "available":
                        if (CurrentGroup != null)
                        {
                            ActiveMember availableMember = CurrentGroup.ActiveMembers.FirstOrDefault(a => a.CharacterName == args.Args[1]);
                            if (availableMember != null)
                            {
                                if (availableMember.Available != Convert.ToBoolean(args.Args[2]))
                                {
                                    availableMember.Available = Convert.ToBoolean(args.Args[2]);
                                }
                            }
                        }
                        break;
                    case "joinedfleet":
                        if (CurrentGroup != null)
                        {
                            ActiveMember joinedFleet = CurrentGroup.ActiveMembers.FirstOrDefault(a => a.CharacterName == args.Args[1]);
                            if (joinedFleet != null)
                            {
                                if (!joinedFleet.InFleet)
                                {
                                    joinedFleet.InFleet = true;
                                }
                            }
                        }
                        break;
                    case "reloadConfig":
                        LoadConfig();
                        break;
                    case "forceupdate":
                        if (Self.CharacterName != null)
                        {
                            RelayAll("active", Self.CharacterName, Self.LeadershipValue.ToString(), Self.Role.ToString());
                            RelayAll("available", Self.CharacterName, Self.Available.ToString());
                            if (Self.InFleet)
                            {
                                RelayAll("joinedfleet", Self.CharacterName);
                            }
                        }
                        break;

                }
            }
            catch { }
        }

        #endregion

        #region Actions

        public bool IsLeader
        {
            get
            {
                if (Leader != null)
                {
                    if (Leader.CharacterName == Self.CharacterName)
                    {
                        return true;
                    }
                    return false;
                }
                return true;            
            }
        }

        public bool InGroup
        {
            get
            {
                if (CurrentGroup != null) return true;
                return false;
            }
        }

        public string LeaderName
        {
            get
            {
                if (Leader != null) return Leader.CharacterName;
                return "";
            }
        }

        public void Debug()
        {
            UI.Debug debugWindow = new UI.Debug();
            debugWindow.Show();
        }

        public void Start()
        {
            if (Idle || CurState.ToString() != "Organize")
            {
                SetAvailable();
                QueueState(Organize);
            }
        }

        public void Stop()
        {
            Clear();
        }

        public void Configure()
        {
            if (Self.CharacterName != null && LoadedSettings)
            {
                UI.GroupControl Configuration = new UI.GroupControl();
                Configuration.ShowDialog();
                RelayAll("reloadConfig", null);
                LoadConfig();
            }
            else
            {
                MessageBox.Show("Don't have a character name yet , can't configure");
            }
        }

        public void SetUnavailable()
        {
            Self.Available = false;
            RelayAll("available", Self.CharacterName, "false");
        }

        public void SetAvailable()
        {
            Self.Available = true;
            RelayAll("available", Self.CharacterName, "true");
        }

        #endregion 

        #region Helper Functions

        public void RelayAll(string Command , params string[] Args)
        {
            string msg = "relay \"all other\" Event[UpdateGroupControl]:Execute[" + Command;
            if (Args != null)
            {
                foreach (string arg in Args)
                {
                    msg = msg + ",\"" + arg + "\"";
                }            
            }
            msg = msg + "]";
            LavishScriptAPI.LavishScript.ExecuteCommand(msg);
        }

        public void LoadConfig()
        {
            if (Self.CharacterName != null)
            {
                GlobalConfig.Load();
                Self.Active = true;
                Self.Available = false;
                if (GlobalConfig.KnownCharacters.ContainsKey(Self.CharacterName))
                {

                    Self.Role = GlobalConfig.KnownCharacters[Self.CharacterName].Role;
                }
                else
                {
                    //first start just add myself
                    GlobalConfig.KnownCharacters.Add(Self.CharacterName, new MemberSettings());
                    GlobalConfig.KnownCharacters[Self.CharacterName].Role = Role.Miner;
                }

                CurrentGroup = new ActiveGroup();
                CurrentGroup.ActiveMembers = new List<ActiveMember>();
                CurrentGroup.GroupSettings = GlobalConfig.Groups.FirstOrDefault(a => a.ID == GlobalConfig.KnownCharacters[Self.CharacterName].CurrentGroup);
                if (CurrentGroup.GroupSettings != null)
                {
                    Log.Log("|oCurrent fleet group");
                    Log.Log(" |-g{0}", CurrentGroup.GroupSettings.FriendlyName);
                    foreach (string member in CurrentGroup.GroupSettings.MemberCharacternames)
                    {
                        if (member == Self.CharacterName)
                        {
                            CurrentGroup.ActiveMembers.Add(Self);
                        }
                        else
                        {
                            CurrentGroup.ActiveMembers.Add(new ActiveMember());
                            CurrentGroup.ActiveMembers.Last().CharacterName = member;
                        }
                    }
                }
                else
                {
                    CurrentGroup = null;
                }
            }
        }

        #endregion        

        #region States

        #region Utility

        bool Blank(object[] Params)
        {
            return true;
        }

        #endregion

        public bool InitializeSelf(object[] Params)
        {
            if ((!Session.InSpace && !Session.InStation) || !Session.Safe) return false;
            if (Me.Name != null)
            {
                Self.CharacterName = Me.Name;
                LoadConfig();
                LoadedSettings = true;
            }

            if (CurrentGroup != null)
            {
                foreach (string skill in GenericLSkills)
                {
                    Skill s = Skill.All.FirstOrDefault(a => a.Type == skill);
                    if (s != null)
                    {
                        Self.LeadershipValue += s.SkillLevel;
                    }
                }

                if (Self.Role == Role.Combat && CurrentGroup.GroupSettings.GroupType == GroupType.AnomalyCombat)
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
                if (Self.Role == Role.Miner && CurrentGroup.GroupSettings.GroupType == GroupType.Mining)
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
                if (Self.Role == Role.Booster)
                {
                    Self.LeadershipValue = -1;
                }
                EVEFrame.Log("My computed leadership value is " + Self.LeadershipValue);
                RelayAll("forceupdate", "");
                RelayAll("active", Self.CharacterName, Self.LeadershipValue.ToString(), Self.Role.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Organize(object[] Params)
        {
            FinishedCycle = false;
            if (CurrentGroup != null)
            {
                    //check for group members who haven't checked it, keep waiting if there are
                    RelayAll("active", Self.CharacterName, Self.LeadershipValue.ToString(), Self.Role.ToString());
                    RelayAll("available", Self.CharacterName, Self.Available.ToString());

                    if (!Session.InFleet)
                    {
                        //i'm not in a fleet, should i wait for an invite or create a fleet?
                        if (!CurrentGroup.ActiveMembers.Any(a => a.InFleet) 
                            && !CurrentGroup.ActiveMembers.Any(a => a.CharacterName == Me.Name && a.Role == Role.Booster && Local.Pilots.Any(b => b.Name == a.CharacterName)))
                        {
                            //nobody else is in a fleet, i can make one
                            Log.Log("|oCreating fleet");
                            Fleet.CreateFleet();
                            RelayAll("joinedfleet", Self.CharacterName);
                            Self.InFleet = true;
                            return false;
                        }
                        else
                        {
                            //someone else is in a fleet , wait for an invite from another group member
                            if (CurrentGroup.ActiveMembers.Any(a => Window.All.OfType<PopupWindow>().Any(b => b.Message.Contains(a.CharacterName))))
                            {
                                Log.Log("|oAccepting fleet invite");
                                Window.All.OfType<PopupWindow>().FirstOrDefault(a => CurrentGroup.ActiveMembers.Any(b => a.Message.Contains(b.CharacterName))).ClickButton(Window.Button.Yes);                            
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (!Self.InFleet)
                    {
                        RelayAll("joinedfleet", Self.CharacterName);
                        Self.InFleet = true;
                        return false;
                    }
                    //am i the only person in the fleet?
                    if (Fleet.Members.Count == 1)
                    {
                        //hand out invites!
                        Pilot ToInvite = Local.Pilots.FirstOrDefault(a => !Fleet.Members.Any(fleetmember => fleetmember.Name == a.Name) && CurrentGroup.ActiveMembers.Any(b => b.CharacterName == a.Name && b.Active && b.Available));
                        if (ToInvite != null)
                        {
                            Log.Log("|oInviting fleet member");
                            Log.Log(" |-g{0}", ToInvite.Name);
                            Fleet.Invite(ToInvite, Fleet.Wings[0], Fleet.Wings[0].Squads[0], FleetRole.SquadMember);
                            return false;
                        }
                    }

                    //who should be squad leader
                    ActiveMember newLeader = CurrentGroup.ActiveMembers.Where(a => a.Active && a.Role != Role.Hauler).OrderByDescending(a => a.LeadershipValue).ThenBy(b => b.CharacterName).FirstOrDefault(a => a.Active && a.Available && Fleet.Members.Any(fleetmember => fleetmember.Name == a.CharacterName));
                    if (newLeader != null)
                    {
                        if (Leader != newLeader)
                        {
                            //oh shit we got a new leader , if it's not me i should check i wasn't the old one
                            Log.Log("|oSelecting new leader");
                            Log.Log(" |-g{0}", newLeader.CharacterName);
                            //check if the new leader isnt me
                            if (newLeader.CharacterName != Self.CharacterName)
                            {
                                //it's not me check if i have to hand boss over
                                if (Fleet.Members.FirstOrDefault(a => a.Boss).Name == Self.CharacterName)
                                {
                                    //i'm the squad leader but no the leader!! better give boss to new leader
                                    Log.Log("|oPassing boss to new leader");
                                    Log.Log(" |-g{0}", newLeader.CharacterName);
                                    Fleet.Members.First(a => a.Name == newLeader.CharacterName).MakeBoss();
                                    Leader = newLeader;
                                    return false;
                                }
                            }
                            Leader = newLeader;
                        }

                        //Am I the leader?
                        if (Leader.CharacterName == Self.CharacterName)
                        {
                            //am I da boss
                            if (Fleet.Members.Any(a => a.Boss && a.Name == Me.Name))
                            {
                                //am i the squad leader?
                                FleetMember commander = Fleet.Wings[0].Members.FirstOrDefault(a => a.Role == FleetRole.SquadCommander);
                                if (commander != null)
                                {
                                    //someone is squad leader, is it me?
                                    if (commander.Name != Me.Name)
                                    {
                                        //it's not me! , demote that guy
                                        commander.Move(Fleet.Wings[0], Fleet.Wings[0].Squads[0], FleetRole.SquadMember);
                                        return false;
                                    }
                                }
                                else
                                {
                                    //nobody is squad leader, make me squad leader!
                                    Fleet.Members.First(a => a.Name == Me.Name).Move(Fleet.Wings[0], Fleet.Wings[0].Squads[0], FleetRole.SquadCommander);
                                    return false;
                                }

                                //are there invites to do?
                                Pilot ToInvite = Local.Pilots.FirstOrDefault(a => !Fleet.Members.Any(fleetmember => fleetmember.Name == a.Name) && CurrentGroup.ActiveMembers.Any(b => b.CharacterName == a.Name && b.Available && b.Active));
                                if (ToInvite != null)
                                {
                                    Log.Log("|oInviting fleet member");
                                    Log.Log(" |-g{0}", ToInvite.Name);
                                    Fleet.Invite(ToInvite, Fleet.Wings[0], Fleet.Wings[0].Squads[0], FleetRole.SquadMember);
                                    return false;
                                }

                                //Is there a booster?
                                FleetMember fleetbooster = Fleet.Wings[0].Squads[0].Members.FirstOrDefault(a => a.RoleBooster == BoosterRole.SquadBooster);
                                if (fleetbooster == null && Fleet.Wings[0].Squads[0].Commander.RoleBooster == BoosterRole.SquadBooster) fleetbooster = Fleet.Wings[0].Squads[0].Commander;
                                ActiveMember booster = CurrentGroup.ActiveMembers.FirstOrDefault(a => a.Role == Role.Booster);
                                if (booster != null)
                                {
                                    if (Fleet.Members.Any(a => a.Name == booster.CharacterName))
                                    {
                                        if (fleetbooster == null)
                                        {
                                            Log.Log("|oSetting squad booster");
                                            Log.Log(" |-g{0}", booster.CharacterName);
                                            Fleet.Members.FirstOrDefault(a => a.Name == booster.CharacterName).SetBooster(BoosterRole.SquadBooster);
                                            return false;
                                        }
                                        if (fleetbooster.Name != booster.CharacterName)
                                        {
                                            Log.Log("|oRevoking squad booster");
                                            Log.Log(" |-g{0}", fleetbooster.Name);
                                            fleetbooster.SetBooster(BoosterRole.NonBooster);
                                            return false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //im not the boss, hopefully the old boss will make me the boss
                                return false;
                            }
                        }
                    }
                    else
                    {
                    }

                    // Don't mark cycle finished if there are more pilots to invite
                    Pilot PendingInvite = Local.Pilots.FirstOrDefault(a => !Fleet.Members.Any(fleetmember => fleetmember.Name == a.Name) && CurrentGroup.ActiveMembers.Any(b => b.CharacterName == a.Name && b.Available && b.Active));
                    if (PendingInvite != null) return false;

                    FinishedCycle = true;
                    return false;
            }
            else
            {
                QueueState(InitializeSelf);
                return true;
            }
        }

        #endregion
    }

}
