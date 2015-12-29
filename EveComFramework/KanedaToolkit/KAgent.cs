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
            if (!Standing.Ready)
            {
                Standing.LoadStandings();
                return false;
            }
            if (agent.Level > 1 && Standing.NPCStanding(agent.FactionID, true) < -2.00) return false;
            if (agent.Level == 1) return true;
            if (agent.Level == 2 && (Standing.NPCStanding(agent.CorporationID, true) > 1.00 || Standing.NPCStanding(agent.FactionID, true) > 1.00)) return true;
            if (agent.Level == 3 && (Standing.NPCStanding(agent.CorporationID, true) > 3.00 || Standing.NPCStanding(agent.FactionID, true) > 3.00)) return true;
            if (agent.Level == 4 && (Standing.NPCStanding(agent.CorporationID, true) > 5.00 || Standing.NPCStanding(agent.FactionID, true) > 5.00)) return true;
            if (agent.Level == 5 && (Standing.NPCStanding(agent.CorporationID, true) > 7.00 || Standing.NPCStanding(agent.FactionID, true) > 7.00)) return true;
            return false;
        }

        /// <summary>
        /// Do we currently have a mission offer from this agent?
        /// </summary>
        /// <returns></returns>
        public static bool HasMissionOffered(this Agent agent)
        {
            return HasMission(agent, AgentMission.MissionState.Offered);
        }

        /// <summary>
        /// Do we currently have a mission accepted from this agent?
        /// </summary>
        /// <returns></returns>
        public static bool HasMissionAccepted(this Agent agent)
        {
            return HasMission(agent, AgentMission.MissionState.Accepted);
        }

        private static bool HasMission(Agent agent, AgentMission.MissionState state)
        {
            return AgentMission.All.Any(a => a.AgentID == agent.ID && a.State == state);
        }

        /// <summary>
        /// Get the agents current mission
        /// </summary>
        /// <returns></returns>
        public static AgentMission GetMission(this Agent agent)
        {
            return AgentMission.All.FirstOrDefault(a => a.AgentID == agent.ID);
        }

    }
}
