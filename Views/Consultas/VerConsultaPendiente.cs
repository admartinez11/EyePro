using OpticaMultivisual.Controllers.Consulta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Consultas
{
    public partial class VerConsultaPendiente : Form
    {
        public VerConsultaPendiente()
        {
            InitializeComponent();
            ControladorConsultaPen Control = new ControladorConsultaPen(this);
        }
    }
}
