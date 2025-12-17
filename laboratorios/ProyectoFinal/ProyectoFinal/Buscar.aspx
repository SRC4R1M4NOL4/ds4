<%@ Page Title="Buscar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Buscar.aspx.cs" Inherits="ProyectoFinal.Buscar" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>🔍 Buscar Canciones</h2>
    <p>Busca tus canciones favoritas en el catálogo de Spotify</p>

    <div class="search-box">
        <asp:TextBox ID="txtSearch" runat="server" placeholder="Escribe el nombre de una canción o artista..." CssClass="search-input"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Buscar" OnClick="btnSearch_Click" CssClass="btn" />
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>

    <div class="track-list">
        <asp:Repeater ID="rptTracks" runat="server">
            <ItemTemplate>
                <div class="track-item">
                    <asp:Image ID="imgTrack" runat="server" 
                               ImageUrl='<%# Eval("ImageUrl") != null ? Eval("ImageUrl") : "~/Images/no-image.png" %>' 
                               CssClass="track-image" />
                    
                    <div class="track-info">
                        <div class="track-name"><%# Eval("Name") %></div>
                        <div class="track-artist">🎤 <%# Eval("Artist") %></div>
                        <div class="track-album">💿 <%# Eval("Album") %></div>
                        
                        <%# !string.IsNullOrEmpty(Eval("PreviewUrl")?.ToString()) ? 
                            "<div class='audio-player'><audio controls><source src='" + Eval("PreviewUrl") + "' type='audio/mpeg'>Tu navegador no soporta audio.</audio></div>" : 
                            "<div style='color: #999; margin-top: 10px;'>⚠️ Vista previa no disponible</div>" %>
                    </div>
                    
                    <asp:Button ID="btnPlay" runat="server" 
                                Text="Reproducir" 
                                CssClass="btn btn-secondary" 
                                CommandArgument='<%# Eval("Id") + "|" + Eval("Name") + "|" + Eval("Artist") %>'
                                OnClick="btnPlay_Click" />
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoResults" runat="server" Text="No se encontraron resultados." 
                   Visible="false" CssClass="message message-error"></asp:Label>
    </div>
</asp:Content>