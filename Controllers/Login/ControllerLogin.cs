using OpticaMultivisual.Models;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Views.Dashboard;
using OpticaMultivisual.Controllers.Helper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OpticaMultivisual.Controllers.Login
{
    public class ControllerLogin
    {
        //Objeto de la vista ViewLogin
        ViewLogin ObjLogin;

        /// <summary>
        /// Constructor de la clase ControllerLogin que inicia los eventos de la vista
        /// </summary>
        /// <param name="Vista"></param>
        public ControllerLogin(ViewLogin Vista)
        {
            ObjLogin = Vista;
            ObjLogin.Load += new EventHandler(ConfigurarValidacionDeComandos);
            ObjLogin.BtnStart.Click += new EventHandler(DataAccess);
            ObjLogin.BtnExit.Click += new EventHandler(QuitApplication);
            //Eventos de Probar Conexión
            ObjLogin.BtnTest.Click += new EventHandler(TestConnection);
            ObjLogin.Lblregistration.Click += new EventHandler(CreateUser);
            ObjLogin.LblForgotpass.Click += new EventHandler(RecoverPassword);
            ObjLogin.PasswordVisible.Click += new EventHandler(ShowPassword);
            ObjLogin.PasswordHide.Click += new EventHandler(HidePassword);
        }

        // Método para guardar las credenciales
        private void GuardarCredenciales(string username, string password)
        {
            CommonClasses commonClasses = new CommonClasses();
            // Guardar el nombre de usuario en la configuración
            Properties.Settings.Default.Username = username;
            // Usamos SHA-256 para encriptar la contraseña antes de guardarla
            Properties.Settings.Default.Password = Encriptar(password);
            // Guardamos que el usuario quiere ser recordado
            Properties.Settings.Default.RememberMe = true;
            // Guardamos los cambios en la configuración
            Properties.Settings.Default.Save();
        }

        // Método para cargar las credenciales si existen
        private void CargarCredenciales()
        {
            if (Properties.Settings.Default.RememberMe)
            {
                ObjLogin.txtUsername.Text = Properties.Settings.Default.Username;
                ObjLogin.txtPassword.Text = Desencriptar(Properties.Settings.Default.Password); // Desencripta la contraseña al cargarla
                ObjLogin.chkRememberMe.Checked = true;
            }
        }

        // Método para eliminar las credenciales guardadas
        private void EliminarCredenciales()
        {
            // Eliminamos el nombre de usuario y la contraseña de la configuración
            Properties.Settings.Default.Username = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.RememberMe = false; // Desmarcamos la opción de recordar
            Properties.Settings.Default.Save(); // Guardamos los cambios
        }

        // Método de ejemplo para encriptar (deberías usar una técnica de encriptación más robusta)
        protected string Encriptar(string text)
        {
            byte[] data = System.Text.Encoding.Unicode.GetBytes(text);
            return Convert.ToBase64String(data);
        }

        // Método de ejemplo para desencriptar
        protected string Desencriptar(string encryptedText)
        {
            byte[] data = Convert.FromBase64String(encryptedText);
            return System.Text.Encoding.Unicode.GetString(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void DataAccess(object sender, EventArgs e)
        {
            try
            {
                // Cambiar el cursor a "Wait" mientras se descarga el archivo
                Cursor.Current = Cursors.WaitCursor;
                // Creando objeto de la clase DAOLogin
                DAOLogin DAOData = new DAOLogin();
                CommonClasses common = new CommonClasses();
                // Utilizando el objeto DAO para invocar a los métodos getter y setter del DTO
                DAOData.Username = ObjLogin.txtUsername.Text;
                string username = ObjLogin.txtUsername.Text.Trim();
                string password = ObjLogin.txtPassword.Text.Trim();
                string cadenaencriptada = common.ComputeSha256Hash(ObjLogin.txtPassword.Text);
                DAOData.Password = cadenaencriptada;
                // Invocando al método Login contenido en el DAO y capturando el resultado
                int answer = DAOData.ValidarLogin();
                // Evaluando el valor de la variable answer
                switch (answer)
                {
                    case 0: // Usuario no existe
                        MessageBox.Show("Usuario o contraseña incorrectos", "Error al iniciar sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case 1: // Contraseña incorrecta
                            // Verificar el estado del usuario (si está activo o inactivo)
                        int userStatus = DAOData.CheckUserStatus(username); // Enviar username como parámetro
                        if (userStatus == 0)
                        {
                            MessageBox.Show("Su usuario ha sido inhabilitado. Pídale al administrador que intervenga.", "Usuario inhabilitado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            // Incrementar el contador de intentos fallidos
                            DAOData.IncrementUserAttempts(username); // Enviar username como parámetro
                            int userAttempts = DAOData.GetUserAttempts(username); // Obtener el número de intentos fallidos
                            if (userAttempts >= 3)
                            {
                                // Deshabilitar el usuario y mostrar el mensaje de bloqueo
                                DAOData.DisableUser(username); // Enviar username como parámetro
                                MessageBox.Show("Su usuario ha sido inhabilitado. Pídale al administrador que intervenga.", "Usuario inhabilitado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos", "Error al iniciar sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        break;
                    case 2: // Usuario y contraseña correctos
                            // Restablecer el contador de intentos fallidos
                        DAOData.ResetUserAttempts(username); // Enviar username como parámetro
                                                             // Verificar si el usuario tiene un VerificationCode
                        bool hasVerificationCode = DAOData.HasVerificationCode(ObjLogin.txtUsername.Text);
                        // Si el usuario selecciona 'Recordar usuario y contraseña'
                        if (ObjLogin.chkRememberMe.Checked == true)
                        {
                            GuardarCredenciales(ObjLogin.txtUsername.Text.Trim(), ObjLogin.txtPassword.Text.Trim());
                        }
                        else
                        {
                            EliminarCredenciales();
                        }
                        // Verificar si la contraseña ingresada es la contraseña por defecto esperada
                        string defaultPassword = ObjLogin.txtUsername.Text + "OP123"; // Concatenar el nombre de usuario con "OP123"
                        bool isDefaultPassword = ObjLogin.txtPassword.Text == defaultPassword;
                        if (hasVerificationCode && isDefaultPassword)
                        {
                            // Mostrar el formulario para ingresar el VerificationCode
                            ObjLogin.Hide();
                            ViewCambiarClave viewEnterVerificationCode = new ViewCambiarClave();
                            viewEnterVerificationCode.Show();
                        }
                        else
                        {
                            // No tiene VerificationCode, proceder normalmente
                            if (ObjLogin.txtPassword.Text.Trim() != DAOData.Username + "OP123")
                            {
                                ObjLogin.Hide();
                                ViewMain viewMain = new ViewMain(ObjLogin.txtUsername.Text);
                                viewMain.Show();
                            }
                            else if (ObjLogin.txtPassword.Text.Trim() == DAOData.Username + "OP123")
                            {
                                // Limpiar los campos txtPassword
                                ObjLogin.txtPassword.Text = "";
                                ViewCambiarClaveDefecto openForm = new ViewCambiarClaveDefecto();
                                openForm.ShowDialog();
                            }
                        }
                        break;
                    default:
                        MessageBox.Show("Error desconocido", "Error al iniciar sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error durante el acceso a los datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Restaurar el cursor a su estado normal después de completar el proceso
                Cursor.Current = Cursors.Default;
            }
        }

        public void ConfigurarValidacionDeComandos(object sender, EventArgs e)
        {
            // Cargar las credenciales guardadas si existen
            CargarCredenciales();
            CommonClasses commonClasses = new CommonClasses();
            // Asociar el evento KeyDown a los TextBox que se quiere proteger
            ObjLogin.txtUsername.KeyDown += commonClasses.ValidarComandos;
            ObjLogin.txtPassword.KeyDown += commonClasses.ValidarComandos;

            // Deshabilitar el menú contextual
            ObjLogin.txtUsername.ContextMenuStrip = new ContextMenuStrip();
            ObjLogin.txtPassword.ContextMenuStrip = new ContextMenuStrip();

            if (!string.IsNullOrEmpty(ObjLogin.txtUsername.Text.Trim()) && !string.IsNullOrEmpty(ObjLogin.txtPassword.Text.Trim()))
            {
                ObjLogin.BtnStart.TabIndex = 0;
                ObjLogin.BtnStart.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestConnection(object sender, EventArgs e)
        {
            //Se hace referencia a la clase dbContext y su método getConnection y se evalúa
            //si el retorno es nulo o no, en caso de ser nulo se mostrará el primer mensaje
            //de lo contrario se mostrará el código del segmento else.
            if (dbContext.getConnection() == null)
            {
                MessageBox.Show("No fue posible realizar la conexión al servidor y/o la base de datos.", "Conexión fallida", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                MessageBox.Show("La conexión al servidor y la base de datos se ha ejecutado correctamente.", "Conexión exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CreateUser(object sender, EventArgs e)
        {
            ViewAddUser viewAddUser = new ViewAddUser();
            viewAddUser.Show();
        }

        private void RecoverPassword(object sender, EventArgs e)
        {
            ViewRecuperaciónPass viewRecuPass = new ViewRecuperaciónPass();
            viewRecuPass.ShowDialog();
        }

        /// <summary>
        /// El evento KeyEnter detecta cuando la tecla enter es presionada y realizará una
        /// determinada acción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ShowPassword(object sender, EventArgs e)
        {
            // Mostrar la contraseña en texto plano
            ObjLogin.txtPassword.UseSystemPasswordChar = false;
            ObjLogin.txtPassword.PasswordChar = '\0'; // Mostrar el texto plano
            ObjLogin.PasswordVisible.Visible = false;
            ObjLogin.PasswordHide.Visible = true;
        }

        private void HidePassword(object sender, EventArgs e)
        {
            // Establecer el carácter de contraseña del sistema manualmente
            ObjLogin.txtPassword.UseSystemPasswordChar = true;
            ObjLogin.txtPassword.PasswordChar = (char)0x25CF; // Usar el punto negro (•), el carácter estándar para contraseñas
            ObjLogin.PasswordVisible.Visible = true;
            ObjLogin.PasswordHide.Visible = false;
        }
    }
}