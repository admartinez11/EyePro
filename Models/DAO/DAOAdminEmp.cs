using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpticaMultivisual.Models.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography;
using OpticaMultivisual.Controllers.Helper;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace OpticaMultivisual.Models.DAO
{
    // Clase DAOAdminUsers que hereda de DTOAdminUsers
    class DAOAdminEmp : DTOAdminEmp
    {
        // Comando SQL que se utiliza para ejecutar consultas y comandos en la base de datos
        readonly SqlCommand Command = new SqlCommand();

        public string ObtenerCorreoPorUsername(string username)
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT emp_correo FROM Empleado WHERE username = @username";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return reader["emp_correo"].ToString(); // Retorna el correo electrónico del usuario
                }
                else
                {
                    return null; // Usuario no encontrado
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("EPV005 - No se pudieron cargar los datos",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        // Método para verificar si el usuario existe en la base de datos
        public bool VerificarUsuario(string username)
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    string query = "SELECT COUNT(1) FROM Usuario WHERE username = @username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    try
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count == 1;
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepción
                        MessageBox.Show("EPV010 - Error de excepción",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                getConnection().Close();
            }
        }

        // Método para validar las credenciales de un administrador
        public bool ValidateAdminCredentials(string username, string password)
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    CommonClasses commonClasses = new CommonClasses();
                    string query = "SELECT COUNT(1) FROM ViewLogin WHERE username = @username AND password = @password AND rol_ID = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", commonClasses.ComputeSha256Hash(password));
                    try
                    {
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count == 1;
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepción
                        MessageBox.Show("EPV010 - Error de excepción",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                getConnection().Close();
            }
        }

        // Método para actualizar la contraseña y el estado del usuario
        public bool ActualizarClaveUsuario(string username)
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    CommonClasses commonClasses = new CommonClasses();
                    string nuevaClave = username + "OP123";  // Generar la nueva clave por defecto
                    string query = "UPDATE Usuario SET password = @nuevaClave, userStatus = 1, userAttempts = 0 WHERE username = @username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nuevaClave", commonClasses.ComputeSha256Hash(nuevaClave));
                    command.Parameters.AddWithValue("@username", username);
                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepción
                       MessageBox.Show("EPV010 - Error de excepción",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally 
            { 
                getConnection().Close();
            }
        }

        public bool VerificarPINSeguridad()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM Usuario WHERE username = @username COLLATE Latin1_General_BIN AND VerificationCode = @pin AND userStatus = @status";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("username", User);
                cmd.Parameters.AddWithValue("pin", VerificationCode);
                cmd.Parameters.AddWithValue("status", true);
                SqlDataReader rd = cmd.ExecuteReader();
                return rd.HasRows;
            }
            catch (SqlException)
            {
                MessageBox.Show("EPV008 - No se pudo almacenar el código de verificación",
                                 "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        // Método para obtener el estado del usuario desde la base de datos
        public bool IsUserActive(string username)
        {
            // Suponiendo que tienes una clase de acceso a datos llamada DAOAdminEmp
            DAOAdminEmp dao = new DAOAdminEmp();
            bool isActive = false;

            // Consulta SQL para obtener el estado del usuario
            string query = "SELECT userStatus FROM Usuario WHERE username = @username";
            try
            {
                // Conexión a la base de datos
                using (SqlConnection connection = getConnection())
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        isActive = Convert.ToBoolean(result);
                    }
                }

                return isActive;
            }
            catch (Exception)
            {
                return false;
            } 
            finally
            {
                getConnection().Close();
            }
        }

        public bool RegistrarPIN()
        {
            try
            {
                Command.Connection = getConnection();
                string queryupdate = "UPDATE Usuario SET VerificationCode = @VerificationCode, CodeExpiration = @CodeExpiration WHERE username = @username";
                SqlCommand cmdupdate = new SqlCommand(queryupdate, Command.Connection);
                cmdupdate.Parameters.AddWithValue("VerificationCode", VerificationCode);
                cmdupdate.Parameters.AddWithValue("username", User);
                // Definir la fecha y hora de expiración del código (15 minutos a partir de ahora)
                DateTime expiryDate = DateTime.Now.AddMinutes(15);
                cmdupdate.Parameters.AddWithValue("@CodeExpiration", expiryDate);

                try
                {
                    int result = cmdupdate.ExecuteNonQuery();
                    return result > 0; // Retorna true si la operación fue exitosa
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    MessageBox.Show("EPV008 - No se pudo almacenar el código de verificación",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public bool RestablecerContrasena()
        {
            try
            {
                Command.Connection = getConnection();
                string queryupdate = "UPDATE Usuario SET password = @valor1 WHERE username = @username";
                SqlCommand cmdupdate = new SqlCommand(queryupdate, Command.Connection);
                cmdupdate.Parameters.AddWithValue("valor1", Password);
                cmdupdate.Parameters.AddWithValue("username", User);
                return cmdupdate.ExecuteNonQuery() > 0 ? true : false;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool UpdatePassword(string username, string newPassword)
        {
            bool isUpdated = false;

            try
            {
                using (SqlConnection connection = getConnection())
                {
                    string query = "UPDATE Usuario SET password = @password WHERE username = @username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        CommonClasses common = new CommonClasses();
                        // Encriptar la nueva contraseña antes de guardarla
                        string encryptedPassword = common.ComputeSha256Hash(newPassword);

                        command.Parameters.AddWithValue("@password", encryptedPassword);
                        command.Parameters.AddWithValue("@username", username);
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("EPV009 - Error al conectar con la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV010 - Error de excepción", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                getConnection().Close();
            }

            return isUpdated;
        }

        // Método para verificar la respuesta de seguridad
        public bool VerifySecurityAnswer(string username, string answer)
        {
            // Verifica si username o answer son nulos o están vacíos
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(answer))
            {
                MessageBox.Show("El username o la respuesta son nulos o están vacíos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool isCorrect = false;
            username = username.Trim(); // Elimina espacios en blanco al principio y al final
            answer = answer.Trim();     // Elimina espacios en blanco al principio y al final

            try
            {
                using (SqlConnection connection = getConnection())
                {
                    string query = "SELECT COUNT(*) FROM Usuario WHERE username = @username AND SecurityAnswer = @SecurityAnswer";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@SecurityAnswer", answer);
                        int count = (int)command.ExecuteScalar();

                        isCorrect = count > 0; // La respuesta es correcta si hay al menos una fila con la combinación de usuario y respuesta
                    }
                }
            }
            catch (SqlException ex)
            {
                // Captura y maneja excepciones de SQL
                MessageBox.Show("EPV009 - Error al conectar con la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción
                MessageBox.Show("Error: EPV010 - Error de excepción", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                getConnection().Close();
            }

            return isCorrect;
        }

        public string ObtenerPreguntaSeguridad(string username)
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    string query = "SELECT SecurityQuestion FROM Usuario WHERE username = @username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    string securityQuestion = command.ExecuteScalar() as string;

                    return securityQuestion;
                }
            }
            finally
            {
                getConnection().Close();
            }
        }

        public bool UpdateSecurityQuestion(string username, string question, string answer)
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    connection.Open();
                    string query = "UPDATE Usuario SET SecurityQuestion = @question, SecurityAnswer = @answer WHERE Username = @username";
                    CommonClasses common = new CommonClasses();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@question", question);
                        cmd.Parameters.AddWithValue("@answer", common.ComputeSha256Hash(answer)); // Encriptar la respuesta
                        cmd.Parameters.AddWithValue("@username", username);

                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la pregunta de seguridad: {ex.Message}");
                return false;
            }
        }

        // Método para llenar un combo con datos de la base de datos
        public DataSet LlenarCombo()
        {
            try
            {
                //Se crea una conexión para garantizar que efectivamente haya conexión a la base de datos.
                Command.Connection = getConnection();
                //Se crea el query que indica la acción que el sistema desea realizar con la base de datos
                //En caso sea una consulta parametrizada se deberá respetar la sintaxis sobre como colocar parametros en la instrucción sql (REVISAR LOS DEMÁS MANTENIMIENTOS PARA VER COMO SE CREAN PARAMETROS Y SE LES DA VALORES).
                string query = "SELECT * FROM Rol";
                //Se crea un comando de tipo sql al cual se le pasa el query y la conexión, esto para que el sistema sepa que hacer y donde hacerlo.
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                //ExecuteNonQuery indicará cuantos filas fueron afectadas, es decir, cuantas filas de datos se ingresaron o encontraron, por lo general cuando es una consulta su valor puede ser 1 o mayor a 1.
                cmd.ExecuteNonQuery();
                //Se crea un objeto SqlDataAdapter para poder llenar el DataSet que posteriormente utilizaremos, además recibe el comando sql
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //Se crea un DataSet que será el objeto de retorno del método
                DataSet ds = new DataSet();
                //Rellenamos el DataSet con los datos encontrados con el SqlDataAdapter, además, indicamos de donde provienen los datos
                adp.Fill(ds, "Rol");
                //Retornamos el objeto DataSet
                return ds;
            }
            catch (Exception)
            {
                //Se retorna null si durate la ejecución del try ocurrió algún error
                return null;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                getConnection().Close();
                LlenarComboGenero();
            }
        }

        public DataSet LlenarComboGenero()
        {
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    connection.Open();
                    // Query para seleccionar los distintos géneros de la tabla Empleado
                    string query = "SELECT DISTINCT emp_genero FROM Empleado";
                    // Se crea un comando de tipo sql al cual se le pasa el query y la conexión
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Se crea un objeto SqlDataAdapter para llenar el DataSet con el resultado del comando
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        // Se crea un DataSet que será el objeto de retorno del método
                        DataSet ds = new DataSet();
                        // Se rellena el DataSet con los datos encontrados por el SqlDataAdapter
                        adp.Fill(ds, "Genero");
                        // Se retorna el objeto DataSet
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                // Se retorna null si durante la ejecución del try ocurre algún error y se puede loguear el error
                //MessageBox.Show("EPV010 - Error de excepción",
                //    "Error",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                getConnection().Close();
            }
        }

        public int ObtenerIdEmpresa()
        {
            int idEmpresa = -1; // Variable para almacenar el ID de la empresa
            try
            {
                using (SqlConnection connection = getConnection())
                {
                    string query = "SELECT TOP 1 idNegocio FROM InfoNegocio ORDER BY idNegocio";
                    SqlCommand command = new SqlCommand(query, connection);

                    try
                    {
                        object result = command.ExecuteScalar();
                        idEmpresa = result != null ? Convert.ToInt32(result) : -1; // Asigna el valor a la variable
                    }
                    catch (Exception)
                    {
                        return -1; // Retorna -1 en caso de error al ejecutar la consulta
                    }
                }
            }
            catch (Exception)
            {
                return -1; // Retorna -1 en caso de error de conexión
            }
            finally
            {
                getConnection().Close();
            }
            return idEmpresa; // Retorna el ID de la empresa
        }

        /// <summary>
        /// Registrar usuario corresponde al primer mantenimiento del CRUD
        /// Inserción de datos a la base de datos
        /// </summary>
        /// <returns></returns>

        public int RegistrarUsuario()
        {
            try
            {
                // Obtener el idNegocio de la primera empresa registrada en la base de datos
                int idNegocio = ObtenerIdEmpresa();
                // Verificar si se pudo obtener un idNegocio válido
                if (idNegocio == -1)
                {
                    MessageBox.Show("No se pudo obtener el ID del negocio.",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return -1;
                }
                //Se crea una conexión para garantizar que efectivamente haya conexión a la base.
                Command.Connection = getConnection();
                CommonClasses common = new CommonClasses();
                //Se crea el query que indica la acción que el sistema desea realizar con la base de datos
                //el query posee parametros para evitar algún tipo de ataque como SQL Injection
                string query2 = "INSERT INTO Usuario (username, password, userStatus, userAttempts, SecurityQuestion, SecurityAnswer, idNegocio) " +
                "VALUES (@username, @password, @userStatus, @userAttempts, @SecurityQuestion, @SecurityAnswer, @idNegocio)";
                //Se crea un comando de tipo sql al cual se le pasa el query y la conexión, esto para que el sistema sepa que hacer y donde hacerlo.
                SqlCommand cmd2 = new SqlCommand(query2, Command.Connection);
                /*Se le da un valor a los parametros contenidos en el query, es importante mencionar que
                lo que esta entre comillas es el nombre del parametro y lo que esta después de la coma es
                el valor que se le asignará al parametro, estos valores vienen del DTO respectivo.*/
                cmd2.Parameters.AddWithValue("username", User);
                cmd2.Parameters.AddWithValue("password", Password);
                cmd2.Parameters.AddWithValue("userStatus", UserStatus);
                cmd2.Parameters.AddWithValue("userAttempts", UserAttempts);
                cmd2.Parameters.AddWithValue("SecurityQuestion", SecurityQuestion);
                cmd2.Parameters.AddWithValue("SecurityAnswer", SecurityAnswer);
                cmd2.Parameters.AddWithValue("idNegocio", idNegocio);
                //Se ejecuta el comando ya con todos los valores de sus parametros.
                /*ExecuteNonQuery indicará cuantos filas fueron afectadas, es decir, cuantas filas de datos se
                ingresaron, por lo general devolvera 1 porque se hace una inserción a la vez.*/
                int respuesta = cmd2.ExecuteNonQuery();
                //Se evalúa el valor de la variable respuesta que contiene el numero de filas afectadas
                if (respuesta == 1)
                {
                    //Si el valor de respuesta es 1, se procede a la inserción de los datos de la persona, como se puede observar en el diagrama de base de datos, primero es el usuario y despues la persona.
                    string query = "INSERT INTO Empleado (emp_nombre, emp_apellido, emp_genero, emp_fnacimiento, emp_correo, emp_telefono, emp_DUI, emp_direccion, rol_ID, username) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10)";
                    //Se crea un comando de tipo sql al cual se le pasa el query y la conexión, esto para que el sistema sepa que hacer y donde hacerlo.
                    SqlCommand cmd = new SqlCommand(query, Command.Connection);
                    //Se le da un valor a los parametros contenidos en el query, es importante mencionar que lo que esta entre comillas es el nombre del parametro y lo que esta después de la coma es el valor que se le asignará al parametro, estos valores vienen del DTO
                    cmd.Parameters.AddWithValue("@param1", Nombre);
                    cmd.Parameters.AddWithValue("@param2", Apellido);
                    cmd.Parameters.AddWithValue("@param3", Genero);
                    cmd.Parameters.AddWithValue("@param4", Nacimiento);
                    cmd.Parameters.AddWithValue("@param5", Correo);
                    cmd.Parameters.AddWithValue("@param6", Telefono);
                    cmd.Parameters.AddWithValue("@param7", Dui);
                    cmd.Parameters.AddWithValue("@param8", Direccion);
                    cmd.Parameters.AddWithValue("@param9", Rol);
                    cmd.Parameters.AddWithValue("@param10", User);
                    //Se ejecuta el comando ya con todos los valores de sus parametros.
                    //ExecuteNonQuery indicará cuantos filas fueron afectadas, es decir, cuantas filas de datos se ingresaron, por lo general devolvera 1 porque se hace una inserción a la vez.
                    respuesta = cmd.ExecuteNonQuery();
                    //Se retorna el valor de respuesta, que si su valor es 1 indica que los valores fueron ingresados.
                    return respuesta;
                }
                else
                {
                    //Se retorna cero si sus valores no pudieron ser ingresados
                    return 0;
                }
            }
            catch (Exception ex)
            {
                RollBack();
                MessageBox.Show("Error: EPV010 - Error de excepción");
                //Se retorna -1 en caso que en el segmento del try haya ocurrido algún error.
                return -1;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                Command.Connection.Close();
            }
        }

        // Método para verificar si el DUI ya está registrado
        public bool VerificarDuiExistente(string dui)
        {
            bool existe = false;
            try
            {
                using (var connection = getConnection())
                {
                    string query = "SELECT COUNT(*) FROM Empleado WHERE emp_DUI = @dui";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@dui", dui);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return existe;
        }

        // Método para verificar si el correo ya está registrado
        public bool VerificarCorreoExistente(string correo)
        {
            bool existe = false;
            try
            {
                using (var connection = getConnection())
                {
                    string query = "SELECT COUNT(*) FROM Empleado WHERE emp_correo = @correo";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return existe;
        }

        public void RollBack()
        {
            //Eliminar el usuario ingresado
            string query = "DELETE FROM Usuario WHERE username = @username";
            SqlCommand cmddel = new SqlCommand(query, Command.Connection);
            cmddel.Parameters.AddWithValue("username", User);
            int retorno = cmddel.ExecuteNonQuery();
        }

        public DataSet ObtenerPersonas()
        {
            try
            {
                //Accedemos a la conexión que ya se tiene
                Command.Connection = getConnection();
                //Instrucción que se hará hacia la base de datos
                string query = "SELECT * FROM ViewEmp";
                //Comando sql en el cual se pasa la instrucción y la conexión
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                //Se ejecuta el comando y con ExecuteNonQuery se verifica su retorno
                //ExecuteNonQuery devuelve un valor entero.
                cmd.ExecuteNonQuery();
                //Se utiliza un adaptador sql para rellenar el dataset
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //Se crea un objeto Dataset que es donde se devolverán los resultados
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciendole de que tabla provienen los datos
                adp.Fill(ds, "ViewEmp");
                //Devolvemos el Dataset
                return ds;
            }
            catch (Exception)
            {
                //Retornamos null si existiera algún error durante la ejecución
                return null;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                Command.Connection.Close();
            }
        }

        public int ActualizarEmpleado()
        {
            try
            {
                //Se crea una conexión para garantizar que efectivamente haya conexión a la base.
                Command.Connection = getConnection();
                //**
                //Se crea el query que indica la acción que el sistema desea realizar con la base de datos
                //el query posee parametros para evitar algún tipo de ataque como SQL Injection
                string query = "UPDATE Empleado SET " +
                                "emp_nombre = @param1, " +
                                "emp_apellido = @param2, " +
                                "emp_genero = @param3, " +
                                "emp_fnacimiento = @param4," +
                                "emp_DUI = @param5," +
                                "emp_direccion = @param6, " +
                                "emp_correo = @param7, " +
                                "emp_telefono = @param8 " +
                                "WHERE emp_ID = @param9";
                //Se crea un comando de tipo sql al cual se le pasa el query y la conexión, esto para que el sistema sepa que hacer y donde hacerlo.
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                //Se le da un valor a los parametros contenidos en el query, es importante mencionar que lo que esta entre comillas es el nombre del parametro y lo que esta después de la coma es el valor que se le asignará al parametro, estos valores vienen del DTO respectivo.
                cmd.Parameters.AddWithValue("@param1", Nombre);
                cmd.Parameters.AddWithValue("@param2", Apellido);
                cmd.Parameters.AddWithValue("@param3", Genero);
                cmd.Parameters.AddWithValue("@param4", Nacimiento);
                cmd.Parameters.AddWithValue("@param5", Dui);
                cmd.Parameters.AddWithValue("@param6", Direccion);
                cmd.Parameters.AddWithValue("@param7", Correo);
                cmd.Parameters.AddWithValue("@param8", Telefono);
                cmd.Parameters.AddWithValue("@param9", Id);
                //Se ejecuta el comando ya con todos los valores de sus parametros.
                //ExecuteNonQuery indicará cuantos filas fueron afectadas, es decir, cuantas filas de datos se ingresaron, por lo general devolvera 1 porque se hace una actualización a la vez.
                int respuesta = cmd.ExecuteNonQuery();
                //Se evalúa el valor de la variable respuesta que contiene el numero de filas afectadas
                if (respuesta == 1)
                {
                    //Si el valor de respuesta es 1 se procede a realizar la actualización del usuario
                    //**
                    //Se crea el query que indica la acción que el sistema desea realizar con la base de datos
                    //el query posee parametros para evitar algún tipo de ataque como SQL Injection
                    string query2 = "UPDATE Empleado SET " +
                                    "rol_ID = @param10 " +
                                    "WHERE username = @param11";
                    //Se crea un comando de tipo sql al cual se le pasa el query y la conexión, esto para que el sistema sepa que hacer y donde hacerlo.
                    SqlCommand cmd2 = new SqlCommand(query2, getConnection());
                    //Se le da un valor a los parametros contenidos en el query, es importante mencionar que lo que esta entre comillas es el nombre del parametro y lo que esta después de la coma es el valor que se le asignará al parametro, estos valores vienen del DTO respectivo.
                    cmd2.Parameters.AddWithValue("param10", Rol);
                    cmd2.Parameters.AddWithValue("param11", User);
                    //Se ejecuta el comando ya con todos los valores de sus parametros.
                    //ExecuteNonQuery indicará cuantos filas fueron afectadas, es decir, cuantas filas de datos se ingresaron, por lo general devolvera 1 porque se hace una inserción a la vez.
                    respuesta = cmd2.ExecuteNonQuery();
                    respuesta = 2;
                }
                return respuesta;
            }
            catch (Exception)
            {
                //Se retorna -1 en caso que en el segmento del try haya ocurrido algún error.
                return -1;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                getConnection().Close();
            }
        }

        public DataSet BuscarEmpleados(string valor)
        {
            try
            {
                //Accedemos a la conexión que ya se tiene
                Command.Connection = getConnection();
                //Instrucción que se hará hacia la base de datos
                string query5 = $"SELECT * FROM ViewEmp WHERE [Nombres] LIKE '%{valor}%' OR [Apellidos] LIKE '%{valor}%' OR [Documento] LIKE '%{valor}%'";
                //Comando sql en el cual se pasa la instrucción y la conexión
                SqlCommand cmd = new SqlCommand(query5, Command.Connection);
                //Se ejecuta el comando y con ExecuteNonQuery se verifica su retorno
                //ExecuteNonQuery devuelve un valor entero.
                cmd.ExecuteNonQuery();
                //Se utiliza un adaptador sql para rellenar el dataset
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //Se crea un objeto Dataset que es donde se devolverán los resultados
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciendole de que tabla provienen los datos
                adp.Fill(ds, "ViewEmp");
                //Devolvemos el Dataset
                return ds;
            }
            catch (Exception)
            {
                //Retornamos null si existiera algún error durante la ejecución
                return null;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                getConnection().Close();
            }
        }

        public int EliminarUsuario()
        {
            try
            {
                //Se crea una conexión para garantizar que efectivamente haya conexión a la base.
                Command.Connection = getConnection();
                //Se crea el query que indica la acción que el sistema desea realizar con la base de datos
                //el query posee parametros para evitar algún tipo de ataque como SQL Injection
                string query = "DELETE Empleado WHERE emp_ID = @param1";
                //Se crea un comando de tipo sql al cual se le pasa el query y la conexión, esto para que el sistema sepa que hacer y donde hacerlo.
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                //Se le da un valor a los parametros contenidos en el query, es importante mencionar que lo que esta entre comillas es el nombre del parametro y lo que esta después de la coma es el valor que se le asignará al parametro, estos valores vienen del DTO respectivo.
                cmd.Parameters.AddWithValue("param1", Id);
                //Se ejecuta el comando ya con todos los valores de sus parametros.
                //ExecuteNonQuery indicará cuantos filas fueron afectadas, es decir, cuantas filas de datos se ingresaron, por lo general devolvera 1 porque se hace una eliminación a la vez.
                int respuesta = cmd.ExecuteNonQuery();
                //Si la ejecución del comando no ha generado errores se procederá a retornar el valor de la variable respuesta que por lo general almacenará un 1 ya que solo se hace una acción a la vez.
                //return respuesta;
                if (respuesta == 1)
                {
                    Command.Connection = getConnection();
                    string querydel = "DELETE Usuario WHERE username = @param2";
                    SqlCommand cmdl = new SqlCommand(querydel, Command.Connection);
                    cmdl.Parameters.AddWithValue("param2", User);
                    respuesta = cmdl.ExecuteNonQuery();
                    respuesta = 1;
                }
                return respuesta;
            }
            catch (Exception)
            {
                //Se retorna -1 en caso que en el segmento del try haya ocurrido algún error.
                return -1;
            }
            finally
            {
                //Independientemente se haga o no el proceso cerramos la conexión
                getConnection().Close();
            }
        }
    }
}
