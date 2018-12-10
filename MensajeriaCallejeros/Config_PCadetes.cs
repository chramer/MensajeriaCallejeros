using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace MensajeriaCallejeros
{
    public partial class Config_PCadetes : Form
    {
        FbConnection conexion = new FbConnection();
        string bandera;
        string sentencia;
        public Config_PCadetes()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                conexion.Open();
                sentencia = "update tb_confi_pc set monto_cadete = '" + textBox1.Text + "' ,monto_interes =  '" + textBox2.Text + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
                MessageBox.Show("Datos Guardados con exitos", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }
        private void trae_info()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                sentencia = "select * from tb_confi_pc";
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    textBox1.Text = Convert.ToString(row[0]);
                    textBox2.Text = Convert.ToString(row[1]);
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void inserta_fila()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                conexion.Open();
                sentencia = "insert into tb_confi_pc (monto_cadete,monto_interes) " +
                    "values (0,0)";
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

        private void Config_PCadetes_Load(object sender, EventArgs e)
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                sentencia = "select count(*) from tb_confi_pc";
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt16(row[0]) == 0)
                    {
                        inserta_fila();
                    }
                    else
                    {
                        trae_info();
                    }
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
