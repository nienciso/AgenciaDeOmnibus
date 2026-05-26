using System;
using System.Collections.Generic;
using System.Web.UI;
using Logica;
using EC;

namespace Presentacion
{
    public partial class ListadoInteractivoServicios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarServiciosVigentes();

                    gvPaquetes.DataSource = null;
                    gvPaquetes.DataBind();

                    lblDetalle.Text = "";
                }
                catch (Exception ex)
                {
                    lblDetalle.Text = "Error al cargar la página " ;
                }
            }
        }

        private void CargarServiciosVigentes()
        {
            try
            {
                List<Servicios> servicios = LogicaServicios.ListarServiciosVigentes();

                Session["Servicios"] = servicios;

                gvServicios.DataSource = servicios;
                gvServicios.DataBind();
            }
            catch (Exception ex)
            {
                lblDetalle.Text = "Error al cargar los servicios vigentes " ;
            }
        }

        protected void gvServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int nro = Convert.ToInt32(gvServicios.SelectedDataKey.Values["NroOmnibus"]);
                DateTime fechaPartida = Convert.ToDateTime(gvServicios.SelectedDataKey.Values["FechaHoraP"]);

                List<Servicios> servicios = Session["Servicios"] as List<Servicios>;
                if (servicios == null)
                {
                    servicios = LogicaServicios.ListarServiciosVigentes();
                    Session["Servicios"] = servicios;
                }

                Servicios servicio = servicios.Find(s =>
                    s.NroOmnibus == nro && s.FechaHoraP == fechaPartida);

                if (servicio == null)
                {
                    lblDetalle.Text = "No se encontró el servicio seleccionado.";
                    return;
                }

                List<Paquetes> paquetes = LogicaPaquete.ListarPaquetesPorServicio(servicio);

                Session["Paquetes"] = paquetes;

                gvPaquetes.DataSource = paquetes;
                gvPaquetes.DataBind();

                lblDetalle.Text = "";
            }
            catch (Exception ex)
            {
                lblDetalle.Text = "Error al seleccionar el servicio ";
            }
        }

        protected void gvPaquetes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codigo = Convert.ToInt32(gvPaquetes.SelectedDataKey.Value);

                List<Paquetes> paquetes = Session["Paquetes"] as List<Paquetes>;
                if (paquetes == null)
                {
                    lblDetalle.Text = "Debe seleccionar un servicio primero.";
                    return;
                }

                Paquetes p = paquetes.Find(x => x.CodigoP == codigo);
                if (p == null)
                {
                    lblDetalle.Text = "No se encontró el paquete seleccionado.";
                    return;
                }

                lblDetalle.Text =
                    "<b>Código:</b> " + p.CodigoP +
                    "<br/><b>Título:</b> " + p.Titulo +
                    "<br/><b>Descripción:</b> " + p.Descripcion +
                    "<br/><b>Destino:</b> " + p.Estados.Codigo +
                    "<br/><b>Días:</b> " + p.CantDias +
                    "<br/><b>Noches:</b> " + p.NochesHospedaje +
                    "<br/><b>Precio Individual:</b> " + p.PrecioI +
                    "<br/><b>Precio Doble:</b> " + p.PrecioD +
                    "<br/><b>Precio Triple:</b> " + p.PrecioT +
                    "<hr/>" +
                    "<b>Servicio:</b> " + p.Servicios.NroOmnibus +
                    "<br/><b>Partida:</b> " + p.Servicios.FechaHoraP.ToString("yyyy-MM-dd HH:mm") +
                    "<br/><b>Llegada:</b> " + p.Servicios.FechaHoraLL.ToString("yyyy-MM-dd HH:mm") +
                    "<br/><b>Precio Servicio:</b> " + p.Servicios.Precio +
                    "<hr/>" +
                    "<b>Hospedaje:</b> " + p.Hospedajes.CodigoInterno +
                    "<br/><b>Nombre:</b> " + p.Hospedajes.Nombre +
                    "<br/><b>Dirección:</b> " + p.Hospedajes.Direccion +
                    "<br/><b>Tipo:</b> " + p.Hospedajes.TipoH +
                    "<br/><b>Precio por noche:</b> " + p.Hospedajes.Precio +
                    "<br/><b>Estado del hospedaje:</b> " + p.Hospedajes.Estados.Codigo;
            }
            catch (Exception ex)
            {
                lblDetalle.Text = "Error al seleccionar el paquete " ;
            }
        }
    }
}