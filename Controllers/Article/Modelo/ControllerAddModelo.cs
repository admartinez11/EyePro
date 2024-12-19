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
    internal class ControllerAddModelo
    {
        ViewAddModelo ObjVistaR;
        private int accion;
        protected int modId;
        public string marca_id;
        public ControllerAddModelo(ViewAddModelo Vista, int accion)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            Vista.Load += new EventHandler(CargaInicial);
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgregarModelo.Click += new EventHandler(NuevoModelo);
            ObjVistaR.bunifuPictureBox2.Click += new EventHandler(AbrirPanelAuxiliar);
            ObjVistaR.toolStripButton1.Click += new EventHandler(CerrarPanelAuxiliar);
            ObjVistaR.btnGuardarMar.Click += new EventHandler(NuevaMarca);
        }
        void AbrirPanelAuxiliar(object sender, EventArgs e)
        {
            ObjVistaR.PanelAuxiliar.Visible = true;
        }

        void CerrarPanelAuxiliar(object sender, EventArgs e)
        {
            ObjVistaR.PanelAuxiliar.Visible = false;
        }
        public void CargaInicial(object sender, EventArgs e)
        {
            LlenarComboMarca();
        }
        void LlenarComboMarca()
        {
            DAOModelo daoFuncion = new DAOModelo();
            DataSet ds = daoFuncion.ObtenerMarca();
            ObjVistaR.cmbMarca.DataSource = ds.Tables["Marca"];
            ObjVistaR.cmbMarca.DisplayMember = "marca_nombre";
            ObjVistaR.cmbMarca.ValueMember = "marca_ID";
            if (accion == 2)
            {
                ObjVistaR.cmbMarca.Text = marca_id;
            }
        }
        public void NuevaMarca(object sender, EventArgs e)
        {
            DAOModelo DAOIngresarR = new DAOModelo
            {
                Marca_nombre = ObjVistaR.txtNombreMar.Text.Trim(),
            };

            int valorRetornado = DAOIngresarR.RegistrarMarca();

            if (valorRetornado == 1)
            {
                MessageBox.Show("Los datos han sido registrados exitosamente",
                                "Proceso completado",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
                LlenarComboMarca();

            }
            else
            {
                MessageBox.Show("EPV006 - No se pudieron registrar los datos",
                                "Proceso interrumpido",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        public void NuevoModelo(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOModelo DAOIngresarR = new DAOModelo
                {
                    Mod_nombre = ObjVistaR.txtModeloNombre.Text.Trim(),
                    Marca_ID = (int)ObjVistaR.cmbMarca.SelectedValue,
                };

                int valorRetornado = DAOIngresarR.RegistrarModelo();

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
            if (string.IsNullOrEmpty(ObjVistaR.txtModeloNombre.Text.Trim()) )
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ObjVistaR.txtModeloNombre.Text.Length > 100)
            {
                MessageBox.Show("El campo de Nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ObjVistaR.cmbMarca.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un Tipo de Articulo.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }
        public ControllerAddModelo(ViewAddModelo Vista, int p_accion, int mod_ID, string Mod_nombre, string Marca_ID)
        {
            // Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;
            this.modId = mod_ID;
            this.marca_id = Marca_ID;
            Vista.Load += new EventHandler(CargaInicial);
            // Verificar la acción a realizar
            verificarAccion();
            // Cargar los valores en la articulo
            Cargarvalores(Mod_nombre);
            // Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizarModelo.Click += new EventHandler(ActualizarRegistro);
            // ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);
        }
        public void Cargarvalores(string Mod_nombre)
        {
            try
            {
                // Asignación correcta
                ObjVistaR.txtModeloNombre.Text = Mod_nombre; // Campo para DUI
            }
            catch (Exception ex)
            {
                // Muestra el mensaje de error si ocurre una excepción
                MessageBox.Show($"{ex.Message}", "Error al cargar valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ActualizarRegistro(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOModelo DAOActualizar = new DAOModelo();
                DAOActualizar.Mod_ID = modId;
                DAOActualizar.Mod_nombre = ObjVistaR.txtModeloNombre.Text.Trim();
                DAOActualizar.Marca_ID = (int)ObjVistaR.cmbMarca.SelectedValue;
                int valorRetornado = DAOActualizar.ActualizarModelo();
                if (valorRetornado > 0)
                {
                    MessageBox.Show("Los datos han sido actualizados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjVistaR.Close();

                }
                else if (valorRetornado == 0)
                {
                    MessageBox.Show("EPV004 - No se encontraron los datos del registro",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error: EPV001 - Error inesperado",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }
        public void verificarAccion()
        {
            if (accion == 1)
            {
                ObjVistaR.btnAgregarModelo.Enabled = true;
                ObjVistaR.btnActualizarModelo.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjVistaR.btnAgregarModelo.Enabled = false;
                ObjVistaR.btnActualizarModelo.Enabled = true;
            }
        }
    }
}
