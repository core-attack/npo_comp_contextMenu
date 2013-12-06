using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace npo_comp_contextMenu
{
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {
            StreamReader log = new StreamReader(Application.StartupPath + "\\" + shortFilename);
            string s = log.ReadToEnd();
            richTextBox1.Text = s;
            log.Close();
        }

        public string shortFilename = "";
        public bool delete = false;

        private void richTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void Log_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                delete = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в методе очиститьСведенияToolStripMenuItem_Click./n" + ex.StackTrace.ToString() + "/n" + ex.Message);
            }
        }

        void setDeveloper()
        {

        }
    }
}
