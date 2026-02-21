using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using RegistroOrdenes.Modelos;

namespace RegistroOrdenes.Datos
{
	// Repositorio para operaciones relacionadas con ordenes de servicio y sus detalles.
	public class OrdenRepositorio
	{
		// Inserta una nueva orden de servicio junto con sus detalles en una transaccion.
		// retorna el folio de la orden recién insertada.
		public int InsertarOrdenConDetalles(OrdenServicio orden, List<DetalleServicio> detalles)
		{
			using (SqlConnection con = new SqlConnection(Conexion.Cadena))
			{
				con.Open();
				SqlTransaction tran = con.BeginTransaction();

				try
				{
					// Insertar encabezado
					SqlCommand cmd = new SqlCommand(
						@"INSERT INTO Ordenes_Servicio (numero_serie, fecha_ingreso, fecha_estimada_entrega, estado, costo_total, activo)
                          VALUES (@numSerie, @fechaIngreso, @fechaEstimada, @estado, @costoTotal, 1);
                          SELECT SCOPE_IDENTITY();", con, tran);

					cmd.Parameters.AddWithValue("@numSerie", orden.NumeroSerie);
					cmd.Parameters.AddWithValue("@fechaIngreso", orden.FechaIngreso);
					cmd.Parameters.AddWithValue("@fechaEstimada", orden.FechaEstimadaEntrega);
					cmd.Parameters.AddWithValue("@estado", orden.Estado);
					cmd.Parameters.AddWithValue("@costoTotal", orden.CostoTotal ?? 0);

					int folio = Convert.ToInt32(cmd.ExecuteScalar());

					// Insertar detalles
					foreach (var det in detalles)
					{
						SqlCommand cmdDet = new SqlCommand(
							@"INSERT INTO Orden_Servicio_Servicios (folio_orden, clave_servicio, cantidad, precio_aplicado)
                              VALUES (@folio, @clave, @cantidad, @precio)", con, tran);
						cmdDet.Parameters.AddWithValue("@folio", folio);
						cmdDet.Parameters.AddWithValue("@clave", det.ClaveServicio);
						cmdDet.Parameters.AddWithValue("@cantidad", det.Cantidad);
						cmdDet.Parameters.AddWithValue("@precio", det.PrecioAplicado);
						cmdDet.ExecuteNonQuery();
					}

					tran.Commit();
					return folio;
				}
				catch
				{
					tran.Rollback();
					throw;
				}
			}
		}
	}
}