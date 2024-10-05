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
    public partial class ViewByQuestion : Form
    {
        public ViewByQuestion()
        {
            InitializeComponent();
            ControllerByQuestion objByQ = new ControllerByQuestion(this);
        }
    }
}
