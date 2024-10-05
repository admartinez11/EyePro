using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Dashboard.Optometrista;

namespace OpticaMultivisual.Views.Dashboard.Optometrista
{
    public partial class ViewAddLens : Form
    {
        //Constructor utilizado para la inserción de datos
        public ViewAddLens(int accion)
        {
            InitializeComponent();
            //Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista, osea que la vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerAddLens objAddLens = new ControllerAddLens(this, accion);
        }


        //Constructor utilizado para la actualización de datos
        public ViewAddLens(int accion, int lens_ID, int con_ID, string OD_esfera, string OD_cilindro, string OD_eje, string OD_prisma, string OD_adicion, string OI_esfera, string OI_cilindro, string OI_eje, string OI_prisma, string OI_adicion)
        {
            InitializeComponent();
            ControllerAddLens objAddLens = new ControllerAddLens(this, accion, lens_ID, con_ID, OD_esfera, OD_cilindro, OD_eje, OD_prisma, OD_adicion, OI_esfera, OI_cilindro, OI_eje, OI_prisma, OI_adicion);
        }

    }
}
