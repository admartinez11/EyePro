using OpticaMultivisual.Controllers.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Login
{
    public partial class ViewCambiarClave : Form
    {
        public ViewCambiarClave()
        {
            InitializeComponent();
            ControllerCambiarContrasena control = new ControllerCambiarContrasena(this);
        }
    }
}
