using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace MensajeriaCallejeros
{
    public partial class Admin_Localidades : Form
    {
        FbConnection conexion = new FbConnection();
        string sentencia;

        public Admin_Localidades()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal id = 0;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Max ID cliente
            try
            {
                conexion.Open();
                sentencia = "select max(cod_localidad) from tb_localidad";
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

            //Inserta Cliente
            if (textBox1.Text == "")
            {
                MessageBox.Show("La Localidad no puede tener descripcion vacia", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conexion.Open();
                sentencia = "insert into tb_localidad(cod_localidad,localidad) values ('" + id + "','" + this.textBox1.Text + "')";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
            textBox1.Clear();
            textBox1.Focus();
            Retrive();
        }

        private void Admin_Localidades_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn col0 = new DataGridViewTextBoxColumn();
            col0.Name = "id";
            col0.Visible = false;
            dataGridView1.Columns.Add(col0);
            dataGridView1.Columns[0].Width = 10;
            dataGridView1.Columns[0].Visible = false;

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Nombre de Localidad";
            dataGridView1.Columns.Add(col1);
            dataGridView1.Columns[1].Width = 295;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.RowHeadersVisible = false;

            Retrive();
        }

        private void Retrive()
        {
            dataGridView1.Rows.Clear();
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Max ID cliente
            try
            {
                conexion.Open();
                sentencia = "select cod_localidad,localidad from tb_localidad";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                foreach (DataRow row in dt.Rows)
                {
                    dataGridView1.Rows.Add(row[0], row[1]);
                }
                dt.Rows.Clear();
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Elimina Localidad
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["id"].Value);

            if (MessageBox.Show("¿Esta seguro que desea eliminar la Localidad?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
                try
                {
                    conexion.Open();
                    sentencia = "delete from tb_localidad where cod_localidad = '" + id + "'";
                    FbCommand cmd = new FbCommand(sentencia, conexion);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conexion.Close();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message.ToString(), "Error");
                }

            }
            Retrive();
        }
    }
}
