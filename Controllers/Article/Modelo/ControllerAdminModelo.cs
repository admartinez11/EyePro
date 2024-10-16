using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.Modelo;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.Modelo
{
    internal class ControllerAdminModelo
    {
        ViewAdminModelo ObjVista;
        public ControllerAdminModelo(ViewAdminModelo Vista)
        {
            ObjVista = Vista;
            ObjVista.Load += new EventHandler(CargarInfo);
            ObjVista.btnBuscar.Click += new EventHandler(BuscarModeloControlador);
            ObjVista.btnNuevoModelo.Click += new EventHandler(AgregarModelo);
            ObjVista.btnEliminarModelo.Click += new EventHandler(EliminarModelo);
            ObjVista.btnActModelo.Click += new EventHandler(ActualizarModelo);
        }
        public void CargarInfo(object sender, EventArgs e)
        {
            ActualizarDatos();
        }
        public void ActualizarDatos()
        {
            DAOModelo ObjRegistro = new DAOModelo();
            DataSet ds = ObjRegistro.ObtenerInfoModelo();
            ObjVista.dgvInfoModelo.DataSource = ds.Tables["VistaModelo"];
        }
        public void BuscarModeloControlador(object sender, EventArgs e)
        {
            DAOModelo ObjRegistro = new DAOModelo();
            DataSet ds = ObjRegistro.BuscarModelo(ObjVista.txtBuscar.Text.Trim());
            ObjVista.dgvInfoModelo.DataSource = ds.Tables["VistaModelo"];
        }
        public void AgregarModelo(object sender, EventArgs e)
        {
            ViewAddModelo openForm = new ViewAddModelo(1);
            openForm.ShowDialog();
            ActualizarDatos();
        }
        public void ActualizarModelo(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoModelo.CurrentRow.Index;
            ViewAddModelo openForm = new ViewAddModelo(2,
            int.Parse(ObjVista.dgvInfoModelo[0, pos].Value.ToString()),//Id del tipo de articulo
            ObjVista.dgvInfoModelo[1, pos].Value.ToString(),        // Nombre
            ObjVista.dgvInfoModelo[2, pos].Value.ToString()     // descripcion
            );

            openForm.ShowDialog();
            ActualizarDatos();
        }
        private void EliminarModelo(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoModelo.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjVista.dgvInfoModelo[1, pos].Value.ToString()} {ObjVista.dgvInfoModelo[2, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOModelo daoDel = new DAOModelo();
                daoDel.Mod_ID = int.Parse(ObjVista.dgvInfoModelo[0, pos].Value.ToString());
                int valorRetornado = daoDel.EliminarModelo();
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
