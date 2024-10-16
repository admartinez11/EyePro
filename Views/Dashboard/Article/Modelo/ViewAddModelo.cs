using OpticaMultivisual.Controllers.Article.Modelo;
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

namespace OpticaMultivisual.Views.Dashboard.Article.Modelo
{
    public partial class ViewAddModelo : Form
    {
        public ViewAddModelo(int accion)
        {
            InitializeComponent();
            ControllerAddModelo objAddUser = new ControllerAddModelo(this, accion);
        }
        public ViewAddModelo(int accion, int mod_ID, string Mod_nombre, string Marca_ID)
        {
            InitializeComponent();
            ControllerAddModelo objAddUser = new ControllerAddModelo(this, accion, mod_ID, Mod_nombre, Marca_ID);
        }
    }
}
