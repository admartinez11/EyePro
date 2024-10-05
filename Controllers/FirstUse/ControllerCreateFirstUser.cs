using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.FirstUse;
using OpticaMultivisual.Views.Login;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Models.DTO;

namespace OpticaMultivisual.Controllers.FirstUse
{
    internal class ControllerCreateFirstUser
    {
        ViewCreateFirstUser ObjVista;

        public ControllerCreateFirstUser(ViewCreateFirstUser Vista)
        {
            ObjVista = Vista;
            Vista.Load += new EventHandler(CargarCombos);
            Vista.btnAdd.Click += new EventHandler(RegistrarPrimerUsuario);
            Vista.toolStripButton1.Click += new EventHandler(Exit);
            Vista.comboRole.Enabled = false;
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

            ObjVista.cmbSecurityQuestion.DataSource = securityQuestions;
        }

        void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void CargarCombos(object sender, EventArgs e)
        {
            //Objeto de la clase DAOAdminUsuarios
            DAOAdminEmp objAdmin = new DAOAdminEmp();
            //Declarando nuevo DataSet para que obtenga los datos del metodo LlenarCombo
            DataSet ds = objAdmin.LlenarCombo();
            //Llenar combobox tbRole
            ObjVista.comboRole.DataSource = ds.Tables["Rol"];
            ObjVista.comboRole.ValueMember = "rol_Id";
            ObjVista.comboRole.DisplayMember = "rol_nombre";
            InitializeSecurityQuestions();
        }

