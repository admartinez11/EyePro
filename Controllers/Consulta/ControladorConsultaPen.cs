using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Consulta
{
    internal class ControladorConsultaPen
    {
        VerConsultaPendiente ObjverConsulta;
        public ControladorConsultaPen(VerConsultaPendiente Vista)
        {
            ObjverConsulta = Vista;
            ObjverConsulta.Load += new EventHandler(CargarInfoConsulta);
            ObjverConsulta.btnBuscarConsulta.Click += new EventHandler(BuscarConsultaControlador);
            ObjverConsulta.btnNuevaConsulta.Click += new EventHandler(AgregarConsulta);
            ObjverConsulta.btnEliminarConsulta.Click += new EventHandler(EliminarConsulta);
            ObjverConsulta.btnActConsulta.Click += new EventHandler(ActualizarConsulta);
        }
        public void CargarInfoConsulta(object sender, EventArgs e)
        {
            ActualizarDatosConsulta();
        }


        public void ActualizarDatosConsulta()
        {
            DAOConsulta dAOConsulta = new DAOConsulta();
            DataSet ds = dAOConsulta.ObtenerInfoConsultaPendiente();
            ObjverConsulta.dgvInfoConsultaPen.DataSource = ds.Tables["VistaConsultas"];

        }
        public void BuscarConsultaControlador(object sender, EventArgs e)
        {
            DAOConsulta dAOConsulta = new DAOConsulta();
            DataSet ds = dAOConsulta.BuscarConsulta(ObjverConsulta.txtBuscarConsulta.Text.Trim());
            ObjverConsulta.dgvInfoConsultaPen.DataSource = ds.Tables["Consulta"];
        }
        public void AgregarConsulta(object sender, EventArgs e)
        {
            AñadirConsulta openForm = new AñadirConsulta(1);
            openForm.ShowDialog();
            ActualizarDatosConsulta();
        }
        private void EliminarConsulta(object sender, EventArgs e)
        {
            int pos = ObjverConsulta.dgvInfoConsultaPen.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea eliminar a la consulta del DUI:\n {ObjverConsulta.dgvInfoConsultaPen[1, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOConsulta dAOConsulta = new DAOConsulta();
                dAOConsulta.Con_ID = int.Parse(ObjverConsulta.dgvInfoConsultaPen[0, pos].Value.ToString());
                int valorRetornado = dAOConsulta.EliminarConsulta();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Registro eliminado", "Acción completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActualizarDatosConsulta();
                }
                else
                {
                    MessageBox.Show("EPV003 - Los datos no pudieron ser eliminados", "Acción interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public void ActualizarConsulta(object sender, EventArgs e)
        {
            int pos = ObjverConsulta.dgvInfoConsultaPen.CurrentRow.Index;
            AñadirConsulta openForm = new AñadirConsulta(
                2, // accion
                ObjverConsulta.dgvInfoConsultaPen[1, pos].Value.ToString(), // cli_DUI
                ObjverConsulta.dgvInfoConsultaPen[2, pos].Value.ToString(), // vis_ID
                DateTime.Parse(ObjverConsulta.dgvInfoConsultaPen[3, pos].Value.ToString()), // con_fechahora
                ObjverConsulta.dgvInfoConsultaPen[4, pos].Value.ToString(), // con_obser
                ObjverConsulta.dgvInfoConsultaPen[5, pos].Value.ToString(), // emp_ID
                int.Parse(ObjverConsulta.dgvInfoConsultaPen[0, pos].Value.ToString()),  // con_ID
                DateTime.Parse(ObjverConsulta.dgvInfoConsultaPen[6, pos].Value.ToString()), // con_fechahora
                ObjverConsulta.dgvInfoConsultaPen[7, pos].Value.ToString() // cli_DUI
            );

            openForm.ShowDialog();
            ActualizarDatosConsulta();
        }

    }
}
