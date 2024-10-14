using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Optometrista;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Dashboard.Optometrista
{
    class ControllerAdminDR
    {
        ViewRecetaBase ObjAdminDR;
        public int accion;


        public ControllerAdminDR(ViewRecetaBase Vista)
        {
            ObjAdminDR = Vista;
            ObjAdminDR.Load += new EventHandler(LoadData);
            //Evento click de botón
            ObjAdminDR.btnAgregar.Click += new EventHandler(NewDR);
            ObjAdminDR.btnEditar.Click += new EventHandler(UpdateDR);
            ObjAdminDR.btnSiguiente.Click += new EventHandler(Next);
            ObjAdminDR.btnBuscar.Click += new EventHandler(SearchRegister);
        }

        public void Next(object sender, EventArgs e)
        {
            ViewLens viewLens = new ViewLens();
            viewLens.ShowDialog();
        }

        public void LoadData(object sender, EventArgs e)
        {
            RefrescarData();
        }

        //DataGridView
        public void RefrescarData()
        {
            //Objeto de la clase DAOAdminDR
            DAO_DR objAdmin = new DAO_DR();
            //Declarando nuevo DataSet para que obtenga los datos del metodo ObtenerPersonas
            DataSet ds = objAdmin.ObtenerPersonas();
            //Llenar DataGridView
            ObjAdminDR.dgvDR.DataSource = ds.Tables["ViewDR"];
            ObjAdminDR.dgvDR.Columns[0].Visible = true;
        }

        private void NewDR(object sender, EventArgs e)
        {
            /*Se invoca al formulario ViewAddDR y se le envía un numero, este numero servirá para indicarle que tipo de acción se quiere realizar, donde 1 significa Inserción y 2 significa Actualización*/
            ViewAddDR openForm = new ViewAddDR(1);
            //Se muestra el formulario
            openForm.ShowDialog();
            //Cuando el formulario ha sido cerrado se procede a refrescar el DataGrid para que se puedan observar los nuevo datos ingresados o actualizados.
            RefrescarData();
        }

        public ControllerAdminDR(ViewRecetaBase frmDR, int accion)
        {
            ObjAdminDR = frmDR;
            this.accion = accion;
            ObjAdminDR.Load += new EventHandler(LoadData);
            ObjAdminDR.btnEliminar.Click += new EventHandler(DeleteRegister);
            ObjAdminDR.btnBuscar.Click += new EventHandler(SearchRegister);
        }

        public void SearchRegister(object sender, EventArgs e)
        {
            DAO_DR ObjADDDR = new DAO_DR();
            DataSet ds = ObjADDDR.BuscarDUI2(ObjAdminDR.txtBuscar.Text.Trim());
            ObjAdminDR.dgvDR.DataSource = ds.Tables["ViewDR"];
        } 

        public void DeleteRegister(object sender, EventArgs e)
        {
            int pos = ObjAdminDR.dgvDR.CurrentRow.Index;
            //Se verifica el valor que retornó el metodo anterior y que fue almacenado en la variable valorRetornado
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjAdminDR.dgvDR[0, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAO_DR daoDel = new DAO_DR();
                daoDel.DR_ID1 = int.Parse(ObjAdminDR.dgvDR[0, pos].Value.ToString());
                int valorRetornado = daoDel.EliminarDR();

                if (valorRetornado == 1)
                {
                    MessageBox.Show("Registro eliminado", "Acción completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefrescarData();
                }
                else
                {
                    MessageBox.Show("EPV003 - Los datos no pudieron ser elimandos.", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateDR(object sender, EventArgs e)
        {
            int pos = ObjAdminDR.dgvDR.CurrentRow.Index;

            int DR_ID; 
            string con_ID, OD_esfera, OD_prisma, OD_adicion, OD_AO, OI_prisma, OI_adicion, OI_AO, OD_cilindro, OI_esfera, OD_eje, OD_AP, OD_DP, OI_cilindro, OI_eje, OI_AP, OI_DP;

            DR_ID = int.Parse(ObjAdminDR.dgvDR[0, pos].Value.ToString());
            con_ID = ObjAdminDR.dgvDR[2, pos].Value.ToString();
            OD_esfera = ObjAdminDR.dgvDR[3, pos].Value.ToString();
            OD_cilindro = ObjAdminDR.dgvDR[4, pos].Value.ToString();
            OD_eje = ObjAdminDR.dgvDR[5, pos].Value.ToString();
            OD_prisma = ObjAdminDR.dgvDR[6, pos].Value.ToString();
            OD_adicion = ObjAdminDR.dgvDR[7, pos].Value.ToString();
            OD_DP = ObjAdminDR.dgvDR[8, pos].Value.ToString();
            OD_AO = ObjAdminDR.dgvDR[9, pos].Value.ToString();
            OD_AP = ObjAdminDR.dgvDR[10, pos].Value.ToString();

            OI_esfera = ObjAdminDR.dgvDR[11, pos].Value.ToString();
            OI_cilindro = ObjAdminDR.dgvDR[12, pos].Value.ToString();
            OI_eje = ObjAdminDR.dgvDR[13, pos].Value.ToString();
            OI_prisma = ObjAdminDR.dgvDR[14, pos].Value.ToString();
            OI_adicion = ObjAdminDR.dgvDR[15, pos].Value.ToString();
            OI_DP = ObjAdminDR.dgvDR[16, pos].Value.ToString();
            OI_AO = ObjAdminDR.dgvDR[17, pos].Value.ToString();
            OI_AP = ObjAdminDR.dgvDR[18, pos].Value.ToString();

            ViewAddDR openForm = new ViewAddDR(2, DR_ID, con_ID, OD_esfera, OD_cilindro, OD_eje, OD_prisma, OD_adicion, OD_AO, OD_AP, OD_DP, OI_esfera, OI_cilindro, OI_eje, OI_prisma, OI_adicion, OI_AO, OI_AP, OI_DP);

            openForm.ShowDialog();
            RefrescarData();
        }
    }
}
