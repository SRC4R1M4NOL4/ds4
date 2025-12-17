<%@ Page Title="Reproducir" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reproducir.aspx.cs" Inherits="ProyectoFinal.Reproducir" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>▶️ Canciones Más Reproducidas</h2>
    <p>Tus canciones más escuchadas basadas en tu historial</p>

    <asp:Button ID="btnRefresh" runat="server" Text="🔄 Actualizar Lista" OnClick="btnRefresh_Click" CssClass="btn" />

    <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>

    <div class="track-list" style="margin-top: 30px;">
        <asp:Repeater ID="rptTopTracks" runat="server">
            <ItemTemplate>
                <div class="track-item">
                    <div style="width: 80px; height: 80px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                                border-radius: 8px; display: flex; align-items: center; justify-content: center; 
                                color: white; font-size: 32px; font-weight: bold; margin-right: 20px;">
                        <%# Container.ItemIndex + 1 %>
                    </div>
                    
                    <div class="track-info">
                        <div class="track-name"><%# Eval("TrackName") %></div>
                        <div class="track-artist">🎤 <%# Eval("ArtistName") %></div>
                        <div style="color: #1DB954; font-weight: bold; margin-top: 8px;">
                            ▶️ Reproducido: <%# Eval("PlayCount") %> veces
                        </div>
                        <div style="color: #999; font-size: 14px; margin-top: 5px;">
                            Última reproducción: <%# ((DateTime)Eval("LastPlayed")).ToString("dd/MM/yyyy HH:mm") %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoTracks" runat="server" 
                   Text="Aún no tienes canciones reproducidas. Ve a la sección de Buscar para comenzar." 
                   Visible="false" 
                   CssClass="message message-error"></asp:Label>
    </div>

    <div style="margin-top: 40px; padding: 20px; background: #f8f9fa; border-radius: 10px;">
        <h3 style="color: #667eea; margin-bottom: 15px;">💡 Consejo</h3>
        <p style="color: #666;">
            Cada vez que reproduces una canción desde la sección de Buscar o Recomendar, 
            se registra en tu historial. Aquí puedes ver tus canciones favoritas ordenadas 
            por la cantidad de veces que las has escuchado.
        </p>
    </div>
</asp:Content>
