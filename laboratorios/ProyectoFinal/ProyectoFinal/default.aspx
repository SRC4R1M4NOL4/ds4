<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProyectoFinal.Default" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: center; padding: 50px 0;">
        <h2>Bienvenido a Spotify App</h2>
        <p style="font-size: 18px; color: #666; margin: 20px 0;">
            Tu aplicación para buscar, reproducir y descubrir música
        </p>

        <div class="stats-grid" style="margin-top: 50px;">
            <div class="stat-card">
                <div class="stat-number">
                    <asp:Label ID="lblTotalSearches" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Búsquedas Totales</div>
            </div>

            <div class="stat-card">
                <div class="stat-number">
                    <asp:Label ID="lblTotalPlays" runat="server" Text="0"></asp:Label>
                </div>
                <div class="stat-label">Reproducciones</div>
            </div>

            <div class="stat-card">
                <div class="stat-number">
                    <asp:Label ID="lblTopArtist" runat="server" Text="--"></asp:Label>
                </div>
                <div class="stat-label">Artista Favorito</div>
            </div>
        </div>

        <div style="margin-top: 50px;">
            <h3>Funcionalidades</h3>
            <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; margin-top: 30px;">
                <div class="feature-card">
                    <h4>🔍 Buscar</h4>
                    <p>Busca tus canciones favoritas en el catálogo de Spotify</p>
                    <a href="Buscar.aspx" class="btn btn-secondary">Ir a Buscar</a>
                </div>

                <div class="feature-card">
                    <h4>▶️ Reproducir</h4>
                    <p>Reproduce previews de canciones y registra tus favoritas</p>
                    <a href="Reproducir.aspx" class="btn btn-secondary">Ir a Reproducir</a>
                </div>

                <div class="feature-card">
                    <h4>💡 Recomendar</h4>
                    <p>Obtén recomendaciones basadas en tus canciones favoritas</p>
                    <a href="Recomendar.aspx" class="btn btn-secondary">Ir a Recomendar</a>
                </div>

                <div class="feature-card">
                    <h4>📊 Reportes</h4>
                    <p>Visualiza estadísticas de tu actividad musical</p>
                    <a href="Reportes.aspx" class="btn btn-secondary">Ver Reportes</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
