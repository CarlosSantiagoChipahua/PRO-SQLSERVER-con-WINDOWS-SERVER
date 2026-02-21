using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RegistroOrdenes.Modelos;

namespace RegistroOrdenes.Datos
{
	// Repositorio para operaciones relacionadas con servicios.
	public class ServicioRepositorio
	{
		// Obtiene todos los servicios activos ordenados por nombre.
		// regresa una lista de servicios con Activo = true.
		public List<Servicio> ObtenerTodosActivos()
		{
			var lista = new List<Servicio>();
			using (SqlConnection con = new SqlConnection(Conexion.Cadena))
			{
				string query = "SELECT clave_servicio, nombre, descripcion, costo_base, tiempo_estimado, activo " +
							   "FROM Servicios WHERE activo = 1 ORDER BY nombre";
				SqlCommand cmd = new SqlCommand(query, con);
				con.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					lista.Add(new Servicio
					{
						ClaveServicio = (int)dr["clave_servicio"],
						Nombre = dr["nombre"].ToString(),
						Descripcion = dr["descripcion"]?.ToString(),
						CostoBase = (decimal)dr["costo_base"],
						TiempoEstimado = (decimal)dr["tiempo_estimado"],
						Activo = (bool)dr["activo"]
					});
				}
			}
			return lista;
		}
	}
}