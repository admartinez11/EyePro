using OpticaMultivisual.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Models.DAO
{
    internal class DAOConsulta : DTOConsulta
    {
        SqlCommand command = new SqlCommand();

        public DataSet ObtenerDUI()
        {
            try
            {
                command.Connection = getConnection();
                //Definir instrucción de lo que se quiere hacer
                string query = "SELECT cli_dui FROM Cliente";
                //Creando un objeto de tipo comando donde recibe la instrucción y la conexión
                SqlCommand cmdSelect = new SqlCommand(query, command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Cliente");
                return ds;
            }
            catch (Exception)
            {
                MessageBox.Show($"Error al obtener el dui del cliente, verifique su conexión a internet o que el acceso al servidor o base de datos esten activos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public DataSet ObtenerVisita()
        {
            try
            {
                command.Connection = getConnection();
                string query = "SELECT vis_ID, vis_dui FROM Visita";
                SqlCommand cmdSelect = new SqlCommand(query, command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Visita");
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el DUI de la Visita: {ex.Message}", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public DataSet ObtenerEmpleado()
        {
            try
            {
                command.Connection = getConnection();
                //Definir instrucción de lo que se quiere hacer
                string query = "SELECT emp_ID, emp_nombre FROM Empleado";
                //Creando un objeto de tipo comando donde recibe la instrucción y la conexión
                SqlCommand cmdSelect = new SqlCommand(query, command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Empleado");
                return ds;
            }
            catch (Exception)
            {
                MessageBox.Show($"Error al obtener el nombre del empleado, verifique su conexión a internet o que el acceso al servidor o base de datos esten activos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public DataSet ObtenerEstado()
        {
            try
            {
                command.Connection = getConnection();
                //Definir instrucción de lo que se quiere hacer
                string query = "SELECT est_ID, est_Nombre FROM Estado";
                //Creando un objeto de tipo comando donde recibe la instrucción y la conexión
                SqlCommand cmdSelect = new SqlCommand(query, command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Estado");
                return ds;
            }
            catch (Exception)
            {
                MessageBox.Show($"Error al obtener el valor del estado, verifique su conexión a internet o que el acceso al servidor o base de datos esten activos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public int RegistrarCliente()
        {
            try
            {
                command.Connection = getConnection();
                string query = "INSERT INTO Consulta (cli_DUI, vis_ID, con_fecha, con_obser, emp_ID, con_hora, est_ID) VALUES (@DUI, @Visita, @Fecha, @Observacion, @Empleado, @Hora, @Estado)";
                SqlCommand cmd = new SqlCommand(query, command.Connection);

                cmd.Parameters.AddWithValue("@DUI", Cli_DUI);
                cmd.Parameters.AddWithValue("@Visita", Vis_ID);
                cmd.Parameters.AddWithValue("@Fecha", Con_fecha);
                cmd.Parameters.AddWithValue("@Observacion", Con_obser);
                cmd.Parameters.AddWithValue("@Empleado", Emp_ID);
                cmd.Parameters.AddWithValue("@Hora", Con_hora);
                cmd.Parameters.AddWithValue("@Estado", Est_ID);

                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error al guardar consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public int ActualizarConsulta()
        {
            try
            {
                command.Connection = getConnection();

                string query = "UPDATE Consulta SET " +
                               "cli_DUI = @param1, " +
                               "vis_ID = @param2, " +
                               "con_fecha = @param3, " +
                               "con_obser = @param4, " +
                               "emp_ID = @param5, " +
                               "con_hora = @param6, " +
                               "est_ID = @param7 " +
                               "WHERE con_ID = @param8";

                SqlCommand cmd = new SqlCommand(query, command.Connection);

                // Asignando los valores a cada parámetro
                cmd.Parameters.AddWithValue("@param1", Cli_DUI);
                cmd.Parameters.AddWithValue("@param2", Vis_ID);
                cmd.Parameters.AddWithValue("@param3", Con_fecha);
                cmd.Parameters.AddWithValue("@param4", Con_obser);
                cmd.Parameters.AddWithValue("@param5", Emp_ID);
                cmd.Parameters.AddWithValue("@param6", Con_hora);
                cmd.Parameters.AddWithValue("@param7", Est_ID);
                cmd.Parameters.AddWithValue("@param8", Con_ID);

                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de SQL: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                getConnection().Close();
            }
        }
        public string ObtenerDUIPorVisita(int vis_ID)
        {
            try
            {
                command.Connection = getConnection();
                string query = "SELECT vis_dui FROM Visita WHERE vis_ID = @vis_ID";
                SqlCommand cmd = new SqlCommand(query, command.Connection);
                cmd.Parameters.AddWithValue("@vis_ID", vis_ID);
                return (string)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                // Manejo de errores
                MessageBox.Show("Error al consultar el DUI: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                getConnection().Close();
            }
        }


        public DataSet ObtenerInfoConsulta()
        {
            try
            {
                command.Connection = getConnection();
                string query = "SELECT * FROM VistaConsultas";
                SqlCommand cmd = new SqlCommand(query, command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaConsultas");
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
        public DataSet ObtenerInfoConsultaPendiente()
        {
            try
            {
                command.Connection = getConnection();
                string query = "SELECT * \r\nFROM VistaConsultas \r\nWHERE [Estado de Consulta] = 0";
                SqlCommand cmd = new SqlCommand(query, command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaConsultas");
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
        public DataSet ObtenerInfoConsultaRealizada()
        {
            try
            {
                command.Connection = getConnection();
                string query = "SELECT * \r\nFROM VistaConsultas \r\nWHERE [Estado de Consulta] = 1";
                SqlCommand cmd = new SqlCommand(query, command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaConsultas");
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
        public int EliminarConsulta()
        {
            try
            {
                command.Connection = getConnection();
                string query = "DELETE Consulta WHERE con_ID = @param1";
                SqlCommand cmd = new SqlCommand(query, command.Connection);
                cmd.Parameters.AddWithValue("param1", Con_ID);
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
        public DataSet BuscarConsulta(string valor)
        {
            try
            {
                string query = $"SELECT * FROM VistaConsultas WHERE [DUI del Cliente] LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, getConnection());
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "VistaConsultas");
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                getConnection().Close();
            }
        }
    }
}

