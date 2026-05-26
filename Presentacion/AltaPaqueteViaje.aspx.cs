using System;
using System.Collections.Generic;
using System.Web.UI;
using EC;
using Logica;

namespace Presentacion
{
    public partial class AltaPaqueteViaje : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstados();
                CargarServicios();
            }
        }

        private void CargarEstados()
        {
            try
            {
                List<Estados> lista = LogicaEstados.ListarTodos();
                Session["SES_ESTADOS"] = lista;

                ddlEstado.Items.Clear();
                ddlEstado.DataSource = lista;
                ddlEstado.DataTextField = "Nombre";
                ddlEstado.DataValueField = "Codigo";
                ddlEstado.DataBind();

                if (ddlEstado.Items.Count > 0)
                {
                    ddlEstado.SelectedIndex = 0;
                    CargarHospedajes();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        private void CargarServicios()
        {
            try
            {
                List<Servicios> lista = LogicaServicios.ListarServiciosVigentes();
                Session["SES_SERVICIOS"] = lista;

                ddlServicios.Items.Clear();
                ddlServicios.DataSource = lista;
                ddlServicios.DataTextField = "TextoServicio";
                ddlServicios.DataValueField = "ClaveServicio";
                ddlServicios.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        private void CargarHospedajes()
        {
            try
            {
                List<Estados> estados = Session["SES_ESTADOS"] as List<Estados>;
                Estados estSeleccionado = null;

                if (estados != null)
                {
                    foreach (Estados e in estados)
                    {
                        if (e.Codigo == ddlEstado.SelectedValue)
                        {
                            estSeleccionado = e;
                            break;
                        }
                    }
                }

                if (estSeleccionado == null) return;

                List<Hospedajes> lista = LogicaHospedaje.ObtenerHospedajePorEstado(estSeleccionado);
                Session["SES_HOSPEDAJES"] = lista;

                ddlHospedajes.Items.Clear();
                ddlHospedajes.DataSource = lista;
                ddlHospedajes.DataTextField = "Nombre";
                ddlHospedajes.DataValueField = "CodigoInterno";
                ddlHospedajes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarHospedajes();
            CalcularPrecios();
        }

        protected void ddlServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularDias();
            CalcularPrecios();
        }

        protected void ddlHospedajes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularPrecios();
        }

        protected void txtNoches_TextChanged(object sender, EventArgs e)
        {
            CalcularPrecios();
        }

        private Servicios ObtenerServicioSeleccionado()
        {
            if (ddlServicios.SelectedValue == "")
                return null;

            string[] partes = ddlServicios.SelectedValue.Split('|');

            int nro = Convert.ToInt32(partes[0]);
            DateTime fecha = Convert.ToDateTime(partes[1]);

            List<Servicios> servicios = Session["SES_SERVICIOS"] as List<Servicios>;

            if (servicios != null)
            {
                foreach (Servicios s in servicios)
                {
                    if (s.NroOmnibus == nro && s.FechaHoraP == fecha)
                        return s;
                }
            }

            return null;
        }

        private Hospedajes ObtenerHospedajeSeleccionado()
        {
            if (ddlHospedajes.SelectedValue == "")
                return null;

            List<Hospedajes> hospedajes = Session["SES_HOSPEDAJES"] as List<Hospedajes>;

            if (hospedajes != null)
            {
                foreach (Hospedajes h in hospedajes)
                {
                    if (h.CodigoInterno == ddlHospedajes.SelectedValue)
                        return h;
                }
            }

            return null;
        }

        private Estados ObtenerEstadoSeleccionado()
        {
            List<Estados> estados = Session["SES_ESTADOS"] as List<Estados>;

            if (estados != null)
            {
                foreach (Estados e in estados)
                {
                    if (e.Codigo == ddlEstado.SelectedValue)
                        return e;
                }
            }

            return null;
        }

        private void CalcularDias()
        {
            try
            {
                Servicios s = ObtenerServicioSeleccionado();
                if (s == null) return;

                int dias = (int)Math.Ceiling((s.FechaHoraLL - s.FechaHoraP).TotalDays);
                if (dias <= 0) dias = 1;

                txtDias.Text = dias.ToString();
            }
            catch
            {
                txtDias.Text = "";
            }
        }

        private void CalcularPrecios()
        {
            try
            {
                if (txtNoches.Text == "") return;

                int noches = Convert.ToInt32(txtNoches.Text);

                Servicios serv = ObtenerServicioSeleccionado();
                Hospedajes hosp = ObtenerHospedajeSeleccionado();

                if (serv == null || hosp == null) return;

                int precioBase = serv.Precio + (hosp.Precio * noches);

                txtPrecioIndividual.Text = (precioBase * 1.35m).ToString("0");
                txtPrecioDoble.Text = ((precioBase * 2) * 1.10m).ToString("0");
                txtPrecioTriple.Text = ((precioBase * 3) * 1.10m).ToString("0");
            }
            catch
            {
                txtPrecioIndividual.Text = "";
                txtPrecioDoble.Text = "";
                txtPrecioTriple.Text = "";
            }
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                int noches = Convert.ToInt32(txtNoches.Text);
                int dias = Convert.ToInt32(txtDias.Text);

                if (noches >= dias)
                {
                    lblMensaje.Text = "Datos no válidos.";
                    return;
                }

                Servicios serv = ObtenerServicioSeleccionado();
                Estados est = ObtenerEstadoSeleccionado();
                Hospedajes hosp = ObtenerHospedajeSeleccionado();

                if (serv == null || est == null || hosp == null)
                {
                    lblMensaje.Text = "Debe seleccionar datos válidos.";
                    return;
                }

                int precioBase = serv.Precio + (hosp.Precio * noches);

                int precioInd = (int)Math.Round(precioBase * 1.35m);
                int precioDoble = (int)Math.Round((precioBase * 2) * 1.10m);
                int precioTriple = (int)Math.Round((precioBase * 3) * 1.10m);

                Paquetes p = new Paquetes(
                    txtTitulo.Text.Trim(),
                    txtDescripcion.Text.Trim(),
                    est,
                    dias,
                    precioInd,
                    precioDoble,
                    precioTriple,
                    serv,
                    hosp,
                    noches
                );

                LogicaPaquete.AltaPaquete(p);

                LimpiarFormulario();
                lblMensaje.Text = "Paquete creado correctamente.";
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

        private void LimpiarFormulario()
        {
            txtTitulo.Text = "";
            txtDescripcion.Text = "";
            txtDias.Text = "";
            txtNoches.Text = "";
            txtPrecioIndividual.Text = "";
            txtPrecioDoble.Text = "";
            txtPrecioTriple.Text = "";
        }
    }
}