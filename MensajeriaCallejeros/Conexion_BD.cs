using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using System.Windows.Forms;

namespace MensajeriaCallejeros
{
    public class Conexion_BD
    {
        public static FbConnectionStringBuilder Recuperar_cadena()
        {
            FbConnectionStringBuilder fb_conexion = new FbConnectionStringBuilder();
            //
            // Assume failure.
            //fb_conexion.ServerType = FbServerType.Default;
            //fb_conexion.UserID = "sysdba";
            //fb_conexion.Password = "masterkey";
            //fb_conexion.Dialect = 3;
            //fb_conexion.Database = "/home/chramerserv/DBFB/mensajeriacallejeros.fdb";
            //fb_conexion.DataSource = "chramersoft.ddns.net";
            //fb_conexion.Pooling = true;
            //fb_conexion.Port = 3050;
            //fb_conexion.ConnectionLifeTime = 15;
            //fb_conexion.MinPoolSize = 0;
            //fb_conexion.MaxPoolSize = 50;
            //fb_conexion.PacketSize = 8192;
            //fb_conexion.Charset = "None";

            fb_conexion.ServerType = FbServerType.Default;
            fb_conexion.UserID = "sysdba";
            fb_conexion.Password = "masterkey";
            fb_conexion.Dialect = 3;
            //  fb_conexion.Database = "C:\\Users\\Nicoa\\OneDrive\\MENSAJERIACALLEJEROS.FDB";
            string fullimagepath = Path.Combine(Application.StartupPath, "BaseData\\MENSAJERIACALLEJEROS.FDB");
            fb_conexion.Database = fullimagepath;
            fb_conexion.DataSource = "localhost";
            fb_conexion.Pooling = true;
          //  fb_conexion.Port = 3050;
            fb_conexion.ConnectionLifeTime = 15;
            fb_conexion.MinPoolSize = 0;
            fb_conexion.MaxPoolSize = 50;
            fb_conexion.PacketSize = 8192;
            fb_conexion.Charset = "None";

            return fb_conexion;
        }
    }
}
