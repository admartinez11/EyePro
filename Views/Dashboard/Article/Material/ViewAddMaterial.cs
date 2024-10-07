using OpticaMultivisual.Controllers.Article.Color;
using OpticaMultivisual.Controllers.Article.Material;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Dashboard.Article.Material
{
    public partial class ViewAddMaterial : Form
    {
        public ViewAddMaterial(int accion)
        {
            InitializeComponent();
            ControllerADDMaterial objAddUser = new ControllerADDMaterial(this, accion);
        }
        public ViewAddMaterial(int accion, int Material_ID, string Material_nombre, string Material_descripcion)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerADDMaterial objAddUser = new ControllerADDMaterial(this, accion, Material_ID, Material_nombre, Material_descripcion);
        }
    }
}
