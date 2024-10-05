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
    public partial class ViewRecuperaciónPass : Form
    {
        public ViewRecuperaciónPass()
        {
            InitializeComponent();
            ControllerMetodosDeRecuperacion next = new ControllerMetodosDeRecuperacion(this);
        }
    }
}
