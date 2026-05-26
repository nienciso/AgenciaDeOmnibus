using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logica;
using EC;

namespace Presentacion
{
    public partial class Default : System.Web.UI.Page
    {
        private const string ListaPaquetesSession = "ListaPaquetes";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPaquetes();
                pnlDetalle.Visible = false;

            }
        }

        private void CargarPaquetes()
        {
            try
            {
                List<Paquetes> paquetes = LogicaPaquete.ListarPaquetes();

                Session[ListaPaquetesSession] = paquetes;

                var listaParaGrid = paquetes.Select(p => new
                {
                    Codigo = p.CodigoP,
                    Titulo = p.Titulo,
                    FechaSalida = p.Servicios != null ? p.Servicios.FechaHoraP : (DateTime?)null,
                    DuracionDias = p.CantDias
                }).ToList();

                gvPaquetes.PageIndex = 0;
                gvPaquetes.DataSource = listaParaGrid;
                gvPaquetes.DataBind();
            }
            catch (Exception)
            {
            }
        }

        protected void gvPaquetes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPaquetes.PageIndex = e.NewPageIndex;

                var paquetes = Session[ListaPaquetesSession] as List<Paquetes>;
                if (paquetes == null) { CargarPaquetes(); return; }

                var listaParaGrid = paquetes.Select(p => new
                {
                    Codigo = p.CodigoP,
                    Titulo = p.Titulo,
                    FechaSalida = p.Servicios != null ? p.Servicios.FechaHoraP : (DateTime?)null,
                    DuracionDias = p.CantDias
                }).ToList();

                gvPaquetes.DataSource = listaParaGrid;
                gvPaquetes.DataBind();

                pnlDetalle.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        protected void gvPaquetes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codigo = Convert.ToInt32(gvPaquetes.SelectedDataKey.Value);

                var paquetes = Session[ListaPaquetesSession] as List<Paquetes>;
                Paquetes p = paquetes?.FirstOrDefault(x => x.CodigoP == codigo);

                if (p == null) return;

                pnlDetalle.Visible = true;

                lblTitulo.Text = "Título: " + p.Titulo;
                lblDescripcion.Text = "Descripción: " + p.Descripcion;
                lblDestino.Text = "Destino: " + p.Estados?.Nombre;
                lblPrecio.Text = "Precio Individual: $" + p.PrecioI;

                if (p.Servicios != null)
                {
                    lblServicio.Text =
                        $"Nro Ómnibus: {p.Servicios.NroOmnibus}<br/>" +
                        $"Salida: {p.Servicios.FechaHoraP:dd/MM/yyyy HH:mm}<br/>" +
                        $"Llegada: {p.Servicios.FechaHoraLL:dd/MM/yyyy HH:mm}<br/>" +
                        $"Precio Servicio: ${p.Servicios.Precio}";
                }
                else lblServicio.Text = "Sin servicio.";

                if (p.Hospedajes != null)
                {
                    lblHospedaje.Text =
                        $"Código: {p.Hospedajes.CodigoInterno}<br/>" +
                        $"Nombre: {p.Hospedajes.Nombre}<br/>" +
                        $"Dirección: {p.Hospedajes.Direccion}<br/>" +
                        $"Tipo: {p.Hospedajes.TipoH}<br/>" +
                        $"Precio por Noche: ${p.Hospedajes.Precio}";
                }
                else lblHospedaje.Text = "Sin hospedaje.";
            }
            catch (Exception)
            {
            }
        }
    }
}