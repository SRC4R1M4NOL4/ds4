<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Laboratorio154._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row text-center" aria-labelledby="aspnetTitle">
            <h2 id="aspnetTitle">Suma de dos números</h2>
            <div style="margin:auto; width: 300px; font-family: Arial;">

                <asp:Label ID="Label1" runat="server" Text="Número 1:" AssociatedControlID="txtNumero1"></asp:Label><br />
                <asp:TextBox ID="txtNumero1" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <asp:Label ID="Label2" runat="server" Text="Número 2:" AssociatedControlID="txtNumero2"></asp:Label><br />
                <asp:TextBox ID="txtNumero2" runat="server" CssClass="form-control"></asp:TextBox>
                <br />

                <asp:Button ID="btnSumar" runat="server" Text="Sumar" CssClass="btn btn-primary" OnClick="btnSumar_Click" />
                <br /><br />

                <asp:Label ID="lblResultado" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
            </div>
        </section>
    </main>

</asp:Content>

