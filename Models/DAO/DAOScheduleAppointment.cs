using OpticaMultivisual.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OpticaMultivisual.Models.DAO
{
    class DAOScheduleAppointment : DTOScheduleAppointment
    {
        readonly SqlCommand Command = new SqlCommand();
        public DataSet ObtenerInfoVisita()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "SELECT * FROM ViewVisita";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "ViewVisita");
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
        public int EliminarVisita()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "DELETE Visita WHERE vis_dui = @DUI";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@DUI", Vis_dui);
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
        public int RegistrarVisita()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "INSERT INTO Visita (vis_fcita, vis_nombre, vis_apellido, vis_tel, vis_correo, vis_obser, vis_dui) VALUES (@Fecha, @Nombre, @Apellido, @Telefono, @Correo, @Observaciones, @DUI)";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);

                cmd.Parameters.AddWithValue("@Nombre", Vis_nombre);
                cmd.Parameters.AddWithValue("@Apellido", Vis_apellido);
                cmd.Parameters.AddWithValue("@Telefono", Vis_tel);
                cmd.Parameters.AddWithValue("@Correo", Vis_correo);
                cmd.Parameters.AddWithValue("@Fecha", Vis_fcita);
                cmd.Parameters.AddWithValue("@DUI", Vis_dui);
                cmd.Parameters.AddWithValue("@Observaciones", Vis_obser);
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
        public DataSet BuscarVisita(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $"SELECT * FROM ViewVisita WHERE DUI LIKE '%{valor}%' OR Nombre LIKE '%{valor}%' OR Apellido LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "ViewVisita");
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
        public int ActualizarVisita()
        {
            try
            {
                Command.Connection = getConnection();
                string query = "UPDATE Visita SET " +
                    "vis_fcita = @Fecha, " +
                    "vis_nombre = @Nombre, " +
                    "vis_apellido = @Apellido, " +
                    "vis_tel = @Telefono, " +
                    "vis_correo = @Correo, " +
                    "vis_obser = @Observaciones " +
                    "WHERE vis_dui = @DUI";

                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.Parameters.AddWithValue("@Nombre", Vis_nombre);
                cmd.Parameters.AddWithValue("@Apellido", Vis_apellido);
                cmd.Parameters.AddWithValue("@Telefono", Vis_tel);
                cmd.Parameters.AddWithValue("@Correo", Vis_correo);
                cmd.Parameters.AddWithValue("@Fecha", Vis_fcita);
                cmd.Parameters.AddWithValue("@DUI", Vis_dui);
                cmd.Parameters.AddWithValue("@Observaciones", Vis_obser);

                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (SqlException ex)
            {
                // Mostrar el mensaje del error para fines de depuración
                Console.WriteLine("EPV002 - Los datos no pudieron ser actualizados correctamente");
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
    }
}