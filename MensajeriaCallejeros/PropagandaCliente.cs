using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FirebirdSql.Data.FirebirdClient;

namespace MensajeriaCallejeros
{
    public partial class PropagandaCliente : Form
    {
        public string nombre;
        public decimal id;
        public decimal modifica;
        public string sentencia;
        FbConnection conexion = new FbConnection();

        public PropagandaCliente()
        {
            InitializeComponent();
        }

        private void PropagandaCliente_Load(object sender, EventArgs e)
        {
            textBox1.Text = nombre;
            modifica = 0;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Datos del cliente
            try
            {
                conexion.Open();
                sentencia = "select count(*) from tb_propaganda_cliente where id_cli = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] != null && Convert.ToDecimal(row[0]) > 0)
                    {
                        modifica = 1;
                        traer_datos();
                    }
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void traer_datos()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Datos del cliente
            try
            {
                conexion.Open();
                sentencia = "select * from tb_propaganda_cliente where id_cli = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if ((row[1]) != null)
                    {
                        pictureBox1.Image = ByteArrayToImage((byte[])row[1]);
                    }
                    dateTimePicker1.Value = Convert.ToDateTime(row[2]);
                    textBox2.Text = Convert.ToString(row[3]);
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (modifica == 0) {
                iserta_propaganda();
            } else
            {
                modifica_propaganda();
            }
            this.Close();
        }

        private void modifica_propaganda()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            byte[] blobData = ImageToByteArray(pictureBox1.Image);

            if (blobData == null)
            {
                MessageBox.Show("Debe cargar una publicidad para el cliente antes de Grabar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("Debe ingresar un precio de publicidad antes de Grabar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.Open();
                sentencia = "update tb_propaganda_cliente set publicidad = @imagen,fecvigencia = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "',precio = '" + textBox2.Text + "' where id_cli = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.Parameters.Add("@imagen", FbDbType.Binary, blobData.Length).Value = blobData;
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
                MessageBox.Show("La Propaganda para el cliente a sido Actualizada", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void iserta_propaganda() {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            byte[] blobData = ImageToByteArray(pictureBox1.Image);

            if (blobData == null)
            {
                MessageBox.Show("Debe cargar una publicidad para el cliente antes de Grabar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("Debe ingresar un precio de publicidad antes de Grabar", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.Open();
                sentencia = "insert into tb_propaganda_cliente(id_cli,publicidad,fecvigencia,precio) values ('" + id + "', @imagen ,'" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "','" + textBox2.Text + "')";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.Parameters.Add("@imagen", FbDbType.Binary, blobData.Length).Value = blobData;
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
                MessageBox.Show("La Propaganda para el cliente a sido Actualizada", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void button3_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
