using OpticaMultivisual.Controllers.Article.Material;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.Color;
using OpticaMultivisual.Views.Dashboard.Article.Material;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.Material
{
    internal class ControllerAdminMaterial
    {
        ViewAdminMaterial ObjVista;

        public ControllerAdminMaterial(ViewAdminMaterial Vista)
        {
            ObjVista = Vista;
            ObjVista.Load += new EventHandler(CargarInfo);
            ObjVista.btnBuscar.Click += new EventHandler(BuscarMaterial);
            ObjVista.btnNuevoArt.Click += new EventHandler(AgregarMaterial);
            ObjVista.btnEliminarArt.Click += new EventHandler(EliminarMaterial);
            ObjVista.btnActArt.Click += new EventHandler(ActualizarMaterial);
        }
        public void CargarInfo(object sender, EventArgs e)
        {
            ActualizarDatos();
        }
        public void ActualizarDatos()
        {
            DAOMaterial ObjRegistro = new DAOMaterial();
            DataSet ds = ObjRegistro.ObtenerInfoMaterial();
            ObjVista.dgvInfoArticulo.DataSource = ds.Tables["vistaMaterial"];
        }
        public void BuscarMaterial(object sender, EventArgs e)
        {
            DAOMaterial ObjRegistro = new DAOMaterial();
            DataSet ds = ObjRegistro.BuscarMaterial(ObjVista.txtBuscar.Text.Trim());
            ObjVista.dgvInfoArticulo.DataSource = ds.Tables["vistaMaterial"];
        }
        public void AgregarMaterial(object sender, EventArgs e)
        {
            ViewAddMaterial openForm = new ViewAddMaterial(1);
            openForm.ShowDialog();
            ActualizarDatos();
        }
        public void ActualizarMaterial(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoArticulo.CurrentRow.Index;
            ViewAddMaterial openForm = new ViewAddMaterial(2,
            int.Parse(ObjVista.dgvInfoArticulo[0, pos].Value.ToString()),//Id del tipo de articulo
            ObjVista.dgvInfoArticulo[1, pos].Value.ToString(),        // Nombre
            ObjVista.dgvInfoArticulo[2, pos].Value.ToString()     // descripcion
            );

            openForm.ShowDialog();
            ActualizarDatos();
        }
        private void EliminarMaterial(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoArticulo.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjVista.dgvInfoArticulo[0, pos].Value.ToString()} {ObjVista.dgvInfoArticulo[1, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOMaterial daoDel = new DAOMaterial();
                daoDel.Material_ID = int.Parse(ObjVista.dgvInfoArticulo[0, pos].Value.ToString());
                int valorRetornado = daoDel.EliminarMaterial();
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
