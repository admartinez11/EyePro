using OpticaMultivisual.Controllers.Dashboard.Optometrista;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Dashboard.PedidoDetalle;

namespace OpticaMultivisual.Views.Dashboard.PedidoDet
{
    public partial class ViewAddPedidoDet : Form
    {
        //Constructor utilizado para la inserción de datos
        public ViewAddPedidoDet(int accion)
        {
            InitializeComponent();
            //Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista, osea que la vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerAddPD objAddDP = new ControllerAddPD(this, accion);
        }

        //Constructor utilizado para la actualización de datos
        public ViewAddPedidoDet(int accion, int pd_ID, string con_ID, DateTime pd_fpedido, DateTime pd_fprogramada, string art_codigo, int art_cant, string pd_obser, bool pd_recetalab)
        {
            InitializeComponent();
            ControllerAddPD objAddLens = new ControllerAddPD(this, accion, pd_ID, con_ID, pd_fpedido, pd_fprogramada, art_codigo, art_cant, pd_obser, pd_recetalab);
        }
    }
}
