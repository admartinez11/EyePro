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
    internal class DAOLens : DTOLens
    {
        readonly SqlCommand Command = new SqlCommand();

        public DataSet ObtenerPersonas()
        {
            try
            {
                //Accedemos a la conexión que ya se tiene
                Command.Connection = getConnection();
                //Instrucción que se hará hacia la base de datos
                string query = "SELECT * FROM ViewLensometria ORDER BY [ID] asc";
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
                adp.Fill(ds, "ViewLensometria");
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

        public DataSet ObtenerConsulta()
        {
            try
            {
                Command.Connection = getConnection();
                //Definir instrucción de lo que se quiere hacer
                string query = "SELECT con_ID, cli_dui FROM Consulta";
                //Creando un objeto de tipo comando donde recibe la instrucción y la conexión
                SqlCommand cmdSelect = new SqlCommand(query, Command.Connection);
                cmdSelect.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmdSelect);
                DataSet ds = new DataSet();
                adp.Fill(ds, "Consulta");
                return ds;
            }
            catch (Exception)
            {
                MessageBox.Show("Error: EPV005 - No se pudieron cargar los datos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                Command.Connection.Close();
            }
        }


        public int InsertarLens()
        {
            try
            {
                Command.Connection = getConnection();
                string query3 = "EXEC InsertarLens @con_ID, @OD_esfera, @OD_cilindro, @OD_eje, @OD_prisma, @OD_adicion, @OI_esfera, @OI_cilindro, @OI_eje, @OI_prisma, @OI_adicion";
                SqlCommand cmd = new SqlCommand(query3, Command.Connection);

                cmd.Parameters.AddWithValue("@con_ID", con_ID1);
                cmd.Parameters.AddWithValue("@OD_esfera", OD_esfera1);
                cmd.Parameters.AddWithValue("@OD_cilindro", OD_cilindro1);
                cmd.Parameters.AddWithValue("@OD_eje", OD_eje1);
                cmd.Parameters.AddWithValue("@OD_prisma", OD_prisma1);
                cmd.Parameters.AddWithValue("@OD_adicion", OD_adicion1);
                cmd.Parameters.AddWithValue("@OI_esfera", OI_esfera1);
                cmd.Parameters.AddWithValue("@OI_cilindro", OI_cilindro1);
                cmd.Parameters.AddWithValue("@OI_eje", OI_eje1);
                cmd.Parameters.AddWithValue("@OI_prisma", OI_prisma1);
                cmd.Parameters.AddWithValue("@OI_adicion", OI_adicion1);

                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;
            }
            catch (Exception x)
            {
                MessageBox.Show("EPV010 - Error de excepción",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                Command.Connection.Close();
            }
        }

        public int ActualizarLens()
        {
            try
            {
                Command.Connection = getConnection();
                string query4 = "EXEC ActualizarLens @lens_ID, @con_ID, @OD_esfera, @OD_cilindro, @OD_eje, @OD_prisma, @OD_adicion, @OI_esfera, @OI_cilindro, @OI_eje, @OI_prisma, @OI_adicion";
                SqlCommand cmd = new SqlCommand(query4, Command.Connection);

                cmd.Parameters.AddWithValue("@lens_ID", lens_ID1);
                cmd.Parameters.AddWithValue("@con_ID", con_ID1);
                cmd.Parameters.AddWithValue("@OD_esfera", OD_esfera1);
                cmd.Parameters.AddWithValue("@OD_cilindro", OD_cilindro1);
                cmd.Parameters.AddWithValue("@OD_eje", OD_eje1);
                cmd.Parameters.AddWithValue("@OD_prisma", OD_prisma1);
                cmd.Parameters.AddWithValue("@OD_adicion", OD_adicion1);
                cmd.Parameters.AddWithValue("@OI_esfera", OI_esfera1);
                cmd.Parameters.AddWithValue("@OI_cilindro", OI_cilindro1);
                cmd.Parameters.AddWithValue("@OI_eje", OI_eje1);
                cmd.Parameters.AddWithValue("@OI_prisma", OI_prisma1);
                cmd.Parameters.AddWithValue("@OI_adicion", OI_adicion1);
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

        public int EliminarLens()
        {
            try
            {
                Command.Connection = getConnection();
                string query4 = "DELETE Lensometria WHERE lens_ID = @param1";
                SqlCommand cmd = new SqlCommand(query4, Command.Connection);
                cmd.Parameters.AddWithValue("param1", lens_ID1);
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
                string query = $"SELECT * FROM dbo.Consulta AS con INNER JOIN dbo.Cliente AS cli ON CAST(con.cli_DUI AS VARCHAR) = cli.cli_dui WHERE cli.cli_dui LIKE '%{valor}%'";
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

        public DataSet BuscarDUI2(string valor)
        {
            try
            {
                Command.Connection = getConnection();
                string query = $@"SELECT * FROM ViewLensometria WHERE [Cliente DUI] LIKE '%{valor}%'";
                SqlCommand cmd = new SqlCommand(query, Command.Connection);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                //Rellenamos con el Adaptador el DataSet diciéndole de que tabla provienen los datos
                adp.Fill(ds, "ViewLensometria");
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
