using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace MensajeriaCallejeros
{
    public partial class ABM_Cadetes : Form { 
        AForge.Video.DirectShow.VideoCaptureDevice videosource;


        public ABM_Cadetes()
        {
            InitializeComponent();
            
        }

        private void ABM_Cadetes_Load(object sender, EventArgs e)
        {
            //
            // Cargo los datos del combobox
            //
            comboBox1.DataSource = DataHelper.LoadDataTable();
            comboBox1.DisplayMember = "localidad";
            comboBox1.ValueMember = "cod_localidad";

            //
            // cargo la lista de items para el autocomplete
            //
            comboBox1.AutoCompleteCustomSource = DataHelper.LoadAutoComplete();
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            retrive();
        }

        private void retrive() {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (button2.Text == "Tomar Foto")
            {
                AForge.Video.DirectShow.FilterInfoCollection videosources = new AForge.Video.DirectShow.FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
                if (videosources != null)
                {
                    videosource = new AForge.Video.DirectShow.VideoCaptureDevice(videosources[0].MonikerString);
                    videosource.NewFrame += (s, a) => pictureBox2.Image = (Bitmap)a.Frame.Clone();
                    videosource.Start();
                    button2.Text = "Capturar";
                }
            }
            else
            {
                pictureBox1.Image = pictureBox2.Image;
                if (videosource != null && videosource.IsRunning)
                {
                    videosource.SignalToStop();
                    videosource = null;
                }
                pictureBox2.Image = null;
                button2.Text = "Tomar Foto";
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Admin_Localidades abmloc = new Admin_Localidades();
            abmloc.ShowDialog();
            //comboBox1.Items.Clear();
            comboBox1.DataSource = DataHelper.LoadDataTable();
            comboBox1.DisplayMember = "localidad";
            comboBox1.ValueMember = "cod_localidad";
        }
    }
}
