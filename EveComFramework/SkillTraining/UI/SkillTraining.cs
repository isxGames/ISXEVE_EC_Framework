using EveComFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace EveComFramework.SkillTraining.UI
{
    public partial class SkillTraining : Form
    {
        public Logger Log = new Logger("SkillTrainingConfig");
        public List<SkillToTrain> SkillPlan;
        public SkillTraining(List<SkillToTrain> currentSkillplan)
        {
            SkillPlan = currentSkillplan;
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                List<SkillToTrain> OldPlan = SkillPlan;
                try
                {
                    XDocument skillXMl = XDocument.Load(openFileDialog1.FileName);
                    if (skillXMl.Descendants("entry").ToList().Count > 0)
                    {
                        plan_displaybox.Items.Clear();
                        List<SkillToTrain> NewPlan = new List<SkillToTrain>();
                        foreach (XElement skill in skillXMl.Descendants("entry"))
                        {
                            plan_displaybox.Items.Add(String.Format("Skill : {0} , Level {1}", skill.Attribute("skill").Value, skill.Attribute("level").Value));
                            SkillToTrain newSkill = new SkillToTrain();
                            newSkill.Type = skill.Attribute("skill").Value;
                            newSkill.Level = Convert.ToInt32(skill.Attribute("level").Value);
                            NewPlan.Add(newSkill);
                        }
                        SkillPlan = NewPlan;
                    }
                }
                catch
                {
                    MessageBox.Show("Something went wrong loading that skillplan, try again");
                    SkillPlan = OldPlan;
                }                    
            }
        }

        private void SkillTraining_Load(object sender, EventArgs e)
        {
            foreach (SkillToTrain stt in SkillPlan)
            {
                plan_displaybox.Items.Add(String.Format("Skill : {0} , Level {1}",stt.Type ,stt.Level));
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            plan_displaybox.Items.Clear();
            SkillPlan = new List<SkillToTrain>();
        }
    }
}
