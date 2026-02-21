using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RegistroOrdenes.Modelos;
using RegistroOrdenes.Datos;

namespace RegistroOrdenes.Controladores
{
	// Controlador que orquesta la logica de negocio para la creacion de ordenes de servicio.
	// Actua como intermediario entre la capa de presentacion y los repositorios.
	public class OrdenServicioControlador
	{
		private readonly ClienteRepositorio _clienteRepositorio;
		private readonly VehiculoRepositorio _vehiculoRepositorio;
		private readonly ServicioRepositorio _servicioRepositorio;
		private readonly OrdenRepositorio _ordenRepositorio;

		public OrdenServicioControlador()
		{
			_clienteRepositorio = new ClienteRepositorio();
			_vehiculoRepositorio = new VehiculoRepositorio();
			_servicioRepositorio = new ServicioRepositorio();
			_ordenRepositorio = new OrdenRepositorio();
		}

		// Obtiene la lista de clientes activos para llenar combos.
		public List<Cliente> ObtenerClientesActivos() => _clienteRepositorio.ObtenerTodosActivos();

		// Obtiene los vehiculos activos de un cliente especifico.
		public List<Vehiculo> ObtenerVehiculosPorCliente(string claveCliente) =>
			_vehiculoRepositorio.ObtenerPorCliente(claveCliente);

		// Obtiene todos los servicios activos.
		public List<Servicio> ObtenerServiciosActivos() => _servicioRepositorio.ObtenerTodosActivos();

		// Guarda una orden de servicio completa, incluyendo validaciones de negocio,
		// calculo de totales y persistencia mediante el repositorio.
		public int GuardarOrden(string numeroSerie, DateTime fechaEstimada, List<DetalleServicioConNombre> detalles)
		{
			if (string.IsNullOrEmpty(numeroSerie))
				throw new ArgumentException("Debe seleccionar un vehículo.");
			if (detalles == null || detalles.Count == 0)
				throw new ArgumentException("Debe agregar al menos un servicio.");

			// Calcular total (subtotal + IVA 16%)
			decimal subtotal = detalles.Sum(d => d.Cantidad * d.PrecioAplicado);
			decimal iva = subtotal * 0.16m;
			decimal total = subtotal + iva;

			var orden = new OrdenServicio
			{
				NumeroSerie = numeroSerie,
				FechaIngreso = DateTime.Now,
				FechaEstimadaEntrega = fechaEstimada,
				Estado = "Abierta",                // Estado inicial por defecto
				CostoTotal = total,
				Activo = true
			};

			// Convertir a DetalleServicio para el repositorio
			var detallesBD = detalles.Select(d => new DetalleServicio
			{
				FolioOrden = 0, // se asignara en el repositorio
				ClaveServicio = d.ClaveServicio,
				Cantidad = d.Cantidad,
				PrecioAplicado = d.PrecioAplicado
			}).ToList();

			return _ordenRepositorio.InsertarOrdenConDetalles(orden, detallesBD);
		}
	}
}