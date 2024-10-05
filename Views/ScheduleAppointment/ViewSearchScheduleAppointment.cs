using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Controllers.ScheduleAppointment;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Dashboard.ScheduleAppointment;

namespace OpticaMultivisual.Views.ScheduleAppointment
{
    public partial class ViewSearchScheduleAppointment : Form
    {
        public ViewSearchScheduleAppointment()
        {
            InitializeComponent();
            ControllerSearchScheduleAppointment objAppointment = new ControllerSearchScheduleAppointment(this);
        }
    }
}
