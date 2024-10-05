using OpticaMultivisual.Controllers.Helper;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.FirstUse;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticaMultivisual.Controllers.FirstUse
{
    internal class ControllerFirstUse
    {
        ViewFirstUse ObjVista;
        bool realizarAccion;
        public ControllerFirstUse(ViewFirstUse Vista)
        {
            ObjVista = Vista;
            Vista.btnSave.Click += new EventHandler(GuardarInformacion);
            Vista.btnAttach.Click += new EventHandler(ColocarImagenPicture);
            Vista.btnCerrar.Click += new EventHandler(CerrarPrograma);
        }

        void CerrarPrograma(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        void GuardarInformacion(object sender, EventArgs e)
        {
            try
            {
                //Validación para verificar que todos los campos esten llenos
                if (!(string.IsNullOrEmpty(ObjVista.txtNameBussines.Text.Trim()) ||
                    string.IsNullOrEmpty(ObjVista.txtAddressBussines.Text.Trim()) ||
                    string.IsNullOrEmpty(ObjVista.txtEmailBussines.Text.Trim()) ||
                    string.IsNullOrEmpty(ObjVista.txtPhone.Text.Trim()) ||
                    string.IsNullOrEmpty(ObjVista.txtPbx.Text.Trim()) ||
                    ObjVista.picBussines.Image == null))
                {
                    DAOFirstUse DAOGuardar = new DAOFirstUse();
                    CommonClasses commonClasses = new CommonClasses();
                    //Validaciones
                    if (ObjVista.txtAddressBussines.Text.Length > 100)
                    {
                        MessageBox.Show("La dirección del negocio excede el máximo de caracteres permitidos en ese campo", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (ObjVista.txtEmailBussines.Text.Length > 100)
                    {
                        MessageBox.Show("EL correo excede el máximo de caracteres permitidos en ese campo", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    string correo = ObjVista.txtEmailBussines.Text.Trim();
                    if (!ValidarCorreo())
                    {
                        return;
                    }
                    if (!commonClasses.EsCorreoValido(correo))
                    {
                        MessageBox.Show("El campo Correo Electrónico no tiene un formato válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DateTime selectedDate = ObjVista.dtCreation.Value;
                    DateTime today = DateTime.Today;
                    if (selectedDate > today)
                    {
                        MessageBox.Show("La fecha de creación no puede ser una fecha futura.", "Validación de Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (ObjVista.txtPhone.Text.Length > 30)
                    {
                        MessageBox.Show("El campo de teléfono ha excedido el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (!commonClasses.ValidarTelefono(ObjVista.txtPhone.Text.Trim()))
                    {
                        MessageBox.Show("El campo de teléfono contiene caracteres no válidos. Solo se permiten números, guiones y paréntesis.",
                                        "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string nombre = ObjVista.txtNameBussines.Text.Trim();
                    if (!commonClasses.EsNombreValido(nombre))
                    {
                        MessageBox.Show("El nombre de negocio ingresado contiene caracteres inválidos", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (ObjVista.txtNameBussines.Text.Length > 50)
                    {
                        MessageBox.Show("El campo de nombre del negocio no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (ObjVista.txtPbx.Text.Length > 30)
                    {
                        MessageBox.Show("EL PBX excede el máximo de caracteres permitidos en ese campo", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!commonClasses.ValidarTelefono(ObjVista.txtPbx.Text.Trim()))
                    {
                        MessageBox.Show("El campo de PBX contiene caracteres no válidos. Solo se permiten números, guiones y paréntesis.",
                                        "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DAOGuardar.NombreNegocio = ObjVista.txtNameBussines.Text.Trim();
                    DAOGuardar.DireccionNegocio = ObjVista.txtAddressBussines.Text.Trim();
                    DAOGuardar.CorreoNegocio = ObjVista.txtEmailBussines.Text.Trim();
                    DAOGuardar.FechaNegocio = ObjVista.dtCreation.Value.Date;
                    DAOGuardar.TelefonoNegocio = ObjVista.txtPhone.Text.Trim();
                    DAOGuardar.PbxNegocio = ObjVista.txtPbx.Text.Trim();
                    //Guardar imagen
                    Image imagen = ObjVista.picBussines.Image;
                    byte[] imageBytes;
                    if (imagen == null)
                    {
                        imageBytes = null;
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream();
                        imagen.Save(ms, ImageFormat.Jpeg);
                        imageBytes = ms.ToArray();
                    }
                    realizarAccion = ValidarCorreo();
                    if (realizarAccion == true)
                    {
                        DAOGuardar.ImagenNegocio = imageBytes;
                        bool respuesta = DAOGuardar.RegistrarNegocio();
                        if (respuesta != false)
                        {
                            MessageBox.Show($"Tu negocio ha sido registrada exitosamente.", "Paso 1 completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ViewCreateFirstUser nextForm = new ViewCreateFirstUser();
                            nextForm.Show();
                            ObjVista.Hide();
                        }
                        else
                        {
                            MessageBox.Show($"Oops, algo salió mal, revisemos los datos e intentemos nuevamente.", "Paso 1 interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Todos los campos son requeridos.", "Datos faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV001 - Error inesperado", "Error al procesar información", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ColocarImagenPicture(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png| Todos los archivos(*.*)| *.* ";
            ofd.Title = "Seleccionar imagen";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string rutaImagen = ofd.FileName;
                ObjVista.picBussines.Image = Image.FromFile(rutaImagen);
            }
        }

        bool ValidarCorreo()
        {
            string email = ObjVista.txtEmailBussines.Text.Trim();
            if (!(email.Contains("@")))
            {
                MessageBox.Show("Formato de correo invalido, verifica que contiene @.", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //adripaosv@gmail.com
            // Validación del dominio (ejemplo simplificado)
            string[] dominiosPermitidos = { "gmail.com", "ricaldone.edu.sv" };
            string extension = email.Substring(email.LastIndexOf('@') + 1);
            if (!dominiosPermitidos.Contains(extension))
            {
                MessageBox.Show("Dominio del correo es invalido, el sistema únicamente admite dominios 'gmail.com' y 'correo institucional'.", "Formato incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}
