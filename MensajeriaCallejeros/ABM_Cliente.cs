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
    public partial class ABM_Cliente : Form
    {
        FbConnection conexion = new FbConnection();
        string sentencia;
        public string bandera;
        public decimal id_mod;
        public ABM_Cliente()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ABM_Cliente_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = false;
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

            if (bandera == "Alta")
            {
                button5.Enabled = false;
                button6.Enabled = false;
                button4.Enabled = false;
            }
            else
            {
                button3.Enabled = false;
                traer_datos();
                button2.Text = "Salir";
            }
            

        }


        private void traer_datos()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera Datos del cliente
            try
            {
                conexion.Open();
                sentencia = "select * from tb_clientes where id_cli = '" + id_mod + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                foreach (DataRow row in dt.Rows)
                {
                    textBox1.Text = Convert.ToString(row[1]);
                    textBox2.Text = Convert.ToString(row[2]);
                    textBox3.Text = Convert.ToString(row[3]);
                    textBox4.Text = Convert.ToString(row[4]);
                    textBox5.Text = Convert.ToString(row[5]);
                    dateTimePicker1.Value  = Convert.ToDateTime(row[6]);
                    comboBox1.Text = Convert.ToString(row[7]);
                    textBox6.Text = Convert.ToString(row[8]);
                }
                dt.Rows.Clear();
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
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
                sentencia = "select max(id_cli) from tb_clientes";
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
            decimal  tel_aux;
            decimal cel;

            //Inserta Cliente
            if (textBox4.Text == "")
            {
                tel_aux = 0;
            }
            else
            {
                tel_aux = Convert.ToDecimal(textBox4.Text);
            }

            if (textBox5.Text == "")
            {
                cel = 0;
            }
            else
            {
                cel = Convert.ToDecimal(textBox5.Text);
            }


            try
            {
                conexion.Open();
                sentencia = "insert into tb_clientes(id_cli,nombre_cli,domicilio_cli,telefonoprin_cli,telefonoaux_cli,telefonocel_cli,fecalta_cli,localidad_cli,comentarios_cli) values ('" + id + "','" + this.textBox1.Text + "','" + this.textBox2.Text + "','" + this.textBox3.Text + "','" + tel_aux + "','" + cel + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "','" + comboBox1.Text + "','" + textBox6.Text + "')";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();

                if (MessageBox.Show("Alta de cliente exitosa. ¿Desea continuar con la configuración del cliente?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes) 
                {
                    button4.Enabled = false;
                    button3.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    comboBox1.Enabled = false;
                    button1.Enabled = false;
                    textBox6.Enabled  = false;
                    id_mod = id;
                }
                else
                {
                    this.Close();
                }

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            decimal tel_aux;
            decimal cel;

            //Inserta Cliente
            if (textBox4.Text == "")
            {
                tel_aux = 0;
            }
            else
            {
                tel_aux = Convert.ToDecimal(textBox4.Text);
            }

            if (textBox5.Text == "")
            {
                cel = 0;
            }
            else
            {
                cel = Convert.ToDecimal(textBox5.Text);
            }


            try
            {
                conexion.Open();
                sentencia = "update tb_clientes set nombre_cli = '" + this.textBox1.Text + "',domicilio_cli = '" + this.textBox2.Text + "',telefonoprin_cli = '" + this.textBox3.Text + "',telefonoaux_cli = '" + tel_aux + "',telefonocel_cli = '" + cel + "',localidad_cli = '" + comboBox1.Text + "',comentarios_cli = '" + textBox6.Text + "' where id_cli = '" + id_mod + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();

                MessageBox.Show("Cliente Modificado con Éxito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Admin_Localidades  abmloc = new Admin_Localidades();
            abmloc.ShowDialog();
            //comboBox1.Items.Clear();
            comboBox1.DataSource = DataHelper.LoadDataTable();
            comboBox1.DisplayMember = "localidad";
            comboBox1.ValueMember = "cod_localidad";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PropagandaCliente propcli = new PropagandaCliente();
            propcli.id = id_mod;
            propcli.nombre = textBox1.Text;
            propcli.ShowDialog();
        }
    }
}
