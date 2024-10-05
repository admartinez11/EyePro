using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Login;

namespace OpticaMultivisual.Views.Login
{
    public partial class ViewCambiarClaveDefecto : Form
    {
        public ViewCambiarClaveDefecto()
        {
            InitializeComponent();
            ControllerCambiarClaveDefecto control = new ControllerCambiarClaveDefecto(this);
        }
    }
}
