<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentacion.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
 <title>Paquetes Disponibles</title>
    <style>
        .panel-detalle {
            border: 1px solid #aaa;
            padding: 15px;
            margin-top: 20px;
            background-color: #f5f5f5;
        }
        .titulo { font-weight: bold; font-size: 18px; margin-bottom: 10px; }
        .subtitulo { font-weight: bold; margin-top: 15px; }

        .btn-link {
        display: inline-block;
        padding: 10px 18px;
        margin-right: 10px;
        background-color: #2d6cdf;
        color: white;
        text-decoration: none;
        border-radius: 6px;
        font-weight: bold;
        font-family: Arial;
    }

        .btn-link:hover {
            background-color: #1f4fb8;
        }
    </style>
</head>
<body>


    <a href="ABMestados.aspx" class="btn-link">ABM Estados</a><br /><br />
    <a href="ABMhospedajes.aspx" class="btn-link">ABM Hospedajes</a><br /><br />
    <a href="ABMservicios.aspx" class="btn-link">ABM Servicios</a><br /><br />
    <a href="AltaPaqueteViaje.aspx" class="btn-link">Alta Paquete de Viaje</a><br /><br />
    <a href="ListadoInteractivoEstados.aspx" class="btn-link">Listado Interactivo de Estados</a><br /><br />
    <a href="ListadoInteractivoServicios.aspx" class="btn-link">Listado Interactivo de Servicios</a><br /><br />



    <form id="form1" runat="server">

        <h2>Paquetes de Viajes Disponibles</h2>

        <asp:GridView ID="gvPaquetes" runat="server"
            AutoGenerateColumns="False"
            AllowPaging="True"
            PageSize="10"
            OnPageIndexChanging="gvPaquetes_PageIndexChanging"
            OnSelectedIndexChanged="gvPaquetes_SelectedIndexChanged"
            DataKeyNames="Codigo">

            <PagerSettings Mode="NumericFirstLast" Position="Bottom" />

            <Columns>
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="FechaSalida" HeaderText="Fecha y Hora de Salida" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                <asp:BoundField DataField="DuracionDias" HeaderText="Duración (días)" />
                <asp:CommandField ShowSelectButton="True" SelectText="Ver Detalle" />
            </Columns>
        </asp:GridView>


        <asp:Panel ID="pnlDetalle" runat="server" Visible="false" CssClass="panel-detalle">

            <div class="titulo">Detalle del Paquete Seleccionado</div>

            <asp:Label ID="lblTitulo" runat="server" /><br />
            <asp:Label ID="lblDescripcion" runat="server" /><br />
            <asp:Label ID="lblDestino" runat="server" /><br />
            <asp:Label ID="lblPrecio" runat="server" /><br />

            <div class="subtitulo">Servicio</div>
            <asp:Label ID="lblServicio" runat="server" /><br />

            <div class="subtitulo">Hospedaje</div>
            <asp:Label ID="lblHospedaje" runat="server" /><br />

        </asp:Panel>

    </form>
</body>
</html>
