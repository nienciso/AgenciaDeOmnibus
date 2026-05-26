<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/publico.Master"
    CodeBehind="ABMservicios.aspx.cs"
    Inherits="Presentacion.ABMservicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>ABM de Servicios</h2>

    <asp:Label Text="Número de Ómnibus (1 a 100):" runat="server" />
    <br />
    <asp:TextBox ID="txtNroOmnibus" runat="server" Width="120" />
    <br /><br />

    <asp:Label Text="Fecha y Hora de Partida:" runat="server" />
    <br />
    <asp:TextBox ID="txtFechaPartida" runat="server" Width="200" />
    <br /><br />

    <asp:Label Text="Fecha y Hora de Llegada:" runat="server" />
    <br />
    <asp:TextBox ID="txtFechaLlegada" runat="server" Width="200" />
    <br /><br />

    <asp:Label Text="Precio del Pasaje Individual:" runat="server" />
    <br />
    <asp:TextBox ID="txtPrecio" runat="server" Width="120" />
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

    <h3>Listado de Servicios</h3>

    <asp:GridView ID="gvServicios" runat="server" AutoGenerateColumns="False"
        DataKeyNames="NroOmnibus"
        AllowPaging="True"
        PageSize="10"
        OnPageIndexChanging="gvServicios_PageIndexChanging">

        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />

        <Columns>
            <asp:BoundField DataField="NroOmnibus" HeaderText="Ómnibus" />
            <asp:BoundField DataField="FechaHoraP" HeaderText="Partida" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="FechaHoraLL" HeaderText="Llegada" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="Precio" HeaderText="Precio" />
        </Columns>
    </asp:GridView>

</asp:Content>