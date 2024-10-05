using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Views.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Server
{
    internal class ControllerConfirmPassword
    {
        ViewConfirmPassword objView;
        private string usuario;
        public ControllerConfirmPassword(ViewConfirmPassword vista, string usuario)
        {
            objView = vista;
            this.usuario = usuario;
            objView.btnConfirmar.Click += new EventHandler(VerificarPassword);
        }

        public void VerificarPassword(object sender, EventArgs e)
        {
            CommonClasses common = new CommonClasses();
            //Utilizando el objeto DAO para invocar a los metodos getter y setter del DTO
            string cadenaencriptada = common.ComputeSha256Hash(objView.txtPassword.Text);
            if (cadenaencriptada == SessionVar.Password)
            {
                ViewAdminConnection objViewConnect = new ViewAdminConnection(2);
                objViewConnect.ShowDialog();
                objView.Hide();
            }
            else
            {
                MessageBox.Show("Acceso denegado, las credenciales no tienen los permisos suficientes.", "Error al validar contraseña", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
