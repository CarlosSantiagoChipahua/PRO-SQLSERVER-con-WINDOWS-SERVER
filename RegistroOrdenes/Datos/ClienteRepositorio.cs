using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using RegistroOrdenes.Modelos;
using System.Web;

namespace RegistroOrdenes.Datos
{
	// Repositorio para realizar operaciones de lectura/escritura de clientes en la base de datos.
	public class ClienteRepositorio
	{
		// Obtiene todos los clientes activos ordenados por nombre.
		// regresa una lista de objetos Cliente con Activo = true.
		public List<Cliente> ObtenerTodosActivos()
		{
			var lista = new List<Cliente>();
			using (SqlConnection con = new SqlConnection(Conexion.Cadena))
			{
				string query = "SELECT clave_interna, rfc, nombre, apellido_paterno, apellido_materno, " +
							   "telefono1, telefono2, telefono3, calle, numero, colonia, codigo_postal, " +
							   "ciudad, email, fecha_registro, activo " +
							   "FROM Clientes WHERE activo = 1 ORDER BY nombre";
				SqlCommand cmd = new SqlCommand(query, con);
				con.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					lista.Add(new Cliente
					{
						ClaveInterna = dr["clave_interna"].ToString(),
						RFC = dr["rfc"].ToString(),
						Nombre = dr["nombre"].ToString(),
						ApellidoPaterno = dr["apellido_paterno"].ToString(),
						ApellidoMaterno = dr["apellido_materno"]?.ToString(),
						Telefono1 = dr["telefono1"].ToString(),
						Telefono2 = dr["telefono2"]?.ToString(),
						Telefono3 = dr["telefono3"]?.ToString(),
						Calle = dr["calle"].ToString(),
						Numero = dr["numero"].ToString(),
						Colonia = dr["colonia"].ToString(),
						CodigoPostal = dr["codigo_postal"].ToString(),
						Ciudad = dr["ciudad"].ToString(),
						Email = dr["email"]?.ToString(),
						FechaRegistro = (System.DateTime)dr["fecha_registro"],
						Activo = (bool)dr["activo"]
					});
				}
			}
			return lista;
		}
	}
}