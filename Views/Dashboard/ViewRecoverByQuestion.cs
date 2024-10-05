using OpticaMultivisual.Controllers.Dashboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OpticaMultivisual.Views.Dashboard
{
    public partial class ViewRecoverByQuestion : Form
    {
        private string username;

        // Constructor que acepta el nombre de usuario
        public ViewRecoverByQuestion(string username)
        {
            InitializeComponent();
            this.username = username;
            // Inicializar el controlador con el nombre de usuario
            ControllerRecoverByQuestion objRecoverq = new ControllerRecoverByQuestion(this, username);
        }
    }
}
