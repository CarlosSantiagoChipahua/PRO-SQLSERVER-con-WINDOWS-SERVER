using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroOrdenes.Modelos
{
	public class DetalleServicio
	{
		// Representa la relacion muchos a muchos entre una orden de servicio y los servicios aplicados.
		// Almacena la cantidad y el precio aplicado en el momento de la orden.
		public int FolioOrden { get; set; }
		public int ClaveServicio { get; set; }
		public int Cantidad { get; set; }
		public decimal PrecioAplicado { get; set; }
	}
}