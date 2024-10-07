using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard.Article.Color;
using OpticaMultivisual.Views.Dashboard.Article.Material;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Article.Material
{
    internal class ControllerADDMaterial
    {

        ViewAddMaterial ObjVistaR;
        private int accion;
        protected int Materialid;
        public ControllerADDMaterial(ViewAddMaterial Vista, int accion)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgendarMaterial.Click += new EventHandler(NuevoMaterial);
        }
        public void NuevoMaterial(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOMaterial DAOIngresarR = new DAOMaterial
                {
                    Material_nombre = ObjVistaR.txtArNombre.Text.Trim(),
                    Material_descripcion = ObjVistaR.txtDescArt.Text.Trim(),
                };

                int valorRetornado = DAOIngresarR.RegistrarMaterial();

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

            if (string.IsNullOrEmpty(ObjVistaR.txtArNombre.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtDescArt.Text.Trim())
                )
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ObjVistaR.txtArNombre.Text.Length > 100)
            {
                MessageBox.Show("El campo de Nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtDescArt.Text.Length > 100)
            {
                MessageBox.Show("El campo de Apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }
        public ControllerADDMaterial(ViewAddMaterial Vista, int p_accion, int Material_ID, string Material_nombre, string Material_descripcion)
        {
            // Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;
            this.Materialid = Material_ID;
            // Verificar la acción a realizar
            verificarAccion();
            // Cargar los valores en la articulo
            Cargarvalores(Material_nombre, Material_descripcion);
            // Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizarArt.Click += new EventHandler(ActualizarRegistro);
            // ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);
        }
        public void Cargarvalores(string Material_nombre, string Material_descripcion)
        {
            try
            {
                // Asignación correcta
                ObjVistaR.txtArNombre.Text = Material_nombre; // Campo para DUI
                ObjVistaR.txtDescArt.Text = Material_descripcion; // Campo para Teléfono
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
                DAOMaterial DAOActualizar = new DAOMaterial();
                DAOActualizar.Material_ID = Materialid;
                DAOActualizar.Material_nombre = ObjVistaR.txtArNombre.Text.Trim();
                DAOActualizar.Material_descripcion = ObjVistaR.txtDescArt.Text.Trim();
                int valorRetornado = DAOActualizar.ActualizarMaterial();
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
                ObjVistaR.btnAgendarMaterial.Enabled = true;
                ObjVistaR.btnActualizarArt.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjVistaR.btnAgendarMaterial.Enabled = false;
                ObjVistaR.btnActualizarArt.Enabled = true;
            }
        }
    }
}
