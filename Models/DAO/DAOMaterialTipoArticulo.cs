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
    internal class DAOMaterialTipoArticulo : DTOMaterialTipoArticulo
    {
        readonly SqlCommand Command = new SqlCommand();

        public DataSet ObtenerInfoTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM VistaTipoArtMat";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaTipoArtMat");
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
        public DataSet ObtenerTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT tipoart_ID, tipoart_nombre FROM TipoArt";
                SqlCommand cmdSelect = new SqlCommand(query, Command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "TipoArt");
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
        public DataSet ObtenerMaterial()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT material_ID, material_nombre FROM Material";
                SqlCommand cmdSelect = new SqlCommand(query, Command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Material");
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
        public int RegistrarMaterialTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO MaterialTipoArt (tipoart_ID, material_ID) VALUES (@tipoart, @material)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@tipoart", Tipoart_ID);
                cmd.Parameters.AddWithValue("@material", Material_ID);
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
        public int EliminarMaterialTipoArticulo()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE MaterialTipoArt WHERE materialtipoart_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@ID", Materialtipoart_ID);
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
