<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="ProyectoFinal.Reportes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>📊 Reportes de Actividad Musical</h2>
    <p>Estadísticas completas de tu actividad en la aplicación</p>

    <asp:Button ID="btnRefresh" runat="server" Text="🔄 Actualizar Reportes" OnClick="btnRefresh_Click" CssClass="btn" />

    <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>

    <!-- Estadísticas Generales -->
    <div class="stats-grid" style="margin-top: 30px;">
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
            <div class="stat-label">Reproducciones Totales</div>
        </div>

        <div class="stat-card">
            <div class="stat-number">
                <asp:Label ID="lblLastActivity" runat="server" Text="--"></asp:Label>
            </div>
            <div class="stat-label">Última Actividad</div>
        </div>
    </div>

    <!-- Top Artistas -->
    <div class="report-section" style="margin-top: 40px;">
        <h3>🎤 Top 5 Artistas Más Escuchados</h3>
        <asp:Repeater ID="rptTopArtists" runat="server">
            <ItemTemplate>
                <div class="list-item">
                    <span style="font-size: 24px; margin-right: 10px;">
                        <%# Container.ItemIndex == 0 ? "🥇" : 
                            Container.ItemIndex == 1 ? "🥈" : 
                            Container.ItemIndex == 2 ? "🥉" : "🎵" %>
                    </span>
                    <strong><%# Container.ItemIndex + 1 %>.</strong> <%# Container.DataItem %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoArtists" runat="server" Text="No hay datos de artistas aún." Visible="false"></asp:Label>
    </div>

    <!-- Canciones Más Reproducidas -->
    <div class="report-section">
        <h3>🔥 Top 10 Canciones Más Reproducidas</h3>
        <asp:Repeater ID="rptMostPlayed" runat="server">
            <ItemTemplate>
                <div class="list-item">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <div>
                            <strong><%# Container.ItemIndex + 1 %>. <%# Eval("TrackName") %></strong>
                            <div style="color: #666; font-size: 14px;"><%# Eval("ArtistName") %></div>
                        </div>
                        <div style="background: #1DB954; color: white; padding: 5px 15px; border-radius: 20px; font-weight: bold;">
                            ▶️ <%# Eval("PlayCount") %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoTracks" runat="server" Text="No hay datos de canciones aún." Visible="false"></asp:Label>
    </div>

    <!-- Búsquedas Más Frecuentes -->
    <div class="report-section">
        <h3>🔍 Top 10 Búsquedas Más Frecuentes</h3>
        <asp:Repeater ID="rptSearchFrequency" runat="server">
            <ItemTemplate>
                <div class="list-item">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <div>
                            <strong><%# Container.ItemIndex + 1 %>.</strong> "<%# Eval("Key") %>"
                        </div>
                        <div style="background: #667eea; color: white; padding: 5px 15px; border-radius: 20px; font-weight: bold;">
                            <%# Eval("Value") %> veces
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoSearches" runat="server" Text="No hay datos de búsquedas aún." Visible="false"></asp:Label>
    </div>

    <!-- Resumen de Actividad -->
    <div class="report-section">
        <h3>📈 Resumen de Actividad</h3>
        <div style="background: white; padding: 20px; border-radius: 10px;">
            <p style="font-size: 16px; color: #666;">
                Has realizado <strong style="color: #1DB954;"><asp:Label ID="lblTotalSearches2" runat="server"></asp:Label></strong> búsquedas 
                y <strong style="color: #667eea;"><asp:Label ID="lblTotalPlays2" runat="server"></asp:Label></strong> reproducciones en total.
            </p>
            <p style="font-size: 14px; color: #999; margin-top: 10px;">
                Última actividad: <asp:Label ID="lblLastActivity2" runat="server"></asp:Label>
            </p>
        </div>
    </div>
</asp:Content>
