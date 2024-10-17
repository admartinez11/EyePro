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

namespace OpticaMultivisual.Controllers.Login
{
    internal class ControllerCambiarContrasena
    {
        ViewCambiarClave vista;
        public ControllerCambiarContrasena(ViewCambiarClave vista)
        {
            this.vista = vista;
            this.vista.BtnVerifyAndReset.Click += new EventHandler(CambiarClave);
        }

        void CambiarClave(object sender, EventArgs e)
        {
            try
            {
                // Cambiar el cursor a "Wait" mientras se descarga el archivo
                Cursor.Current = Cursors.WaitCursor;
                if (!(string.IsNullOrEmpty(vista.txtPIN.Text.Trim()) ||
                string.IsNullOrEmpty(vista.txtUsuario.Text.Trim()) ||
                string.IsNullOrEmpty(vista.txtNuevaContra.Text.Trim()) ||
                string.IsNullOrEmpty(vista.txtConfirmarNuevaContra.Text.Trim())))
                {
                    if (VerificarUsuario())
                    {
                        if (VerificarPIN())
                        {
                            if (RestablecerClave())
                            {
                                MessageBox.Show("Contraseña restablecida con éxito, ya puedes iniciar sesión con tu nueva contraseña.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                vista.Dispose();
                                ViewLogin viewLogin = new ViewLogin();
                                viewLogin.Show();
                            }
                            else
                            {
                                MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("El PIN o el usuario son incorrectos, verifique la información proporcionada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Todos los campos requeridos, favor complétalos para establecer una nueva contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // Restaurar el cursor a su estado normal después de completar la descarga
                Cursor.Current = Cursors.Default;
            }
        }

        bool VerificarUsuario()
        {
            string username = vista.txtUsuario.Text;
            DAOAdminEmp dAOAdminEmp = new DAOAdminEmp();
            dAOAdminEmp.User = username;
            if (dAOAdminEmp.VerificarUsuario(username))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Usuario inexistente, ingrese un usuario existente", "El usuario no existe.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        bool VerificarPIN()
        {
            DAOAdminEmp daoVerificar = new DAOAdminEmp();
            CommonClasses commonClasses = new CommonClasses();
            daoVerificar.VerificationCode = commonClasses.ComputeSha256Hash(vista.txtPIN.Text.Trim());
            daoVerificar.User = vista.txtUsuario.Text.Trim();
            return daoVerificar.VerificarPINSeguridad();
        }

        bool RestablecerClave()
        {
            DAOAdminEmp daoVerificar = new DAOAdminEmp();
            CommonClasses commonClasses = new CommonClasses();

            if (vista.txtNuevaContra.Text.Trim().Equals(vista.txtConfirmarNuevaContra.Text.Trim()))
            {
                if (commonClasses.EsValida((vista.txtConfirmarNuevaContra.Text.Trim())))
                {
                    daoVerificar.Password = commonClasses.ComputeSha256Hash(vista.txtConfirmarNuevaContra.Text.Trim());
                    daoVerificar.User = vista.txtUsuario.Text.Trim();
                    return daoVerificar.RestablecerContrasena();
                }
                else
                {
                    MessageBox.Show("Contraseña no pudo ser actualizada debido a que no cumple con los requisitos de seguridad, verifique al lado izquierdo de la ventana.", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            else
            {
                MessageBox.Show("Las contraseñas no coindicen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
