using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RegistroOrdenes.Modelos;

namespace RegistroOrdenes.Datos
{
	// Repositorio para operaciones relacionadas con vehiculos.
	public class VehiculoRepositorio
	{
		// Obtiene los vehiculos activos pertenecientes a un cliente especifico.
		// regresa una lista de vehiculos activos del cliente.
		public List<Vehiculo> ObtenerPorCliente(string claveCliente)
		{
			var lista = new List<Vehiculo>();
			using (SqlConnection con = new SqlConnection(Conexion.Cadena))
			{
				string query = "SELECT numero_serie, placas, marca, modelo, anio, color, kilometraje, tipo, clave_interna, activo " +
							   "FROM Vehiculos WHERE clave_interna = @clave AND activo = 1";
				SqlCommand cmd = new SqlCommand(query, con);
				cmd.Parameters.AddWithValue("@clave", claveCliente);
				con.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					lista.Add(new Vehiculo
					{
						NumeroSerie = dr["numero_serie"].ToString(),
						Placas = dr["placas"].ToString(),
						Marca = dr["marca"].ToString(),
						Modelo = dr["modelo"].ToString(),
						Anio = (int)dr["anio"],
						Color = dr["color"].ToString(),
						Kilometraje = (int)dr["kilometraje"],
						Tipo = dr["tipo"].ToString(),
						ClaveInterna = dr["clave_interna"].ToString(),
						Activo = (bool)dr["activo"]
					});
				}
			}
			return lista;
		}
	}
}