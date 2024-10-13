using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Optometrista;
using OpticaMultivisual.Views.Dashboard.PedidoDet;
using OpticaMultivisual.Views.ScheduleAppointment;
using static System.Net.Mime.MediaTypeNames;

namespace OpticaMultivisual.Controllers.Dashboard.PedidoDetalle
{
    class ControllerAdminPD
    {
        ViewPedidoDet ObjAdminPD;
        public int accion;

        public ControllerAdminPD(ViewPedidoDet Vista)
        {
            ObjAdminPD = Vista;
            ObjAdminPD.Load += new EventHandler(LoadData);
            //Evento click de botón
            ObjAdminPD.btnAgregarCliente.Click += new EventHandler(NewPD);
            ObjAdminPD.btnActualizar.Click += new EventHandler(UpdatePD);
            ObjAdminPD.btnBuscar.Click += new EventHandler(SearchRegister);
        }

        public void LoadData(object sender, EventArgs e)
        {
            RefrescarData();
        }

        //DataGridView
        public void RefrescarData()
        {
            //Objeto de la clase DAOPedidoDet
            DAOPedidoDet objAdmin = new DAOPedidoDet();
            //Declarando nuevo DataSet para que obtenga los datos del metodo ObtenerDatos
            DataSet ds = objAdmin.ObtenerDatos();
            //Llenar DataGridView
            ObjAdminPD.dgvPD.DataSource = ds.Tables["ViewPedidoDet"];
        }

        private void NewPD(object sender, EventArgs e)
        {
            /*Se invoca al formulario ViewAddPD y se le envía un numero, este numero servirá para indicarle que tipo de acción se quiere realizar, donde 1 significa Inserción y 2 significa Actualización*/
            ViewAddPedidoDet openForm = new ViewAddPedidoDet(1);
            //Se muestra el formulario
            openForm.ShowDialog();
            //Cuando el formulario ha sido cerrado se procede a refrescar el DataGrid para que se puedan observar los nuevo datos ingresados o actualizados.
            RefrescarData();
        }

        public ControllerAdminPD(ViewPedidoDet frmDR, int accion)
        {
            ObjAdminPD = frmDR;
            this.accion = accion;
            ObjAdminPD.Load += new EventHandler(LoadData);
            ObjAdminPD.btnEliminar.Click += new EventHandler(DeleteRegister);
            ObjAdminPD.btnBuscar.Click += new EventHandler(SearchRegister);
        }

        public void SearchRegister(object sender, EventArgs e)
        {
            DAOPedidoDet ObjADDPD = new DAOPedidoDet();
            DataSet ds = ObjADDPD.BuscarDUI2(ObjAdminPD.txtBuscar.Text.Trim());
            ObjAdminPD.dgvPD.DataSource = ds.Tables["ViewPedidoDet"];
        }

        public void DeleteRegister(object sender, EventArgs e)
        {
            int pos = ObjAdminPD.dgvPD.CurrentRow.Index;
            //Se verifica el valor que retornó el metodo anterior y que fue almacenado en la variable valorRetornado
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjAdminPD.dgvPD[0, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOPedidoDet daopd = new DAOPedidoDet();
                daopd.pd_ID1 = int.Parse(ObjAdminPD.dgvPD[0, pos].Value.ToString());
                int valorRetornado = daopd.EliminarPD();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Registro eliminado", "Acción completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefrescarData();
                }
                else
                {
                    MessageBox.Show("EPV003 - Los datos no pudieron ser eliminados", "Acción interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdatePD(object sender, EventArgs e)
        {
            int pos = ObjAdminPD.dgvPD.CurrentRow.Index;
            int pd_ID, art_cant, pd_recetalab;
            string con_ID, art_codigo, pd_obser;
            DateTime pd_fpedido, pd_fprogramada;

            pd_ID = int.Parse(ObjAdminPD.dgvPD[0, pos].Value.ToString());
            con_ID = ObjAdminPD.dgvPD[1, pos].Value.ToString();
            MessageBox.Show($"{con_ID}");
            pd_fpedido = DateTime.Parse(ObjAdminPD.dgvPD[2, pos].Value.ToString());
            pd_fprogramada = DateTime.Parse(ObjAdminPD.dgvPD[3, pos].Value.ToString());
            art_codigo = ObjAdminPD.dgvPD[4, pos].Value.ToString();
            art_cant = int.Parse(ObjAdminPD.dgvPD[5, pos].Value.ToString());
            pd_obser = ObjAdminPD.dgvPD[6, pos].Value.ToString();
            pd_recetalab = int.Parse(ObjAdminPD.dgvPD[7, pos].Value.ToString());

            ViewAddPedidoDet openForm = new ViewAddPedidoDet(2, pd_ID, con_ID, pd_fpedido, pd_fprogramada, art_codigo, art_cant, pd_obser, pd_recetalab);

            openForm.ShowDialog();
            RefrescarData();
        }
    }
}
