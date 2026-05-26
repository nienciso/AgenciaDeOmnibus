using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using EC;
using Logica;

namespace Presentacion
{
    public partial class ABMestados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarEstados();
        }

        private void CargarEstados()
        {
            try
            {
                List<Estados> lista = LogicaEstados.ListarTodos();

                Session["SES_ESTADOS_ABM"] = lista;

                gvEstados.DataSource = lista;
                gvEstados.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        protected void gvEstados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvEstados.PageIndex = e.NewPageIndex;

                List<Estados> lista = Session["SES_ESTADOS_ABM"] as List<Estados>;

                gvEstados.DataSource = lista;
                gvEstados.DataBind();
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
                Estados est = new Estados(
                    txtCodigo.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtPais.Text.Trim()
                );

                LogicaEstados.Agregar(est);

                lblMensaje.Text = "Estado agregado correctamente.";

                CargarEstados();
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
                string codigo = txtCodigo.Text.Trim();

                LogicaEstados.Eliminar(codigo);

                lblMensaje.Text = "Estado eliminado correctamente.";

                CargarEstados();
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
                Estados est = new Estados(
                    txtCodigo.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtPais.Text.Trim()
                );

                LogicaEstados.Modificar(est);

                lblMensaje.Text = "Estado modificado correctamente.";

                CargarEstados();
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

                List<Estados> lista = Session["SES_ESTADOS_ABM"] as List<Estados>;
                Estados encontrado = null;

                if (lista != null)
                {
                    foreach (Estados estados in lista)
                    {
                        if (estados.Codigo == codigo)
                        {
                            encontrado = estados;
                            break;
                        }
                    }
                }

                if (encontrado != null)
                {
                    txtNombre.Text = encontrado.Nombre;
                    txtPais.Text = encontrado.Pais;
                    lblMensaje.Text = "Estado encontrado.";
                }
                else
                {
                    lblMensaje.Text = "No existe un estado con ese código.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtPais.Text = "";
        }

        protected void btnVaciar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtPais_TextChanged(object sender, EventArgs e)
        {

        }
    }
}