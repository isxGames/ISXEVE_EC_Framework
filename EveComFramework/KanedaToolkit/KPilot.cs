#pragma warning disable 1591
using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for Pilot
    /// </summary>
    public static class KPilot
    {
        public static double DerivedStanding(this Pilot pilot)
        {
            double relationship = 0.0;
            double[] relationships = {
				pilot.ToCorp.FromCharDouble,
				pilot.ToChar.FromCharDouble,
				pilot.ToAlliance.FromCharDouble,
				pilot.ToChar.FromCorpDouble,
				pilot.ToCorp.FromCorpDouble,
				pilot.ToAlliance.FromCorpDouble,
				pilot.ToChar.FromAllianceDouble,
				pilot.ToCorp.FromAllianceDouble,
				pilot.ToAlliance.FromAllianceDouble
			};

            foreach (double r in relationships)
            {
                if (r != 0.0 && r > relationship || relationship == 0.0)
                {
                    relationship = r;
                }
            }

            return relationship;
        }

        public static bool Hostile(this Pilot pilot)
        {
            if (Me.CorpID > 999999 && pilot.CorpID == Me.CorpID) return false;
            if (Me.AllianceID > 0 && pilot.AllianceID == Me.AllianceID) return false;
            if (pilot.DerivedStanding() > 0.0) return false;
            return true;
        }

    }

}
