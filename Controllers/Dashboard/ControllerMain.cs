using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.Article;
using OpticaMultivisual.Views.Consultas;
using OpticaMultivisual.Views.Dashboard;
using OpticaMultivisual.Views.Dashboard.Optometrista;
using OpticaMultivisual.Views.Dashboard.PedidoDet;
using OpticaMultivisual.Views.Login;
using OpticaMultivisual.Views.ScheduleAppointment;
using OpticaMultivisual.Views.Server;

namespace OpticaMultivisual.Controllers.Dashboard
{
    internal class ControllerMain
    {
        ViewMain ObjMain;
        Form currentForm;

        // Constructor que recibe la vista ViewMain como parámetro
        public ControllerMain(ViewMain View, string username)
        {
            //Se asigna al objeto ObjMain todo lo que proviene el objeto View del constructor
            ObjMain = View;
            //Se utiliza el evento Load, el cual se ejecuta de forma inmediata cuando el formulario se inicia.
            View.Load += new EventHandler(EventosIniciales);
            //Al componente lblUsername se le asigna el valor de variable de sesión
            ObjMain.lblUsername.Text = SessionVar.Username;
            //Se invoca al evento AbrirFormularioAdminUsuarios para que este puede ser mostrado según el boton que el usuario presione.
            ObjMain.btnAdmin.Click += new EventHandler(AbrirFormularioAdminUsuarios);
            ObjMain.btnClientes.Click += new EventHandler(AbrirFormularioAdminClientes);
            ObjMain.btnReceta.Click += new EventHandler(AbrirFormularioRecetaBase);
            ObjMain.btnExit.Click += new EventHandler(CerrarSesion);
            ObjMain.btnConsult.Click += new EventHandler(AbrirFormularioConsulta);
            ObjMain.btnDetalle.Click += new EventHandler(AbrirFormularioPD);
            ObjMain.FormClosing += new FormClosingEventHandler(cerrarPrograma);
            ObjMain.btnVisita.Click += new EventHandler(AbrirFormularioVis);
            ObjMain.btnArticulo.Click += new EventHandler(AbrirFormArt);
            ObjMain.btnServer.Click += new EventHandler(ConfServer);
            ObjMain.btnDoc.Click += new EventHandler(DownloadManual);
        }

