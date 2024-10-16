using OpticaMultivisual.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Views.Dashboard.PedidoDet;

namespace OpticaMultivisual.Models.DAO
{
    internal class DAOPedidoDet : DTOPedidoDet
    {
        readonly SqlCommand Command = new SqlCommand();

        public DataSet ObtenerDatos()
        {
            try
            {
                //Accedemos a la conexión que ya se tiene
                Command.Connection = getConnection();
                //Instrucción que se hará hacia la base de datos
                string query = "SELECT * FROM ViewPedidoDet ORDER BY [Pedido ID] asc";
                //Comando sql en el cual se pasa la instrucción y la conexión
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                //Se ejecuta el comando y con ExecuteNonQuery se verifica su retorno
                //ExecuteNonQuery devuelve un valor entero.
                cmd.Parameters.AddWithValue("valor", true);
                cmd.ExecuteNonQuery();
                //Se utiliza un adaptador sql para rellenar el dataset
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //Se crea un objeto Dataset que es donde se devolverán los resultados
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciendole de que tabla provienen los datos
                adp.Fill(ds, "ViewPedidoDet");
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

        public DataSet ObtenerArticulos()
        {
            try
            {
                Command.Connection = getConnection();
                //Definir instrucción de lo que se quiere hacer
                string query = "SELECT art_codigo FROM Articulo";
                //Creando un objeto de tipo comando donde recibe la instrucción y la conexión
                SqlCommand cmdSelect = new SqlCommand(query, Command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Articulo");
                return ds;
            }
            catch (Exception)
            {
                MessageBox.Show("EPV005 - No se pudieron cargar los datos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public DataSet ObtenerConsulta()
        {
            try
            {
                Command.Connection = getConnection();
                // Definir instrucción de lo que se quiere hacer
                string query = "SELECT con_ID, cli_DUI FROM Consulta";
                //string query = "SELECT * FROM ViewPedidoDet";

                // Creando un objeto de tipo comando donde recibe la instrucción y la conexión
                SqlCommand cmdSelect = new SqlCommand(query, Command.Connection);

                // Llenando el DataSet con SqlDataAdapter
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Consulta");

                // Retornar el DataSet
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"EPV005 - No se pudieron cargar los datos. Error: {ex.Message}", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                // Asegurarse de cerrar la conexión
                Command.Connection.Close();
            }
        }


        public int ObtenerSoloIDPorConcatenacion(string clienteDUIConcatenado)
        {
            int idConsulta = -1;
            try
            {
                // Extraemos el ID de la consulta desde el valor concatenado "con_ID - DUI"
                string[] partes = clienteDUIConcatenado.Split('-');
                if (partes.Length > 0)
                {
                    string idConsultaString = partes[0].Trim(); // Extraemos el ID de la consulta

                    // Verificamos si el valor extraído es un número entero válido
                    if (int.TryParse(idConsultaString, out idConsulta))
                    {
                        return idConsulta; // Devolvemos el ID de la consulta (con_ID)
                    }
                    else
                    {
                        MessageBox.Show("El ID extraído no es un número válido.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("El formato del DUI concatenado no es válido.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idConsulta; // Devolvemos -1 si no se encuentra un ID válido
        }

        public int InsertarPedido()
        {
            try
            {
                Command.Connection = getConnection();
                string query1 = "EXEC InsertarPD @con_ID, @pd_fpedido, @pd_fprogramada, @art_codigo, @art_cant, @pd_obser, @pd_recetalab";
                SqlCommand cmd = new SqlCommand(query1, Command.Connection);

                cmd.Parameters.AddWithValue("@con_ID", con_ID1);
                cmd.Parameters.AddWithValue("@pd_fpedido", pd_fpedido1);
                cmd.Parameters.AddWithValue("@pd_fprogramada", pd_fprogramada1);
                cmd.Parameters.AddWithValue("@art_codigo", art_codigo1);
                cmd.Parameters.AddWithValue("@art_cant", art_cant1);
                cmd.Parameters.AddWithValue("@pd_obser", pd_obser1);
                cmd.Parameters.AddWithValue("@pd_recetalab", pd_recetalab1);

                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV010 - Error de excepción",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return -1;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public int ActualizarPD()
        {
            try
            {
                Command.Connection = getConnection();
                string query4 = "UPDATE PedidoDet SET con_ID = @con_ID, pd_fpedido = @pd_fpedido, pd_fprogramada = @pd_fprogramada, art_codigo = @art_codigo, art_cant = @art_cant, pd_obser = @pd_obser, pd_recetalab = @pd_recetalab WHERE pd_ID = @pd_ID";
                SqlCommand cmd = new SqlCommand(query4, Command.Connection);

                cmd.Parameters.AddWithValue("@pd_ID", pd_ID1);
                cmd.Parameters.AddWithValue("@con_ID", con_ID1);
                cmd.Parameters.AddWithValue("@pd_fpedido", pd_fpedido1);
                cmd.Parameters.AddWithValue("@pd_fprogramada", pd_fprogramada1);
                cmd.Parameters.AddWithValue("@art_codigo", art_codigo1);
                cmd.Parameters.AddWithValue("@art_cant", art_cant1);
                cmd.Parameters.AddWithValue("@pd_obser", pd_obser1);
                cmd.Parameters.AddWithValue("@pd_recetalab", pd_recetalab1);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public int EliminarPD()
        {
            try
            {
                Command.Connection = getConnection();
                string query4 = "DELETE PedidoDet WHERE pd_ID = @param1";
                SqlCommand cmd = new SqlCommand(query4, Command.Connection);
                cmd.Parameters.AddWithValue("param1", pd_ID1);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public DataSet BuscarDUI(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $@"SELECT * FROM ViewPedidoDet WHERE DUI LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "ViewPedidoDet");
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

        public DataSet BuscarDUI2(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $@"SELECT * FROM Consulta WHERE cli_DUI LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "Consulta");
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
    }
}
