using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace MensajeriaCallejeros
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void administraciónDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel_Cliente PcL = new Panel_Cliente();
            PcL.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void administraciónDeCadetesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Panel_Cadete  PCadet = new Panel_Cadete();
            PCadet.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pagoCadetesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config_PCadetes CPC = new Config_PCadetes();
            CPC.ShowDialog();
        }
    }
}
