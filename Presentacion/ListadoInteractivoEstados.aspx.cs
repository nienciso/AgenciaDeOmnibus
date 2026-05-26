using System;
using System.Collections.Generic;
using System.Web.UI;
using EC;
using Logica;

namespace Presentacion
{
    public partial class ListadoInteractivoEstados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarEstados();

                    gvPaquetes.DataSource = null;
                    gvPaquetes.DataBind();

                    gvHospedajes.DataSource = null;
                    gvHospedajes.DataBind();

                    lblDetalle.Text = "";
                }
                catch (Exception ex)
                {
                    lblDetalle.Text = "Error al cargar la página " ;
                }
            }
        }

        private void CargarEstados()
        {
            try
            {
                List<Estados> estados = LogicaEstados.ListarTodos();

                Session["Estados"] = estados;

                gvEstados.DataSource = estados;
                gvEstados.DataBind();
            }
            catch (Exception ex)
            {
                lblDetalle.Text = "Error al cargar los estados" ;
            }
        }

        protected void gvEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string codigoEstado = gvEstados.SelectedDataKey.Value.ToString();

                List<Estados> estados = Session["Estados"] as List<Estados>;
                if (estados == null)
                {
                    estados = LogicaEstados.ListarTodos();
                    Session["Estados"] = estados;
                }

                Estados est = estados.Find(x => x.Codigo == codigoEstado);
                if (est == null)
                {
                    lblDetalle.Text = "No se encontró el estado seleccionado.";
                    return;
                }

                List<Paquetes> paquetes = LogicaPaquete.ListarPaquetesPorEstado(est);
                Session["Paquetes"] = paquetes;

                gvPaquetes.DataSource = paquetes;
                gvPaquetes.DataBind();

                gvHospedajes.DataSource = LogicaHospedaje.ObtenerHospedajePorEstado(est);
                gvHospedajes.DataBind();

                lblDetalle.Text = "";
            }
            catch (Exception ex)
            {
                lblDetalle.Text = "Error al seleccionar el estado " ;
            }
        }

        protected void gvPaquetes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codigoPaquete = Convert.ToInt32(gvPaquetes.SelectedDataKey.Value);

                List<Paquetes> paquetes = Session["Paquetes"] as List<Paquetes>;
                if (paquetes == null)
                {
                    lblDetalle.Text = "Debe seleccionar un estado primero.";
                    return;
                }

                Paquetes p = paquetes.Find(x => x.CodigoP == codigoPaquete);
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
                    "<br/><b>Nombre Hospedaje:</b> " + p.Hospedajes.Nombre +
                    "<br/><b>Dirección:</b> " + p.Hospedajes.Direccion +
                    "<br/><b>Tipo:</b> " + p.Hospedajes.TipoH +
                    "<br/><b>Precio por noche:</b> " + p.Hospedajes.Precio;
            }
            catch (Exception ex)
            {
                lblDetalle.Text = "Error al seleccionar el paquete " ;
            }
        }
    }
}