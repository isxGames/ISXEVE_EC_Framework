using System;
using EveCom;
using System.Collections.Generic;
using System.Linq;

namespace EveComFramework.KanedaToolkit
{

    /// <summary>
    /// Mission Toolkit
    /// </summary>
    public class MissionToolkit
    {
        /// <summary>
        /// Extended Mission Objective State
        /// </summary>
        public enum MissionObjectiveState
        {
            TravelTo,
            MissionFetch,
            MissionFetchContainer,
            MissionFetchMine,
            MissionFetchMineTrigger,
            MissionFetchTarget,
            AllObjectivesComplete,
            TransportItemsPresent,
            TransportItemsMissing,
            FetchObjectAcquiredDungeonDone,
            GoToGate,
            KillTrigger,
            DestroyLCSAndAll,
            Destroy,
            Attack,
            Approach,
            Hack,
            Salvage,
            DestroyAll,
            _Unknown,
            _NotAccepted
        }

        #region Instantiation
        static MissionToolkit _Instance;
        /// <summary>
        /// Singletoner
        /// </summary>
        public static MissionToolkit Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MissionToolkit();
                }
                return _Instance;
            }
        }

        private MissionToolkit() { }

        #endregion

        #region Helper Methods

        #endregion
    }
}
