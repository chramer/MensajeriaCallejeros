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
using FirebirdSql.Data.FirebirdClient;


namespace MensajeriaCallejeros
{
    public partial class Panel_Cadete : Form
    {
        FbConnection conexion = new FbConnection();
        public Panel_Cadete()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ABM_Cadetes ABM_cad = new ABM_Cadetes();
            ABM_cad.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel_Cadete_Load(object sender, EventArgs e)
        {

        }
    }
}
