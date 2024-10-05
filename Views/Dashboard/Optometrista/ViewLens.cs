using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Dashboard.Optometrista;

namespace OpticaMultivisual.Views.Dashboard.Optometrista
{
    public partial class ViewLens : Form
    {
        public ViewLens()
        {
            InitializeComponent();
            int accion = 0;
            ControllerAdminLens ObjAddLens = new ControllerAdminLens(this);
            ControllerAdminLens ObjAdminLens = new ControllerAdminLens(this, accion);
        }
    }
}
