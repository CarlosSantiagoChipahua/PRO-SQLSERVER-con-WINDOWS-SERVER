using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroOrdenes.Modelos
{
	public class Servicio
	{
		// Representa un servicio que puede ser ofrecido en el taller
		// (ej. Cambio de aceite, Alineacion).
		public int ClaveServicio { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public decimal CostoBase { get; set; }
		public decimal TiempoEstimado { get; set; } // Tiempo estimado de duracion en horas
		public bool Activo { get; set; }
	}
}