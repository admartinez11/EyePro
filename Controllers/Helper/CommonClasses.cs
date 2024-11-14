using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Views.Login;
using OpticaMultivisual.Views.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace OpticaMultivisual.Controllers.Helper
{
    public class CommonClasses
    {
        /// <summary>
        /// Método para crear bordes redondos en el formulario
        /// </summary>
        /// <param name="nLeftRect"></param>
        /// <param name="nTopRect"></param>
        /// <param name="nRightRect"></param>
        /// <param name="nBottomRect"></param>
        /// <param name="nWidthEllipse"></param>
        /// <param name="nHeightEllipse"></param>
        /// <returns></returns>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public string ComputeSha256Hash(string rawData)
        {
            // Crear una instancia de SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar el hash - devuelve un arreglo de bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convertir byte array a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void LeerArchivoXMLConexion()
        {
            // Construir la ruta del archivo XML combinando el directorio actual con el nombre del archivo
            /*Toma el directorio actual junto con el nombre del archivo*/
            string path = Path.Combine(Directory.GetCurrentDirectory().ToString(), "config_server.xml");
            // Verificar si el archivo XML existe en la ruta especificada
            if (File.Exists(path))
            {
                // Crear una instancia de XmlDocument para cargar y trabajar con el archivo XML
                XmlDocument doc = new XmlDocument();
                // Cargar el archivo XML en el objeto XmlDocument
                doc.Load(path);
                // Obtener el elemento raíz del XML
                XmlNode root = doc.DocumentElement;
                // Seleccionar los nodos específicos que contienen los datos de conexión (servidor y base de datos)
                XmlNode serverNode = root.SelectSingleNode("Server/text()");
                XmlNode databaseNode = root.SelectSingleNode("Database/text()");
                // Obtener los valores de los nodos como cadenas
                string serverCode = serverNode.Value;
                string databaseCode = databaseNode.Value;
                // Decodificar las cadenas cifradas utilizando el método DescifrarCadena
                DTOdbContext.Server = DescifrarCadena(serverCode);
                DTOdbContext.Database = DescifrarCadena(databaseCode);
                // Verificar si los nodos de autenticación SQL (SqlAuth y SqlPass) existen
                XmlNode sqlAuthNode = root.SelectSingleNode("SqlAuth/text()");
                XmlNode sqlPassNode = root.SelectSingleNode("SqlPass/text()");
                if (sqlAuthNode != null && sqlPassNode != null)
                {
                    // Si existen los nodos, significa que es una conexión en línea con autenticación SQL
                    string userCode = sqlAuthNode.Value;
                    string passwordCode = sqlPassNode.Value;
                    // Decodificar las credenciales de usuario y contraseña
                    DTOdbContext.User = DescifrarCadena(userCode);
                    DTOdbContext.Password = DescifrarCadena(passwordCode);
                }
                else
                {
                    // Si no existen los nodos, es una conexión local (autenticación de Windows)
                    DTOdbContext.User = null;
                    DTOdbContext.Password = null;
                }
            }
            else
            {
                // Si el archivo XML no existe, abrir la vista para configurar la conexión
                ViewAdminConnection openForm = new ViewAdminConnection(1);
                openForm.ShowDialog();
            }
        }

        public string DescifrarCadena(string cadenaCode)
        {
            try
            {
                // Decodificar la cadena cifrada en Base64 a un arreglo de bytes
                byte[] decodedBytes = Convert.FromBase64String(cadenaCode);
                // Convertir los bytes decodificados a una cadena utilizando UTF-8
                string decodedString = Encoding.UTF8.GetString(decodedBytes);
                // Retornar la cadena decodificada
                return decodedString.ToString();
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante la decodificación, retornar un mensaje de error
                return $"Error al descifrar: {ex.Message}";
            }
        }

        public bool TieneAlMenos8Caracteres(string contrasena)
        {
            // Verifica si la longitud de la contraseña es mayor o igual a 8 caracteres.
            return contrasena.Length >= 8;
        }

        public bool ContieneAlMenosUnNumero(string contrasena)
        {
            // Verifica si la contraseña contiene al menos un dígito numérico utilizando LINQ.
            // Any() devuelve true si al menos un elemento cumple la condición.
            return contrasena.Any(char.IsDigit);
        }

        public bool ContieneAlMenosUnaMayuscula(string contrasena)
        {
            // Verifica si la contraseña contiene al menos una letra mayúscula.
            return contrasena.Any(char.IsUpper);
        }

        public bool ContieneAlMenosUnaMinuscula(string contrasena)
        {
            // Verifica si la contraseña contiene al menos una letra minúscula.
            return contrasena.Any(char.IsLower);
        }

        public bool ContieneAlMenosUnSimbolo(string contrasena)
        {
            // Define una cadena con los símbolos permitidos.
            string simbolos = "@$#_";
            // Verifica si la contraseña contiene al menos uno de los símbolos definidos.
            return contrasena.Any(simbolo => simbolos.Contains(simbolo));
        }

        public bool EsCorreoValido(string correo)
        {
            // Define una expresión regular para validar la estructura básica de un correo electrónico.
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            // Utiliza Regex.IsMatch para verificar si la cadena coincide con la expresión regular.
            return Regex.IsMatch(correo, patron);
        }

        // Método para validar los comandos de teclado
        public void ValidarComandos(object sender, KeyEventArgs e)
        {
            // Detectar si se presiona Ctrl+C, Ctrl+X o Ctrl+V
            if (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.X || e.KeyCode == Keys.V))
            {
                // Cancelar la operación
                e.SuppressKeyPress = true;
            }
        }

        // Método para validar el formato del teléfono
        public bool ValidarTelefono(string telefono)
        {
            // Expresión regular que permite números, guiones, paréntesis y espacios
            string patron = @"^[0-9\-\(\)\s]+$";

            // Verificar que el teléfono coincida con el patrón
            return Regex.IsMatch(telefono, patron);
        }

        // Método para validar la fecha de nacimiento
        public bool ValidarFechaNacimiento(DateTime fechaNacimiento)
        {
            // Calcular la edad
            int edad = DateTime.Today.Year - fechaNacimiento.Year;

            // Restar un año si la fecha de nacimiento aún no ha ocurrido este año
            if (fechaNacimiento > DateTime.Today.AddYears(-edad))
            {
                edad--;
            }

            // Validar que la edad sea mayor o igual a 18
            if (edad < 18)
            {
                return false;
            }

            // Validar que la fecha no sea mayor a la fecha actual
            if (fechaNacimiento > DateTime.Today)
            {
                return false;
            }

            // Validar que la fecha no sea demasiado antigua (más de 120 años)
            if (fechaNacimiento < DateTime.Today.AddYears(-120))
            {
                return false;
            }

            return true;
        }

        public bool ValidarDecimal(string valor)
        {
            // Eliminar espacios adicionales
            valor = valor.Trim();

            // Separar el valor en partes usando el punto como delimitador
            string[] partes = valor.Split('.');

            // Verificar la longitud de la parte antes del punto
            if (partes.Length > 0 && partes[0].Length > 5)
            {
                MessageBox.Show("La parte antes del punto ha excedido el máximo de 5 caracteres.",
                                "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Verificar la longitud de la parte después del punto, si existe
            if (partes.Length > 1 && partes[1].Length > 2)
            {
                MessageBox.Show("La parte después del punto ha excedido el máximo de 2 caracteres.",
                                "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // La validación fue exitosa
            return true;
        }

        public string GenerarPin()
        {
            int longitud = 6; // Establece la longitud deseada de la cadena hexadecimal
            byte[] data = new byte[longitud / 2]; // Crea un arreglo de bytes con la mitad de la longitud deseada, ya que cada byte representa dos caracteres hexadecimales
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider()) // Crea un generador de números aleatorios criptográficamente seguro
            {
                crypto.GetBytes(data); // Llena el arreglo de bytes con números aleatorios generados por el RNG
            }
            string hex = BitConverter.ToString(data); // Convierte el arreglo de bytes a una cadena hexadecimal, separando cada byte con un guión
            return hex.Replace("-", "").Substring(0, longitud); // Elimina los guiones de la cadena hexadecimal y devuelve la subcadena inicial con la longitud deseada
        }

        public bool ValidarNumero(string numero)
        {
            // Expresión regular que permite solo números y un punto
            string patron = @"^[0-9]+$";

            // Verificar que el número coincida con el patrón
            return Regex.IsMatch(numero, patron);
        }

        public bool ValidarNumeroConPunto(string numero)
        {
            // Expresión regular que permite solo números y un punto
            string patron = @"^[0-9.]+$";

            // Verificar que el número coincida con el patrón
            return Regex.IsMatch(numero, patron);
        }

        public bool EsNombreValido(string nombre)
        {
            // Permite letras con tildes, espacios, guiones y apóstrofes.
            string patron = @"^[a-zA-Z\u00C0-\u017F\s'-]+$";
            return Regex.IsMatch(nombre, patron);
        }

        public bool ValidateEmail(string email)
        {
            // Intenta crear un objeto MailAddress con la dirección de correo electrónico.
            // Si se produce una excepción, significa que la dirección no es válida.
            try
            {
                var addr = new MailAddress(email);
                // Verifica si la dirección extraída del objeto MailAddress es igual a la original.
                // Esto ayuda a detectar casos especiales donde la dirección podría ser modificada.
                return addr.Address == email;
            }
            catch
            {
                // Si se produce una excepción, significa que la dirección no es válida.
                return false;
            }
        }

        public bool EsValida(string contrasena)
        {
            // Combina las validaciones de longitud, números, mayúsculas, minúsculas y símbolos.
            // Si todas las condiciones se cumplen, la contraseña es válida.
            return TieneAlMenos8Caracteres(contrasena) &&
                   ContieneAlMenosUnNumero(contrasena) &&
                   ContieneAlMenosUnaMayuscula(contrasena) &&
                   ContieneAlMenosUnaMinuscula(contrasena) &&
                   ContieneAlMenosUnSimbolo(contrasena);
        }

        public bool ValidarFechaPedido(DateTime pd_fpedido, DateTime pd_fprogramada)
        {
            // Validar que la fecha del pedido no sea mayor que la fecha programada
            if (pd_fpedido > pd_fprogramada)
            {
                return false;
            }

            // Validar que la fecha no sea demasiado antigua (más de 2 años)
            if (pd_fpedido < DateTime.Today.AddYears(-2))
            {
                return false;
            }

            // Validar que la fecha no sea demasiado antigua (más de 2 años)
            if (pd_fprogramada < DateTime.Today.AddYears(-2))
            {
                return false;
            }
            return true;
        }
        public bool ValidarArtCant(string art_cant)
        {
            // Expresión regular que permite solo números sin un punto
            string patron = @"^[0-9]+$";

            // Verificar que el número coincida con el patrón
            return Regex.IsMatch(art_cant, patron);
        }

        public bool ObservacionValida(string pd_obser)
        {
            // Permite letras con tildes, espacios, guiones y apóstrofes.
            string patron = @"^[a-zA-Z\u00C0-\u017F\s'-]+$";
            return Regex.IsMatch(pd_obser, patron);
        }
    }
}
