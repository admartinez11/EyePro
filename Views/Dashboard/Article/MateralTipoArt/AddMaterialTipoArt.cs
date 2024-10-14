using OpticaMultivisual.Controllers.Article.MaterialTipoArt;
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

namespace OpticaMultivisual.Views.Dashboard.Article.MateralTipoArt
{
    public partial class AddMaterialTipoArt : Form
    {
        public AddMaterialTipoArt()
        {
            InitializeComponent();
            ControllerAdMaterialTipoArt objAddUser = new ControllerAdMaterialTipoArt(this);
        }
    }
}
