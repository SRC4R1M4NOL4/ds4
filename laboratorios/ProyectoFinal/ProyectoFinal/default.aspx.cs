using System;
using System.Web.UI;
using ProyectoFinal.Services;

namespace ProyectoFinal
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
            }
        }

        private void LoadStatistics()
        {
            try
            {
                var report = DatabaseService.Instance.GenerateUserReport();

                lblTotalSearches.Text = report.TotalSearches.ToString();
                lblTotalPlays.Text = report.TotalPlays.ToString();

                if (report.TopArtists.Count > 0)
                {
                    lblTopArtist.Text = report.TopArtists[0];
                }
                else
                {
                    lblTopArtist.Text = "--";
                }
            }
            catch (Exception ex)
            {
                // Manejar error silenciosamente en la página de inicio
                lblTotalSearches.Text = "0";
                lblTotalPlays.Text = "0";
                lblTopArtist.Text = "--";
            }
        }
    }
}