        void RegistrarPrimerUsuario(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(ObjVista.txtFirstName.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVista.txtLastName.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVista.mskDocument.Text) ||
                string.IsNullOrEmpty(ObjVista.txtAddress.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVista.txtEmail.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVista.txtPhone.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVista.txtUsername.Text.Trim())))
            {
                CommonClasses commonClasses = new CommonClasses();
                DAOAdminEmp daoAdminEmp = new DAOAdminEmp();
                string nombre = ObjVista.txtFirstName.Text.Trim();
                if (!commonClasses.EsNombreValido(nombre))
                {
                    MessageBox.Show("El nombre ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (ObjVista.txtFirstName.Text.Length > 100)
                {
                    MessageBox.Show("El campo de nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string apellido = ObjVista.txtLastName.Text.Trim();
                if (!commonClasses.EsNombreValido(apellido))
                {
                    MessageBox.Show("El apellido ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (ObjVista.txtLastName.Text.Length > 100)
                {
                    MessageBox.Show("El campo de apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validaciones de campos NOT NULL y longitud de caracteres
                if (string.IsNullOrWhiteSpace(ObjVista.txtEmail.Text.Trim()))
                {
                    MessageBox.Show("El campo de correo es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!commonClasses.ValidateEmail(ObjVista.txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Por favor, ingrese un correo electrónico válido.",
                                    "Correo inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (ObjVista.txtEmail.Text.Length > 100)
                {
                    MessageBox.Show("El campo de correo no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ObjVista.txtPhone.Text.Trim()))
                {
                    MessageBox.Show("El campo de teléfono es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (ObjVista.txtPhone.Text.Length > 25)
                {
                    MessageBox.Show("El campo de teléfono ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!commonClasses.ValidarTelefono(ObjVista.txtPhone.Text.Trim()))
                {
                    MessageBox.Show("El campo de teléfono contiene caracteres no válidos. Solo se permiten números, guiones y paréntesis.",
                                    "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!ObjVista.mskDocument.MaskCompleted)
                {
                    MessageBox.Show("El campo de DUI es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string dui = ObjVista.mskDocument.Text.Replace("-", "").Trim(); // Reemplazar el guion o cualquier otro carácter de máscara
                if (dui.Length > 10)
                {
                    MessageBox.Show("El campo de DUI ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string correo = ObjVista.txtEmail.Text.Trim();
                if (!ValidarCorreo())
                {
                    return;
                }
                if (!commonClasses.EsCorreoValido(correo))
                {
                    MessageBox.Show("El campo Correo Electrónico no tiene un formato válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Verificar si el DUI ya está registrado
                bool existeDui = daoAdminEmp.VerificarDuiExistente(ObjVista.mskDocument.Text.Trim());
                if (existeDui)
                {
                    MessageBox.Show("El DUI ingresado ya está asociado a otro usuario. Por favor, verifique e ingrese un DUI diferente.",
                                    "Error de validación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return; // Detener el proceso si el DUI ya existe
                }

                // Verificar si el correo ya está registrado
                bool existeCorreo = daoAdminEmp.VerificarCorreoExistente(ObjVista.txtEmail.Text.Trim());
                if (existeCorreo)
                {
                    MessageBox.Show("El correo electrónico ingresado ya está asociado a otro usuario. Por favor, verifique e ingrese un correo electrónico diferente.",
                                    "Error de validación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return; // Detener el proceso si el correo ya existe
                }

                if (string.IsNullOrWhiteSpace(ObjVista.txtUsername.Text.Trim()))
                {
                    MessageBox.Show("El campo de nombre de usuario es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (ObjVista.txtUsername.Text.Length > 100)
                {
                    MessageBox.Show("El campo de nombre de usuario no debe exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ObjVista.comboRole.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un rol.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ObjVista.txtSecurityAnswer.Text.Trim()))
                {
                    MessageBox.Show("El campo de respuesta de seguridad es obligatorio.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (ObjVista.txtSecurityAnswer.Text.Length > 256)
                {
                    MessageBox.Show("El campo de respuesta de seguridad ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime fechaNacimiento = ObjVista.dtBirth.Value.Date;
                if (!commonClasses.ValidarFechaNacimiento(fechaNacimiento))
                {
                    MessageBox.Show("La fecha de nacimiento no es válida.",
                                    "Error de validación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                //Se crea una instancia de la clase DAOAdminUsers llamada DAOInsert
                DAOAdminEmp DAOInsert = new DAOAdminEmp();
                // Datos para creación de persona
                DAOInsert.Nombre = ObjVista.txtFirstName.Text.Trim();
                DAOInsert.Apellido = ObjVista.txtLastName.Text.Trim();
                DAOInsert.Genero = ObjVista.cmbGender.Text.Trim();
                DAOInsert.Nacimiento = ObjVista.dtBirth.Value.Date;
                DAOInsert.Correo = ObjVista.txtEmail.Text.Trim();
                DAOInsert.Telefono = ObjVista.txtPhone.Text.Trim();
                DAOInsert.Dui = ObjVista.mskDocument.Text.Trim();
                DAOInsert.Direccion = ObjVista.txtAddress.Text.Trim();
                DAOInsert.Rol = int.Parse(ObjVista.comboRole.SelectedValue.ToString());
                // Datos para creación de usuario
                DAOInsert.User = ObjVista.txtUsername.Text.Trim();
                DAOInsert.Password = commonClasses.ComputeSha256Hash(ObjVista.txtUsername.Text.Trim() + "OP123");
                DAOInsert.UserStatus = true;
                DAOInsert.UserAttempts = 0;
                DAOInsert.SecurityQuestion = ObjVista.cmbSecurityQuestion.Text.Trim();
                DAOInsert.SecurityAnswer = ObjVista.txtSecurityAnswer.Text.Trim();
                //Se invoca al método RegistrarUsuario mediante el objeto DAOInsert
                int valorRetornado = DAOInsert.RegistrarUsuario();
                //Se verifica el valor que retornó el método anterior y que fue almacenado en la variable valorRetornado
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    MessageBox.Show($"Usuario administrador: {ObjVista.txtUsername.Text.Trim()}\nContraseña de usuario: {ObjVista.txtUsername.Text.Trim()}OP123",
                                    "Credenciales de acceso",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    ViewLogin login = new ViewLogin();
                    login.Show();
                    ObjVista.Hide();
                }
                else
                {
                    MessageBox.Show("EPV006 - No se pudieron registrar los datos",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados y verifique que la fecha seleccionada corresponde a una persona mayor de edad.",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
            }
        }

        bool ValidarCorreo()
        {
            string email = ObjVista.txtEmail.Text.Trim();
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
    }
}
