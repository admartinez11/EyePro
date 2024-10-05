using OpticaMultivisual.Controllers.Article.Modelo;
using OpticaMultivisual.Controllers.Article.TipoArticulo;
using OpticaMultivisual.Controllers.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Dashboard.Article.Modelo
{
    public partial class ViewAdminModelo : Form
    {
        public ViewAdminModelo()
        {
            InitializeComponent();
            ControllerAdminModelo objAppointment = new ControllerAdminModelo(this);
        }
    }
}