        private void DownloadManual(object sender, EventArgs e)
        {
            // Mostrar un cuadro de diálogo de confirmación antes de comenzar la descarga
            DialogResult result = MessageBox.Show("¿Está seguro que desea descargar el manual de usuario?", "Confirmación de descarga", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Si el usuario elige "Sí", proceder con la descarga
            if (result == DialogResult.Yes)
            {
                // URL del archivo en Google Drive del Manual de Usuario
                string url = "https://drive.google.com/uc?export=download&id=1sqpmAPaGkwrhZ0OoAyAG0PM0slw9pVP1";
                // Crear una instancia de SaveFileDialog para que el usuario elija la ubicación
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";  // Filtrar solo archivos PDF
                saveFileDialog.Title = "Guardar manual de usuario";
                saveFileDialog.FileName = "Manual de Usuario EyePro - V 1.0.pdf";  // Nombre predeterminado del archivo que se va a guardar
                // Mostrar el diálogo de guardado
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Obtener la ruta seleccionada por el usuario
                    string destinationPath = saveFileDialog.FileName;
                    using (var client = new WebClient())
                    {
                        try
                        {
                            // Descargar el archivo desde Google Driv
                            client.DownloadFile(url, destinationPath);
                            MessageBox.Show("Manual descargado con éxito", "Manual", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al descargar el manual de usuario", "Manual", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                // Si el usuario selecciona "No", cancelar la descarga
                MessageBox.Show("Descarga cancelada", "Manual", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ConfServer(object sender, EventArgs e)
        {
            ViewConfirmPassword objview = new ViewConfirmPassword(ObjMain.lblUsername.Text);
            objview.ShowDialog();
        }

        private void AbrirFormularioConsulta(object sender, EventArgs e)
        {
            AbrirFormulario<Consultas>();
        }

        private void AbrirFormularioPD(object sender, EventArgs e)
        {
            AbrirFormulario<ViewPedidoDet>();
        }

        private void AbrirFormArt(object sender, EventArgs e)
        {
            AbrirFormulario<ViewAdminArticle>();
        }

        private void AbrirFormularioVis(object sender, EventArgs e)
        {
            AbrirFormulario<ViewSearchScheduleAppointment>();
        }

        private void cerrarPrograma(Object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Desea cerrar el programa directamente, considere que se cerrará la sesión de forma automática", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Si selecciona "Sí", se cierra el programa
                Environment.Exit(0);
            }
            else
            {
                // Si selecciona "No", se cancela el cierre del formulario
                e.Cancel = true;
            }
        }

        private void CerrarSesion(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpiarVariablesSesion();
                ViewLogin backForm = new ViewLogin();
                backForm.Show();
                ObjMain.Dispose();
            }
        }

        void LimpiarVariablesSesion()
        {
            SessionVar.Username = string.Empty;
            SessionVar.Password = string.Empty;
            SessionVar.FullName = string.Empty;
            SessionVar.Access = string.Empty;
            SessionVar.RoleId = 0;
        }

        void EventosIniciales(object sender, EventArgs e)
        {
            Acceso();
        }

        public void Acceso()
        {
            //Estructura selectiva para evaluar los posibles valores de la variable Access
            switch (SessionVar.Access)
            {
                case "Administrador":
                    break;
                case "Optometrista":
                    ObjMain.btnAdmin.Visible = false;
                    ObjMain.btnVisita.Visible = false;
                    ObjMain.btnServer.Visible = false;
                    break;
                case "Empleado":
                    ObjMain.btnAdmin.Visible = false;
                    ObjMain.btnReceta.Visible = false;
                    ObjMain.btnServer.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Método para abrir formularios dentro del panel contenedor del formulario principal
        /// </summary>
        /// <typeparam name="MiForm"></typeparam>
        private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        {
            //Se declara objeto de tipo Form llamada formulario
            Form formulario;
            //Se guarda en el panel contenedor del formulario principal todos los controles del formulario que desea abrir <MiForm> posteriormente se guarda todo en el objeto de tipo formulario
            formulario = ObjMain.PanelContenedor.Controls.OfType<MiForm>().FirstOrDefault();
            //Si el formulario no existe se procederá a crearlo de lo contrario solo se traerá al frente (ver clausula else)
            if (formulario == null)
            {
                //Se define un nuevo formulario para guardarse como nuevo objeto MiForm
                formulario = new MiForm();
                //Se especifica que el formulario debe mostrarse como ventana
                formulario.TopLevel = false;
                //Se eliminan los bordes del formulario
                formulario.FormBorderStyle = FormBorderStyle.None;
                //Se establece que se abrira en todo el espacio del formulario padre
                formulario.Dock = DockStyle.Fill;
                //Se le asigna una opacidad de 0.75
                formulario.Opacity = 0.75;
                //Se evalua el formulario actual para verificar si es nulo
                if (currentForm != null)
                {
                    //Se cierra el formulario actual para mostrar el nuevo formulario
                    currentForm.Close();
                    //Se eliminan del panel contenedor todos los controles del formulario que se cerrará
                    ObjMain.PanelContenedor.Controls.Remove(currentForm);
                }
                //Se establece como nuevo formulario actual el formulario que se está abriendo
                currentForm = formulario;
                //Se agregan los controles del nuevo formulario al panel contenedor
                ObjMain.PanelContenedor.Controls.Add(formulario);
                //Tag es una propiedad genérica disponible para la mayoría de los controles en aplicaciones .NET, incluyendo los paneles.
                ObjMain.PanelContenedor.Tag = formulario;
                //Se muestra el formulario en el panel contenedor
                formulario.Show();
                //Se trae al frente el formulario armado
                formulario.BringToFront();
            }
            else
            {
                formulario.BringToFront();
            }
            // Forzar actualización del tamaño al agregar el formulario al panel
            currentForm.Dock = DockStyle.Fill;
            currentForm.Refresh();
        }

        private void AbrirFormularioAdminUsuarios(object sender, EventArgs e)
        {
            AbrirFormulario<ViewAdminEmp>();
        }

        private void AbrirFormularioAdminClientes(object sender, EventArgs e)
        {
            AbrirFormulario<AdministrarCientes>();
        }

        private void AbrirFormularioRecetaBase(object sender, EventArgs e)
        {
            AbrirFormulario<ViewRecetaBase>();
        }
    }
}
