using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace RegistroOrdenes.Datos
{
	// Clase estatica que proporciona la cadena de conexion a la base de datos
	// obtenida desde el archivo de configuracion en Web.config
	public class Conexion
	{
		/// Cadena de conexión para SQL Server.
		public static string Cadena = ConfigurationManager.ConnectionStrings["TallerMecanicoConnectionString"].ConnectionString;
	}
}