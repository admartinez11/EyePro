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

namespace OpticaMultivisual.Views.Dashboard.Article.TipoArticulo
{
    public partial class ViewAddTipoArt : Form
    {
        public ViewAddTipoArt(int accion)
        {
            InitializeComponent();
            ControllerAddTipoArt objAddUser = new ControllerAddTipoArt(this, accion);
        }

        public ViewAddTipoArt(int accion, int Tipoart_ID, string Tipoart_nombre, string Tipoart_descripcion)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerAddTipoArt objAddUser = new ControllerAddTipoArt(this, accion, Tipoart_ID, Tipoart_nombre, Tipoart_descripcion);
        }
    }
}
