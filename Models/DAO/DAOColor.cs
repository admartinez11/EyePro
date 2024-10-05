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
    internal class DAOColor : DTOColor
    {
        readonly SqlCommand Command = new SqlCommand();
        public DataSet ObtenerInfoColor()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM  VistaColor";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaColor");
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

        public int RegistrarColor()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO Color (color_nombre, color_descripcion) VALUES (@Nombre, @Descripcion)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Color_nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Color_descripcion);
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

        public DataSet BuscarTipoArticulo(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $"SELECT * FROM Color WHERE color_nombre LIKE '%{valor}%' OR color_ID LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "Color");
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

        public int ActualizarColor()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Color SET color_nombre = @Nombre, color_descripcion = @Descripcion WHERE color_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@Nombre", Color_nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Color_descripcion);
                cmd.Parameters.AddWithValue("@ID", Color_ID);
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

        public int EliminarColor()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE Color WHERE Color_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@ID", Color_ID);
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
