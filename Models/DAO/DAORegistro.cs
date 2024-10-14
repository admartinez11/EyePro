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
using System.Windows.Forms;
using AdministrarClientes.Modelo.DTO;



namespace OpticaMultivisual.Models.DAO
{
    class DAORegistro : DTORegistro
    {
        readonly SqlCommand Command = new SqlCommand();
        public DataSet ObtenerInfoClientes()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM VistaClientes";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaClientes");
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                getConnection().Close();
            }

        }
        public int EliminarUsuario()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE Cliente WHERE cli_dui = @param1";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("param1", DUI);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public int RegistrarCliente()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO Cliente (cli_nombre, cli_apellido, cli_tel, cli_edad, cli_correo, cli_profesion, cli_dui, cli_padecimientos, cli_genero, cli_menor) VALUES (@Nombre, @Apellido, @Telefono, @Edad, @Correo_E, @Profesion, @DUI, @Padecimientos, @Genero, @menor)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@Apellido", Apellido);
                cmd.Parameters.AddWithValue("@Telefono", Telefono);
                cmd.Parameters.AddWithValue("@Edad", Edad);
                cmd.Parameters.AddWithValue("@Correo_E", Correo_E);
                cmd.Parameters.AddWithValue("@Profesion", Profesion);
                cmd.Parameters.AddWithValue("@DUI", DUI);
                cmd.Parameters.AddWithValue("@Padecimientos", Padecimientos);
                cmd.Parameters.AddWithValue("@Genero", Genero);
                cmd.Parameters.AddWithValue("@menor", Menor);

                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error al registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public DataSet BuscarClientes(string valor)
        {
            try
            {

                Command.Connection = getConnection();
                string query = $"SELECT * FROM Cliente WHERE cli_nombre LIKE '%{valor}%' OR cli_dui LIKE '%{valor}%' OR cli_apellido LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciendole de que tabla provienen los datos
                adp.Fill(ds, "Cliente");
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public int ActualizarCliente()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Cliente SET " +
                               "cli_nombre = @param1, " +
                               "cli_apellido = @param2, " +
                               "cli_correo = @param3, " +
                               "cli_edad = @param4, " +
                               "cli_genero = @param5, " +
                               "cli_profesion = @param6, " +
                               "cli_padecimientos = @param7, " +
                               "cli_tel = @param8, " +
                               "cli_menor = @param9 " +
                               "WHERE cli_dui = @param10";

                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@param1", Nombre);
                cmd.Parameters.AddWithValue("@param2", Apellido);
                cmd.Parameters.AddWithValue("@param3", Correo_E);
                cmd.Parameters.AddWithValue("@param4", Edad);
                cmd.Parameters.AddWithValue("@param5", Genero);
                cmd.Parameters.AddWithValue("@param6", Profesion);
                cmd.Parameters.AddWithValue("@param7", Padecimientos);
                cmd.Parameters.AddWithValue("@param8", Telefono);
                cmd.Parameters.AddWithValue("@param9", Menor);
                cmd.Parameters.AddWithValue("@param10", DUI);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (SqlException ex)
            {
                // Mostrar el mensaje del error para fines de depuración
                Console.WriteLine("Error de SQL: " + ex.Message);
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public string AsignarSufijoDUI(string duiBase)
        {
            try
            {
                Command.Connection = getConnection();

                // Extraer todos los DUIs que empiecen con el mismo base
                string query = "SELECT cli_dui FROM Cliente WHERE cli_dui LIKE @duiBase + '%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@duiBase", duiBase);

                SqlDataReader reader = cmd.ExecuteReader();
                HashSet<int> sufijosExistentes = new HashSet<int>();

                // Recorrer los DUI ya existentes
                while (reader.Read())
                {
                    string duiExistente = reader["cli_dui"].ToString();
                    string[] partes = duiExistente.Split('-');

                    if (partes.Length == 3 && int.TryParse(partes[2], out int sufijo))
                    {
                        sufijosExistentes.Add(sufijo); // Agregar sufijo si ya existe
                    }
                    else
                    {
                        sufijosExistentes.Add(0); // Si no hay sufijo, agregar 0
                    }
                }

                reader.Close();

                // Buscar el siguiente sufijo no usado
                for (int i = 1; i <= 99; i++)
                {
                    if (!sufijosExistentes.Contains(i))
                    {
                        string nuevoDUI = $"{duiBase}-{i:D2}";
                        return nuevoDUI; // Retorna el nuevo DUI con sufijo
                    }
                }

                // Si no se encontró sufijo disponible
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar sufijo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (Command.Connection != null && Command.Connection.State == ConnectionState.Open)
                {
                    Command.Connection.Close();
                }
            }
        }

    }
}
