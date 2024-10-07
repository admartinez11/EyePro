using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.Color;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.Color
{
    internal class ControllerAdminColor
    {
        ViewAdminColor ObjVista;

        public ControllerAdminColor(ViewAdminColor Vista)
        {
            ObjVista = Vista;
            ObjVista.Load += new EventHandler(CargarInfo);
            ObjVista.btnBuscar.Click += new EventHandler(BuscarColor);
            ObjVista.btnNuevoColor.Click += new EventHandler(AgregarColor);
            ObjVista.btnEliminarColor.Click += new EventHandler(EliminarColor);
            ObjVista.btnActColor.Click += new EventHandler(ActualizarColor);
        }
        public void CargarInfo(object sender, EventArgs e)
        {
            ActualizarDatos();
        }
        public void ActualizarDatos()
        {
            DAOColor ObjRegistro = new DAOColor();
            DataSet ds = ObjRegistro.ObtenerInfoColor();
            ObjVista.dgvInfoColor.DataSource = ds.Tables["VistaColor"];
        }
        public void BuscarColor(object sender, EventArgs e)
        {
            DAOColor ObjRegistro = new DAOColor();
            DataSet ds = ObjRegistro.BuscarColor(ObjVista.txtBuscar.Text.Trim());
            ObjVista.dgvInfoColor.DataSource = ds.Tables["VistaColor"];
        }
        public void AgregarColor(object sender, EventArgs e)
        {
            ViewAddColor openForm = new ViewAddColor(1);
            openForm.ShowDialog();
            ActualizarDatos();
        }
        public void ActualizarColor(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoColor.CurrentRow.Index;
            ViewAddColor openForm = new ViewAddColor(2,
            int.Parse(ObjVista.dgvInfoColor[0, pos].Value.ToString()),//Id del tipo de articulo
            ObjVista.dgvInfoColor[1, pos].Value.ToString(),        // Nombre
            ObjVista.dgvInfoColor[2, pos].Value.ToString()     // descripcion
            );

            openForm.ShowDialog();
            ActualizarDatos();
        }
        private void EliminarColor(object sender, EventArgs e)
        {
            int pos = ObjVista.dgvInfoColor.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjVista.dgvInfoColor[1, pos].Value.ToString()} {ObjVista.dgvInfoColor[2, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOColor daoDel = new DAOColor();
                daoDel.Color_ID = int.Parse(ObjVista.dgvInfoColor[0, pos].Value.ToString());
                int valorRetornado = daoDel.EliminarColor();
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
