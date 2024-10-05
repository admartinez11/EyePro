using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpticaMultivisual.Models.DTO;
using System.Windows.Forms;

namespace OpticaMultivisual.Models.DAO
{
    class DAO_DR : DTORecetaBase
    {
        readonly SqlCommand Command = new SqlCommand();

        public DataSet ObtenerPersonas()
        {
            try
            {
                //Accedemos a la conexión que ya se tiene
                Command.Connection = getConnection();
                //Instrucción que se hará hacia la base de datos
                string query = "SELECT * FROM ViewDR ORDER BY [ID] asc";
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
                adp.Fill(ds, "ViewDR");
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
            }
        }

        public DataSet ObtenerConsulta()
        {
                getConnection().Close();
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
                MessageBox.Show("EPV005 - No se pudieron cargar los datos", "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                Command.Connection.Close();
            }
        }


        public int InsertarDR()
        {
            try
            {
                Command.Connection = getConnection();
                string query3 = "EXEC InsertarDR @con_ID, @OD_esfera, @OD_cilindro, @OD_eje, @OD_prisma, @OD_adicion, @OD_AO, @OD_AP, @OD_DP, @OI_esfera, @OI_cilindro, @OI_eje, @OI_prisma, @OI_adicion, @OI_AO, @OI_AP, @OI_DP";
                SqlCommand cmd = new SqlCommand(query3, Command.Connection);

                cmd.Parameters.AddWithValue("@con_ID", con_ID1);
                cmd.Parameters.AddWithValue("@OD_esfera", OD_esfera1);
                cmd.Parameters.AddWithValue("@OD_cilindro", OD_cilindro1);
                cmd.Parameters.AddWithValue("@OD_eje", OD_eje1);
                cmd.Parameters.AddWithValue("@OD_prisma", OD_prisma1);
                cmd.Parameters.AddWithValue("@OD_adicion", OD_adicion1);
                cmd.Parameters.AddWithValue("@OD_AO", OD_AO1);
                cmd.Parameters.AddWithValue("@OD_AP", OD_AP1);
                cmd.Parameters.AddWithValue("@OD_DP", OD_DP1);
                cmd.Parameters.AddWithValue("@OI_esfera", OI_esfera1);
                cmd.Parameters.AddWithValue("@OI_cilindro", OI_cilindro1);
                cmd.Parameters.AddWithValue("@OI_eje", OI_eje1);
                cmd.Parameters.AddWithValue("@OI_prisma", OI_prisma1);
                cmd.Parameters.AddWithValue("@OI_adicion", OI_adicion1);
                cmd.Parameters.AddWithValue("@OI_AO", OI_AO1);
                cmd.Parameters.AddWithValue("@OI_AP", OI_AP1);
                cmd.Parameters.AddWithValue("@OI_DP", OI_DP1);

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

        public int ActualizarDR()
        {
            try
            {
                Command.Connection = getConnection();
                string query4 = "EXEC ActualizarDR @DR_ID, @con_ID, @OD_esfera, @OD_cilindro, @OD_eje, @OD_prisma, @OD_adicion, @OD_AO, @OD_AP, @OD_DP, @OI_esfera, @OI_cilindro, @OI_eje, @OI_prisma, @OI_adicion, @OI_AO, @OI_AP, @OI_DP";
                SqlCommand cmd = new SqlCommand(query4, Command.Connection);

                cmd.Parameters.AddWithValue("@con_ID", con_ID1); 
                cmd.Parameters.AddWithValue("@DR_ID", DR_ID1);
                cmd.Parameters.AddWithValue("@OD_esfera", OD_esfera1);
                cmd.Parameters.AddWithValue("@OD_cilindro", OD_cilindro1);
                cmd.Parameters.AddWithValue("@OD_eje", OD_eje1);
                cmd.Parameters.AddWithValue("@OD_prisma", OD_prisma1);
                cmd.Parameters.AddWithValue("@OD_adicion", OD_adicion1);
                cmd.Parameters.AddWithValue("@OD_AO", OD_AO1);
                cmd.Parameters.AddWithValue("@OD_AP", OD_AP1);
                cmd.Parameters.AddWithValue("@OD_DP", OD_DP1);
                cmd.Parameters.AddWithValue("@OI_esfera", OI_esfera1);
                cmd.Parameters.AddWithValue("@OI_cilindro", OI_cilindro1);
                cmd.Parameters.AddWithValue("@OI_eje", OI_eje1);
                cmd.Parameters.AddWithValue("@OI_prisma", OI_prisma1);
                cmd.Parameters.AddWithValue("@OI_adicion", OI_adicion1);
                cmd.Parameters.AddWithValue("@OI_AO", OI_AO1);
                cmd.Parameters.AddWithValue("@OI_AP", OI_AP1);
                cmd.Parameters.AddWithValue("@OI_DP", OI_DP1);
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

        public int EliminarDR()
        {
            try
            {
                Command.Connection = getConnection();
                string query4 = "DELETE DRefractivo WHERE DR_ID = @param1";
                SqlCommand cmd = new SqlCommand(query4, Command.Connection);
                cmd.Parameters.AddWithValue("param1", DR_ID1);
                int respuesta = cmd.ExecuteNonQuery();
                return respuesta;

                //string query4 = "EXEC EliminarDR @DR_ID";
                //SqlCommand cmd = new SqlCommand(query4, Command.Connection);
                //cmd.Parameters.AddWithValue("@DR_ID", DR_ID1);
                //int respuesta = cmd.ExecuteNonQuery();
                //return respuesta;
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
    }
}
