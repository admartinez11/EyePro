using System;
using System.Collections.Generic;
using System.Linq;
using OpticaMultivisual.Views.Dashboard;
using System.Text;
using OpticaMultivisual.Models.DAO;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using OpticaMultivisual.Controllers.Helper;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;

namespace OpticaMultivisual.Controllers.Dashboard
{
    class ControllerAdminEmp
    {
        ViewAdminEmp ObjAdminEmp;
        public ControllerAdminEmp(ViewAdminEmp Vista)
        {
            ObjAdminEmp = Vista;
            ObjAdminEmp.Load += new EventHandler(LoadData);
            //Evento click de botón
            ObjAdminEmp.btnNuevo.Click += new EventHandler(NewUser);
            ObjAdminEmp.btnEditar.Click += new EventHandler(UpdateUser);
            ObjAdminEmp.btnEliminar.Click += new EventHandler(DeleteUser);
            ObjAdminEmp.cmsResPass.Click += new EventHandler(RestartPassword);
            ObjAdminEmp.btnBuscar.Click += new EventHandler(BuscarPersonasControllerEvent);
            ObjAdminEmp.txtSearch.KeyPress += new KeyPressEventHandler(Search);
        }

        void RestartPassword(object sender, EventArgs e)
        {
            CommonClasses commonClasses = new CommonClasses();
            DAOAdminEmp daoRestartPassword = new DAOAdminEmp();
            int pos = ObjAdminEmp.dgvEmpleados.CurrentRow.Index;
            string usuario = ObjAdminEmp.dgvEmpleados[9, pos].Value.ToString();
            // Verificar el estado del usuario
            if (!daoRestartPassword.IsUserActive(usuario))
            {
                MessageBox.Show($"El usuario: {usuario} esta bloqueado, se debe restablecer por intervención directa del administrador.",
                                "Usuario bloqueado",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return; // Salir del método si el usuario no está activo
            }
            if (MessageBox.Show($"¿Está seguro que desea restablecer la contraseña del usuario: {usuario}?", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Capturando nombres del usuario
                string firstName = ObjAdminEmp.dgvEmpleados[1, pos].Value.ToString();
                string lastName = ObjAdminEmp.dgvEmpleados[2, pos].Value.ToString();
                string nombrePersona = firstName + " " + lastName;
                string emailDestinatario = ObjAdminEmp.dgvEmpleados[5, pos].Value.ToString();
                daoRestartPassword.User = ObjAdminEmp.dgvEmpleados[9, pos].Value.ToString();
                //Generando PIN de seguridad y enviado PIN a la base de datos
                string pin = commonClasses.GenerarPin();
                daoRestartPassword.VerificationCode = commonClasses.ComputeSha256Hash(pin);
                //Enviando PIN al correo de usuario
                if (ValidateEmail(emailDestinatario))
                {
                    bool pinRegistrado = daoRestartPassword.RegistrarPIN();
                    bool correoEnviado = false;

                    if (pinRegistrado)
                    {
                        correoEnviado = EnviarPinPorCorreo(emailDestinatario, pin, nombrePersona);
                    }

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
                }
                else
                {
                    MessageBox.Show("La dirección de correo no es válida.",
                                    "Correo inválido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            }
        }

        // Método para validar el correo electrónico
        private bool ValidateEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        bool EnviarPinPorCorreo(string emailDestinatario, string pin, string nombrePersona)
        {
            MailMessage mail = new MailMessage();
            int puertoSmtp = 587; // Puerto común para SMTP
            string SmtpServer = "smtp.gmail.com";
            string remitente = "ccom.ptc2024@gmail.com";
            string contraseña = "ngqy wagb fchr uvfr";
            // Crear un mensaje de correo electrónico
            MailMessage mensaje = new MailMessage(remitente, emailDestinatario);
            mensaje.Subject = "Restablecimiento de contraseña";
            mensaje.Body = $"Hola {nombrePersona}.\n\nEl administrador ha restablecido tu contraseña y para tu seguridad te hemos enviado un PIN el cual deberás ingresar para crear una nueva contraseña.\n\nDirígete al Inicio de Sesión y haz click en ¿Olvido su contraseña? posteriormente selecciona la opción de Restablecimiento de usuario.\n\nEl pin que deberás introducir es: {pin}, no compartas este PIN y tampoco el acceso a tu correo electrónico registrado en el sistema.\nEn caso no solicitaste el restablecimiento de tu usuario, contacta con el administrador.";
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
                MessageBox.Show($"{ex.Message}");
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                return false;

            }
        }

            public void Search(object sender, KeyPressEventArgs e)
        {
            BuscarPersonasController();
        }

        public void BuscarPersonasControllerEvent(object sender, EventArgs e) { BuscarPersonasController(); }
        void BuscarPersonasController()
        {
            //Objeto de la clase DAOAdminUsuarios
            DAOAdminEmp objAdmin = new DAOAdminEmp();
            //Declarando nuevo DataSet para que obtenga los datos del método ObtenerPersonas
            DataSet ds = objAdmin.BuscarEmpleados(ObjAdminEmp.txtSearch.Text.Trim());
            //Llenar DataGridView
            ObjAdminEmp.dgvEmpleados.DataSource = ds.Tables["ViewEmp"];
        }

        public void ConfigurarValidacionDeComandos()
        {
            CommonClasses commonClasses = new CommonClasses();
            // Asociar el evento KeyDown a los TextBox que se quiere proteger
            ObjAdminEmp.txtSearch.KeyDown += commonClasses.ValidarComandos;

            // Deshabilitar el menú contextual
            ObjAdminEmp.txtSearch.ContextMenuStrip = new ContextMenuStrip();
        }

        public void LoadData(object sender, EventArgs e)
        {
            RefrescarData();
            ConfigurarValidacionDeComandos();
        }

        public void RefrescarData()
        {
            //Objeto de la clase DAOAdminUsuarios
            DAOAdminEmp objAdmin = new DAOAdminEmp();
            //Declarando nuevo DataSet para que obtenga los datos del metodo ObtenerPersonas
            DataSet ds = objAdmin.ObtenerPersonas();
            //Llenar DataGridView
            ObjAdminEmp.dgvEmpleados.DataSource = ds.Tables["ViewEmp"];
            ObjAdminEmp.dgvEmpleados.Columns[0].Visible = false;
        }

        private void NewUser(object sender, EventArgs e)
        {
            /*Se invoca al formulario ViewAddUser y se le envía un numero, este numero servirá para indicarle que tipo de acción se quiere realizar, donde 1 significa Inserción y 2 significa Actualización*/
            ViewAddUser openForm = new ViewAddUser(1);
            //Se muestra el formulario
            openForm.ShowDialog();
            //Cuando el formulario ha sido cerrado se procede a refrescar el DataGrid para que se puedan observar los nuevo datos ingresados o actualizados.
            RefrescarData();
        }

        private void UpdateUser(object sender, EventArgs e)
        {
            //Se captura el numero de la fila a la cual se le dió click, sabiendo que la primer fila tiene como valor cero.
            int pos = ObjAdminEmp.dgvEmpleados.CurrentRow.Index;
            /*Se invoca al formulario llamado ViewAddUser y se crea un objeto de el, posteriormente se envían los datos del datagrid al constructor del formulario según el orden establecido (se sugiere ver el código del formulario para observar ambos constructores).
             * El numero dos indicado en la linea posterior significa que la acción que se desea realizar es una actualización.*/
            ViewAddUser openForm = new ViewAddUser(2,
                int.Parse(ObjAdminEmp.dgvEmpleados[0, pos].Value.ToString()),
                ObjAdminEmp.dgvEmpleados[1, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[2, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[3, pos].Value.ToString(),
                DateTime.Parse(ObjAdminEmp.dgvEmpleados[4, pos].Value.ToString()),
                ObjAdminEmp.dgvEmpleados[5, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[6, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[7, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[8, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[9, pos].Value.ToString(),
                ObjAdminEmp.dgvEmpleados[10, pos].Value.ToString());
            //Una vez los datos han sido enviados al constructor de la vista se procede a mostrar el formulario (se sugiere ver el código del constructor que esta en la vista)
            openForm.ShowDialog();
            //Una vez el formulario se haya cerrado se procederá a refrescar el dataGrid para mostrar los nuevos datos.
            RefrescarData();
        }

        private void DeleteUser(object sender, EventArgs e)
        {
            int pos = ObjAdminEmp.dgvEmpleados.CurrentRow.Index;
            if (MessageBox.Show($"¿Esta seguro que desea elimar a:\n {ObjAdminEmp.dgvEmpleados[1, pos].Value.ToString()} {ObjAdminEmp.dgvEmpleados[2, pos].Value.ToString()}.\nConsidere que dicha acción no se podrá revertir.", "Confirmar acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DAOAdminEmp daoDel = new DAOAdminEmp();
                daoDel.Id = int.Parse(ObjAdminEmp.dgvEmpleados[0, pos].Value.ToString());
                daoDel.User = ObjAdminEmp.dgvEmpleados[9, pos].Value.ToString();
                int valorRetornado = daoDel.EliminarUsuario();
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Registro eliminado", "Acción completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefrescarData();
                }
                else
                {
                    MessageBox.Show("Registro no pudo ser eliminado, verifique que el registro no tenga datos asociados.", "Acción interrumpida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
