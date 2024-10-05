using OpticaMultivisual.Controllers.Article;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Dashboard.Article
{
    public partial class ViewAddArticle : Form
    {
        private ControllerAddArticle objAddArticle;
        public ViewAddArticle(int accion)
        {
            InitializeComponent();
            objAddArticle = new ControllerAddArticle(this, accion);
            CargarFormulario();
        }

        public ViewAddArticle(int accion, string art_codigo, string art_nombre, string art_descripcion, int tipoart_ID, int mod_ID, string art_medidas, int material_ID, int color_ID, string art_urlimagen, string art_comentarios, string art_punitario)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            objAddArticle = new ControllerAddArticle(this, accion, art_codigo, art_nombre, art_descripcion, tipoart_ID, mod_ID, art_medidas, material_ID, color_ID, art_urlimagen, art_comentarios, art_punitario);
            CargarFormulario();
        }
        private void CargarFormulario()
        {
            if (objAddArticle != null)
            {
                objAddArticle.CargaInicial(null, null);  // Llama al controlador para cargar los ComboBox
            }
            else
            {
                MessageBox.Show("El controlador no se ha inicializado correctamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
