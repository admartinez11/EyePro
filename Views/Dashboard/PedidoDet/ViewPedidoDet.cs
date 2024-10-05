using OpticaMultivisual.Controllers.Dashboard.Optometrista;
using OpticaMultivisual.Controllers.Dashboard.PedidoDetalle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Dashboard.PedidoDet
{
    public partial class ViewPedidoDet : Form
    {
        public ViewPedidoDet()
        {
            InitializeComponent();
            int accion = 0;
            ControllerAdminPD pD = new ControllerAdminPD(this);
            ControllerAdminPD ObjAdminPD = new ControllerAdminPD(this, accion);

        }
    }
}
