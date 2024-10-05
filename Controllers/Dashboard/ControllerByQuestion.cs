using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.Dashboard
{
    class ControllerByQuestion
    {
        public ViewByQuestion ObjVerifyQ;
        private DAOAdminEmp daoSecurityQuestion;

        public ControllerByQuestion(ViewByQuestion view)
        {
            ObjVerifyQ = view;
            daoSecurityQuestion = new DAOAdminEmp();
            ObjVerifyQ.BtnVerify.Click += new EventHandler(VerificarUsuario);
        }
        public void VerificarUsuario(object sender, EventArgs e)
        {
            string username = ObjVerifyQ.txtUsername.Text;
            daoSecurityQuestion.User = username;
            if (daoSecurityQuestion.VerificarUsuario(username))
            {
                MessageBox.Show("Usuario existente.", "Siguiente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ObjVerifyQ.Close();
                ViewRecoverByQuestion viewRecoverByQuestion = new ViewRecoverByQuestion(username);
                viewRecoverByQuestion.ShowDialog();
            }
            else
            {
                MessageBox.Show("Usuario inexistente, ingrese un usuario existente", "El usuario no existe.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
