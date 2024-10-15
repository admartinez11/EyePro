using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using OpticaMultivisual.Views.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.TipoArticulo
{
    internal class ControllerAdminTipoArt
    {
        ViewAdminTipoArt ObjVista;
        public ControllerAdminTipoArt(ViewAdminTipoArt Vista)
        {
            ObjVista = Vista;
            ObjVista.Load += new EventHandler(CargarInfo);
            ObjVista.btnBuscar.Click += new EventHandler(BuscarTIPOARTControlador);
            ObjVista.btnNuevoTipArt.Click += new EventHandler(AgregarTipArt);
            ObjVista.btnEliminarTipArt.Click += new EventHandler(EliminarTipArt);
            ObjVista.btnActTipArt.Click += new EventHandler(ActualizarTipArt);
        }
        public void CargarInfo(object sender, EventArgs e)
        {
            ActualizarDatos();
        }
        public void ActualizarDatos()
        {
            DAOTipoArticulo ObjRegistro = new DAOTipoArticulo();
            DataSet ds = ObjRegistro.ObtenerInfoTipoArticulo();
            ObjVista.dgvInfoTipoArticulo.DataSource = ds.Tables["VistaTipoArt"];
        }
        public void BuscarTIPOARTControlador(object sender, EventArgs e)
        {
            DAOTipoArticulo ObjRegistro = new DAOTipoArticulo();
            DataSet ds = ObjRegistro.BuscarTipoArticulo(ObjVista.txtBuscar.Text.Trim());
            ObjVista.dgvInfoTipoArticulo.DataSource = ds.Tables["VistaTipoArt"];
        }
        public void AgregarTipArt(object sender, EventArgs e)
        {
            ViewAddTipoArt openForm = new ViewAddTipoArt(1);
            openForm.ShowDialog();
            ActualizarDatos();
        }
        public void ActualizarTipArt(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoTipoArticulo.CurrentRow.Index;
            ViewAddTipoArt openForm = new ViewAddTipoArt(2,
            int.Parse(ObjVista.dgvInfoTipoArticulo[0, pos].Value.ToString()),// Codigo del articulo
            ObjVista.dgvInfoTipoArticulo[1, pos].Value.ToString(),// Nombre
            ObjVista.dgvInfoTipoArticulo[2, pos].Value.ToString() // Descripcion
            );

            openForm.ShowDialog();
            ActualizarDatos();
        }
        private void EliminarTipArt(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoTipoArticulo.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjVista.dgvInfoTipoArticulo[1, pos].Value.ToString()} {ObjVista.dgvInfoTipoArticulo[2, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOTipoArticulo daoDel = new DAOTipoArticulo();
                daoDel.Tipoart_ID = int.Parse(ObjVista.dgvInfoTipoArticulo[0, pos].Value.ToString());
                int valorRetornado = daoDel.EliminarMaterialTipoArticulo();
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
    }
}
