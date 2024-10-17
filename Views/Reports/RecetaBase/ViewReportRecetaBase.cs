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
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Optometrista;

namespace OpticaMultivisual.Views.Reports.RecetaBase
{
    public partial class ViewReportRecetaBase : Form
    {
        public int Id;
        public ViewReportRecetaBase(string id)
        {
            // Asignar el valor de Id en el constructor
            this.Id = ObtenerId(id);
            InitializeComponent();
            if (this.dRefractivoTableAdapter == null || this.dataSet_ReportRecetaBase == null)
            {
                MessageBox.Show("Error: El adaptador o el dataset no están inicializados.");
                return;
            }
        }

        public int ObtenerId(string id)
        {
            if (int.TryParse(id, out int parsedId))
            {
                this.Id = parsedId;
                return this.Id;
            }
            else
            {
                MessageBox.Show("El valor de Id no es válido.");
                return 0; // o manejar el error de otra manera
            }
        }

        private void ViewReportRecetaBase_Load(object sender, EventArgs e)
        {
            try
            {
                this.dRefractivoTableAdapter.ObtenerRecetaBase(this.dataSet_ReportRecetaBase.DRefractivo, Id);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }
    }
}