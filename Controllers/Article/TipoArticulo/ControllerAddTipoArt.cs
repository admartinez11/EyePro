using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using OpticaMultivisual.Views.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.TipoArticulo
{
    internal class ControllerAddTipoArt
    {
        ViewAddTipoArt ObjVistaR;
        private int accion;
        protected int tipoartID;

        public ControllerAddTipoArt(ViewAddTipoArt Vista, int accion)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgregarTipArt.Click += new EventHandler(NuevaVisita);
        }
        public void NuevaVisita(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOTipoArticulo DAOIngresarR = new DAOTipoArticulo
                {
                    Tipoart_nombre = ObjVistaR.txtTipArNombre.Text.Trim(),
                    Tipoart_descripcion = ObjVistaR.txtDescTipArt.Text.Trim(),
                };

                int valorRetornado = DAOIngresarR.RegistrarTipoArticulo();

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

            if (string.IsNullOrEmpty(ObjVistaR.txtTipArNombre.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtDescTipArt.Text.Trim())
                )
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ObjVistaR.txtTipArNombre.Text.Length > 100)
            {
                MessageBox.Show("El campo de Nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtDescTipArt.Text.Length > 100)
            {
                MessageBox.Show("El campo de Apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }

        public ControllerAddTipoArt(ViewAddTipoArt Vista, int p_accion, int Tipoart_ID, string Tipoart_nombre, string Tipoart_descripcion)
        {
            // Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;
            this.tipoartID = Tipoart_ID;
            // Verificar la acción a realizar
            verificarAccion();
            // Cargar los valores en la articulo
            Cargarvalores(Tipoart_nombre, Tipoart_descripcion);
            // Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizarTipArt.Click += new EventHandler(ActualizarRegistro);
            // ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);
        }
        public void Cargarvalores(string Tipoart_nombre, string Tipoart_descripcion)
        {
            try
            {
                // Asignación correcta
                ObjVistaR.txtTipArNombre.Text = Tipoart_nombre; // Campo para DUI
                ObjVistaR.txtDescTipArt.Text = Tipoart_descripcion; // Campo para Teléfono
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
                DAOTipoArticulo dAOTipoArticulo = new DAOTipoArticulo();
                dAOTipoArticulo.Tipoart_ID = tipoartID;
                dAOTipoArticulo.Tipoart_nombre = ObjVistaR.txtTipArNombre.Text.Trim();
                dAOTipoArticulo.Tipoart_descripcion = ObjVistaR.txtDescTipArt.Text.Trim();
                int valorRetornado = dAOTipoArticulo.ActualizarTipoArticulo();
                MessageBox.Show($"{valorRetornado}", "Error al cargar valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (valorRetornado == 1)
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
                ObjVistaR.btnAgregarTipArt.Enabled = true;
                ObjVistaR.btnActualizarTipArt.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjVistaR.btnAgregarTipArt.Enabled = false;
                ObjVistaR.btnActualizarTipArt.Enabled = true;
            }
        }
    }
}
