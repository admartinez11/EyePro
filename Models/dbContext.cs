using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Views.Server;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Windows.Forms;

namespace OpticaMultivisual.Models
{
    public class dbContext
    {
        public static SqlConnection getConnection()
        {
            try
            {
                string connectionString;

                // Si no se proporcionan usuario y contraseña, usamos autenticación de Windows
                if (string.IsNullOrEmpty(DTOdbContext.User) && string.IsNullOrEmpty(DTOdbContext.Password))
                {
                    connectionString = $"Server={DTOdbContext.Server};Database={DTOdbContext.Database};Trusted_Connection=True;";
                }
                else
                {
                    // Autenticación SQL Server
                    connectionString = $"Server={DTOdbContext.Server};Database={DTOdbContext.Database};User Id={DTOdbContext.User};Password={DTOdbContext.Password};";
                }

                // Crear y abrir la conexión
                SqlConnection conexion = new SqlConnection(connectionString);
                conexion.Open();
                return conexion;
            }
            catch (SqlException ex)
            {
                DialogResult result = MessageBox.Show(
                    $"{ex.Message} Código de error: EC-001 \nNo fue posible conectarse a la base de datos, favor verifique las credenciales o que tenga acceso al sistema y a internet en el caso de estar conectado a una base de datos en línea. ¿Desea cambiar los datos de conexión?",
                       "Error crítico",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Error
                        );

                if (result == DialogResult.Yes)
                {
                    ViewAdminConnection objViewConnect = new ViewAdminConnection(2);
                    var dialogResult = objViewConnect.ShowDialog();
                    MessageBox.Show(
                       "Vuelva a abrir el programa. Si el problema persiste, asegúrese de tener conexión a internet, especialmente si está utilizando una base de datos en línea.",
                       "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                return null;
            }
        }

        public static SqlConnection testConnection(string server, string database, string user, string password)
        {
            try
            {
                string connectionString;

                // Si no se proporcionan usuario y contraseña, usamos autenticación de Windows
                if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(password))
                {
                    connectionString = $"Server={server};Database={database};Trusted_Connection=True;";
                }
                else
                {
                    // Autenticación SQL Server
                    connectionString = $"Server={server};Database={database};User Id={user};Password={password};";
                }

                // Crear y abrir la conexión
                SqlConnection conexion = new SqlConnection(connectionString);
                //SqlConnection conexion = new SqlConnection($"Server = {server}; DataBase = {database}; User Id = {user}; Password = {password}");
                conexion.Open();
                return conexion;
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"{ex.Message} Código de error: EC-001 \nNo fue posible conectarse a la base de datos, verifique las credenciales, consulte el manual de usuario.", "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
