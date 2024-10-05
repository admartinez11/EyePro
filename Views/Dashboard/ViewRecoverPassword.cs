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

namespace OpticaMultivisual.Views.Dashboard
{
    public partial class ViewRecoverPassword : Form
    {
        public ViewRecoverPassword()
        {
            InitializeComponent();
            ControllerRecoverPassword objRecoverpass = new ControllerRecoverPassword(this);
        }
    }
}
