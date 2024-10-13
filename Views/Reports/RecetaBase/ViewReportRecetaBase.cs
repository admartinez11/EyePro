using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Views.Reports.RecetaBase
{
    public partial class ViewReportRecetaBase : Form
    {
        public ViewReportRecetaBase()
        {
            InitializeComponent();
        }

        private void ViewReportRecetaBase_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
