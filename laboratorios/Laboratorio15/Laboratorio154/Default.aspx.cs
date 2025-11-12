using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Laboratorio154
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSumar_Click(object sender, EventArgs e)
        {
            double num1, num2;

            if (double.TryParse(txtNumero1.Text, out num1) && double.TryParse(txtNumero2.Text, out num2))
            {
                double suma = num1 + num2;
                lblResultado.Text = "El resultado es: " + suma.ToString();
            }
            else
            {
                lblResultado.Text = "Por favor, ingrese valores numéricos válidos.";
            }
        }
    }
}