using System;
using System.Web.UI;
using ProyectoFinal.Services;

namespace ProyectoFinal
{
    public partial class Reportes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReports();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            try
            {
                var report = DatabaseService.Instance.GenerateUserReport();

                // Estadísticas generales
                lblTotalSearches.Text = report.TotalSearches.ToString();
                lblTotalPlays.Text = report.TotalPlays.ToString();
                lblTotalSearches2.Text = report.TotalSearches.ToString();
                lblTotalPlays2.Text = report.TotalPlays.ToString();

                if (report.LastActivity.HasValue)
                {
                    lblLastActivity.Text = report.LastActivity.Value.ToString("dd/MM/yyyy");
                    lblLastActivity2.Text = report.LastActivity.Value.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    lblLastActivity.Text = "Sin actividad";
                    lblLastActivity2.Text = "Sin actividad";
                }

                // Top Artistas
                if (report.TopArtists.Count > 0)
                {
                    rptTopArtists.DataSource = report.TopArtists;
                    rptTopArtists.DataBind();
                    lblNoArtists.Visible = false;
                }
                else
                {
                    rptTopArtists.DataSource = null;
                    rptTopArtists.DataBind();
                    lblNoArtists.Visible = true;
                }

                // Canciones más reproducidas
                if (report.MostPlayedTracks.Count > 0)
                {
                    rptMostPlayed.DataSource = report.MostPlayedTracks;
                    rptMostPlayed.DataBind();
                    lblNoTracks.Visible = false;
                }
                else
                {
                    rptMostPlayed.DataSource = null;
                    rptMostPlayed.DataBind();
                    lblNoTracks.Visible = true;
                }

                // Búsquedas frecuentes
                if (report.SearchFrequency.Count > 0)
                {
                    rptSearchFrequency.DataSource = report.SearchFrequency;
                    rptSearchFrequency.DataBind();
                    lblNoSearches.Visible = false;
                }
                else
                {
                    rptSearchFrequency.DataSource = null;
                    rptSearchFrequency.DataBind();
                    lblNoSearches.Visible = true;
                }

                ShowMessage("Reportes actualizados correctamente.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error al cargar reportes: " + ex.Message, false);
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