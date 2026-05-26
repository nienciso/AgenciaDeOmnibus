<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/publico.Master"
    CodeBehind="ListadoInteractivoEstados.aspx.cs"
    Inherits="Presentacion.ListadoInteractivoEstados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Listado Interactivo de Estados</h2>

    <h3>Estados</h3>
    <asp:GridView ID="gvEstados" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="Codigo"
        OnSelectedIndexChanged="gvEstados_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" SelectText="Ver" />
            <asp:BoundField DataField="Codigo" HeaderText="Código" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Pais" HeaderText="País" />
        </Columns>
    </asp:GridView>

    <br /><hr /><br />

    <h3>Paquetes del estado seleccionado</h3>
    <asp:GridView ID="gvPaquetes" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="CodigoP"
        OnSelectedIndexChanged="gvPaquetes_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" SelectText="Detalle" />
            <asp:BoundField DataField="CodigoP" HeaderText="Código" />
            <asp:BoundField DataField="Titulo" HeaderText="Título" />
            <asp:BoundField DataField="CantDias" HeaderText="Días" />
            <asp:BoundField DataField="NochesHospedaje" HeaderText="Noches" />
            <asp:BoundField DataField="PrecioI" HeaderText="Precio Individual" />
        </Columns>
    </asp:GridView>

    <br /><hr /><br />

    <h3>Hospedajes del estado seleccionado</h3>
    <asp:GridView ID="gvHospedajes" runat="server"
        AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="CodigoInterno" HeaderText="Código" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
            <asp:BoundField DataField="TipoH" HeaderText="Tipo" />
            <asp:BoundField DataField="Precio" HeaderText="Precio" />
        </Columns>
    </asp:GridView>

    <br /><hr /><br />

    <h3>Detalle del paquete (incluye datos del servicio)</h3>
    <asp:Label ID="lblDetalle" runat="server" />

</asp:Content>