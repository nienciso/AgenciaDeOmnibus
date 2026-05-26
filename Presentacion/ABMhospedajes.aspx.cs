using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC;
using Logica;

namespace Presentacion
{
    public partial class ABMhospedaje : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstados();
                CargarHospedajes();
            }
        }

        private void CargarEstados()
        {
            try
            {
                List<Estados> lista = LogicaEstados.ListarTodos();
                Session["SES_ESTADOS_ABM"] = lista;

                ddlEstado.Items.Clear();
                ddlEstado.DataSource = lista;
                ddlEstado.DataTextField = "Nombre";
                ddlEstado.DataValueField = "Codigo";
                ddlEstado.DataBind();
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
                List<Estados> estados = Session["SES_ESTADOS_ABM"] as List<Estados>;
                List<Hospedajes> lista = new List<Hospedajes>();

                if (estados != null)
                {
                    foreach (Estados est in estados)
                    {
                        List<Hospedajes> hosps = LogicaHospedaje.ObtenerHospedajePorEstado(est);
                        lista.AddRange(hosps);
                    }
                }

                Session["SES_HOSPEDAJES_ABM"] = lista;

                gvHospedajes.DataSource = lista;
                gvHospedajes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void gvHospedajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHospedajes.PageIndex = e.NewPageIndex;

                List<Hospedajes> lista = Session["SES_HOSPEDAJES_ABM"] as List<Hospedajes>;
                gvHospedajes.DataSource = lista;
                gvHospedajes.DataBind();
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
                Estados est = ObtenerEstadoSeleccionado();
                if (est == null)
                {
                    lblMensaje.Text = "Debe seleccionar un estado válido.";
                    return;
                }

                if (!int.TryParse(txtPrecio.Text.Trim(), out int precio))
                {
                    lblMensaje.Text = "El precio debe ser numérico.";
                    return;
                }

                Hospedajes h = new Hospedajes(
                    txtCodigo.Text.Trim(),
                    ddlTipo.SelectedValue,
                    txtNombre.Text.Trim(),
                    txtDireccion.Text.Trim(),
                    precio,
                    est
                );

                LogicaHospedaje.Agregar(h);

                lblMensaje.Text = "Hospedaje agregado correctamente.";

                CargarHospedajes();
                LimpiarFormulario();
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
                LogicaHospedaje.Eliminar(txtCodigo.Text.Trim());

                lblMensaje.Text = "Hospedaje eliminado correctamente.";

                CargarHospedajes();
                LimpiarFormulario();
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
                Estados est = ObtenerEstadoSeleccionado();
                if (est == null)
                {
                    lblMensaje.Text = "Debe seleccionar un estado válido.";
                    return;
                }

                if (!int.TryParse(txtPrecio.Text.Trim(), out int precio))
                {
                    lblMensaje.Text = "El precio debe ser numérico.";
                    return;
                }

                Hospedajes h = new Hospedajes(
                    txtCodigo.Text.Trim(),
                    ddlTipo.SelectedValue,
                    txtNombre.Text.Trim(),
                    txtDireccion.Text.Trim(),
                    precio,
                    est
                );

                LogicaHospedaje.Modificar(h);

                lblMensaje.Text = "Hospedaje modificado correctamente.";

                CargarHospedajes();
                LimpiarFormulario();
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
                string codigo = txtCodigo.Text.Trim();

                List<Hospedajes> lista = Session["SES_HOSPEDAJES_ABM"] as List<Hospedajes>;
                Hospedajes encontrado = null;

                if (lista != null)
                {
                    foreach (Hospedajes h in lista)
                    {
                        if (h.CodigoInterno == codigo)
                        {
                            encontrado = h;
                            break;
                        }
                    }
                }

                if (encontrado != null)
                {
                    txtNombre.Text = encontrado.Nombre;
                    txtDireccion.Text = encontrado.Direccion;
                    ddlTipo.SelectedValue = encontrado.TipoH;
                    txtPrecio.Text = encontrado.Precio.ToString();
                    ddlEstado.SelectedValue = encontrado.Estados.Codigo;

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

        private Estados ObtenerEstadoSeleccionado()
        {
            List<Estados> estados = Session["SES_ESTADOS_ABM"] as List<Estados>;

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

        private void LimpiarFormulario()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";
            txtPrecio.Text = "";

            ddlTipo.SelectedIndex = 0;
            if (ddlEstado.Items.Count > 0)
                ddlEstado.SelectedIndex = 0;
        }
    }
}