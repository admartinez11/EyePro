using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Models;
using OpticaMultivisual.Controllers.Helper;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.Remoting.Messaging;

namespace OpticaMultivisual.Models.DAO
{
    // La clase DAOLogin hereda de DTOLogin y se encarga de manejar la lógica de acceso a datos para el inicio de sesión
    public class DAOLogin : DTOLogin
    {
        SqlCommand Command = new SqlCommand();

        public int CheckUserStatus(string username)
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT userStatus FROM Usuario WHERE username = @username COLLATE Latin1_General_BIN";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@username", username);
                int userStatus = Convert.ToInt32(cmd.ExecuteScalar());
                return userStatus;
            }
            catch (SqlException sqlex)
            {
                // Manejar el error
                MessageBox.Show("EPV010 - Error de excepción");
                return -1;
            }
            catch (Exception ex)
            {
                // Manejar el error
                MessageBox.Show("EPV010 - Error de excepción");
                return -1;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public void IncrementUserAttempts(string username)
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Usuario SET userAttempts = userAttempts + 1 WHERE username = @username COLLATE Latin1_General_BIN";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                // Manejar el error
                MessageBox.Show("EPV010 - Error de excepción");
            }
            catch (Exception ex)
            {
                // Manejar el error
                MessageBox.Show("EPV010 - Error de excepción");
            }
            finally
            {
                getConnection().Close();
            }
        }

        public int GetUserAttempts(string username)
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT userAttempts FROM Usuario WHERE username = @username COLLATE Latin1_General_BIN";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@username", username);
                int userAttempts = Convert.ToInt32(cmd.ExecuteScalar());
                return userAttempts;
            }
            catch (SqlException sqlex)
            {
                // Manejar el error
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
                return -1;
            }
            catch (Exception ex)
            {
                // Manejar el error
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public bool HasVerificationCode(string username)
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    // Consulta para verificar si el usuario tiene un código de verificación
                    string query = "SELECT COUNT(1) FROM Usuario WHERE username = @username COLLATE Latin1_General_BIN AND VerificationCode IS NOT NULL";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    // Ejecutar la consulta y obtener el resultado
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    // Si cuenta es mayor que 0, significa que existe un VerificationCode
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepción en caso de error
                MessageBox.Show("EPV010 - Error de excepción",
                         "Error",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                return false; // En caso de error, devuelve false por defecto
            }
        }

        public void DisableUser(string username)
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Usuario SET userStatus = 0 WHERE username = @username COLLATE Latin1_General_BIN";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                // Manejar el error
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
            }
            catch (Exception ex)
            {
                // Manejar el error
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
            }
            finally
            {
                getConnection().Close();
            }
        }

        public void ResetUserAttempts(string username)
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Usuario SET userAttempts = 0 WHERE username = @username COLLATE Latin1_General_BIN";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                // Manejar el error
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
            }
            catch (Exception ex)
            {
                // Manejar el error
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
            }
            finally
            {
                getConnection().Close();
            }
        }

        public int ValidarLogin()
        {
            try
            {
                Command.Connection = getConnection();
                // Primero, verificar si el usuario existe
                //COLLATE Latin1_General_BIN Esta collation asegura que las comparaciones del campo username sean sensibles a mayúsculas y minúsculas.
                string queryUserExists = "SELECT COUNT(*) FROM ViewLogin WHERE username = @username COLLATE Latin1_General_BIN";
                SqlCommand cmdUserExists = new SqlCommand(queryUserExists, Command.Connection);
                cmdUserExists.Parameters.AddWithValue("username", Username);
                int userCount = (int)cmdUserExists.ExecuteScalar();

                if (userCount == 0)
                {
                    // El usuario no existe
                    return 0;
                }

                // Si el usuario existe, verificar la contraseña
                string query = "SELECT * FROM ViewLogin WHERE username = @username COLLATE Latin1_General_BIN AND password = @password AND userStatus = @status";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("username", Username);
                cmd.Parameters.AddWithValue("password", Password);
                cmd.Parameters.AddWithValue("status", true);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    // Usuario y contraseña correctos
                    while (rd.Read())
                    {
                        SessionVar.Username = rd.GetString(0);
                        SessionVar.Password = rd.GetString(1);
                        SessionVar.RoleId = rd.GetInt32(3);
                        SessionVar.Access = rd.GetString(4);
                        SessionVar.FullName = rd.GetString(5);
                    }
                    return 2;
                }
                else
                {
                    // Usuario existe, pero la contraseña es incorrecta
                    return 1;
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
                return -1; // Error en la base de datos
            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV005 - No se pudieron cargar los datos");
                return -1; // Otro tipo de error
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public int ValidarPrimerUsoSistema()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT COUNT(*) FROM ViewLogin";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                int totalUsuarios = (int)cmd.ExecuteScalar();
                return totalUsuarios;
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("EPV010 - Error de excepción");
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV010 - Error de excepción");
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
    }
}