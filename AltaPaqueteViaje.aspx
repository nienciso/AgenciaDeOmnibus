<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/publico.Master"
    CodeBehind="AltaPaqueteViaje.aspx.cs"
    Inherits="Presentacion.AltaPaqueteViaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Alta de Paquete de Viaje</h2>

    <asp:Label Text="Título del paquete:" runat="server"></asp:Label><br />
    <asp:TextBox ID="txtTitulo" runat="server" Width="250"></asp:TextBox>
    <br /><br />

    <asp:Label Text="Descripción:" runat="server"></asp:Label><br />
    <asp:TextBox ID="txtDescripcion" runat="server" Width="300"
        TextMode="MultiLine" Rows="3"></asp:TextBox>
    <br /><br />

    <asp:Label Text="Estado / Destino:" runat="server"></asp:Label><br />
    <asp:DropDownList
        ID="ddlEstado"
        runat="server"
        Width="200"
        AutoPostBack="true"
        AppendDataBoundItems="false"
        OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
    </asp:DropDownList>
    <br /><br />

    <asp:Label Text="Servicio (Ómnibus):" runat="server"></asp:Label><br />
    <asp:DropDownList
        ID="ddlServicios"
        runat="server"
        AutoPostBack="true"
        OnSelectedIndexChanged="ddlServicios_SelectedIndexChanged">
    </asp:DropDownList>
    <br /><br />

    <asp:Label Enabled= "false" Text="Duración del paquete (días):" runat="server"></asp:Label><br />
    <asp:TextBox Enabled= "false" ID="txtDias" runat="server" Width="80" ReadOnly="true"></asp:TextBox>
    <br /><br />

    <asp:Label Text="Hospedaje:" runat="server"></asp:Label><br />
    <asp:DropDownList
        ID="ddlHospedajes"
        runat="server"
        Width="250">
    </asp:DropDownList>
    <br /><br />

    <asp:Label Text="Noches de alojamiento:" runat="server"></asp:Label><br />
    <asp:TextBox
        ID="txtNoches"
        runat="server"
        AutoPostBack="true"
        OnTextChanged="txtNoches_TextChanged">
    </asp:TextBox>
    <br /><br />

    <asp:Label Enabled= "false" Text="Precio Individual:" runat="server"></asp:Label><br />
    <asp:TextBox Enabled= "false" ID="txtPrecioIndividual" runat="server" Width="100" ReadOnly="true"></asp:TextBox>
    <br /><br />

    <asp:Label Enabled= "false" Text="Precio Base Doble (2 personas):" runat="server"></asp:Label><br />
    <asp:TextBox Enabled= "false" ID="txtPrecioDoble" runat="server" Width="100" ReadOnly="true"></asp:TextBox>
    <br /><br />

    <asp:Label Enabled= "false" Text="Precio Base Triple (3 personas):" runat="server"></asp:Label><br />
    <asp:TextBox Enabled= "false" ID="txtPrecioTriple" runat="server" Width="100" ReadOnly="true"></asp:TextBox>
    <br /><br />

    <asp:Button ID="btnCrear" Text="Crear Paquete" runat="server" OnClick="btnCrear_Click" />
    &nbsp;
<asp:Button ID="btnVaciar" runat="server" Text="Vaciar" OnClick="btnVaciar_Click" />
    <br /><br />

    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>