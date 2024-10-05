using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net;
using OpticaMultivisual.Views.Login;
using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DTO;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace OpticaMultivisual.Controllers.Dashboard
{
    // Controlador para la vista ViewAddUser
    // Controlador para la vista ViewAddUser
    public class ControllerAddUser
    {
        private ViewAddUser ObjAddUser;
        private DAOAdminEmp daoAdminEmp;
        private string username;
        private int accion;
        private string role;
        private string securityQuestion;
        private string securityAnswer;

        /// <summary>
        /// Constructor para inserción de datos
        /// </summary>
        /// <param name="Vista">Instancia de la vista ViewAddUser</param>
        /// <param name="accion">Acción a realizar (inserción)</param>
        public ControllerAddUser(ViewAddUser Vista, int accion)
        {
            ObjAddUser = Vista;
            this.accion = accion;
            daoAdminEmp = new DAOAdminEmp();
            verificarAccion(); // Verifica y configura la interfaz según la acción
            ObjAddUser.Load += new EventHandler(InitialCharge); // Carga inicial de datos y configuraciones
            ObjAddUser.btnAdd.Click += new EventHandler(NewRegister);
        }

        /// <summary>
        /// Constructor para actualización de datos
        /// </summary>
        /// <param name="Vista">Instancia de la vista ViewAddUser</param>
        /// <param name="p_accion">Acción a realizar</param>
        /// <param name="id">ID del usuario</param>
        /// <param name="nombre">Nombre del usuario</param>
        /// <param name="apellido">Apellido del usuario</param>
        /// <param name="genero">Género del usuario</param>
        /// <param name="nacimiento">Fecha de nacimiento del usuario</param>
        /// <param name="email">Correo electrónico del usuario</param>
        /// <param name="telefono">Número de teléfono del usuario</param>
        /// <param name="dui">Número de DUI del usuario</param>
        /// <param name="direccion">Dirección del usuario</param>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="role">Rol del usuario</param>
        public ControllerAddUser(ViewAddUser Vista, int p_accion, int id, string nombre, string apellido, string genero, DateTime nacimiento, string email, string telefono, string dui, string direccion, string username, string role)
        {
            ObjAddUser = Vista;
            this.accion = p_accion;
            this.role = role;
            daoAdminEmp = new DAOAdminEmp();

            ObjAddUser.Load += new EventHandler(InitialCharge);
            verificarAccion();
            ChargeValues(id, nombre, apellido, genero, nacimiento, email, telefono, dui, direccion, username);
            ObjAddUser.btnUpdate.Click += new EventHandler(UpdateRegister);
        }

        private void InitializeSecurityQuestions()
        {
            List<string> securityQuestions = new List<string>
        {
            "¿Cuál es tu color favorito?",
            "¿En qué ciudad naciste?",
            "¿Cuál es tu deporte favorito?",
            "¿Cuál es tu comida favorita?",
            "¿Cuál es tu canción favorita?"
        };

            ObjAddUser.cmbSecurityQuestion.DataSource = securityQuestions;
        }

        public void ConfigurarValidacionDeComandos()
        {
            CommonClasses commonClasses = new CommonClasses();
            // Asociar el evento KeyDown a los TextBox que se quiere proteger
            ObjAddUser.txtFirstName.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.txtLastName.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.txtEmail.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.txtAddress.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.txtPhone.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.txtSecurityAnswer.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.mskDocument.KeyDown += commonClasses.ValidarComandos;
            ObjAddUser.txtUsername.KeyDown += commonClasses.ValidarComandos;

            // Deshabilitar el menú contextual
            ObjAddUser.txtFirstName.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.txtLastName.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.txtEmail.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.txtAddress.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.txtPhone.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.txtSecurityAnswer.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.mskDocument.ContextMenuStrip = new ContextMenuStrip();
            ObjAddUser.txtUsername.ContextMenuStrip = new ContextMenuStrip();
        }

        public void InitialCharge(object sender, EventArgs e)
        {
            DataSet ds = daoAdminEmp.LlenarCombo();
            // Llenar combobox Rol
            ObjAddUser.comboRole.DataSource = ds.Tables["Rol"];
            ObjAddUser.comboRole.ValueMember = "rol_ID";
            ObjAddUser.comboRole.DisplayMember = "rol_nombre";
            //La condición sirve para que al actualizar un registro, el valor del registro aparezca seleccionado.
            if (accion == 2)
            {
                ObjAddUser.comboRole.Text = role;
            }

            DataSet dsGenero = daoAdminEmp.LlenarCombo();
            if (dsGenero != null)
            {
                ObjAddUser.cmbGender.DataSource = dsGenero.Tables["Genero"];
                ObjAddUser.cmbGender.DisplayMember = "emp_genero";
                ObjAddUser.cmbGender.ValueMember = "emp_genero";
            }
            else
            {
                MessageBox.Show("EPV005 - No se pudieron cargar los datos.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            ConfigurarValidacionDeComandos();
            InitializeSecurityQuestions(); // Inicializa las preguntas de seguridad
        }

        public void verificarAccion()
        {
            if (accion == 1)
            {
                ObjAddUser.btnAdd.Enabled = true;
                ObjAddUser.btnUpdate.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjAddUser.cmbSecurityQuestion.Enabled = false;
                ObjAddUser.txtSecurityAnswer.Enabled = false;
                ObjAddUser.btnAdd.Enabled = false;
                ObjAddUser.btnUpdate.Enabled = true;
                ObjAddUser.txtUsername.Enabled = false;
            }
        }

        public void NewRegister(object sender, EventArgs e)
        {
            if (ValidarRe())
            {
                CommonClasses commonClasses = new CommonClasses();
                // Datos para creación de persona
                daoAdminEmp.Nombre = ObjAddUser.txtFirstName.Text.Trim();
                daoAdminEmp.Apellido = ObjAddUser.txtLastName.Text.Trim();
                daoAdminEmp.Genero = ObjAddUser.cmbGender.Text.Trim();
                daoAdminEmp.Nacimiento = ObjAddUser.dtBirth.Value.Date;
                daoAdminEmp.Correo = ObjAddUser.txtEmail.Text.Trim();
                daoAdminEmp.Telefono = ObjAddUser.txtPhone.Text.Trim();
                daoAdminEmp.Dui = ObjAddUser.mskDocument.Text.Trim();
                daoAdminEmp.Direccion = ObjAddUser.txtAddress.Text.Trim();
                daoAdminEmp.Rol = int.Parse(ObjAddUser.comboRole.SelectedValue.ToString());
                // Datos para creación de usuario
                daoAdminEmp.User = ObjAddUser.txtUsername.Text.Trim();
                daoAdminEmp.Password = commonClasses.ComputeSha256Hash(ObjAddUser.txtUsername.Text.Trim() + "OP123");
                daoAdminEmp.UserStatus = true;
                daoAdminEmp.UserAttempts = 0;
                daoAdminEmp.SecurityQuestion = ObjAddUser.cmbSecurityQuestion.Text.Trim();
                daoAdminEmp.SecurityAnswer = ObjAddUser.txtSecurityAnswer.Text.Trim();
                // Se invoca al método RegistrarUsuario y guarda el valor retornado
                int valorRetornado = daoAdminEmp.RegistrarUsuario();
                // Verificar si se registró el usuario con éxito y luego guardar la pregunta de seguridad
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    MessageBox.Show($"Usuario: {ObjAddUser.txtUsername.Text.Trim()}\nContraseña de usuario: {ObjAddUser.txtUsername.Text.Trim()}OP123",
                                    "Credenciales de acceso",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    ObjAddUser.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - Los datos no pudieron ser registrados",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("EPV001 - Error inesperado",
                                    "Proceso incompleto",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        bool ValidarCorreo()
        {
            string email = ObjAddUser.txtEmail.Text.Trim();
            if (!(email.Contains("@")))
            {
                MessageBox.Show("Formato de correo invalido, verifica que contiene @.", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //adripaosv@gmail.com
            // Validación del dominio (ejemplo simplificado)
            string[] dominiosPermitidos = { "gmail.com", "ricaldone.edu.sv" };
            string extension = email.Substring(email.LastIndexOf('@') + 1);
            if (!dominiosPermitidos.Contains(extension))
            {
                MessageBox.Show("Dominio del correo es invalido, el sistema únicamente admite dominios 'gmail.com' y 'correo institucional'.", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public void UpdateRegister(object sender, EventArgs e)
        {
            if (ValidarUp())
            {
                CommonClasses commonClasses = new CommonClasses();
                daoAdminEmp.Id = int.Parse(ObjAddUser.txtId.Text.Trim());
                daoAdminEmp.Nombre = ObjAddUser.txtFirstName.Text.Trim();
                daoAdminEmp.Apellido = ObjAddUser.txtLastName.Text.Trim();
                daoAdminEmp.Genero = ObjAddUser.cmbGender.Text.Trim();
                daoAdminEmp.Nacimiento = ObjAddUser.dtBirth.Value;
                daoAdminEmp.Dui = ObjAddUser.mskDocument.Text.Trim();
                daoAdminEmp.Direccion = ObjAddUser.txtAddress.Text.Trim();
                daoAdminEmp.Correo = ObjAddUser.txtEmail.Text.Trim();
                daoAdminEmp.Telefono = ObjAddUser.txtPhone.Text.Trim();
                daoAdminEmp.Rol = (int)ObjAddUser.comboRole.SelectedValue;
                daoAdminEmp.User = ObjAddUser.txtUsername.Text.Trim();
                int valorRetornado = daoAdminEmp.ActualizarEmpleado();
                if (valorRetornado == 2)
                {
                    if (!string.IsNullOrEmpty(securityQuestion) && !string.IsNullOrEmpty(securityAnswer))
                    {
                        if (!daoAdminEmp.UpdateSecurityQuestion(ObjAddUser.txtUsername.Text.Trim(), securityQuestion, securityAnswer))
                        {
                            MessageBox.Show("Los datos del usuario se actualizaron, pero hubo un problema al actualizar la pregunta de seguridad.",
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                    }

                    MessageBox.Show("Los datos han sido actualizados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAddUser.Close();
                }
                else if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos no pudieron ser actualizados completamente",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Los datos no pudieron ser actualizados debido a un error inesperado",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Los datos no pudieron ser registrados",
                                    "Proceso incompleto",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
            }
        }

        public bool ValidarRe()
        {
            CommonClasses commonClasses = new CommonClasses();
            string nombre = ObjAddUser.txtFirstName.Text.Trim();
            if (!commonClasses.EsNombreValido(nombre))
            {
                MessageBox.Show("El nombre ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (ObjAddUser.txtFirstName.Text.Length > 100)
            {
                MessageBox.Show("El campo de nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string apellido = ObjAddUser.txtLastName.Text.Trim();
            if (!commonClasses.EsNombreValido(apellido))
            {
                MessageBox.Show("El apellido ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (ObjAddUser.txtLastName.Text.Length > 100)
            {
                MessageBox.Show("El campo de apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validaciones de campos NOT NULL y longitud de caracteres
            if (string.IsNullOrWhiteSpace(ObjAddUser.txtEmail.Text.Trim()))
            {
                MessageBox.Show("El campo de correo es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!commonClasses.ValidateEmail(ObjAddUser.txtEmail.Text.Trim()))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico válido.",
                                "Correo inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtEmail.Text.Length > 100)
            {
                MessageBox.Show("El campo de correo no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAddUser.txtPhone.Text.Trim()))
            {
                MessageBox.Show("El campo de teléfono es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtPhone.Text.Length > 25)
            {
                MessageBox.Show("El campo de teléfono ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!commonClasses.ValidarTelefono(ObjAddUser.txtPhone.Text.Trim()))
            {
                MessageBox.Show("El campo de teléfono contiene caracteres no válidos. Solo se permiten números, guiones y paréntesis.",
                                "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string telefono = ObjAddUser.txtPhone.Text.Trim();
            if (!EsTelValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener un guion (-).", "Validación de Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ObjAddUser.mskDocument.MaskCompleted)
            {
                MessageBox.Show("El campo de DUI es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string dui = ObjAddUser.mskDocument.Text.Replace("-", "").Trim(); // Reemplazar el guion o cualquier otro carácter de máscara
            if (dui.Length > 10)
            {
                MessageBox.Show("El campo de DUI ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string correo = ObjAddUser.txtEmail.Text.Trim();
            if (!ValidarCorreo())
            {
                return false;
            }
            if (!commonClasses.EsCorreoValido(correo))
            {
                MessageBox.Show("El campo Correo Electrónico no tiene un formato válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verificar si el DUI ya está registrado
            bool existeDui = daoAdminEmp.VerificarDuiExistente(ObjAddUser.mskDocument.Text.Trim());
            if (existeDui)
            {
                MessageBox.Show("El DUI ingresado ya está asociado a otro usuario. Por favor, verifique e ingrese un DUI diferente.",
                                "Error de validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false; // Detener el proceso si el DUI ya existe
            }

            // Verificar si el correo ya está registrado
            bool existeCorreo = daoAdminEmp.VerificarCorreoExistente(ObjAddUser.txtEmail.Text.Trim());
            if (existeCorreo)
            {
                MessageBox.Show("El correo electrónico ingresado ya está asociado a otro usuario. Por favor, verifique e ingrese un correo electrónico diferente.",
                                "Error de validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false; // Detener el proceso si el correo ya existe
            }

            if (string.IsNullOrWhiteSpace(ObjAddUser.txtUsername.Text.Trim()))
            {
                MessageBox.Show("El campo de nombre de usuario es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtUsername.Text.Length > 100)
            {
                MessageBox.Show("El campo de nombre de usuario no debe exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ObjAddUser.comboRole.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un rol.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAddUser.txtSecurityAnswer.Text.Trim()))
            {
                MessageBox.Show("El campo de respuesta de seguridad es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtSecurityAnswer.Text.Length > 256)
            {
                MessageBox.Show("El campo de respuesta de seguridad ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            DateTime fechaNacimiento = ObjAddUser.dtBirth.Value.Date;
            if (!commonClasses.ValidarFechaNacimiento(fechaNacimiento))
            {
                MessageBox.Show("La fecha de nacimiento no es válida.",
                                "Error de validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool ValidarUp()
        {
            CommonClasses commonClasses = new CommonClasses();
            string nombre = ObjAddUser.txtFirstName.Text.Trim();
            if (!commonClasses.EsNombreValido(nombre))
            {
                MessageBox.Show("El nombre ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (ObjAddUser.txtFirstName.Text.Length > 100)
            {
                MessageBox.Show("El campo de nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string apellido = ObjAddUser.txtLastName.Text.Trim();
            if (!commonClasses.EsNombreValido(apellido))
            {
                MessageBox.Show("El apellido ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (ObjAddUser.txtLastName.Text.Length > 100)
            {
                MessageBox.Show("El campo de apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validaciones de campos NOT NULL y longitud de caracteres
            if (string.IsNullOrWhiteSpace(ObjAddUser.txtEmail.Text.Trim()))
            {
                MessageBox.Show("El campo de correo es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!commonClasses.ValidateEmail(ObjAddUser.txtEmail.Text.Trim()))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico válido.",
                                "Correo inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtEmail.Text.Length > 100)
            {
                MessageBox.Show("El campo de correo no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string correo = ObjAddUser.txtEmail.Text.Trim();
            if (!ValidarCorreo())
            {
                return false;
            }
            if (!commonClasses.EsCorreoValido(correo))
            {
                MessageBox.Show("El campo Correo Electrónico no tiene un formato válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //// Verificar si el DUI ya está registrado
            //bool existeDui = daoAdminEmp.VerificarDuiExistente(ObjAddUser.mskDocument.Text.Trim());
            //if (existeDui)
            //{
            //    MessageBox.Show("El DUI ingresado ya está asociado a otro usuario. Por favor, verifique e ingrese un DUI diferente.",
            //                    "Error de validación",
            //                    MessageBoxButtons.OK,
            //                    MessageBoxIcon.Error);
            //    return false; // Detener el proceso si el DUI ya existe
            //}

            // Verificar si el correo ya está registrado
            bool existeCorreo = daoAdminEmp.VerificarCorreoExistente(ObjAddUser.txtEmail.Text.Trim());
            if (existeCorreo)
            {
                MessageBox.Show("El correo electrónico ingresado ya está asociado a otro usuario. Por favor, verifique e ingrese un correo electrónico diferente.",
                                "Error de validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false; // Detener el proceso si el correo ya existe
            }

            if (string.IsNullOrWhiteSpace(ObjAddUser.txtPhone.Text.Trim()))
            {
                MessageBox.Show("El campo de teléfono es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtPhone.Text.Length > 25)
            {
                MessageBox.Show("El campo de teléfono ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (!commonClasses.ValidarTelefono(ObjAddUser.txtPhone.Text.Trim()))
            {
                MessageBox.Show("El campo de teléfono contiene caracteres no válidos. Solo se permiten números, guiones y paréntesis.",
                                "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string telefono = ObjAddUser.txtPhone.Text.Trim();
            if (!EsTelValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener un guion (-).", "Validación de Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!ObjAddUser.mskDocument.MaskCompleted)
            {
                MessageBox.Show("El campo de DUI es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string dui = ObjAddUser.mskDocument.Text.Replace("-", "").Trim(); // Reemplazar el guion o cualquier otro carácter de máscara
            if (dui.Length > 10)
            {
                MessageBox.Show("El campo de DUI ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAddUser.txtUsername.Text.Trim()))
            {
                MessageBox.Show("El campo de nombre de usuario es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (ObjAddUser.txtUsername.Text.Length > 100)
            {
                MessageBox.Show("El campo de nombre de usuario no debe exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ObjAddUser.comboRole.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un rol.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            DateTime fechaNacimiento = ObjAddUser.dtBirth.Value.Date;
            if (!commonClasses.ValidarFechaNacimiento(fechaNacimiento))
            {
                MessageBox.Show("La fecha de nacimiento no es válida.",
                                "Error de validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool EsTelValido(string telefono)
        {
            if (!telefono.Contains("-"))
            {
                return false;
            }
            return true;
        }

        public void ChargeValues(int id, string nombre, string apellido, string genero, DateTime nacimiento, string email, string telefono, string dui, string direccion, string username)
        {
            // Asigna los valores recibidos a los campos correspondientes en la vista ObjAddUser
            ObjAddUser.txtId.Text = id.ToString();
            ObjAddUser.txtFirstName.Text = nombre;
            ObjAddUser.txtLastName.Text = apellido;
            ObjAddUser.cmbGender.Text = genero;
            ObjAddUser.dtBirth.Value = nacimiento;
            ObjAddUser.mskDocument.Text = dui;
            ObjAddUser.txtAddress.Text = direccion;
            ObjAddUser.txtEmail.Text = email;
            ObjAddUser.txtPhone.Text = telefono;
            ObjAddUser.txtUsername.Text = username;
        }
    }
}