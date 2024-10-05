using OpticaMultivisual.Models;
using OpticaMultivisual.Views.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpticaMultivisual.Models.DTO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace OpticaMultivisual.Controllers.Server
{
    internal class ControllerAdminConnection
    {
        ViewAdminConnection ObjView;
        int origen;

        // Constructor del controlador para la conexión administrativa
        public ControllerAdminConnection(ViewAdminConnection View, int origen)
        {
            // Asignar la vista pasada como parámetro a la variable de instancia
            ObjView = View;
            // Llamar al método verificarOrigen para inicializar la vista según el origen
            verificarOrigen(origen);
            // Asociar eventos de cambio de estado a los controles de la vista
            View.rdDeshabilitarWindows.CheckedChanged += new EventHandler(rdFalseMarked);
            View.rdHabilitarWindows.CheckedChanged += new EventHandler(rdTrueMarked);
            // Asociar el evento de clic del botón de guardar a su manejador
            View.btnGuardar.Click += new EventHandler(GuardarRegistro);
        }

        // Método para verificar el origen y configurar la vista según el origen
        public void verificarOrigen(int origen)
        {
            if (origen == 2)
            {
                // Si el origen es 2, configurar los campos de texto con los datos del contexto de datos
                ObjView.txtServer.Text = DTOdbContext.Server;
                ObjView.txtDatabase.Text = DTOdbContext.Database;
                ObjView.txtSqlAuth.Text = DTOdbContext.User;
                ObjView.txtSqlPass.Text = DTOdbContext.Password;
            }
        }

        #region Configuración del servidor

        // Manejador del evento CheckedChanged para el radio button rdDeshabilitarWindows
        void rdFalseMarked(object sender, EventArgs e)
        {
            // Si el radio button rdDeshabilitarWindows está marcado, habilitar el panel de autenticación
            if (ObjView.rdDeshabilitarWindows.Checked == true)
            {
                ObjView.panelAuth.Enabled = true;
            }
        }

        // Manejador del evento CheckedChanged para el radio button rdHabilitarWindows
        void rdTrueMarked(object sender, EventArgs e)
        {
            // Si el radio button rdHabilitarWindows está marcado, deshabilitar el panel de autenticación
            // y limpiar los campos de autenticación y contraseña
            if (ObjView.rdHabilitarWindows.Checked == true)
            {
                ObjView.panelAuth.Enabled = false;
                ObjView.txtSqlAuth.Clear();
                ObjView.txtSqlPass.Clear();
            }
        }

        // Manejador del evento Click para el botón de guardar
        void GuardarRegistro(object sender, EventArgs e)
        {
            // Llamar al método para guardar la configuración en un archivo XML
            GuardarConfiguracionXML();
        }

        // Método para guardar la configuración en un archivo XML
        public void GuardarConfiguracionXML()
        {
            try
            {
                // Crear una instancia de XmlDocument para trabajar con XML
                XmlDocument doc = new XmlDocument();
                // Crear una declaración XML con versión 1.0 y codificación UTF-8 (admite caracteres especiales)
                XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(decl);
                // Crear el elemento raíz del XML
                XmlElement root = doc.CreateElement("Conn");
                doc.AppendChild(root);
                // Crear un elemento "Server" y establecer su valor codificado en Base64
                XmlElement servidor = doc.CreateElement("Server");
                string servidorCode = CodificarBase64String(ObjView.txtServer.Text.Trim());
                servidor.InnerText = servidorCode;
                root.AppendChild(servidor);
                // Crear un elemento "Database" y establecer su valor codificado en Base64
                XmlElement Database = doc.CreateElement("Database");
                string DatabaseCode = CodificarBase64String(ObjView.txtDatabase.Text.Trim());
                Database.InnerText = DatabaseCode;
                root.AppendChild(Database);
                // Dependiendo de la selección de los radio buttons, agregar elementos de autenticación SQL
                if (ObjView.rdDeshabilitarWindows.Checked == true)
                {
                    XmlElement SqlAuth = doc.CreateElement("SqlAuth");
                    string sqlAuthCode = CodificarBase64String(ObjView.txtSqlAuth.Text.Trim());
                    SqlAuth.InnerText = sqlAuthCode;
                    root.AppendChild(SqlAuth);
                    XmlElement SqlPass = doc.CreateElement("SqlPass");
                    string SqlPassCode = CodificarBase64String(ObjView.txtSqlPass.Text.Trim());
                    SqlPass.InnerText = SqlPassCode;
                    root.AppendChild(SqlPass);
                }
                else
                {
                    // Si se habilita Windows Authentication, agregar elementos vacíos para SqlAuth y SqlPass
                    XmlElement SqlAuth = doc.CreateElement("SqlAuth");
                    SqlAuth.InnerText = string.Empty;
                    root.AppendChild(SqlAuth);
                    XmlElement SqlPass = doc.CreateElement("SqlPass");
                    SqlPass.InnerText = string.Empty;
                    root.AppendChild(SqlPass);
                }
                // Probar la conexión con los parámetros proporcionados
                SqlConnection con = dbContext.testConnection(ObjView.txtServer.Text.Trim(), ObjView.txtDatabase.Text.Trim(), ObjView.txtSqlAuth.Text.Trim(), ObjView.txtSqlPass.Text.Trim());
                // Si la conexión es válida, guardar el XML y actualizar el contexto de datos
                if (con != null)
                {
                    doc.Save("config_server.xml");
                    DTOdbContext.Server = ObjView.txtServer.Text.Trim();
                    DTOdbContext.Database = ObjView.txtDatabase.Text.Trim();
                    DTOdbContext.User = ObjView.txtSqlAuth.Text.Trim();
                    DTOdbContext.Password = ObjView.txtSqlPass.Text.Trim();
                    MessageBox.Show($"El archivo fue creado exitosamente.", "Archivo de configuración", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ObjView.Dispose();
                }
            }
            catch (XmlException ex)
            {
                // Manejar cualquier excepción que ocurra durante la creación del archivo XML
                MessageBox.Show($"{ex.Message}, no se pudo crear el archivo de configuración.", "Consulte el manual técnico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        // Método para codificar una cadena en Base64
        public string CodificarBase64String(string textoacifrar)
        {
            try
            {
                // Convertir la cadena a un arreglo de bytes usando UTF-8
                byte[] bytes = Encoding.UTF8.GetBytes(textoacifrar);
                // Codificar el arreglo de bytes en una cadena Base64
                string base64String = Convert.ToBase64String(bytes);
                return base64String;
            }
            catch (Exception)
            {
                // Si ocurre una excepción, retornar una cadena vacía
                return string.Empty;
            }
        }
    }
}

