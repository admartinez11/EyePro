using OpticaMultivisual.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Models.DAO
{
    internal class DAOModelo : DTOModelo
    {
        readonly SqlCommand Command = new SqlCommand();
        public DataSet ObtenerInfoModelo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM VistaModelo";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaModelo");
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
        public DataSet ObtenerMarca()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT marca_ID, marca_nombre FROM Marca";
                SqlCommand cmdSelect = new SqlCommand(query, Command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Marca");
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV005 - No se pudieron cargar los datos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                Command.Connection.Close();
            }
        }
        public int RegistrarModelo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO Modelo (mod_nombre, marca_ID) VALUES (@Nombre, @Descripcion)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Mod_nombre);
                cmd.Parameters.AddWithValue("@Marca", Marca_ID);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV006 - No se pudieron registrar los datos", "Error al registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public int RegistrarMarca()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO Marca (marca_nombre) VALUES (@Nombre)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Marca_nombre);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV006 - No se pudieron registrar los datos", "Error al registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public DataSet BuscarModelo(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $"SELECT * FROM Modelo WHERE mod_nombre LIKE '%{valor}%' OR mod_ID LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "Modelo");
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
        public int ActualizarModelo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Modelo SET mod_nombre = @Nombre, marca_ID = @Marca WHERE mod_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@Nombre", Mod_nombre);
                cmd.Parameters.AddWithValue("@Marca", Marca_ID);
                cmd.Parameters.AddWithValue("@ID", Mod_ID);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (SqlException ex)
            {
                // Mostrar el mensaje del error para fines de depuración
                MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente" + ex.Message);
                return -1;
            }
            finally
            {
                // Asegúrate de cerrar la conexión después de usarla
                if (Command.Connection.State == ConnectionState.Open)
                {
                    Command.Connection.Close();
                }
            }
        }
        public int EliminarModelo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE Modelo WHERE mod_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@ID", Mod_ID);
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
    }
}
