using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.MateralTipoArt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.MaterialTipoArt
{
    internal class ControllerAdMaterialTipoArt
    {
        AddMaterialTipoArt ObjVistaR;
        protected int mattipoartID;

        public ControllerAdMaterialTipoArt(AddMaterialTipoArt Vista)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            Vista.Load += new EventHandler(CargaInicial);
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgendarMaterial.Click += new EventHandler(NuevoArticulo);
        }
        public void CargaInicial(object sender, EventArgs e)
        {
            LlenarComboTipoArticulo();
            LlenarComboMaterial();
        }
        void LlenarComboTipoArticulo()
        {
            DAOMaterialTipoArticulo daoFuncion = new DAOMaterialTipoArticulo();
            DataSet ds = daoFuncion.ObtenerTipoArticulo();
            ObjVistaR.cmbTipoart.DataSource = ds.Tables["TipoArt"];
            ObjVistaR.cmbTipoart.DisplayMember = "tipoart_nombre";
            ObjVistaR.cmbTipoart.ValueMember = "tipoart_ID";
        }
        void LlenarComboMaterial()
        {
            DAOMaterialTipoArticulo daoFuncion = new DAOMaterialTipoArticulo();
            DataSet ds = daoFuncion.ObtenerMaterial();
            ObjVistaR.cmbMaterial.DataSource = ds.Tables["Material"];
            ObjVistaR.cmbMaterial.DisplayMember = "material_nombre";
            ObjVistaR.cmbMaterial.ValueMember = "material_ID";
        }
        public void NuevoArticulo(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOMaterialTipoArticulo DAOIngresarR = new DAOMaterialTipoArticulo
                {
                    Tipoart_ID = (int)ObjVistaR.cmbTipoart.SelectedValue,
                    Material_ID = (int)ObjVistaR.cmbMaterial.SelectedValue,
                };

                int valorRetornado = DAOIngresarR.RegistrarMaterialTipoArticulo();

                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjVistaR.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - No se pudieron registrar los datos",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidarCampos()
        {
            if (ObjVistaR.cmbMaterial.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un Modelo.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ObjVistaR.cmbTipoart.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un Tipo de Articulo.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }
    }
}
