<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/publico.Master"
    CodeBehind="ABMestados.aspx.cs"
    Inherits="Presentacion.ABMestados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>ABM de Estados</h2>

    <asp:Label Text="Código (4 letras):" runat="server" />
    <br />
    <asp:TextBox ID="txtCodigo" runat="server" MaxLength="4" Width="120"
        OnTextChanged="txtCodigo_TextChanged" />
    <br /><br />

    <asp:Label Text="Nombre:" runat="server" />
    <br />
    <asp:TextBox ID="txtNombre" runat="server" Width="200"
        OnTextChanged="txtNombre_TextChanged" style="height: 22px" />
    <br /><br />

    <asp:Label Text="País:" runat="server" />
    <br />
    <asp:TextBox ID="txtPais" runat="server" Width="200"
        OnTextChanged="txtPais_TextChanged" />
    <br /><br />

    <asp:Button ID="btnAlta" runat="server" Text="Alta" OnClick="btnAlta_Click" />
    &nbsp;
    <asp:Button ID="btnBaja" runat="server" Text="Baja" OnClick="btnBaja_Click" />
    &nbsp;
    <asp:Button ID="btnModificar" runat="server" Text="Modificar" OnClick="btnModificar_Click" />
    &nbsp;
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
    &nbsp;
    <asp:Button ID="btnVaciar" runat="server" Text="Vaciar" OnClick="btnVaciar_Click" Height="26px" />

    <br /><br />

    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>

    <br /><br />

    <h3>Listado de Estados</h3>

    <asp:GridView ID="gvEstados" runat="server"
        AutoGenerateColumns="True"
        AllowPaging="True"
        PageSize="10"
        OnPageIndexChanging="gvEstados_PageIndexChanging">
        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
    </asp:GridView>

</asp:Content>
