using System.Linq;
using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for Agent
    /// </summary>
    public static class KAgent
    {
        /// <summary>
        /// Check if the standing requirements for this agent are matched
        /// </summary>
        public static bool HasStanding(this Agent agent)
        {
            if (agent.Level > 1 && NPCStandingModified(Standing.NPCStanding(agent.FactionID)) < -2.00) return false;
            if (agent.Level == 2 && NPCStandingModified(Standing.NPCStanding(agent.CorporationID)) > 1.00) return true;
            if (agent.Level == 3 && NPCStandingModified(Standing.NPCStanding(agent.CorporationID)) > 3.00) return true;
            if (agent.Level == 4 && NPCStandingModified(Standing.NPCStanding(agent.CorporationID)) > 5.00) return true;
            if (agent.Level == 5 && NPCStandingModified(Standing.NPCStanding(agent.CorporationID)) > 7.00) return true;
            return false;
        }

        #region Helper Methods
        /// <summary>
        /// Apply social skill to standing
        /// </summary>
        public static double NPCStandingModified(double standing)
        {
            if (standing < 0)
            {
                Skill Diplomacy = Skill.All.FirstOrDefault(a => a.TypeID == 3357);
                if (Diplomacy != null)
                {
                    standing = 10.0 - (10.0 - standing)*(1.00 - 0.04*Diplomacy.SkillLevel);
                }
            }
            else if (standing > 0) /* @TODO: Once inexistent standings return something different to 0.0 we need to apply connections for 0.0 standings too */
            {
                Skill Connections = Skill.All.FirstOrDefault(a => a.TypeID == 3359);
                if (Connections != null)
                {
                    standing = 10.0 - (10.0 - standing)*(1.00 - 0.04*Connections.SkillLevel);
                }
            }
            return standing;
        }

        #endregion

    }
}
