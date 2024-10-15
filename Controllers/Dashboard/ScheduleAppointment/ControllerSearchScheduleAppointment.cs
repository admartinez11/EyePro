using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Dashboard.ScheduleAppointment
{
    class ControllerSearchScheduleAppointment
    {
        ViewSearchScheduleAppointment ObjVista;

        public ControllerSearchScheduleAppointment(ViewSearchScheduleAppointment Vista)
        {
            ObjVista = Vista;
            ObjVista.Load += new EventHandler(CargarInfo);
            ObjVista.btnBuscar.Click += new EventHandler(BuscarVisitaControlador);
            ObjVista.btnNuevoVis.Click += new EventHandler(AgregarVisita);
            ObjVista.btnEliminarVis.Click += new EventHandler(EliminarVisita);
            ObjVista.btnActVis.Click += new EventHandler(ActualizarVisita);
            ObjVista.btnCliente.Click += new EventHandler(AñadirCliente);
        }

        public void CargarInfo(object sender, EventArgs e)
        {
            ActualizarDatos();
        }
        public void ActualizarDatos()
        {
            DAOScheduleAppointment ObjRegistro = new DAOScheduleAppointment();
            DataSet ds = ObjRegistro.ObtenerInfoVisita();
            ObjVista.dgvInfoVisita.DataSource = ds.Tables["ViewVisita"];

        }
        public void BuscarVisitaControlador(object sender, EventArgs e)
        {
            DAOScheduleAppointment Objadminregistro = new DAOScheduleAppointment();
            DataSet ds = Objadminregistro.BuscarVisita(ObjVista.txtBuscar.Text.Trim());
            ObjVista.dgvInfoVisita.DataSource = ds.Tables["ViewVisita"];
        }
        public void AgregarVisita(object sender, EventArgs e)
        {
            ViewScheduleAppointment openForm = new ViewScheduleAppointment(1);
            openForm.ShowDialog();
            ActualizarDatos();
        }
        private void EliminarVisita(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoVisita.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjVista.dgvInfoVisita[1, pos].Value.ToString()} {ObjVista.dgvInfoVisita[2, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOScheduleAppointment daoDel = new DAOScheduleAppointment();
                daoDel.Vis_dui = ObjVista.dgvInfoVisita[7, pos].Value.ToString();
                int valorRetornado = daoDel.EliminarVisita();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Registro eliminado", "Acción completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ActualizarDatos();
                }
                else
                {
                    MessageBox.Show("EPV003 - Los datos no pudieron ser eliminados", "Acción interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public void ActualizarVisita(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoVisita.CurrentRow.Index;
            ViewScheduleAppointment openForm = new ViewScheduleAppointment(2,
            DateTime.Parse(ObjVista.dgvInfoVisita[1, pos].Value.ToString()),//fecha visita
            ObjVista.dgvInfoVisita[2, pos].Value.ToString(),        // Nombre
            ObjVista.dgvInfoVisita[3, pos].Value.ToString(),     // Apellido
            ObjVista.dgvInfoVisita[4, pos].Value.ToString(),   // Teléfono
            ObjVista.dgvInfoVisita[5, pos].Value.ToString(),   // Correo
            ObjVista.dgvInfoVisita[6, pos].Value.ToString(),   // Observaciones
            ObjVista.dgvInfoVisita[7, pos].Value.ToString() // DUI
            );

            openForm.ShowDialog();
            ActualizarDatos();
        }
        public void AñadirCliente(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoVisita.CurrentRow.Index;
            RegistroClientes openForm = new RegistroClientes(1,
            ObjVista.dgvInfoVisita[2, pos].Value.ToString(),        // Nombre
            ObjVista.dgvInfoVisita[3, pos].Value.ToString(),     // Apellido
            ObjVista.dgvInfoVisita[4, pos].Value.ToString(),   // Teléfono
            ObjVista.dgvInfoVisita[5, pos].Value.ToString(),   // Correo
            ObjVista.dgvInfoVisita[7, pos].Value.ToString() // DUI
            );

            openForm.ShowDialog();
            ActualizarDatos();
        }
    }
}
