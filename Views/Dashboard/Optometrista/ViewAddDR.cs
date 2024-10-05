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
    public partial class ViewAddDR : Form
    {
        //Constructor utilizado para la inserción de datos
        public ViewAddDR(int accion)
        {
            InitializeComponent();
            //Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista, osea que la vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerAddDR objAddDR = new ControllerAddDR(this, accion);
        }


        //Constructor utilizado para la actualización de datos
        public ViewAddDR(int accion, int DR_ID, string con_ID, string OD_esfera, string OD_cilindro, string OD_eje, string OD_prisma, string OD_adicion, string OD_AO, string OD_AP, string OD_DP, string OI_esfera, string OI_cilindro, string OI_eje, string OI_prisma, string OI_adicion, string OI_AO, string OI_AP, string OI_DP)
        {
            InitializeComponent();
            ControllerAddDR objAddDR = new ControllerAddDR(this, accion, DR_ID, con_ID, OD_esfera, OD_cilindro, OD_eje, OD_prisma, OD_adicion, OD_AO, OD_AP, OD_DP, OI_esfera, OI_cilindro, OI_eje, OI_prisma, OI_adicion, OI_AO, OI_AP, OI_DP);
        }
    }
}
