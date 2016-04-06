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
        public static bool SiteTaken(this SystemScanResult systemScanResult)
        {
            return systemScanResult.SiteTaken(Comms.Comms.Instance.LocalChat);
        }
        public static bool SiteTaken(this SystemScanResult systemScanResult, ChatChannel chatChannel) 
        {
            return chatChannel.Messages.Any(a => a.Time.AddHours(1) > Session.Now && Comms.Comms.MatchMessageAnom(a.Text, systemScanResult.ID) && a.SenderName != Me.Name);
        }

    }

}
