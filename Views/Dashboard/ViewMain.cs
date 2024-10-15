using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Dashboard;

namespace OpticaMultivisual.Views.Dashboard
{
    // Clase parcial ViewMain que hereda de Form
    public partial class ViewMain : Form
    {
        // Constructor de la clase ViewMain
        public ViewMain(string username)
        {
            // Método generado automáticamente para inicializar componentes visuales
            InitializeComponent();
            // Se instancia un objeto ControllerDashboard y se le pasa esta instancia de ViewMain como parámetro
            ControllerMain objDash = new ControllerMain(this, username);

        }
            
    }
}
