using OpticaMultivisual.Models.DTO;
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
                //SqlConnection conexion = new SqlConnection($"Server = {DTOdbContext.Server}; DataBase = {DTOdbContext.Database}; User Id = {DTOdbContext.User}; Password = {DTOdbContext.Password}");
                conexion.Open();
                return conexion;
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"{ex.Message} Código de error: EC-001 \nNo fue posible conectarse a la base de datos, favor verifique las credenciales o que tenga acceso al sistema.", "Error crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
