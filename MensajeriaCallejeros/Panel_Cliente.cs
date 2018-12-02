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
    public partial class Panel_Cliente : Form
    {
        FbConnection conexion = new FbConnection();
        string sentencia;
        public string bandera;
        public Panel_Cliente()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel_Cliente_Load(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn col0 = new DataGridViewTextBoxColumn();
            col0.Name = "Id";
            dataGridView1.Columns.Add(col0);
            dataGridView1.Columns[0].Width = 25;

            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Nombre de Cliente";
            dataGridView1.Columns.Add(col1);
            dataGridView1.Columns[1].Width = 225;

            DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            col2.Name = "Domicilio";
            dataGridView1.Columns.Add(col2);
            dataGridView1.Columns[2].Width = 262;

            DataGridViewTextBoxColumn col3 = new DataGridViewTextBoxColumn();
            col3.Name = "Telefono";
            dataGridView1.Columns.Add(col3);
            dataGridView1.Columns[3].Width = 98;

            DataGridViewTextBoxColumn col4 = new DataGridViewTextBoxColumn();
            col4.Name = "Telefono 2";
            dataGridView1.Columns.Add(col4);
            dataGridView1.Columns[4].Width = 98;

            DataGridViewTextBoxColumn col5 = new DataGridViewTextBoxColumn();
            col5.Name = "Celular";
            dataGridView1.Columns.Add(col5);
            dataGridView1.Columns[5].Width = 98;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.RowHeadersVisible = false;

            
            bandera = "general";
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
                conexion.Open();
                if (bandera == "general")
                {
                    sentencia = "select id_cli,nombre_cli,domicilio_cli,telefonoprin_cli,telefonoaux_cli,telefonocel_cli from tb_clientes";
                }

                if (bandera == "nombre")
                {
                    sentencia = "select id_cli,nombre_cli,domicilio_cli,telefonoprin_cli,telefonoaux_cli,telefonocel_cli from tb_clientes where UPPER(nombre_cli) like UPPER('%" + textBox1.Text + "%')";
                }

                if (bandera == "domicilio")
                {
                    sentencia = "select id_cli,nombre_cli,domicilio_cli,telefonoprin_cli,telefonoaux_cli,telefonocel_cli from tb_clientes where  UPPER(domicilio_cli) like UPPER('%" + textBox1.Text + "%')";
                }

                if (bandera == "telefono")
                {
                    sentencia = "select id_cli,nombre_cli,domicilio_cli,telefonoprin_cli,telefonoaux_cli,telefonocel_cli from tb_clientes where   (UPPER(CAST(telefonoprin_cli as varchar(50))) like UPPER('%" + textBox1.Text + "%')) OR (UPPER(CAST(telefonoaux_cli as varchar(50))) like UPPER('%" + textBox1.Text + "%')) OR (UPPER(CAST(telefonocel_cli as varchar(50))) like UPPER('%" + textBox1.Text + "%'))  ";
                }

                if (bandera == "id")
                {
                    sentencia = "select id_cli,nombre_cli,domicilio_cli,telefonoprin_cli,telefonoaux_cli,telefonocel_cli from tb_clientes where   UPPER(CAST(id_cli as varchar(50))) like UPPER('%" + textBox1.Text + "%')";
                }

                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                foreach (DataRow row in dt.Rows)
                {
                    dataGridView1.Rows.Add(row[0], row[1], row[2], row[3], row[4], row[5]);
                }
                dt.Rows.Clear();
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

        private void button2_Click(object sender, EventArgs e)
        {
            //Llama a ventana con concepto de insercion
            ABM_Cliente abmcli = new ABM_Cliente();
            abmcli.bandera = "Alta";
            abmcli.ShowDialog();
            Retrive();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Elimina Cliente
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null) {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            if (MessageBox.Show("¿Esta seguro que desea eliminar el Cliente?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            else
            {
                conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
                try
                {
                    conexion.Open();
                    sentencia = "delete from tb_clientes where id_cli = '" + id + "'";
                    FbCommand cmd = new FbCommand(sentencia, conexion);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conexion.Close();
                    MessageBox.Show("Cliente Eliminado con Exito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            //Recupera Id para modificar
            Decimal id = 0;
            DataGridViewRow row = new DataGridViewRow();
            row = dataGridView1.CurrentRow;

            if (row == null)
            {
                return;
            }

            id = Convert.ToDecimal(row.Cells["Id"].Value);

            //Llama a ventana con concepto de modificacón
            ABM_Cliente abmcli = new ABM_Cliente();
            abmcli.bandera = "Modi";
            abmcli.id_mod  = id;
            abmcli.ShowDialog();
            Retrive();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "nombre";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "telefono";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "domicilio";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            bandera = "id";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           Retrive();
        }
    }
}
