using System;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoFinal.Services;

namespace ProyectoFinal
{
    public partial class Recomendar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserTopTracks();
                lblMessage.Visible = false;
                lblNoRecommendations.Visible = true;
            }
        }

        protected async void btnGetRecommendations_Click(object sender, EventArgs e)
        {
            string trackId = txtTrackId.Text.Trim();

            if (string.IsNullOrEmpty(trackId))
            {
                ShowMessage("Por favor ingresa un ID de canción.", false);
                return;
            }

            try
            {
                var recommendations = await SpotifyService.Instance.GetRecommendations(trackId);

                if (recommendations.Count > 0)
                {
                    rptRecommendations.DataSource = recommendations;
                    rptRecommendations.DataBind();
                    lblNoRecommendations.Visible = false;
                    ShowMessage($"Se encontraron {recommendations.Count} recomendaciones basadas en la canción seleccionada.", true);
                }
                else
                {
                    rptRecommendations.DataSource = null;
                    rptRecommendations.DataBind();
                    lblNoRecommendations.Visible = true;
                    lblNoRecommendations.Text = "No se encontraron recomendaciones para esta canción.";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al obtener recomendaciones: " + ex.Message, false);
                rptRecommendations.DataSource = null;
                rptRecommendations.DataBind();
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
                    ShowMessage($"¡Canción guardada en favoritos! {trackName} - {artistName}", true);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error al guardar canción: " + ex.Message, false);
                }
            }
        }

        private void LoadUserTopTracks()
        {
            try
            {
                var topTracks = DatabaseService.Instance.GetTopTracks(5);
                rptUserTopTracks.DataSource = topTracks;
                rptUserTopTracks.DataBind();
            }
            catch (Exception ex)
            {
                rptUserTopTracks.DataSource = null;
                rptUserTopTracks.DataBind();
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