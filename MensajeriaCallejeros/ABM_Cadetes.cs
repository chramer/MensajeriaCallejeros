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
using System.IO;
using FirebirdSql.Data.FirebirdClient;

namespace MensajeriaCallejeros
{
    public partial class ABM_Cadetes : Form { 
        AForge.Video.DirectShow.VideoCaptureDevice videosource;

        FbConnection conexion = new FbConnection();
        string sentencia;
        public string bandera;
        public decimal id_mod;

        public ABM_Cadetes()
        {
            InitializeComponent();
            
        }

        private void ABM_Cadetes_Load(object sender, EventArgs e)
        {
            textBox4.Enabled = false;
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

            if (bandera == "alta")
            {
                button4.Visible = false;
                checkBox1.Checked = true;
                setea_numMovil();
            }
            else {
                button3.Visible = false;
                traer_datos();
            }
        }

        private void setea_numMovil()
        {
            decimal nummovil = 0;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Max ID cliente
            try
            {
                conexion.Open();
                sentencia = "select max(nummov_cadet) from tb_cadetes";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;


                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == DBNull.Value)
                    {
                        nummovil = 0;
                    }
                    else
                    {
                        nummovil = Convert.ToDecimal(row[0]);
                    }
                }

                nummovil = nummovil + 1;
                dt.Rows.Clear();
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }

            textBox4.Text = Convert.ToString(nummovil);
        }

        private void traer_datos()
        {
            sentencia = "select nom_cadet,ape_cadet,dni_cadet,fecnac_cadet,fecalt_cadet,nummov_cadet,vehi_cadet,marc_cadet,patent_cadet,domi_cadet,localidad_cadet,foto_cadet,cel_cadet,comen_cadet,activo from tb_cadetes where id_cadet = '" +  id_mod + "'";
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                foreach (DataRow row1 in dt.Rows)
                {
                    textBox1.Text = Convert.ToString(row1[0]);
                    textBox2.Text = Convert.ToString(row1[1]);
                    textBox3.Text = Convert.ToString(row1[2]);
                    dateTimePicker1.Value = Convert.ToDateTime(row1[3]);
                    dateTimePicker2.Value = Convert.ToDateTime(row1[4]);
                    textBox4.Text = Convert.ToString(row1[5]);
                    textBox5.Text = Convert.ToString(row1[6]);
                    textBox6.Text = Convert.ToString(row1[7]);
                    textBox7.Text = Convert.ToString(row1[8]);
                    textBox8.Text = Convert.ToString(row1[9]);
                    comboBox1.Text = Convert.ToString(row1[10]);
                    if ((row1[11]) != null)
                    {
                        pictureBox1.Image = ByteArrayToImage((byte[])row1[11]);
                    }
                    textBox9.Text = Convert.ToString(row1[12]);
                    textBox10.Text = Convert.ToString(row1[13]);
                    Decimal valor;
                    valor = Convert.ToDecimal(row1[14]);

                    if (valor == 0)
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                }

                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
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

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box
                Image img = new Bitmap(open.FileName);
                pictureBox1.Image = img;
                // Byte Array that can store as BLOB in DB
                //byte[] blobData = ImageToByteArray(img);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            decimal id = 0;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Max ID cliente
            try
            {
                conexion.Open();
                sentencia = "select max(id_cadet) from tb_cadetes";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;


                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == DBNull.Value)
                    {
                        id = 0;
                    }
                    else
                    {
                        id = Convert.ToDecimal(row[0]);
                    }
                }

                id = id + 1;
                dt.Rows.Clear();
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }

            string nombre, apellido, vehiculo, marca, patente, domicilio, localidad, celular,comentario;
            int dni, nom_movil,activo;
            string fecnac, fecalt;
            byte[] blobData = ImageToByteArray(pictureBox1.Image);

            if (checkBox1.Checked == true)
            {
                activo = 0;
            }
            else
            {
                activo = 1;
            }

            nombre = textBox1.Text;
            apellido = textBox2.Text;
            dni = Convert.ToInt32(textBox3.Text);
            fecnac = String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value);
            fecalt = String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value); ;
            nom_movil = Convert.ToInt32(textBox4.Text);
            vehiculo = textBox5.Text;
            marca = textBox6.Text;
            patente = textBox7.Text;
            domicilio = textBox8.Text;
            localidad = comboBox1.Text;
            celular = textBox9.Text;
            comentario = textBox10.Text;

            try
            {
                conexion.Open();
                sentencia = "insert into tb_cadetes (id_cadet,nom_cadet,ape_cadet,dni_cadet,fecnac_cadet,fecalt_cadet,nummov_cadet,vehi_cadet,marc_cadet,patent_cadet,domi_cadet,localidad_cadet,foto_cadet,cel_cadet,comen_cadet,activo) " +
                    "values ('" + id + "','" + nombre + "','" + apellido + "','" + dni + "','" + fecnac + "','" + fecalt + "','" + nom_movil + "','" + vehiculo + "','" + marca + "','" + patente + "','" + domicilio + "','" + localidad + "', @imagen,'" + celular + "','" + comentario + "','" + activo + "')";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.Parameters.Add("@imagen", FbDbType.Binary, blobData.Length).Value = blobData;
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
                MessageBox.Show("Alta de Cadete Exitosa", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        public static byte[] ImageToByteArray(Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            string nombre, apellido, vehiculo, marca, patente, domicilio, localidad, celular, comentario;
            int dni, nom_movil, activo;
            string fecnac, fecalt;
            byte[] blobData = ImageToByteArray(pictureBox1.Image);

            if (checkBox1.Checked == true)
            {
                activo = 0;
            }
            else
            {
                activo = 1;
            }

            nombre = textBox1.Text;
            apellido = textBox2.Text;
            dni = Convert.ToInt32(textBox3.Text);
            fecnac = String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value);
            fecalt = String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value); ;
            nom_movil = Convert.ToInt32(textBox4.Text);
            vehiculo = textBox5.Text;
            marca = textBox6.Text;
            patente = textBox7.Text;
            domicilio = textBox8.Text;
            localidad = comboBox1.Text;
            celular = textBox9.Text;
            comentario = textBox10.Text;

            try
            {
                sentencia = "update tb_cadetes set nom_cadet = '" + nombre + "',ape_cadet = '" + apellido + "' ,dni_cadet = '" + dni + "'" +
               ",fecnac_cadet = '" + fecnac + "',fecalt_cadet = '" + fecalt + "',nummov_cadet = '" + nom_movil + "',vehi_cadet = '" + vehiculo + "'" +
               ",marc_cadet = '" + marca + "',patent_cadet = '" + patente + "',domi_cadet = '" + domicilio + "',localidad_cadet = '" + localidad + "'," +
               "foto_cadet = @imagen,cel_cadet = '" + celular + "',comen_cadet = '" + comentario + "',activo = '" + activo + "' where id_cadet = '" + id_mod + "'";

                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.Parameters.Add("@imagen", FbDbType.Binary, blobData.Length).Value = blobData;
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
                MessageBox.Show("Datos de Cadete modificados correctamente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
