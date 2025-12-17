<%@ Page Title="Recomendar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Recomendar.aspx.cs" Inherits="ProyectoFinal.Recomendar" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>💡 Recomendaciones Musicales</h2>
    <p>Descubre nuevas canciones basadas en tus gustos</p>

    <div class="search-box">
        <asp:TextBox ID="txtTrackId" runat="server" placeholder="Ingresa el ID de una canción para obtener recomendaciones..." CssClass="search-input"></asp:TextBox>
        <asp:Button ID="btnGetRecommendations" runat="server" Text="Obtener Recomendaciones" OnClick="btnGetRecommendations_Click" CssClass="btn" />
    </div>

    <div style="margin: 20px 0; padding: 15px; background: #fff3cd; border-radius: 8px; border-left: 4px solid #ffc107;">
        <strong>💡 Consejo:</strong> Busca primero una canción en la sección "Buscar" y usa el botón "Recomendar" de cualquier resultado.
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>

    <div class="track-list">
        <asp:Repeater ID="rptRecommendations" runat="server">
            <ItemTemplate>
                <div class="track-item">
                    <asp:Image ID="imgTrack" runat="server" 
                               ImageUrl='<%# Eval("ImageUrl") != null ? Eval("ImageUrl") : "~/Images/no-image.png" %>' 
                               CssClass="track-image" />
                    
                    <div class="track-info">
                        <div class="track-name"><%# Eval("Name") %></div>
                        <div class="track-artist">🎤 <%# Eval("Artist") %></div>
                        <div class="track-album">💿 <%# Eval("Album") %></div>
                        <div style="color: #999; font-size: 14px; margin-top: 5px;">
                            ID: <%# Eval("Id") %>
                        </div>
                        
                        <%# !string.IsNullOrEmpty(Eval("PreviewUrl")?.ToString()) ? 
                            "<div class='audio-player'><audio controls><source src='" + Eval("PreviewUrl") + "' type='audio/mpeg'>Tu navegador no soporta audio.</audio></div>" : 
                            "<div style='color: #999; margin-top: 10px;'>⚠️ Vista previa no disponible</div>" %>
                    </div>
                    
                    <asp:Button ID="btnPlay" runat="server" 
                                Text="❤️ Me Gusta" 
                                CssClass="btn btn-secondary" 
                                CommandArgument='<%# Eval("Id") + "|" + Eval("Name") + "|" + Eval("Artist") %>'
                                OnClick="btnPlay_Click" />
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoRecommendations" runat="server" 
                   Text="Ingresa un ID de canción para obtener recomendaciones." 
                   Visible="false" 
                   CssClass="message message-error"></asp:Label>
    </div>

    <div style="margin-top: 50px;">
        <h3 style="color: #667eea;">🔥 Tus Canciones Más Populares</h3>
        <p>Haz clic en "Obtener Recomendaciones" usando el ID de estas canciones</p>
        
        <asp:Repeater ID="rptUserTopTracks" runat="server">
            <ItemTemplate>
                <div class="list-item">
                    <strong><%# Eval("TrackName") %></strong> - <%# Eval("ArtistName") %>
                    <br />
                    <small style="color: #999;">Reproducido <%# Eval("PlayCount") %> veces</small>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
