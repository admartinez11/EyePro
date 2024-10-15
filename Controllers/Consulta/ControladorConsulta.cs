using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Consultas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Consulta
{
    internal class ControladorConsulta
    {
        VerConsulta ObjverConsulta;
        AñadirConsulta ObjConsulta;
        public ControladorConsulta(VerConsulta Vista)
        {
            ObjverConsulta = Vista;
            ObjverConsulta.Load += new EventHandler(CargarInfoConsulta);
            ObjverConsulta.btnBuscarConsulta.Click += new EventHandler(BuscarConsultaControlador);
            ObjverConsulta.btnNuevaConsulta.Click += new EventHandler(AgregarConsulta);
            ObjverConsulta.btnEliminarConsulta.Click += new EventHandler(EliminarConsulta);
            ObjverConsulta.btnActConsulta.Click += new EventHandler(ActualizarConsulta);

            // Asociar el evento SelectedIndexChanged del ComboBox
            ObjverConsulta.CMBTipoEstado.SelectedIndexChanged += new EventHandler(ComboBoxEstConsulta);
        }

        public void CargarInfoConsulta(object sender, EventArgs e)
        {
            // Cargar la información inicial de las consultas al cargar la vista
            ActualizarDatosConsulta();
            ObjverConsulta.dgvInfoConsulta.ReadOnly = true;
        }

        private void ComboBoxEstConsulta(object sender, EventArgs e)
        {
            // Obtener la selección actual del ComboBox
            string selectedValue = ObjverConsulta.CMBTipoEstado.SelectedItem.ToString();

            // Realizar acciones dependiendo del valor seleccionado
            if (selectedValue == "Realizada")
            {
                ComboBoxEstConsultaRealizado(sender, e);
            }
            else if (selectedValue == "Pendiente")
            {
                ComboBoxEstConsultaPendiente(sender, e);
            }
            else if (selectedValue == "Todas")
            {
                ActualizarDatosConsulta();
            }
        }

        public void ActualizarDatosConsulta()
        {
            DAOConsulta dAOConsulta = new DAOConsulta();
            DataSet ds = dAOConsulta.ObtenerInfoConsulta();
            ObjverConsulta.dgvInfoConsulta.DataSource = ds.Tables["VistaConsultas"];
        }

        public void ComboBoxEstConsultaPendiente(object sender, EventArgs e)
        {
            DAOConsulta dAOConsulta = new DAOConsulta();
            DataSet ds = dAOConsulta.ObtenerInfoConsultaPendiente();
            ObjverConsulta.dgvInfoConsulta.DataSource = ds.Tables["VistaConsultas"];
        }

        public void ComboBoxEstConsultaRealizado(object sender, EventArgs e)
        {
            DAOConsulta dAOConsulta = new DAOConsulta();
            DataSet ds = dAOConsulta.ObtenerInfoConsultaRealizada();  // Asumiendo que este método existe
            ObjverConsulta.dgvInfoConsulta.DataSource = ds.Tables["VistaConsultas"];
        }

        public void BuscarConsultaControlador(object sender, EventArgs e)
        {
            DAOConsulta dAOConsulta = new DAOConsulta();
            DataSet ds = dAOConsulta.BuscarConsulta(ObjverConsulta.txtBuscarConsulta.Text.Trim());
            ObjverConsulta.dgvInfoConsulta.DataSource = ds.Tables["VistaConsultas"];
        }
        public void AgregarConsulta(object sender, EventArgs e)
        {
            AñadirConsulta openForm = new AñadirConsulta(1);
            openForm.ShowDialog();
            ActualizarDatosConsulta();
        }
        private void EliminarConsulta(object sender, EventArgs e)
        {
            int pos = ObjverConsulta.dgvInfoConsulta.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea eliminar a la consulta del DUI:\n {ObjverConsulta.dgvInfoConsulta[1, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOConsulta dAOConsulta = new DAOConsulta();
                dAOConsulta.Con_ID = int.Parse(ObjverConsulta.dgvInfoConsulta[0, pos].Value.ToString());
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
            int pos = ObjverConsulta.dgvInfoConsulta.CurrentRow.Index;

            AñadirConsulta openForm = new AñadirConsulta(
                2, // accion
                ObjverConsulta.dgvInfoConsulta[1, pos].Value.ToString(), // cli_DUI
                ObjverConsulta.dgvInfoConsulta[4, pos].Value.ToString(), // vis_ID
                DateTime.Parse(ObjverConsulta.dgvInfoConsulta[5, pos].Value.ToString()), // con_fechahora
                ObjverConsulta.dgvInfoConsulta[7, pos].Value.ToString(), // con_obser
                ObjverConsulta.dgvInfoConsulta[8, pos].Value.ToString(), // emp_ID
                int.Parse(ObjverConsulta.dgvInfoConsulta[0, pos].Value.ToString()),  // con_ID
                DateTime.Parse(ObjverConsulta.dgvInfoConsulta[6, pos].Value.ToString()), // con_fechahora
                bool.Parse(ObjverConsulta.dgvInfoConsulta[9, pos].Value.ToString()) // cmbEstado.Checked
            );

            openForm.cmbEstado.Checked = bool.Parse(ObjverConsulta.dgvInfoConsulta[9, pos].Value.ToString());

            openForm.ShowDialog();
            ActualizarDatosConsulta();
        }

    }
}
