using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroOrdenes.Modelos
{
	public class OrdenServicio
	{
		// Cabecera de una orden de servicio.
		// Contiene la informacion general de la reparacion o mantenimiento.
		public int FolioOrden { get; set; }
		public string NumeroSerie { get; set; }
		public DateTime FechaIngreso { get; set; }
		public DateTime FechaEstimadaEntrega { get; set; }
		public DateTime? FechaRealEntrega { get; set; }
		public string Estado { get; set; } 
		public decimal? CostoTotal { get; set; }
		public bool Activo { get; set; }
	}
}