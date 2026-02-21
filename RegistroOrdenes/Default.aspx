<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RegistroOrdenes._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Registro de Órdenes de Servicio</h1>
            <p class="lead">Servicio Automotriz General</p>
        </section>

        <div class="row">
            <section class="content">
                <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-info" Visible="false"></asp:Label>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Folio:</label>
                            <asp:Label ID="lblFolio" runat="server" Text="(Nuevo)" CssClass="form-control-static" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>Fecha:</label>
                            <asp:Label ID="lblFecha" runat="server" CssClass="form-control-static"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Fecha estimada de entrega:</label>
                            <asp:TextBox ID="txtFechaEstimada" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Cliente:</label>
                            <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label>Vehículo:</label>
                            <asp:DropDownList ID="ddlVehiculo" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <hr />
                <h4>Servicios a realizar</h4>

                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label>Servicio:</label>
                            <asp:DropDownList ID="ddlServicio" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <label>Cantidad:</label>
                            <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" Text="1" TextMode="Number" min="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="form-group">
                            <label>&nbsp;</label>
                            <asp:Button ID="btnAgregarServicio" runat="server" Text="Agregar Servicio" CssClass="btn btn-primary form-control" OnClick="btnAgregarServicio_Click" />
                        </div>
                    </div>
                </div>

                <asp:GridView ID="gvServicios" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                    ShowFooter="True" OnRowCommand="gvServicios_RowCommand" DataKeyNames="ClaveServicio">
                    <Columns>
                        <asp:BoundField DataField="ClaveServicio" HeaderText="ID Servicio" />
                        <asp:BoundField DataField="Nombre" HeaderText="Descripción" />
                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                        <asp:BoundField DataField="PrecioAplicado" HeaderText="Precio" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:C}" />
                        <asp:ButtonField Text="Eliminar" CommandName="Eliminar" ButtonType="Button" ControlStyle-CssClass="btn btn-danger btn-sm" />
                    </Columns>
                </asp:GridView>

                <div class="row">
                    <div class="col-md-4 col-md-offset-8">
                        <table class="table table-bordered">
                            <tr>
                                <td><strong>Subtotal:</strong></td>
                                <td><asp:Label ID="lblSubtotal" runat="server" Text="$0.00"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><strong>IVA (16%):</strong></td>
                                <td><asp:Label ID="lblIVA" runat="server" Text="$0.00"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><strong>Total:</strong></td>
                                <td><asp:Label ID="lblTotal" runat="server" Text="$0.00"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Orden" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-default" OnClick="btnCancelar_Click" CausesValidation="false" />
                </div>
            </section>
        </div>
    </main>

</asp:Content>