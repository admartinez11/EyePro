using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Server;

namespace OpticaMultivisual.Views.Server
{
    public partial class ViewAdminConnection : Form
    {
        public ViewAdminConnection(int origen)
        {
            InitializeComponent();
            ControllerAdminConnection objConnection = new ControllerAdminConnection(this, origen);
        }
    }
}
