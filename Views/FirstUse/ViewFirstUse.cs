using OpticaMultivisual.Controllers.FirstUse;
using OpticaMultivisual.Controllers.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.FirstUse
{
    public partial class ViewFirstUse : Form
    {
        public ViewFirstUse()
        {
            InitializeComponent();
            //Inicializar controlador en la vista
            ControllerFirstUse control = new ControllerFirstUse(this);
            //Se invoca el método CreateRoundRectRgn contenido en la clase Helper
            Region = Region.FromHrgn(CommonClasses.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
    }
}
