using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Login
{
    internal class ControllerCambiarClaveDefecto
    {
        ViewCambiarClaveDefecto vista;

        public ControllerCambiarClaveDefecto(ViewCambiarClaveDefecto vista)
        {
            this.vista = vista;
            vista.btnCambiarClave.Click += new EventHandler(CambiarClaveDefecto);
            vista.Load += new EventHandler(ConfigurarValidacionDeComandos);
        }

        public void ConfigurarValidacionDeComandos(object sender, EventArgs e)
        {
            CommonClasses commonClasses = new CommonClasses();
            // Asociar el evento KeyDown a los TextBox que se quiere proteger
            vista.txtUsuario.KeyDown += commonClasses.ValidarComandos;
            vista.txtNuevaContra.KeyDown += commonClasses.ValidarComandos;
            vista.txtConfirmarNuevaContra.KeyDown += commonClasses.ValidarComandos;

            // Deshabilitar el menú contextual
            vista.txtUsuario.ContextMenuStrip = new ContextMenuStrip();
            vista.txtNuevaContra.ContextMenuStrip = new ContextMenuStrip();
            vista.txtConfirmarNuevaContra.ContextMenuStrip = new ContextMenuStrip();
        }

        void CambiarClaveDefecto(object sender, EventArgs e)
        {
            DAOAdminEmp daoUpdatePassword = new DAOAdminEmp();
            CommonClasses common = new CommonClasses();
            daoUpdatePassword.User = vista.txtUsuario.Text.Trim();
            if (vista.txtNuevaContra.Text.Trim().Equals(vista.txtConfirmarNuevaContra.Text.Trim()))
            {
                if (common.EsValida(vista.txtConfirmarNuevaContra.Text.Trim()))
                {
                    daoUpdatePassword.Password = common.ComputeSha256Hash(vista.txtConfirmarNuevaContra.Text.Trim());
                    if (daoUpdatePassword.RestablecerContrasena())
                    {
                        MessageBox.Show("Contraseña actualizada con éxito.", "Proceso finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        vista.Close();
                    }
                    else
                    {
                        MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("La contraseña no cumple con los requisitos.", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden, favor escriba la misma contraseña.", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
