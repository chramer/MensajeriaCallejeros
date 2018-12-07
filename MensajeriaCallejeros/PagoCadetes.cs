using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using FirebirdSql.Data.FirebirdClient;

namespace MensajeriaCallejeros
{
    public partial class PagoCadetes : Form
    {
        FbConnection conexion = new FbConnection();
        string bandera;
        string sentencia;
        public string nombre;
        public decimal id;
        decimal importe_cadete = 0;
        decimal importe_interes = 0;
        public PagoCadetes()
        {
            InitializeComponent();
        }

        private void PagoCadetes_Load(object sender, EventArgs e)
        {
            // Fecha seleccionada en el control DataTimePicker
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera clientes
            recupera_config();
            try
            {
                sentencia = "select count(*) from tb_cadete_pago where id_cadet = '" + id + "'";
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
                        bandera = "PP";
                    }
                    else
                    {
                        bandera = "PN";
                        sentencia = "select fecha_venc from tb_cadete_pago where id_cadet = '" + id + "'";
                        conexion.Open();
                        FbCommand cmd1 = new FbCommand(sentencia, conexion);
                        FbDataReader fb_datareader1 = cmd1.ExecuteReader();
                        DataTable dt1 = new DataTable();
                        dt1.Load(fb_datareader1);
                        conexion.Close();
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            DateTime fecha = Convert.ToDateTime(row1[0]);
                            if (fecha == DateTime.Today)
                            {
                                fecha = DateTime.Today;
                                dateTimePicker2.Value = fecha.AddDays(7);
                                actualizar_fecysdo();
                            }
                            else
                            {
                                dateTimePicker2.Value = fecha.AddDays(7); ;
                            }
                            
                        }
                    }
                }
                dt.Rows.Clear();
                label2.Text = nombre;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
            
        }
        private void actualizar_fecysdo()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            recupera_config();
            try
            {
                conexion.Open();
                sentencia = "update tb_cadete_pago " +
                            "set fecha_venc = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value) + "' ," +
                            "monto_tot = '" + importe_cadete  + "'  where id_cadet = '" + id + "'";
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

        private void recupera_config()
        {
            /*Traigo configuración*/
            sentencia = "select * from tb_confi_PC";
            try
            {
                conexion.Open();
                FbCommand cmd1 = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader1 = cmd1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(fb_datareader1);
                conexion.Close();
                foreach (DataRow row1 in dt1.Rows)
                {
                    importe_cadete = Convert.ToDecimal(row1[0]);
                    importe_interes = Convert.ToDecimal(row1[1]);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
        }

        private void inserta_fila()
        {
            decimal importe;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                importe = 0; 

                if (textBox1.Text != "0")
                {
                    importe = Convert.ToDecimal(textBox2.Text);
                }

                if (textBox2.Text != "0")
                {
                    importe = Convert.ToDecimal(textBox3.Text);
                }

                if (textBox3.Text != "0")
                {
                    importe = Convert.ToDecimal(textBox4.Text);
                }

                conexion.Open();
                sentencia = "insert into tb_cadete_pago (id_cadet,ultimo_pago,pago_actual,fecha_pago,fecha_venc,monto_tot) " +
                    "values ('" + id + "',0,'" + importe + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value)  + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value) + "','" + importe_cadete +"')";
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bandera == "PP")
            {
                inserta_fila();
                inserta_regpag();
            }
            else
            {
                actualiza_cadet_pag();
                inserta_regpag();
            }
        }

        private void actualiza_cadet_pag() {
            decimal importe;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                importe = 0;

                if (textBox1.Text != "0")
                {
                    importe = Convert.ToDecimal(textBox2.Text);
                }

                if (textBox2.Text != "0")
                {
                    importe = Convert.ToDecimal(textBox3.Text);
                }

                if (textBox3.Text != "0")
                {
                    importe = Convert.ToDecimal(textBox4.Text);
                }

                conexion.Open();
                sentencia = "update tb_cadete_pago " +
                            "set ultimo_pago = pago_actual ," +
                            "pago_actual = '" + importe + "' ," +
                            "fecha_pago = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "' " +
                            "where id_cadet = '" + id + "'";
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

        private void inserta_regpag()
        {
            decimal id_reg;
            id_reg = 0;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                conexion.Open();
                sentencia = "select max(id_reg_pg) from TB_CADET_REG_PAGO";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;


                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == DBNull.Value)
                    {
                        id_reg = 0;
                    }
                    else
                    {
                        id_reg = Convert.ToDecimal(row[0]);
                    }
                }

                id_reg = id_reg + 1;
                dt.Rows.Clear();
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
            }
            /*Logica de Inserción */
            decimal importe = 0;
            decimal resto = 0;
            int compensa = 0;
            int tip_pago = 0;

            if (textBox1.Text != "0")
            {
                importe = Convert.ToDecimal(textBox2.Text);
            }

            if (textBox2.Text != "0")
            {
                importe = Convert.ToDecimal(textBox3.Text);
            }

            if (textBox3.Text != "0")
            {
                importe = Convert.ToDecimal(textBox4.Text);
            }

            if (checkBox1.Checked == true)
            {
                compensa = 1;
                tip_pago = 0; //Pago compensado por Admin/Usuario

                resto = importe_cadete - importe;
                importe = resto;
                resto = 0;
            }
            else
            {
                resto = importe_cadete - importe;
                compensa = 0;
                if(resto == 0)
                {
                    tip_pago = 1; //Pago Completado
                }
                else
                {
                    tip_pago = 2; //Pago Pago Parcial
                }
            }

            try
            {
                conexion.Open();
                sentencia = "insert into TB_CADET_REG_PAGO (id_reg_pg,id_cadet,fecpago,fecven,fecreal,fecpro,importe,sdo_total,sdo_restante,tip_pago,compensado,comentario) " +
                    "values ('" + id_reg + "','" + id + "','"+ String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value) + "'," +
                    "'" + String.Format("{0:yyyy-MM-dd hh:mm:ss tt}", DateTime.Today) + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + "','" + importe + "','" + importe_cadete + "','" + resto + "','" + tip_pago + "','" + compensa + "','" + textBox1.Text + "')";
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
    }
}
