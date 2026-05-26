using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC;
using Logica;

namespace Presentacion
{
    public partial class ABMservicios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarServicios();
        }

        private void LimpiarFormulario()
        {
            txtNroOmnibus.Text = "";
            txtFechaPartida.Text = "";
            txtFechaLlegada.Text = "";
            txtPrecio.Text = "";

            txtNroOmnibus.ReadOnly = false;
            txtFechaPartida.ReadOnly = false;

            Session["NRO_ORIGINAL"] = null;
            Session["FECHA_ORIGINAL"] = null;
        }

        private void CargarServicios()
        {
            try
            {
                List<Servicios> lista = LogicaServicios.ListarServiciosVigentes();

                Session["SES_SERVICIOS_ABM"] = lista;

                gvServicios.DataSource = lista;
                gvServicios.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvServicios.PageIndex = e.NewPageIndex;

                List<Servicios> lista = Session["SES_SERVICIOS_ABM"] as List<Servicios>;
                gvServicios.DataSource = lista;
                gvServicios.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void btnAlta_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNroOmnibus.Text.Trim(), out int nro))
                {
                    lblMensaje.Text = "El número de ómnibus debe ser numérico.";
                    return;
                }

                if (!DateTime.TryParse(txtFechaPartida.Text.Trim(), out DateTime fechaP))
                {
                    lblMensaje.Text = "Fecha/hora de partida inválida.";
                    return;
                }

                if (!DateTime.TryParse(txtFechaLlegada.Text.Trim(), out DateTime fechaL))
                {
                    lblMensaje.Text = "Fecha/hora de llegada inválida.";
                    return;
                }

                if (!int.TryParse(txtPrecio.Text.Trim(), out int precio))
                {
                    lblMensaje.Text = "El precio debe ser numérico.";
                    return;
                }

                Servicios s = new Servicios(nro, fechaP, fechaL, precio);

                LogicaServicios.Agregar(s);

                lblMensaje.Text = "Servicio agregado correctamente.";
                LimpiarFormulario();
                CargarServicios();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void btnBaja_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNroOmnibus.Text.Trim(), out int nro))
                {
                    lblMensaje.Text = "El número de ómnibus debe ser numérico.";
                    return;
                }

                if (!DateTime.TryParse(txtFechaPartida.Text.Trim(), out DateTime fechaP))
                {
                    lblMensaje.Text = "Fecha/hora de partida inválida.";
                    return;
                }

                LogicaServicios.Eliminar(nro, fechaP);

                lblMensaje.Text = "Servicio eliminado correctamente.";
                LimpiarFormulario();
                CargarServicios();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["NRO_ORIGINAL"] == null || Session["FECHA_ORIGINAL"] == null)
                {
                    lblMensaje.Text = "Debe seleccionar un servicio antes de modificar.";
                    return;
                }

                int nro = (int)Session["NRO_ORIGINAL"];
                DateTime fechaP = (DateTime)Session["FECHA_ORIGINAL"];

                if (!DateTime.TryParse(txtFechaLlegada.Text.Trim(), out DateTime fechaL))
                {
                    lblMensaje.Text = "Fecha/hora de llegada inválida.";
                    return;
                }

                if (!int.TryParse(txtPrecio.Text.Trim(), out int precio))
                {
                    lblMensaje.Text = "El precio debe ser numérico.";
                    return;
                }

                Servicios s = new Servicios(nro, fechaP, fechaL, precio);

                LogicaServicios.Modificar(s);

                lblMensaje.Text = "Servicio modificado correctamente.";
                LimpiarFormulario();
                CargarServicios();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNroOmnibus.Text.Trim(), out int nro))
                {
                    lblMensaje.Text = "El número de ómnibus debe ser numérico.";
                    return;
                }

                if (!DateTime.TryParse(txtFechaPartida.Text.Trim(), out DateTime fechaP))
                {
                    lblMensaje.Text = "Fecha/hora de partida inválida.";
                    return;
                }

                List<Servicios> lista = Session["SES_SERVICIOS_ABM"] as List<Servicios>;
                Servicios encontrado = null;

                if (lista != null)
                {
                    foreach (Servicios s in lista)
                    {
                        if (s.NroOmnibus == nro && s.FechaHoraP == fechaP)
                        {
                            encontrado = s;
                            break;
                        }
                    }
                }

                if (encontrado != null)
                {
                    txtNroOmnibus.Text = encontrado.NroOmnibus.ToString();
                    txtFechaPartida.Text = encontrado.FechaHoraP.ToString("yyyy-MM-dd HH:mm");
                    txtFechaLlegada.Text = encontrado.FechaHoraLL.ToString("yyyy-MM-dd HH:mm");
                    txtPrecio.Text = encontrado.Precio.ToString();

                    Session["NRO_ORIGINAL"] = encontrado.NroOmnibus;
                    Session["FECHA_ORIGINAL"] = encontrado.FechaHoraP;

                    txtNroOmnibus.ReadOnly = true;
                    txtFechaPartida.ReadOnly = true;

                    lblMensaje.Text = "Registro encontrado.";
                }
                else
                {
                    lblMensaje.Text = "No se encontró el registro.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void btnVaciar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}