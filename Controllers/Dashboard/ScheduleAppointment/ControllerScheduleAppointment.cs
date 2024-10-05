using OpticaMultivisual.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Views;
using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Views.ScheduleAppointment;
using static OpticaMultivisual.Models.DAO.DAOScheduleAppointment;
using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Models.DAO;
using System.Text.RegularExpressions;

namespace OpticaMultivisual.Controllers.ScheduleAppointment
{
    class ControllerScheduleAppointment
    {
        ViewScheduleAppointment ObjVistaR;
        private int accion;

        public ControllerScheduleAppointment(ViewScheduleAppointment Vista, int accion)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgendar.Click += new EventHandler(NuevaVisita);
        }
        public void NuevaVisita(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAOScheduleAppointment DAOIngresarR = new DAOScheduleAppointment
                {
                    Vis_dui = ObjVistaR.txtCiDUI.Text.Trim(),
                    Vis_tel = ObjVistaR.txtCiTel.Text.Trim(),
                    Vis_obser = ObjVistaR.txtCiObs.Text.Trim(),
                    Vis_fcita = ObjVistaR.DTPfechacita.Value,
                    Vis_nombre = ObjVistaR.txtCiNombre.Text.Trim(),
                    Vis_apellido = ObjVistaR.txtCiApellido.Text.Trim(),
                    Vis_correo = ObjVistaR.txtCiCorreo.Text.Trim(),
                };

                int valorRetornado = DAOIngresarR.RegistrarVisita();

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

            if (string.IsNullOrEmpty(ObjVistaR.txtCiNombre.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtCiApellido.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtCiObs.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtCiDUI.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtCiTel.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtCiCorreo.Text.Trim())
                )
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DateTime Vis_fcita = ObjVistaR.DTPfechacita.Value;
            if (!ValidarFechaSeleccionada(Vis_fcita))
            {
                MessageBox.Show("El campo Fecha solo puede contener fechas validas", "Validación de Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string telefono = ObjVistaR.txtCiTel.Text.Trim();
            if (!EsTelValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener un guion (-).", "Validación de Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string DUI = ObjVistaR.txtCiDUI.Text.Trim();
            if (!EsDUIValido(DUI))
            {
                MessageBox.Show("El campo DUI solo puede contener números y un solo guion.", "Validación de DUI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string correo = ObjVistaR.txtCiCorreo.Text.Trim();
            if (!EsCorreoValido(correo))
            {
                MessageBox.Show("El campo Correo Electrónico no tiene un formato válido.", "Validación de Correo Electrónico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ObjVistaR.txtCiNombre.Text.Length > 100)
            {
                MessageBox.Show("El campo de Nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtCiApellido.Text.Length > 100)
            {
                MessageBox.Show("El campo de Apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtCiTel.Text.Length > 25)
            {
                MessageBox.Show("El campo de Telefóno no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtCiCorreo.Text.Length > 100)
            {
                MessageBox.Show("El campo de Correo no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtCiObs.Text.Length > 200)
            {
                MessageBox.Show("El campo de Motivo no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtCiDUI.Text.Length > 10)
            {
                MessageBox.Show("El campo de DUI no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DateTime vis_fcita = ObjVistaR.DTPfechacita.Value;
            if (!ValidarFechaFutura(vis_fcita))
            {
                MessageBox.Show("El campo Fecha debe ser una fecha futura.", "Validación de Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private bool EsCorreoValido(string correo)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }
        private bool EsDUIValido(string dui)
        {
            // Expresión regular para validar el formato del DUI: 8 dígitos seguidos de un guion y un dígito.
            string patronDUI = @"^\d{8}-\d{1}$";

            // Verifica si el DUI ingresado cumple con el formato usando una expresión regular
            if (System.Text.RegularExpressions.Regex.IsMatch(dui, patronDUI))
            {
                return true; // El formato es válido
            }
            else
            {
                MessageBox.Show("El DUI ingresado no es válido. Debe tener el formato 12345678-9.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // El formato es inválido
            }
        }
        private bool EsTelValido(string telefono)
        {
            if (!telefono.Contains("-"))
            {
                return false;
            }
            return true;
        }
        private bool ValidarFechaSeleccionada(DateTime fecha)
        {
            if (fecha == DateTime.MinValue)
            {
                MessageBox.Show("Debe seleccionar una fecha.");
                return false;
            }
            return true;
        }
        private bool ValidarFechaFutura(DateTime fecha)
        {
            if (fecha < DateTime.Now.Date)
            {
                MessageBox.Show("La fecha seleccionada debe ser posterior a la fecha actual.");
                return false;
            }
            return true;
        }
        public ControllerScheduleAppointment(ViewScheduleAppointment Vista, int p_accion, DateTime Vis_fcita, string Vis_nombre, string Vis_apellido, string Vis_tel, string Vis_correo, string Vis_dui, string Vis_obser)
        {
            // Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;

            // Verificar la acción a realizar
            verificarAccion();

            // Cargar los valores en la vista
            Cargarvalores(Vis_fcita, Vis_nombre, Vis_apellido, Vis_tel, Vis_correo, Vis_dui, Vis_obser);

            // Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizar.Click += new EventHandler(ActualizarRegistro);
            // ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);
        }

        public void Cargarvalores(DateTime Vis_fcita, string Vis_nombre, string Vis_apellido, string Vis_tel, string Vis_correo, string Vis_dui, string Vis_obser)
        {
            try
            {
                // Asignación correcta
                ObjVistaR.txtCiDUI.Text = Vis_obser; // Campo para DUI
                ObjVistaR.txtCiTel.Text = Vis_tel; // Campo para Teléfono
                ObjVistaR.txtCiObs.Text = Vis_dui; // Campo para Observaciones
                ObjVistaR.DTPfechacita.Value = Vis_fcita; // Control DateTimePicker para Fecha
                ObjVistaR.txtCiNombre.Text = Vis_nombre; // Campo para Nombre
                ObjVistaR.txtCiApellido.Text = Vis_apellido; // Campo para Apellido
                ObjVistaR.txtCiCorreo.Text = Vis_correo; // Campo para Correo
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
                    DAOScheduleAppointment DAOActualizar = new DAOScheduleAppointment
                    {
                        Vis_dui = ObjVistaR.txtCiDUI.Text.Trim(),
                        Vis_tel = ObjVistaR.txtCiTel.Text.Trim(),
                        Vis_obser = ObjVistaR.txtCiObs.Text.Trim(),
                        Vis_fcita = ObjVistaR.DTPfechacita.Value,
                        Vis_nombre = ObjVistaR.txtCiNombre.Text.Trim(),
                        Vis_apellido = ObjVistaR.txtCiApellido.Text.Trim(),
                        Vis_correo = ObjVistaR.txtCiCorreo.Text.Trim()
                    };

                    int valorRetornado = DAOActualizar.ActualizarVisita();
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
                ObjVistaR.btnAgendar.Enabled = true;
                ObjVistaR.btnActualizar.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjVistaR.btnAgendar.Enabled = false;
                ObjVistaR.btnActualizar.Enabled = true;
            }
        }
    }
}
