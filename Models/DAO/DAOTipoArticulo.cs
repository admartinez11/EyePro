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
    internal class DAOTipoArticulo : DTOTipoArticulo
    {
        readonly SqlCommand Command = new SqlCommand();

        public DataSet ObtenerInfoTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM VistaTipoArt";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaTipoArt");
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

        public int RegistrarTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO TipoArt (tipoart_nombre, tipoart_descripcion) VALUES (@Nombre, @Descripcion)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Tipoart_nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Tipoart_descripcion);
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
                string query = $"SELECT * FROM TipoArt WHERE tipoart_nombre LIKE '%{valor}%' OR tipoart_ID LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "TipoArt");
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

        public int ActualizarTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                MessageBox.Show($"{Tipoart_descripcion} {Tipoart_ID} {Tipoart_nombre}", "Error al cargar valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string query = "UPDATE TipoArt SET tipoart_nombre = @Nombre, tipoart_descripcion = @Descripcion " + "WHERE tipoart_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@Nombre", Tipoart_nombre);
                cmd.Parameters.AddWithValue("@Descripcion", Tipoart_descripcion);
                cmd.Parameters.AddWithValue("@ID", Tipoart_ID);
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
                Command.Connection.Close();
            }
        }

        public int EliminarMaterialTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE TipoArt WHERE tipoart_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@ID", Tipoart_ID);
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
