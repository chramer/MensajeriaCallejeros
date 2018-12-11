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
        DateTime fecha;
        decimal importe_cadete = 0;
        decimal importe_normal = 0;
        decimal importe_interes = 0;
        int marca_deuda = 0;
        int bandera_denewsemana = 0;

        public PagoCadetes()
        {
            InitializeComponent();
        }

        private void PagoCadetes_Load(object sender, EventArgs e)
        {
            // Fecha seleccionada en el control DataTimePicker
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            recupera_config();
            //Recupera clientes
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
                            fecha = Convert.ToDateTime(row1[0]);
                            if (fecha == DateTime.Today)
                            {
                                fecha = DateTime.Today;
                                dateTimePicker2.Value = fecha.AddDays(7);
                                actualizar_fecysdo();
                            }
                            else {
                                dateTimePicker2.Value = fecha;
                            }
                            traer_mon_tot_cadet();
                            importe_cadete = importe_normal;
                        }
                    }
                }
                dt.Rows.Clear();
                label2.Text = nombre;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }
            Retrive();
        }

        private void traer_mon_tot_cadet()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera clientes
            try
            {
                sentencia = "select monto_tot,marca_deuda from tb_cadete_pago where id_cadet = '" + id + "'";
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    importe_normal = Convert.ToDecimal(row[0]);
                    marca_deuda = Convert.ToInt16(row[1]);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }
        }

            private void actualizar_fecysdo()
            {
            bandera_denewsemana = 1;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            recupera_config();
            decimal id_reg = 0;
            try
            {
                conexion.Open();
                sentencia = "select max(id_reg_pg) from TB_CADET_REG_PAGO where id_cadet = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
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
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }
            traer_mon_tot_cadet();
            importe_normal = Busca_resto(id_reg, conexion, sentencia, importe_cadete, 0);
            try
            {
                if (importe_normal != 0) {
                    importe_cadete = importe_cadete + importe_normal;
                    if (marca_deuda > 1)
                    {
                        marca_deuda = marca_deuda + 1;
                    }
                    else
                    {
                        marca_deuda = 1;
                    }
                }

                conexion.Open();
                sentencia = "update tb_cadete_pago " +
                            "set fecha_venc = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value) + "' ," +
                            "monto_tot = '" + importe_cadete  + "', marca_deuda = '" + marca_deuda + "'  where id_cadet = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
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
                return;
            }
        }

        private void inserta_fila()
        {
            decimal importe;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                importe = 0;

                if ((Convert.ToDecimal(textBox2.Text)) != 0)
                {
                    importe = Convert.ToDecimal(textBox2.Text);
                }

                if ((Convert.ToDecimal(textBox3.Text)) != 0)
                {
                    importe = Convert.ToDecimal(textBox3.Text);
                }

                if ((Convert.ToDecimal(textBox4.Text)) != 0)
                {
                    importe = Convert.ToDecimal(textBox4.Text);
                }

                fecha = dateTimePicker1.Value;
                dateTimePicker2.Value = fecha.AddDays(7);

                conexion.Open();
                sentencia = "insert into tb_cadete_pago (id_cadet,ultimo_pago,pago_actual,fecha_pago,fecha_venc,monto_tot,marca_deuda) " +
                    "values ('" + id + "',0,'" + importe + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value)  + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value) + "','" + importe_cadete +"','" + marca_deuda + "')";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
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
                inserta_regpag();
                inserta_fila();
            }
            else
            {
                inserta_regpag();
                actualiza_cadet_pag();
            }
        }

        private void actualiza_cadet_pag() {
            decimal importe;
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                importe = 0;

                if ((Convert.ToDecimal(textBox2.Text)) != 0)
                {
                    importe = Convert.ToDecimal(textBox2.Text);
                }

                if ((Convert.ToDecimal(textBox3.Text)) != 0)
                {
                    importe = Convert.ToDecimal(textBox3.Text);
                }

                if ((Convert.ToDecimal(textBox4.Text)) != 0)
                {
                    importe = Convert.ToDecimal(textBox4.Text);
                }

                conexion.Open();
                sentencia = "update tb_cadete_pago " +
                            "set ultimo_pago = pago_actual ," +
                            "pago_actual = '" + importe + "' ," +
                            "fecha_pago = '" + String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "', " +
                            " marca_deuda = '" + marca_deuda + "' where id_cadet = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }
        }

        private void inserta_regpag()
        {
            decimal id_reg = 0;
            decimal id_reg2 = 0;
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
                conexion.Close();
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
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }
            /*Logica de Inserción */
            decimal importe = 0;
            decimal resto = 0;
            int compensa = 0;
            int tip_pago = 0;


            try
            {
                conexion.Open();
                sentencia = "select max(id_reg_pg) from TB_CADET_REG_PAGO where id_cadet = '"+ id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == DBNull.Value)
                    {
                        id_reg2 = 0;
                    }
                    else
                    {
                        id_reg2 = Convert.ToDecimal(row[0]);
                    }
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }

            try
            {
                sentencia = "select count(*) from TB_CADET_REG_PAGO where id_reg_pg = '" + id_reg2 + "'";
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (bandera_denewsemana == 1)
                    {
                        resto = importe_cadete;
                    }
                    else
                    {
                        if (Convert.ToInt16(row[0]) == 0)
                        {
                            resto = importe_cadete;
                        }
                        else
                        {
                            resto = Busca_resto(id_reg2, conexion, sentencia, importe_cadete, resto);
                        }
                    }
                }
                dt.Rows.Clear();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }

            decimal origen = 0;

            if ((Convert.ToDecimal(textBox2.Text)) != 0)
            {
                importe = importe + Convert.ToDecimal(textBox2.Text);
                origen = 1; //Pago efectivo
            }

            if ((Convert.ToDecimal(textBox3.Text)) != 0)
            {
                importe = importe + Convert.ToDecimal(textBox3.Text);
                origen = 2; //Pago Cupon
            }

            if ((Convert.ToDecimal(textBox4.Text)) != 0)
            {
                importe = importe + Convert.ToDecimal(textBox4.Text);
                origen = 3; //Pago CC
            }

            if (checkBox1.Checked == true)
            {
                compensa = 1;
                tip_pago = 0; //Pago compensado por Admin/Usuario

                resto = resto - importe;
                importe = resto;
                resto = 0;
                origen = 4;
            }
            else
            {
                resto = resto - importe;
                compensa = 0;
                if(resto == 0)
                {
                    marca_deuda = 0;
                    //Pago Completado
                    tip_pago = 1;
                    if (resto < 0)
                    {
                        if (MessageBox.Show("El Cadete realiza un pago que dejara saldo a su favor. ¿Desea continuar con la Operación? ", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.No) {
                            return;
                        }
                    }
                    else {
                        MessageBox.Show("El Cadete completo el Pago semanal", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    tip_pago = 2; //Pago Pago Parcial
                    resto = resto + importe_interes;
                }
            }

            string fecha = DateTime.Now.ToString("yyyy-MM-dd");
            string tiempo = DateTime.Now.ToString("HH:mm:ss.ffff");
            fecha = fecha + " " + tiempo;
            try
            {
                conexion.Open();
                sentencia = "insert into TB_CADET_REG_PAGO (id_reg_pg,id_cadet,fecpago,fecven,fecreal,fecpro,importe,sdo_total,sdo_restante,tip_pago,compensado,comentario,origen_pago) " +
                    "values ('" + id_reg + "','" + id + "','"+ String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value) + "','" + String.Format("{0:yyyy-MM-dd}", dateTimePicker2.Value) + "'," +
                    "'" + fecha + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + "','" + importe + "','" + importe_cadete + "','" + resto + "','" + tip_pago + "','" + compensa + "','" + textBox1.Text + "','" + origen + "')";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                cmd.ExecuteNonQuery();
                cmd = null;
                conexion.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }
        }
        private static  decimal Busca_resto(decimal id_reg2,FbConnection conexion,string sentencia,decimal importe_cadete,decimal resto)
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            try
            {
                conexion.Open();
                sentencia = "select sdo_restante from TB_CADET_REG_PAGO where id_reg_pg = '" + id_reg2 + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == null)
                    {
                        resto = Convert.ToDecimal(importe_cadete);
                    }
                    else
                    {
                        resto = Convert.ToDecimal(row[0]);
                    }
                }
                dt.Rows.Clear();
                return resto;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return -1;
            }
        }
        private void Retrive()
        {
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());
            //Recupera clientes
            decimal marca_deuda = 0;
            try
            {
                sentencia = "select pago_actual,fecha_pago,marca_deuda from tb_cadete_pago where id_cadet = '" + id + "'";
                conexion.Open();
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                        label18.Text = Convert.ToString(row[0]);
                        label15.Text = Convert.ToString(row[1]);
                        marca_deuda = Convert.ToDecimal(row[2]);
                }
                if (dt.Rows.Count == 0)
                {
                    label18.Text = "Sin datos";
                    label15.Text = "Sin datos";
                    marca_deuda = 9;
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }

            decimal id_reg2 = 0;
            decimal sdo_total = 0;
            decimal origen_pago = 0;
            decimal tipo_pago = 0;
            try
            {
                conexion.Open();
                sentencia = "select max(id_reg_pg) from TB_CADET_REG_PAGO where id_cadet = '" + id + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == DBNull.Value)
                    {
                        id_reg2 = 0;

                    }
                    else
                    {
                        id_reg2 = Convert.ToDecimal(row[0]);
                    }
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }

            try
            {
                conexion.Open();
                sentencia = "select origen_pago,sdo_restante,tip_pago from TB_CADET_REG_PAGO where id_reg_pg = '" + id_reg2 + "'";
                FbCommand cmd = new FbCommand(sentencia, conexion);
                FbDataReader fb_datareader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(fb_datareader);
                cmd = null;
                conexion.Close();
                foreach (DataRow row in dt.Rows)
                {
                    label12.Text = Convert.ToString(row[1]);
                    origen_pago = Convert.ToDecimal(row[0]);
                    tipo_pago = Convert.ToDecimal(row[2]);
                }
                if (dt.Rows.Count == 0)
                {
                    label12.Text = "Sin Datos";
                    origen_pago = 9;
                    tipo_pago = 9;
                }
                dt.Rows.Clear();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message.ToString(), "Error");
                return;
            }

            if(marca_deuda == 0 && tipo_pago == 1)
            {
                label10.Text = "Al día";
                label10.ForeColor = Color.Green;
            }
            else
            {
                if (marca_deuda == 0 && tipo_pago == 2)
                {
                    label10.Text = "Al día,con Pago Parcial";
                    label10.ForeColor = Color.Green;
                }
                else
                {
                    if (marca_deuda == 0 && tipo_pago == 0)
                    {
                        label10.Text = "Al día,Conpensado por Usuario";
                        label10.ForeColor = Color.Green;
                    }
                }
            }

            if (marca_deuda == 1)
            {
                label10.Text = "Con Atraso Moderado";
                label10.ForeColor = Color.Orange;
            }

            if (marca_deuda > 1)
            {
                label10.Text = "Con Atraso Riesgoso";
                label10.ForeColor = Color.Red;
            }

            if (origen_pago == 1)
            {
                label20.Text = "Efectivo";
            }

            if (origen_pago == 2)
            {
                label20.Text = "Cupones";
            }

            if (origen_pago == 3)
            {
                label20.Text = "Cuenta Corriente";
            }

            if (marca_deuda == 9)
            {
                label10.Text = "Sin Datos, Cadete Nuevo";
                label10.ForeColor = Color.Black;
            }
            if (origen_pago  == 9)
            {
                label20.Text = "Sin Datos";
            }
        }
    }
}
