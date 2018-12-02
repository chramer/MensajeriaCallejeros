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
    public partial class Bloqueo_Cadete : Form
    {
        FbConnection conexion = new FbConnection();
        int bandera;
        string sentencia;
        public decimal id;
        public string nombre;
        public Bloqueo_Cadete()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bloqueo_Cadete_Load(object sender, EventArgs e)
        {
            label2.Text = nombre;
            verifica_inserta();
        }

        private void verifica_inserta() {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera clientes
            try
            {
                sentencia = "select count(*) from tb_bloqcadet where id_cadet = '" + id + "'";
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
                        bandera = 1;
                        label3.Text = "Estado: Desbloqueado";
                        label3.ForeColor = Color.Green;
                    }
                    else
                    {
                        setea_valor();
                        bandera = 0;
                    }
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
                sentencia = "insert into tb_bloqcadet (id_cadet,marc_bloqueo) " +
                    "values ('" + id + "',0)";
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

        private void setea_valor()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera clientes
            try
            {
                sentencia = "select marc_bloqueo,fecha_bloqueo,comentario from tb_bloqcadet where id_cadet = '" + id + "'";
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                foreach (DataRow row in dt.Rows)
                {
                    int marca;
                    string comentario;
                    marca = Convert.ToInt16(row[0]);
                    comentario = Convert.ToString(row[2]);
                    textBox1.Text = comentario;
                    if (marca == 0)
                    {
                        label3.Text = "Estado: Desbloqueado";
                        label3.ForeColor = Color.Green;
                        button2.Text = "Bloquear";
                    }
                    else
                    {
                        label3.Text = "Estado: Bloqueado";
                        label3.ForeColor = Color.Red;
                        button2.Text = "Desbloquear";
                        dateTimePicker1.Value = Convert.ToDateTime(row[1]);
                    }

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
            if (button2.Text == "Bloquear")
            {
                conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
                try
                {
                    conexion.Open();
                    sentencia = "update tb_bloqcadet set marc_bloqueo = 1,fecha_bloqueo = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "' , comentario = '" + textBox1.Text + "' where id_cadet = '" + id + "'";
                    FbCommand cmd = new FbCommand(sentencia, conexion);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conexion.Close();
                    MessageBox.Show("El Cadete fue Bloqueado con Exito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message.ToString(), "Error");
                }
            }
            else
            {
                if (MessageBox.Show("¿Esta seguro que desea Desbloquear al Cadete?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
                try
                {
                    conexion.Open();
                    sentencia = "update tb_bloqcadet set marc_bloqueo = 0,fecha_bloqueo = null, comentario = null where id_cadet = '" + id + "'";
                    FbCommand cmd = new FbCommand(sentencia, conexion);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conexion.Close();
                    MessageBox.Show("El Cadete fue desbloqueado con Exito", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message.ToString(), "Error");
                }
            }
        }
    }
}
