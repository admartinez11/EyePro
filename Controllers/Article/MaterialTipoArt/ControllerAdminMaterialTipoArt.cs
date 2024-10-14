using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.MateralTipoArt;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.MaterialTipoArt
{
    internal class ControllerAdminMaterialTipoArt
    {
        ViewAddMaterialTipoArt ObjVista;
        public ControllerAdminMaterialTipoArt(ViewAddMaterialTipoArt Vista)
        {
            ObjVista = Vista;
            ObjVista.Load += new EventHandler(CargarInfo);
            ObjVista.btnNuevoArt.Click += new EventHandler(AgregarTipArt);
            ObjVista.btnEliminarArt.Click += new EventHandler(EliminarTipArt);
        }
        public void CargarInfo(object sender, EventArgs e)
        {
            ActualizarDatos();
        }
        public void ActualizarDatos()
        {
            DAOMaterialTipoArticulo ObjRegistro = new DAOMaterialTipoArticulo();
            DataSet ds = ObjRegistro.ObtenerInfoTipoArticulo();
            ObjVista.dgvInfoArticulo.DataSource = ds.Tables["VistaTipoArtMat"];
        }
        public void AgregarTipArt(object sender, EventArgs e)
        {
            AddMaterialTipoArt openForm = new AddMaterialTipoArt();
            openForm.ShowDialog();
            ActualizarDatos();
        }
        private void EliminarTipArt(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoArticulo.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjVista.dgvInfoArticulo[1, pos].Value.ToString()} {ObjVista.dgvInfoArticulo[2, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOMaterialTipoArticulo daoDel = new DAOMaterialTipoArticulo();
                daoDel.Materialtipoart_ID = int.Parse(ObjVista.dgvInfoArticulo[0, pos].Value.ToString());
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
