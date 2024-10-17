using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using System.Net.Mail;
using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Views.Dashboard;
using OpticaMultivisual.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace OpticaMultivisual.Controllers.Login
{
    internal class ControllerIntAdmin
    {
        ViewIntAdmin ObjAdmin;
        public ControllerIntAdmin(ViewIntAdmin ObjAdmin)
        {
            this.ObjAdmin = ObjAdmin;
            this.ObjAdmin.BtnRest.Click += new EventHandler(CambiarClave);
        }

        // Método que se ejecuta cuando se hace clic en el botón de restablecer
        private void CambiarClave(object sender, EventArgs e)
        {
            try
            {
                // Cambiar el cursor a "Wait" mientras se descarga el archivo
                Cursor.Current = Cursors.WaitCursor;
                if (string.IsNullOrEmpty(ObjAdmin.txtUserEmp.Text.Trim()) ||
                string.IsNullOrEmpty(ObjAdmin.txtUserAd.Text.Trim()) ||
                string.IsNullOrEmpty(ObjAdmin.txtPassAd.Text.Trim()))
                {
                    MessageBox.Show("Todos los campos son requeridos, por favor complétalos para restablecer la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!VerificarAdmin())
                {
                    MessageBox.Show("Las credenciales del administrador son incorrectas o las credenciales ingresadas no son de un administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!VerificarUsuario())
                {
                    MessageBox.Show("El usuario ingresado no existe, por favor ingresa un usuario existente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (RestablecerClave())
                {
                    DAOAdminEmp daoAdminEmp = new DAOAdminEmp();
                    CommonClasses commonClasses = new CommonClasses();
                    string pin = commonClasses.GenerarPin();  // Genera el PIN
                    daoAdminEmp.VerificationCode = commonClasses.ComputeSha256Hash(pin);
                    // Registrar el PIN en la base de datos
                    daoAdminEmp.User = ObjAdmin.txtUserEmp.Text.Trim();
                    bool pinRegistrado = daoAdminEmp.RegistrarPIN();
                    // Obtener el correo del usuario
                    string correoUsuario = daoAdminEmp.ObtenerCorreoPorUsername(ObjAdmin.txtUserEmp.Text.Trim());
                    string user = ObjAdmin.txtUserEmp.Text.Trim();
                    // Enviar el PIN por correo si se registró correctamente
                    bool correoEnviado = false;
                    if (pinRegistrado && !string.IsNullOrEmpty(correoUsuario))
                    {
                        correoEnviado = EnviarCorreoConPIN(correoUsuario, pin, user);
                    }

                    // Mostrar mensajes dependiendo de los resultados
                    if (pinRegistrado && correoEnviado)
                    {
                        MessageBox.Show("PIN de seguridad generado correctamente, indique al empleado que el PIN ha sido enviado a su correo registrado en el sistema.", "PIN de seguridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (!pinRegistrado && !correoEnviado)
                    {
                        MessageBox.Show("El PIN no pudo ser registrado en la base de datos, por lo tanto no se envió al correo del destinatario", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!pinRegistrado)
                    {
                        MessageBox.Show("El PIN no pudo ser registrado en la base de datos.", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!correoEnviado)
                    {
                        MessageBox.Show("El PIN fue registrado en la base de datos, pero no pudo enviarse al correo del destinatario.", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Cerrar la vista actual y abrir la vista de login si todo fue exitoso
                    if (pinRegistrado && correoEnviado)
                    {
                        ObjAdmin.Dispose();  // Cierra la vista actual
                        ViewLogin viewLogin = new ViewLogin();  // Abre la vista de login
                        viewLogin.Show();
                    }
                }
                else
                {
                    MessageBox.Show("La contraseña no pudo ser actualizada. Vuelve a intentarlo. Si el error persiste, contacta al administrador del sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // Restaurar el cursor a su estado normal después de completar la descarga
                Cursor.Current = Cursors.Default;
            }
        }

        // Método para enviar un correo electrónico con el PIN
        private bool EnviarCorreoConPIN(string correo, string pin, string user)
        {
            MailMessage mail = new MailMessage();
            int puertoSmtp = 587; // Puerto común para SMTP
            string SmtpServer = "smtp.gmail.com";
            string remitente = "ccom.ptc2024@gmail.com";
            string contraseña = "ngqy wagb fchr uvfr";
            string username = ObjAdmin.txtUserEmp.Text.Trim();
            // Crear un mensaje de correo electrónico
            MailMessage mensaje = new MailMessage(remitente, correo);
            mensaje.Subject = "Restablecimiento de contraseña";
            mensaje.Body = $"Hola {username},\n\nTu contraseña ha sido restablecida a la contraseña por defecto: {username}OP123.\nTu PIN para la activación es: {pin}.\n\nEste PIN expirará en 15 minutos.";
            // Configurar el cliente SMTP
            SmtpClient clienteSmtp = new SmtpClient(SmtpServer, puertoSmtp);
            clienteSmtp.Credentials = new NetworkCredential(remitente, contraseña);
            clienteSmtp.EnableSsl = true;
            // Enviar el correo  
            try
            {
                clienteSmtp.Send(mensaje);
                return true;
            }
            catch (SmtpException ex)
            {
                MessageBox.Show("EPV010 - Error de excepción");
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV010 - Error de excepción");
                return false;

            }
        }

        // Método para verificar si el usuario del empleado existe
        private bool VerificarUsuario()
        {
            string username = ObjAdmin.txtUserEmp.Text.Trim();
            DAOAdminEmp dAOAdminEmp = new DAOAdminEmp();
            return dAOAdminEmp.VerificarUsuario(username);
        }

        // Método para verificar las credenciales del administrador
        private bool VerificarAdmin()
        {
            string username = ObjAdmin.txtUserAd.Text.Trim();
            string password = ObjAdmin.txtPassAd.Text.Trim();
            DAOAdminEmp dAOAdminEmp = new DAOAdminEmp();
            return dAOAdminEmp.ValidateAdminCredentials(username, password);
        }

        // Método para restablecer la contraseña del usuario del empleado
        private bool RestablecerClave()
        {
            string username = ObjAdmin.txtUserEmp.Text.Trim();
            DAOAdminEmp dAOAdminEmp = new DAOAdminEmp();
            return dAOAdminEmp.ActualizarClaveUsuario(username);
        }
    }
}
