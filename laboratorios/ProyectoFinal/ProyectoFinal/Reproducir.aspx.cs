using System;
using System.Web.UI;
using ProyectoFinal.Services;

namespace ProyectoFinal
{
    public partial class Reproducir : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTopTracks();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTopTracks();
        }

        private void LoadTopTracks()
        {
            try
            {
                var topTracks = DatabaseService.Instance.GetTopTracks(20);

                if (topTracks.Count > 0)
                {
                    rptTopTracks.DataSource = topTracks;
                    rptTopTracks.DataBind();
                    lblNoTracks.Visible = false;
                    ShowMessage($"Se encontraron {topTracks.Count} canciones en tu historial.", true);
                }
                else
                {
                    rptTopTracks.DataSource = null;
                    rptTopTracks.DataBind();
                    lblNoTracks.Visible = true;
                    lblMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al cargar canciones: " + ex.Message, false);
                rptTopTracks.DataSource = null;
                rptTopTracks.DataBind();
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