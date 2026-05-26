<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/publico.Master"
    CodeBehind="ListadoInteractivoServicios.aspx.cs"
    Inherits="Presentacion.ListadoInteractivoServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Servicios vigentes</h2>

    <asp:GridView ID="gvServicios" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="NroOmnibus,FechaHoraP"
        OnSelectedIndexChanged="gvServicios_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" SelectText="Ver paquetes" />
            <asp:BoundField DataField="NroOmnibus" HeaderText="Ómnibus" />
            <asp:BoundField DataField="FechaHoraP" HeaderText="Partida"
                DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField DataField="FechaHoraLL" HeaderText="Llegada"
                DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField DataField="Precio" HeaderText="Precio" />
        </Columns>
    </asp:GridView>

    <br />
    <hr />
    <br />

    <h2>Paquetes del servicio seleccionado</h2>

    <asp:GridView ID="gvPaquetes" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="CodigoP"
        OnSelectedIndexChanged="gvPaquetes_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" SelectText="Ver detalle" />
            <asp:BoundField DataField="CodigoP" HeaderText="Código" />
            <asp:BoundField DataField="Titulo" HeaderText="Título" />
            <asp:BoundField DataField="CantDias" HeaderText="Días" />
            <asp:BoundField DataField="NochesHospedaje" HeaderText="Noches" />
            <asp:BoundField DataField="PrecioI" HeaderText="Precio Individual" />
        </Columns>
    </asp:GridView>

    <br />
    <hr />
    <br />

    <h2>Detalle del paquete (incluye hospedaje)</h2>

    <asp:Label ID="lblDetalle" runat="server" />

</asp:Content>