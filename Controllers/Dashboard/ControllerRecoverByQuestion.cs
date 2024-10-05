using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Models.DTO;
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
    public class ControllerRecoverByQuestion
    {
        public ViewRecoverByQuestion viewRecoverByQuestion;
        private DAOAdminEmp daoAdminEmp;
        private string username; // Almacena el nombre de usuario

        // Constructor que inicializa la vista, el DAO y el nombre de usuario
        public ControllerRecoverByQuestion(ViewRecoverByQuestion viewRecover, string username)
        {
            this.viewRecoverByQuestion = viewRecover;
            this.username = username; // Guarda el nombre de usuario
            this.daoAdminEmp = new DAOAdminEmp();
            this.viewRecoverByQuestion.lblSecurityQuestion.Text = SessionVar.SecurityQuestion;
            this.viewRecoverByQuestion.btnRecover.Click += new EventHandler(VerifyAnswer);
            // Carga la pregunta de seguridad cuando se deje el campo de nombre de usuario
            LoadSecurityQuestion(null, EventArgs.Empty);
        }

        // Método para cargar la pregunta de seguridad en la vista según el username
        private void LoadSecurityQuestion(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(username))
            {
                string question = daoAdminEmp.ObtenerPreguntaSeguridad(username); // Obtén la pregunta de seguridad
                if (!string.IsNullOrEmpty(question))
                {
                    this.viewRecoverByQuestion.lblSecurityQuestion.Text = question;
                }
                else
                {
                    MessageBox.Show("No se encontró una pregunta de seguridad para el usuario especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Método para verificar la respuesta de seguridad
        private void VerifyAnswer(object sender, EventArgs e)
        {
            string answer = this.viewRecoverByQuestion.txtSecurityAnswer.Text.Trim();
            string newPassword = this.viewRecoverByQuestion.txtNewPassword.Text.Trim();

            CommonClasses common = new CommonClasses();

            // Verifica si la respuesta y la nueva contraseña están presentes
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(answer) && !string.IsNullOrEmpty(newPassword))
            {
                bool isCorrect = daoAdminEmp.VerifySecurityAnswer(username, answer); // Verifica la respuesta

                if (isCorrect)
                {
                    // Verifica si la contraseña cumple con los requisitos
                    if (common.EsValida(viewRecoverByQuestion.txtNewPassword.Text.Trim()))
                    {
                        // Si la respuesta es correcta y la contraseña cumple los requisitos, actualiza la contraseña
                        daoAdminEmp.Password = viewRecoverByQuestion.txtNewPassword.Text.Trim();

                        bool isPasswordUpdated = daoAdminEmp.UpdatePassword(username, daoAdminEmp.Password);

                        if (isPasswordUpdated)
                        {
                            MessageBox.Show("La respuesta es correcta. Contraseña actualizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            viewRecoverByQuestion.Close();
                            ViewLogin viewLogin = new ViewLogin();
                            viewLogin.Show();
                        }
                        else
                        {
                            MessageBox.Show("Ocurrió un error al actualizar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("La nueva contraseña no cumple con los requisitos.", "Proceso incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("• Mínimo 8 caracteres de longitud\n" +
                                        "• Un signo $, @, _\n" +
                                        "• Al menos una mayúscula A-Z\n" +
                                        "• Al menos una minúscula a-z\n" +
                                        "• Al menos un número\n", "Características que debe cumplir la nueva contraseña", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("La respuesta de seguridad es incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
