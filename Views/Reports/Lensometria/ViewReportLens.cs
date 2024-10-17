using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Reports.Lensometria
{
    public partial class ViewReportLens : Form
    {
        public ViewReportLens()
        {
            InitializeComponent();
        }

        private void ViewReportLens_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
