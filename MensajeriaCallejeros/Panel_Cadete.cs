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
using System.IO;
using FirebirdSql.Data.FirebirdClient;


namespace MensajeriaCallejeros
{
    public partial class Panel_Cadete : Form
    {
        FbConnection conexion = new FbConnection();
        string bandera;
        string sentencia;
        public Panel_Cadete()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ABM_Cadetes ABM_cad = new ABM_Cadetes();
            ABM_cad.bandera = "alta";
            ABM_cad.ShowDialog();
            bandera = "general";
            Retrive();
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel_Cadete_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn col0 = new DataGridViewTextBoxColumn();
            col0.Name = "Id";
            dataGridView1.Columns.Add(col0);
            dataGridView1.Columns[0].Width = 30;

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Nombre Cadete";
            dataGridView1.Columns.Add(col1);
            dataGridView1.Columns[1].Width = 200;

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "DNI";
            dataGridView1.Columns.Add(col2);
            dataGridView1.Columns[2].Width = 90;

            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Vehiculo";
            dataGridView1.Columns.Add(col3);
            dataGridView1.Columns[3].Width = 80;

            DataGridViewTextBoxColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Celular";
            dataGridView1.Columns.Add(col4);
            dataGridView1.Columns[4].Width = 100;

            DataGridViewTextBoxColumn col5 = new DataGridViewTextBoxColumn();
            col5.Name = "Domicilio";
            dataGridView1.Columns.Add(col5);
            dataGridView1.Columns[5].Width = 213;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.RowHeadersVisible = false;

            bandera = "general";
            label6.Visible = false;
            label7.Visible = false;

            Retrive();
            radioButton1.Checked = true;
            
        }

        private void Retrive()
        {
            dataGridView1.Rows.Clear();
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera clientes
            try
            {
                //id_cadet,nom_cadet,ape_cadet,dni_cadet,fecnac_cadet,fecalt_cadet,nummov_cadet,vehi_cadet,marc_cadet,patent_cadet,domi_cadet,localidad_cadet,foto_cadet,cel_cadet
                if (bandera == "nombre")
                {
                    sentencia = "select id_cadet,nom_cadet,ape_cadet,dni_cadet,vehi_cadet,cel_cadet,domi_cadet from tb_cadetes where (UPPER(nom_cadet) like UPPER('%" + textBox1.Text + "%')) OR (UPPER(ape_cadet) like UPPER('%" + textBox1.Text + "%'))";
                }

                if (bandera == "dni")
                {
                    sentencia = "select id_cadet,nom_cadet,ape_cadet,dni_cadet,vehi_cadet,cel_cadet,domi_cadet from tb_cadetes where CAST(dni_cadet as varchar(50)) like '%" + textBox1.Text + "%'";
                }

                if (bandera == "domicilio")
                {
                    sentencia = "select id_cadet,nom_cadet,ape_cadet,dni_cadet,vehi_cadet,cel_cadet,domi_cadet from tb_cadetes where (UPPER(domi_cadet) like UPPER('%" + textBox1.Text + "%'))";
                }

                if (bandera == "movil")
                {
                    sentencia = "select select id_cadet,nom_cadet,ape_cadet,dni_cadet,vehi_cadet,cel_cadet,domi_cadet from tb_cadetes where nummov_cadet = '" + textBox1.Text + "'";
                }

                if (bandera == "id")
                {
                    sentencia = "select id_cadet,nom_cadet,ape_cadet,dni_cadet,vehi_cadet,cel_cadet,domi_cadet from tb_cadetes where id_cadet = '" + textBox1.Text + "'";
                }
                if ((bandera == "general") || (textBox1.Text == ""))
                {
                    sentencia = "select id_cadet,nom_cadet,ape_cadet,dni_cadet,vehi_cadet,cel_cadet,domi_cadet from tb_cadetes";
                }

                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                foreach (DataRow row in dt.Rows)
                {
                    string nombre;
                    nombre = Convert.ToString(row[1]) + " " + Convert.ToString(row[2]);
                    dataGridView1.Rows.Add(row[0], nombre, row[3], row[4], row[5], row[6]);
                }
                dt.Rows.Clear();
                conexion.Close();
                datos_rapidos();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void datos_rapidos()
        {
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            sentencia = "select nom_cadet,ape_cadet,dni_cadet,cel_cadet,nummov_cadet,foto_cadet,activo from tb_cadetes where id_cadet = '" + id + "'";
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
                    textBox2.Text = Convert.ToString(row1[0]);
                    textBox6.Text = Convert.ToString(row1[1]);
                    textBox3.Text = Convert.ToString(row1[2]);
                    textBox4.Text = Convert.ToString(row1[3]);
                    textBox5.Text = Convert.ToString(row1[4]);

                    if ((row1[5]) != null)
                    {
                        pictureBox1.Image = ByteArrayToImage((byte[])row1[5]);
                    }
                    if (Convert.ToInt32(row1[6]) == 1)
                    {
                        label6.Visible = true;
                        label7.Visible = false;
                    }
                    else {
                        label7.Visible = true;
                        label6.Visible = false;
                    }
                }

                conexion.Close();
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "nombre";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "dni";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "domicilio";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "movil";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "id";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Retrive();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            datos_rapidos();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Elimina Cliente
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            if (MessageBox.Show("¿Esta seguro que desea eliminar el Cadete?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
                try
                {
                    conexion.Open();
                    sentencia = "delete from tb_cadetes where id_cadet = '" + id + "'";
                    FbCommand cmd = new FbCommand(sentencia, conexion);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conexion.Close();
                    MessageBox.Show("Cadete Eliminado con Exito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message.ToString(), "Error");
                }
                Retrive();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            ABM_Cadetes ABM_cad = new ABM_Cadetes();
            ABM_cad.bandera = "modi";
            ABM_cad.id_mod = id;
            ABM_cad.ShowDialog();
            bandera = "general";
            Retrive();
            radioButton1.Checked = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {

                sentencia = "update tb_cadetes set nom_cadet = '" + textBox2.Text + "',ape_cadet = '" + textBox6.Text + "' ,dni_cadet = '" + textBox3.Text + "'" +
               ",nummov_cadet = '" + textBox5.Text + "',cel_cadet = '" + textBox4.Text + "' where id_cadet = '" + id + "'";

                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
                MessageBox.Show("Datos de Cadete modificados correctamente", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
            Retrive();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Decimal id = 0;
            String nombre;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);
            nombre = Convert.ToString(row.Cells["Nombre Cadete"].Value);

            Bloqueo_Cadete bloc = new Bloqueo_Cadete();
            bloc.id = id;
            bloc.nombre = nombre;
            bloc.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Decimal id = 0;
            String nombre;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            nombre = Convert.ToString(row.Cells["Nombre Cadete"].Value);
            PagoCadetes pg = new PagoCadetes();
            pg.id = id;
            pg.nombre = nombre;
            pg.ShowDialog();
        }
    }
}
