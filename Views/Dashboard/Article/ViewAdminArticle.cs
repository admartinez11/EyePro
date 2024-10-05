using OpticaMultivisual.Controllers.Article;
using OpticaMultivisual.Controllers.Dashboard.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Article
{
    public partial class ViewAdminArticle : Form
    {
        public ViewAdminArticle()
        {
            InitializeComponent();
            ControllerAdminArticle objArticle = new ControllerAdminArticle(this);
        }

    }
}