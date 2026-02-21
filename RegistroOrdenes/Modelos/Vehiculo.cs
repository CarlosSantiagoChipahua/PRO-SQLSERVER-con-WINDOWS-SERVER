using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistroOrdenes.Modelos
{
	public class Vehiculo
	{
		// Representa un vehículo perteneciente a un cliente.
		public string NumeroSerie { get; set; }
		public string Placas { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public int Anio { get; set; }
		public string Color { get; set; }
		public int Kilometraje { get; set; }
		public string Tipo { get; set; }
		public string ClaveInterna { get; set; } 
		public int Antigüedad { get; set; }     
		public bool Activo { get; set; }

		// Propiedad para mostrar descripcion en dropdowns
		// Descripcion para mostrar en combos o listas.
		// Formato: "Marca Modelo Año - Placas"
		public string Descripcion => $"{Marca} {Modelo} {Anio} - {Placas}";
	}
}