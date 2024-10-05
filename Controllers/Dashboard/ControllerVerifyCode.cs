using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard;
using OpticaMultivisual.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Dashboard
{
    public class ControllerVerifyCode
    {
        public ViewVerifyCode ObjVerifyCode;

        public ControllerVerifyCode(ViewVerifyCode view)
        {
            ObjVerifyCode = view;
            ObjVerifyCode.Load += new EventHandler(ConfigurarValidacionDeComandos);
            ObjVerifyCode.BtnVerifyAndReset.Click += new EventHandler(VerifyAndResetPassword);
        }

        public void ConfigurarValidacionDeComandos(object sender, EventArgs e)
        {
            CommonClasses commonClasses = new CommonClasses();
            // Asociar el evento KeyDown a los TextBox que se quiere proteger
            ObjVerifyCode.txtVerificationCode.KeyDown += commonClasses.ValidarComandos;
            ObjVerifyCode.txtNewPassword.KeyDown += commonClasses.ValidarComandos;
            ObjVerifyCode.txtConfirmPassword.KeyDown += commonClasses.ValidarComandos;

            // Deshabilitar el menú contextual
            ObjVerifyCode.txtVerificationCode.ContextMenuStrip = new ContextMenuStrip();
            ObjVerifyCode.txtNewPassword.ContextMenuStrip = new ContextMenuStrip();
            ObjVerifyCode.txtConfirmPassword.ContextMenuStrip = new ContextMenuStrip();
        }

        private void VerifyAndResetPassword(object sender, EventArgs e)
        {
            string verificationCode = ObjVerifyCode.txtVerificationCode.Text.Trim();
            string newPassword = ObjVerifyCode.txtNewPassword.Text.Trim();
            string confirmPassword = ObjVerifyCode.txtConfirmPassword.Text.Trim();
            string userEmail = SessionVar.UserEmail; // Obtener el correo electrónico almacenado en SessionVar

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            DAORecoverPassword DAOReset = new DAORecoverPassword();
            CommonClasses commonClasses = new CommonClasses();

            // Validar el código de verificación
            if (DAOReset.ValidateVerificationCode(userEmail, verificationCode))
            {
                // Verifica si la contraseña cumple con los requisitos
                if (commonClasses.EsValida(newPassword))
                {
                    if (DAOReset.UpdatePasswordUsingCode(userEmail, newPassword, verificationCode))
                    {
                        MessageBox.Show("Contraseña restablecida exitosamente.",
                                        "Proceso completado",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        ObjVerifyCode.Close();
                        // Muestra el formulario de inicio de sesión (ViewLogin)
                        ViewLogin viewLogin = new ViewLogin();
                        viewLogin.Show();
                    }
                    else
                    {
                        MessageBox.Show("Error: EPV002 - Los datos no pudieron ser actualizados correctamente",
                                        "Proceso interrumpido",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("La nueva contraseña no cumple con los requisitos.",
                                    "Proceso incompleto",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    MessageBox.Show("• Mínimo 8 caracteres de longitud\n" +
                                    "• Un signo $, @, _\n" +
                                    "• Al menos una mayúscula A-Z\n" +
                                    "• Al menos una minúscula a-z\n" +
                                    "• Al menos un número\n",
                                    "Características que debe cumplir la nueva contraseña",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Código de verificación incorrecto o expirado.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}