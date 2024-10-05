using OpticaMultivisual.Controllers.FirstUse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.FirstUse
{
    public partial class ViewCreateFirstUser : Form
    {
        public ViewCreateFirstUser()
        {
            InitializeComponent();
            ControllerCreateFirstUser control = new ControllerCreateFirstUser(this);
        }
    }
}
