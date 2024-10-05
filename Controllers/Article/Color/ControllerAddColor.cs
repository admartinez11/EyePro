using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.Color;
using OpticaMultivisual.Views.Dashboard.Article.TipoArticulo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.Color
{
    internal class ControllerAddColor
    {

        ViewAddColor ObjVistaR;
        private int accion;
        protected int Colorid;
        public ControllerAddColor(ViewAddColor Vista, int accion)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgregarColor.Click += new EventHandler(NuevaVisita);
        }
        public void NuevaVisita(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOColor DAOIngresarR = new DAOColor
                {
                    Color_nombre = ObjVistaR.txtColorNombre.Text.Trim(),
                    Color_descripcion = ObjVistaR.txtDescColor.Text.Trim(),
                };

                int valorRetornado = DAOIngresarR.RegistrarColor();

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

            if (string.IsNullOrEmpty(ObjVistaR.txtColorNombre.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtDescColor.Text.Trim())
                )
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ObjVistaR.txtColorNombre.Text.Length > 100)
            {
                MessageBox.Show("El campo de Nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtDescColor.Text.Length > 100)
            {
                MessageBox.Show("El campo de Apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }
        public ControllerAddColor(ViewAddColor Vista, int p_accion, int Color_ID, string Color_nombre, string Color_descripcion)
        {
            // Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;
            this.Colorid = Color_ID;
            // Verificar la acción a realizar
            verificarAccion();
            // Cargar los valores en la articulo
            Cargarvalores(Color_nombre, Color_descripcion);
            // Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizarColor.Click += new EventHandler(ActualizarRegistro);
            // ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);
        }
        public void Cargarvalores(string Color_nombre, string Color_descripcion)
        {
            try
            {
                // Asignación correcta
                ObjVistaR.txtColorNombre.Text = Color_nombre; // Campo para DUI
                ObjVistaR.txtDescColor.Text = Color_descripcion; // Campo para Teléfono
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
                DAOColor DAOActualizar = new DAOColor();
                DAOActualizar.Color_ID = Colorid;
                DAOActualizar.Color_nombre = ObjVistaR.txtColorNombre.Text.Trim();
                DAOActualizar.Color_descripcion = ObjVistaR.txtDescColor.Text.Trim();
                int valorRetornado = DAOActualizar.ActualizarColor();
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
                ObjVistaR.btnAgregarColor.Enabled = true;
                ObjVistaR.btnActualizarColor.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjVistaR.btnAgregarColor.Enabled = false;
                ObjVistaR.btnActualizarColor.Enabled = true;
            }
        }
    }
}
