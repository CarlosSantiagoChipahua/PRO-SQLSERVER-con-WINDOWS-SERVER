using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroOrdenes.Modelos
{
	public class DetalleServicioConNombre : DetalleServicio
	{
		// Extiende DetalleServicio para incluir el nombre del servicio.
		// Utilizado principalmente para mostrar en GridView sin hacer joins adicionales en cada acceso.
		public string Nombre { get; set; }

		// Propiedad calculada que devuelve el importe total de este renglon (cantidad * precio aplicado).
		public decimal Importe => Cantidad * PrecioAplicado;
	}
}