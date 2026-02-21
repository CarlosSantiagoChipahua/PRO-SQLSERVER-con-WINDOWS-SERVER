using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using RegistroOrdenes.Controladores;
using RegistroOrdenes.Modelos;

namespace RegistroOrdenes
{
	public partial class _Default : System.Web.UI.Page
	{
		private OrdenServicioControlador _controlador = new OrdenServicioControlador();
		private List<DetalleServicioConNombre> listaServicios
		{
			get
			{
				if (Session["listaServicios"] == null)
					Session["listaServicios"] = new List<DetalleServicioConNombre>();
				return (List<DetalleServicioConNombre>)Session["listaServicios"];
			}
			set { Session["listaServicios"] = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
				CargarClientes();
				CargarServicios();
				txtFechaEstimada.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
			}
		}

		private void CargarClientes()
		{
			ddlCliente.DataSource = _controlador.ObtenerClientesActivos();
			ddlCliente.DataTextField = "NombreCompleto";
			ddlCliente.DataValueField = "ClaveInterna";
			ddlCliente.DataBind();
			ddlCliente.Items.Insert(0, new ListItem("-- Seleccione un cliente --", ""));
		}

		private void CargarServicios()
		{
			ddlServicio.DataSource = _controlador.ObtenerServiciosActivos();
			ddlServicio.DataTextField = "Nombre";
			ddlServicio.DataValueField = "ClaveServicio";
			ddlServicio.DataBind();
			ddlServicio.Items.Insert(0, new ListItem("-- Seleccione un servicio --", "0"));
		}

		protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
		{
			string claveCliente = ddlCliente.SelectedValue;
			if (!string.IsNullOrEmpty(claveCliente))
			{
				var vehiculos = _controlador.ObtenerVehiculosPorCliente(claveCliente);
				ddlVehiculo.DataSource = vehiculos;
				ddlVehiculo.DataTextField = "Descripcion";
				ddlVehiculo.DataValueField = "NumeroSerie";
				ddlVehiculo.DataBind();
				ddlVehiculo.Items.Insert(0, new ListItem("-- Seleccione un vehículo --", ""));
			}
			else
			{
				ddlVehiculo.Items.Clear();
				ddlVehiculo.Items.Insert(0, new ListItem("-- Primero seleccione un cliente --", ""));
			}
		}

		protected void btnAgregarServicio_Click(object sender, EventArgs e)
		{
			int claveServicio = Convert.ToInt32(ddlServicio.SelectedValue);
			if (claveServicio == 0)
			{
				MostrarMensaje("Debe seleccionar un servicio.", "warning");
				return;
			}

			int cantidad = int.Parse(txtCantidad.Text);
			if (cantidad <= 0)
			{
				MostrarMensaje("La cantidad debe ser mayor a cero.", "warning");
				return;
			}

			// Buscar el servicio seleccionado
			var servicio = _controlador.ObtenerServiciosActivos().FirstOrDefault(s => s.ClaveServicio == claveServicio);
			if (servicio == null)
			{
				MostrarMensaje("Servicio no encontrado.", "danger");
				return;
			}

			// Verificar si el servicio ya existe en la lista actual
			var existente = listaServicios.FirstOrDefault(s => s.ClaveServicio == claveServicio);
			if (existente != null)
			{
				// Si ya existe, solo incrementamos la cantidad
				existente.Cantidad += cantidad;
			}
			else
			{
				// Si no existe, agregamos un nuevo detalle
				listaServicios.Add(new DetalleServicioConNombre
				{
					ClaveServicio = servicio.ClaveServicio,
					Nombre = servicio.Nombre,
					Cantidad = cantidad,
					PrecioAplicado = servicio.CostoBase
				});
			}
			// Refrescar la vista y recalcular totales
			RefrescarGrid();
			CalcularTotales();

			// Limpiar la seleccion del servicio
			ddlServicio.SelectedIndex = 0;
			txtCantidad.Text = "1";
			CalcularTotales();
		}

		private void RefrescarGrid()
		{
			gvServicios.DataSource = listaServicios;
			gvServicios.DataBind();
		}

		protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Eliminar")
			{
				int index = Convert.ToInt32(e.CommandArgument);
				int clave = (int)gvServicios.DataKeys[index].Value;
				var item = listaServicios.FirstOrDefault(s => s.ClaveServicio == clave);
				if (item != null)
					listaServicios.Remove(item);
				RefrescarGrid();
				CalcularTotales();
			}
		}

		private void CalcularTotales()
		{
			decimal subtotal = listaServicios.Sum(s => s.Cantidad * s.PrecioAplicado);
			decimal iva = subtotal * 0.16m;
			decimal total = subtotal + iva;

			lblSubtotal.Text = subtotal.ToString("C");
			lblIVA.Text = iva.ToString("C");
			lblTotal.Text = total.ToString("C");
		}

		protected void btnGuardar_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(ddlCliente.SelectedValue))
				{
					MostrarMensaje("Debe seleccionar un cliente.", "warning");
					return;
				}
				if (string.IsNullOrEmpty(ddlVehiculo.SelectedValue))
				{
					MostrarMensaje("Debe seleccionar un vehiculo.", "warning");
					return;
				}
				if (listaServicios.Count == 0)
				{
					MostrarMensaje("Debe agregar al menos un servicio.", "warning");
					return;
				}

				DateTime fechaEstimada;
				if (!DateTime.TryParse(txtFechaEstimada.Text, out fechaEstimada))
				{
					MostrarMensaje("Fecha estimada no valida.", "warning");
					return;
				}

				int folio = _controlador.GuardarOrden(ddlVehiculo.SelectedValue, fechaEstimada, listaServicios);
				MostrarMensaje($"Orden guardada exitosamente con folio {folio}.", "success");

				// Limpiar sesion y controles
				listaServicios.Clear();
				RefrescarGrid();
				CalcularTotales();
				ddlCliente.SelectedIndex = 0;
				ddlVehiculo.Items.Clear();
				lblFolio.Text = folio.ToString();
			}
			catch (Exception ex)
			{
				MostrarMensaje("Error al guardar: " + ex.Message, "danger");
			}
		}

		protected void btnCancelar_Click(object sender, EventArgs e)
		{
			listaServicios.Clear();
			RefrescarGrid();
			CalcularTotales();
			ddlCliente.SelectedIndex = 0;
			ddlVehiculo.Items.Clear();
			txtFechaEstimada.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
			lblFolio.Text = "(Nuevo)";
			lblMensaje.Visible = false;
		}

		private void MostrarMensaje(string mensaje, string tipo)
		{
			lblMensaje.Text = mensaje;
			lblMensaje.CssClass = $"alert alert-{tipo}";
			lblMensaje.Visible = true;
		}
	}
}