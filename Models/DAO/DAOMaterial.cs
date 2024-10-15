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
    internal class DAOMaterial : DTOMaterial
    {
        readonly SqlCommand Command = new SqlCommand();
        public DataSet ObtenerInfoMaterial()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM  vistaMaterial";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "vistaMaterial");
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
        public int RegistrarMaterial()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO Material (material_nombre, material_descripcion) VALUES (@Nombre, @Descripcion)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Material_nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Material_descripcion);
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
        public DataSet BuscarMaterial(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $"SELECT * FROM vistaMaterial WHERE ID LIKE '%{valor}%' OR Nombre LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "vistaMaterial");
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
        public int ActualizarMaterial()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Material SET material_nombre = @Nombre, material_descripcion = @Descripcion WHERE material_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@Nombre", Material_nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Material_descripcion);
                cmd.Parameters.AddWithValue("@ID", Material_ID);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (SqlException ex)
            {
                // Mostrar el mensaje del error para fines de depuración
                MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente");
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
        public int EliminarMaterial()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE Material WHERE material_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@ID", Material_ID);
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
