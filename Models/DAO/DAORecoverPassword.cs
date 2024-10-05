using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OpticaMultivisual.Models.DTO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Models;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using OpticaMultivisual.Controllers.Helper;

namespace OpticaMultivisual.Models.DAO
{
    public class DAORecoverPassword : DTORecoverPassword
    {
        SqlCommand Command = new SqlCommand();

        // Método para almacenar el código de verificación

        public bool UpdatePasswordUsingCode(string userEmail, string newPassword, string verificationCode)
        {
            using (SqlConnection connection = getConnection())
            {
                string query = @"
                    UPDATE Usuario
                    SET password = @NewPassword, VerificationCode = NULL, CodeExpiration = NULL
                    WHERE username = (
                    SELECT username FROM Empleado WHERE emp_correo = @Email
                    )
                    AND VerificationCode = @VerificationCode
                    ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    CommonClasses commonClasses = new CommonClasses();
                    command.Parameters.AddWithValue("@NewPassword", commonClasses.ComputeSha256Hash(newPassword)); // Asegúrate de encriptar la contraseña
                    command.Parameters.AddWithValue("@Email", userEmail);
                    command.Parameters.AddWithValue("@VerificationCode", verificationCode);

                    try
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0; // Retorna true si la operación fue exitosa
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public bool StoreVerificationCode(string userEmail, string verificationCode)
        {
            using (SqlConnection connection = getConnection())
            {

                string query = @"
                    UPDATE Usuario
                    SET VerificationCode = @VerificationCode, CodeExpiration = @CodeExpiration
                    WHERE username = (
                    SELECT username FROM Empleado WHERE emp_correo = @Email
                    )
                    ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VerificationCode", verificationCode);
                    command.Parameters.AddWithValue("@Email", userEmail);

                    // Definir la fecha y hora de expiración del código (15 minutos a partir de ahora)
                    DateTime expiryDate = DateTime.Now.AddMinutes(15);
                    command.Parameters.AddWithValue("@CodeExpiration", expiryDate);

                    try
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0; // Retorna true si la operación fue exitosa
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        MessageBox.Show("EPV008 - No se pudo almacenar el código de verificación",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // Método para validar el código de verificación
        public bool ValidateVerificationCode(string userEmail, string verificationCode)
        {
            using (SqlConnection connection = getConnection())
            {

                string query = @"
                    SELECT COUNT(1)
                    FROM Usuario
                    WHERE username = (
                    SELECT username FROM Empleado WHERE emp_correo = @Email
                    )
                    AND VerificationCode = @VerificationCode
                    AND CodeExpiration > @CurrentDate
                    ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", userEmail);
                    command.Parameters.AddWithValue("@VerificationCode", verificationCode);
                    command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);

                    try
                    {
                        int count = (int)command.ExecuteScalar();
                        return count > 0; // Retorna true si el código es válido y no ha expirado
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        MessageBox.Show("EPV007 - No se pudo enviar el código de verificación",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}