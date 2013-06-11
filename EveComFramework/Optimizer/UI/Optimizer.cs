using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveComFramework.Optimizer.UI
{
    /// <summary>
    /// Optimizer configuration form
    /// </summary>
    public partial class Optimizer : Form
    {
        internal OptimizerSettings Config = EveComFramework.Optimizer.Optimizer.Instance.Config;

        /// <summary>
        /// Constructor
        /// </summary>
        public Optimizer()
        {
            InitializeComponent();
            Config.Updated += LoadSettings;
        }

        private void Optimizer_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            checkDisable3D.Checked = !Config.Enable3D;
            numericMaxMemorySize.Value = Config.MaxMemorySize;
        }

        private void checkDisable3D_CheckedChanged(object sender, EventArgs e)
        {
            Config.Enable3D = !checkDisable3D.Checked;
            Config.Save();
        }

        private void numericMaxMemorySize_ValueChanged(object sender, EventArgs e)
        {
            Config.MaxMemorySize = (int)numericMaxMemorySize.Value;
            Config.Save();
        }
    }
}
