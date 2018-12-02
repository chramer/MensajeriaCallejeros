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

        public PagoCadetes()
        {
            InitializeComponent();
        }

        private void PagoCadetes_Load(object sender, EventArgs e)
        {
            // Fecha seleccionada en el control DataTimePicker
            DateTime fecha = dateTimePicker2.Value.Date;
            DateTime fechaLunes = GetFirstDayOfWeek(fecha);
            dateTimePicker2.Value = fechaLunes;

            label2.Text = nombre;
        }

        public DateTime GetFirstDayOfWeek(DateTime currentDate)
        {

            // Referenciamos la cultura invariable.
            // 
            CultureInfo ci = CultureInfo.InvariantCulture;

            // Obtenemos el día de la semana correspondiente a la fecha actual.
            // 
            DayOfWeek ds = ci.Calendar.GetDayOfWeek(currentDate);

            // Como el primer día de la semana es el 0 (Domingo), construimos
            // un array para conocer los días que hay que restar de la fecha.
            // Así, si es Domingo (0) restaremos 6 días, si es Sábado (6)
            // restaremos 5 días, y si es Lunes (1) restaremos 0 días.
            // Recordar que en .NET los índices de los arrays están en base cero.
            // 
            int[] dias = new[] { -6, 0, 1, 2, 3, 4, 5 };

            // De la fecha actual restamos los días correspondientes.
            // 
            return currentDate.Subtract(new TimeSpan(dias[Convert.ToInt16(ds)], 0, 0, 0));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
