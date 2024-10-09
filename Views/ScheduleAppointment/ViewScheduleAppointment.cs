using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Controllers.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpticaMultivisual.Controllers.ScheduleAppointment.ControllerScheduleAppointment;

namespace OpticaMultivisual.Views.ScheduleAppointment
{
    //caca
    public partial class ViewScheduleAppointment : Form
    {
        public DateTimePicker dtpFechaCita { get; set; }

        public ViewScheduleAppointment(int accion)
        {
            InitializeComponent();
            ControllerScheduleAppointment objAddUser = new ControllerScheduleAppointment(this, accion);
        }

        public ViewScheduleAppointment(int accion, DateTime Vis_fcita, string Vis_nombre, string Vis_apellido, string Vis_tel, string Vis_correo, string Vis_dui, string vis_obser)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            ControllerScheduleAppointment objAddUser = new ControllerScheduleAppointment(this, accion, Vis_fcita, Vis_nombre, Vis_apellido, Vis_tel, Vis_correo, Vis_dui, vis_obser);
        }
        
    }
}
