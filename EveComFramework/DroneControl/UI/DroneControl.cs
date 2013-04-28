using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace EveComFramework.DroneControl.UI
{
    public partial class DroneControl : Form
    {
        internal DroneControlConfig Config = new DroneControlConfig();
        
        public DroneControl()
        {
            InitializeComponent();
        }

        #region Mouseover Controller

        private void HookupMouseEnterEvents(Control control)
        {
            foreach (Control childControl in control.Controls)
            {
                childControl.MouseEnter += new EventHandler(Help_MouseEnter);

                // Recurse on this child to get all of its descendents.
                HookupMouseEnterEvents(childControl);
            }
        }

        private void Help_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Control)
            {
                Control senderCtl = sender as Control;
                if (!string.IsNullOrEmpty((string)senderCtl.Tag))
                {
                    lblHelpText.Text = (string)senderCtl.Tag;
                }
                else
                {
                    lblHelpText.Text = "Mouseover an element for help";
                }
            }
        }

        #endregion


        private void DroneControl_Load(object sender, EventArgs e)
        {
            CombatDrones.Checked = Config.CombatDrones;
            CombatTargetsReserved.Value = Config.CombatTargetsReserved;
            lblCombatTargetsReserved.Text = String.Format("Use {0} target slots for combat drones", CombatTargetsReserved.Value);
            CombatTimeout.Value = Config.CombatTimeout;
            lblCombatTimeout.Text = String.Format("Switch to utility drones after {0} seconds", CombatTimeout.Value);
            Sentries.Checked = Config.Sentries;
            SentryRange.Value = Config.SentryRange;
            lblSentryRange.Text = String.Format("Use sentry drones for targets over {0}km away", SentryRange.Value);
            SentryDistanceLimit.Value = Config.SentryDistanceLimit;
            lblSentryDistanceLimit.Text = String.Format("Don't switch to non-sentry drones farther than {0}km", SentryDistanceLimit.Value);
            SentryCountLimit.Value = Config.SentryCountLimit;
            lblSentryCountLimit.Text = String.Format("While there are more then {0} sentry drones", SentryCountLimit.Value);
            MiningDrones.Checked = Config.MiningDrones;
            MiningLockCount.Value = Config.MiningLockCount;
            lblMiningDroneTargets.Text = String.Format("Use {0} target slots for mining drones", MiningLockCount.Value);
            SalvageDrones.Checked = Config.SalvageDrones;
            SalvageLockCount.Value = Config.SalvageLockCount;
            lblSalvageLockCount.Text = String.Format("Use {0} target slots for salvage drones", SalvageLockCount.Value);
            LogisticsDrones.Checked = Config.LogisticsDrones;
            LogiDroneCount.Value = Config.LogiDroneCount;
            lblLogiDroneCount.Text = String.Format("Use {0} drone slots for logistics drones", LogiDroneCount.Value);
            LogiDroneTargets.Value = Config.LogiDroneTargets;
            lblLogiDroneTargets.Text = String.Format("Use {0} target slots for logistics drones", LogiDroneTargets.Value);
            
        }

        private void UseCombatDrones_CheckedChanged(object sender, EventArgs e)
        {
            Config.CombatDrones = CombatDrones.Checked;
            Config.Save();
        }

        private void CombatTargetsReserved_Scroll(object sender, EventArgs e)
        {
            Config.CombatTargetsReserved = CombatTargetsReserved.Value;
            lblCombatTargetsReserved.Text = String.Format("Use {0} target slots for combat drones", CombatTargetsReserved.Value);
            Config.Save();
        }

        private void CombatTimeout_Scroll(object sender, EventArgs e)
        {
            Config.CombatTimeout = CombatTimeout.Value;
            lblCombatTimeout.Text = String.Format("Switch to utility drones after {0} seconds", CombatTimeout.Value);
            Config.Save();
        }

        private void SentryDrones_CheckedChanged(object sender, EventArgs e)
        {
            Config.Sentries = Sentries.Checked;
            Config.Save();
        }

        private void SentryRange_Scroll(object sender, EventArgs e)
        {
            Config.SentryRange = SentryRange.Value;
            lblSentryRange.Text = String.Format("Use sentry drones for targets over {0}km away", SentryRange.Value);
            Config.Save();
        }

        private void SentryDistanceLimit_Scroll(object sender, EventArgs e)
        {
            Config.SentryDistanceLimit = SentryDistanceLimit.Value;
            lblSentryDistanceLimit.Text = String.Format("Don't switch to non-sentry drones farther than {0}km", SentryDistanceLimit.Value);
            Config.Save();
        }

        private void SentryCountLimit_Scroll(object sender, EventArgs e)
        {
            Config.SentryCountLimit = SentryCountLimit.Value;
            lblSentryCountLimit.Text = String.Format("While there are more then {0} sentry drones", SentryCountLimit.Value);
            Config.Save();
        }

        private void MiningDrones_CheckedChanged(object sender, EventArgs e)
        {
            Config.MiningDrones = MiningDrones.Checked;
            Config.Save();
        }

        private void MiningDroneTargets_Scroll(object sender, EventArgs e)
        {
            Config.MiningLockCount = MiningLockCount.Value;
            lblMiningDroneTargets.Text = String.Format("Use {0} target slots for mining drones", MiningLockCount.Value);
            Config.Save();
        }

        private void LogisticsDrones_CheckedChanged(object sender, EventArgs e)
        {
            Config.LogisticsDrones = LogisticsDrones.Checked;
            Config.Save();
        }

        private void LogiDroneCount_Scroll(object sender, EventArgs e)
        {
            Config.LogiDroneCount = LogiDroneCount.Value;
            lblLogiDroneCount.Text = String.Format("Use {0} drone slots for logistics drones", LogiDroneCount.Value);
            Config.Save();
        }

        private void LogiDroneTargets_Scroll(object sender, EventArgs e)
        {
            Config.LogiDroneTargets = LogiDroneTargets.Value;
            lblLogiDroneTargets.Text = String.Format("Use {0} target slots for logistics drones", LogiDroneTargets.Value);
            Config.Save();
        }

        private void SalvageDrones_CheckedChanged(object sender, EventArgs e)
        {
            Config.SalvageDrones = SalvageDrones.Checked;
            Config.Save();
        }

        private void SalvageDroneTargets_Scroll(object sender, EventArgs e)
        {
            Config.SalvageLockCount = SalvageLockCount.Value;
            lblSalvageLockCount.Text = String.Format("Use {0} target slots for salvage drones", SalvageLockCount.Value);
            Config.Save();
        }



       
    }
}
