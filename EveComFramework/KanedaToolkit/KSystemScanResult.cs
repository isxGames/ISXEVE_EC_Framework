#pragma warning disable 1591
using System.Linq;
using EveCom;

namespace EveComFramework.KanedaToolkit
{
    /// <summary>
    /// extension methods for SystemScanResult
    /// </summary>
    public static class KSystemScanResult
    {
        /// <summary>
        /// Is this site taken?
        /// </summary>
        public static bool SiteTaken(this SystemScanResult SystemScanResult)
        {
            return Comms.Comms.Instance.LocalChat.Messages.Any(a => a.Time.AddHours(1) > Session.Now && a.Text.ToUpper() == SystemScanResult.ID.Substring(0, 3) && a.SenderName != Me.Name && (!Session.InFleet || !Fleet.Members.Any(b => b.Name == a.SenderName)));
        }

    }

}
