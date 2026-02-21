using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroOrdenes.Modelos
{
	// Representa un cliente del taller.
	// Almacena información personal, de contacto y dirección.
	public class Cliente
	{
		public string ClaveInterna { get; set; }
		public string RFC { get; set; }
		public string Nombre { get; set; }
		public string ApellidoPaterno { get; set; }
		public string ApellidoMaterno { get; set; }
		public string Telefono1 { get; set; }
		public string Telefono2 { get; set; }
		public string Telefono3 { get; set; }
		public string Calle { get; set; }
		public string Numero { get; set; }
		public string Colonia { get; set; }
		public string CodigoPostal { get; set; }
		public string Ciudad { get; set; }
		public string Email { get; set; }
		public DateTime FechaRegistro { get; set; }
		public bool Activo { get; set; }

		// Propiedad para mostrar nombre completo
		public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();
	}
}