using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace MensajeriaCallejeros
{
    class DataHelper
    {
        
        string sentencia;
        public static DataTable LoadDataTable()
        {
            DataTable dt = new DataTable();
            FbConnection conexion = new FbConnection();
            conexion.ConnectionString = Convert.ToString(Conexion_BD.Recuperar_cadena());

            FbCommand  command = new FbCommand();
            command.Connection = conexion;
            command.CommandText = "SELECT * FROM tb_localidad";
            FbDataAdapter  da = new FbDataAdapter(command);

            da.Fill(dt);

            return dt;
        }

        public static AutoCompleteStringCollection LoadAutoComplete()
        {
            DataTable dt = LoadDataTable();

            AutoCompleteStringCollection stringCol = new AutoCompleteStringCollection();

            foreach (DataRow row in dt.Rows)
            {
                stringCol.Add(Convert.ToString(row["Localidad"]));
            }

            return stringCol;
        }
    }
}
