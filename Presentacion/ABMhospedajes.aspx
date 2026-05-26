<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/publico.Master"
    CodeBehind="ABMhospedajes.aspx.cs"
    Inherits="Presentacion.ABMhospedaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>ABM de Hospedajes</h2>

    <asp:Label Text="Código Interno (máx. 10 letras):" runat="server" />
    <br />
    <asp:TextBox ID="txtCodigo" runat="server" Width="150" MaxLength="10" />
    <br /><br />

    <asp:Label Text="Nombre del Hospedaje:" runat="server" />
    <br />
    <asp:TextBox ID="txtNombre" runat="server" Width="250" />
    <br /><br />

    <asp:Label Text="Dirección:" runat="server" />
    <br />
    <asp:TextBox ID="txtDireccion" runat="server" Width="300" />
    <br /><br />

    <asp:Label Text="Tipo de Hospedaje:" runat="server" />
    <br />
    <asp:DropDownList ID="ddlTipo" runat="server">
        <asp:ListItem Text="Hotel STD" Value="Hotel STD" />
        <asp:ListItem Text="Posada" Value="Posada" />
        <asp:ListItem Text="All Inclusive" Value="All Inclusive" />
    </asp:DropDownList>
    <br /><br />

    <asp:Label Text="Precio por Noche (por persona):" runat="server" />
    <br />
    <asp:TextBox ID="txtPrecio" runat="server" Width="120" />
    <br /><br />

    <asp:Label Text="Estado donde se ubica:" runat="server" />
    <br />
    <asp:DropDownList ID="ddlEstado" runat="server" Width="150"></asp:DropDownList>
    <br /><br />

    <asp:Button ID="btnAlta" Text="Alta" runat="server" OnClick="btnAlta_Click" />
    &nbsp;&nbsp;
    <asp:Button ID="btnBaja" Text="Baja" runat="server" OnClick="btnBaja_Click" />
    &nbsp;&nbsp;
    <asp:Button ID="btnModificar" Text="Modificar" runat="server" OnClick="btnModificar_Click" />
    &nbsp;&nbsp;
    <asp:Button ID="btnBuscar" Text="Buscar" runat="server" OnClick="btnBuscar_Click" />
    &nbsp;
<asp:Button ID="btnVaciar" runat="server" Text="Vaciar" OnClick="btnVaciar_Click" />

    <br /><br />

    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" />

    <br /><br />

    <h3>Listado de Hospedajes</h3>

    <asp:GridView ID="gvHospedajes" runat="server" AutoGenerateColumns="False"
        AllowPaging="True"
        PageSize="10"
        OnPageIndexChanging="gvHospedajes_PageIndexChanging">

        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />

        <Columns>
            <asp:BoundField DataField="CodigoInterno" HeaderText="Código" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
            <asp:BoundField DataField="TipoH" HeaderText="Tipo" />
            <asp:BoundField DataField="Precio" HeaderText="Precio" />
            <asp:BoundField DataField="EstadoNombre" HeaderText="Estado" />
        </Columns>
    </asp:GridView>

</asp:Content>