using OpticaMultivisual.Controllers.Article.Color;
using OpticaMultivisual.Controllers.Article.TipoArticulo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Dashboard.Article.Color
{
    public partial class ViewAddColor : Form
    {
        public ViewAddColor(int accion)
        {
            InitializeComponent();
            ControllerAddColor objAddUser = new ControllerAddColor(this, accion);
        }
        public ViewAddColor(int accion, int Color_ID, string Color_nombre, string Color_descripcion)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerAddColor objAddUser = new ControllerAddColor(this, accion, Color_ID, Color_nombre, Color_descripcion);
        }
    }
}
