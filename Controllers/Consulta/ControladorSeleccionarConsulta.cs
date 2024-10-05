using OpticaMultivisual.Views.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Controllers.Consulta
{
    internal class ControladorSeleccionarConsulta
    {
        SeleccionarConsulta ObjSelecionar;
        public ControladorSeleccionarConsulta(SeleccionarConsulta Vista)
        {
            ObjSelecionar = Vista;
            ObjSelecionar.btnAllConst.Click += new EventHandler(VerTodasConsultas);
            ObjSelecionar.btnConPendiente.Click += new EventHandler(VerPendiConsultas);
            ObjSelecionar.btnConRealizada.Click += new EventHandler(VerRealConsultas);
        }
        public void VerTodasConsultas(object sender, EventArgs e)
        {
            VerConsultasAll openForm = new VerConsultasAll();
            openForm.ShowDialog();
        }
        public void VerPendiConsultas(object sender, EventArgs e)
        {
            VerConsultaPendiente openForm = new VerConsultaPendiente();
            openForm.ShowDialog();
        }
        public void VerRealConsultas(object sender, EventArgs e)
        {
            VerConsultaRealizada openForm = new VerConsultaRealizada();
            openForm.ShowDialog();
        }
    }
}
