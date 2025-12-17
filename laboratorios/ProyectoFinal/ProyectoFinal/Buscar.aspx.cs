using System;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoFinal.Services;

namespace ProyectoFinal
{
    public partial class Buscar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                lblNoResults.Visible = false;
            }
        }

        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                ShowMessage("Por favor ingresa un término de búsqueda.", false);
                return;
            }

            try
            {
                var tracks = await SpotifyService.Instance.SearchTracks(query);

                if (tracks.Count > 0)
                {
                    rptTracks.DataSource = tracks;
                    rptTracks.DataBind();
                    lblNoResults.Visible = false;
                    ShowMessage($"Se encontraron {tracks.Count} resultados para '{query}'", true);
                }
                else
                {
                    rptTracks.DataSource = null;
                    rptTracks.DataBind();
                    lblNoResults.Visible = true;
                    lblMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al buscar canciones: " + ex.Message, false);
                rptTracks.DataSource = null;
                rptTracks.DataBind();
            }
        }

        protected void btnPlay_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string[] data = btn.CommandArgument.Split('|');

            if (data.Length == 3)
            {
                string trackId = data[0];
                string trackName = data[1];
                string artistName = data[2];

                try
                {
                    DatabaseService.Instance.SavePlayedTrack(trackId, trackName, artistName);
                    ShowMessage($"Reproducción registrada: {trackName} - {artistName}", true);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error al registrar reproducción: " + ex.Message, false);
                }
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "message message-success" : "message message-error";
            lblMessage.Visible = true;
        }
    }
}