using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace AdministrarClientes.Controlador   
{
    public class Controlador_Registrar
    {
        RegistroClientes ObjVistaR;
        private int accion;
        public Controlador_Registrar(RegistroClientes Vista, int accion)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            //Métodos iniciales: estos metodos se ejecutan cuando el formulario está cargando
            ObjVistaR.btnAgregarCliente.Click += new EventHandler(NuevoCliente);
            //ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);
        }
        public void NuevoCliente(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                DAORegistro DAOIngresarR = new DAORegistro
                {
                    DUI = ObjVistaR.txtdui.Text.Trim(),
                    Nombre = ObjVistaR.txtNombre.Text.Trim(),
                    Apellido = ObjVistaR.txtApellido.Text.Trim(),
                    Correo_E = ObjVistaR.txtcorreo_electronico.Text.Trim(),
                    Telefono = ObjVistaR.txtTelefono.Text.Trim(),
                    Edad = ObjVistaR.txtEdad.Text.Trim(),
                    Genero = char.Parse(ObjVistaR.txtGenero.Text.Trim()),
                    Padecimientos = ObjVistaR.txtpadecimientos.Text.Trim(),
                    Profesion = ObjVistaR.txtprofecion.Text.Trim(),

                };

                int valorRetornado = DAOIngresarR.RegistrarCliente();

                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjVistaR.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - No se pudieron registrar los datos",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }


        private bool ValidarCampos()
        {
            string genero = ObjVistaR.txtGenero.Text.Trim();
            if (genero != "M" && genero != "F" && genero != "f" && genero != "m")
            {
                MessageBox.Show("El campo Género solo puede contener 'M' o 'F'.", "Validación de Género", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string telefono = ObjVistaR.txtTelefono.Text.Trim();
            if (!EsTelValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener un guion (-).", "Validación de Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            string DUI = ObjVistaR.txtdui.Text.Trim();
            if (!EsDUIValido(DUI))
            {
                MessageBox.Show("El campo DUI solo puede contener números y un solo guion.", "Validación de DUI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string correo = ObjVistaR.txtcorreo_electronico.Text.Trim();
            if (!EsCorreoValido(correo))
            {
                MessageBox.Show("El campo Correo Electrónico no tiene un formato válido.", "Validación de Correo Electrónico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (ObjVistaR.txtNombre.Text.Length > 100)
            {
                MessageBox.Show("El campo de Nombre no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (ObjVistaR.txtApellido.Text.Length > 100)
            {
                MessageBox.Show("El campo de Apellido no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ObjVistaR.txtpadecimientos.Text.Length > 100)
            {
                MessageBox.Show("El campo de Padecimiento no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ObjVistaR.txtprofecion.Text.Length > 100)
            {
                MessageBox.Show("El campo de Profecion no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (ObjVistaR.txtcorreo_electronico.Text.Length > 100)
            {
                MessageBox.Show("El campo de Correo Electronico no debe de exceder el máximo de caracteres.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(ObjVistaR.txtNombre.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtApellido.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtpadecimientos.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtEdad.Text.Trim()) ||
                string.IsNullOrEmpty(ObjVistaR.txtprofecion.Text.Trim())
                )
            {
                MessageBox.Show("Existen campos vacíos, complete cada uno de los apartados", "Proceso interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool EsCorreoValido(string correo)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }
        private bool EsDUIValido(string dui)
        {
            // Expresión regular para validar el formato del DUI: 8 dígitos seguidos de un guion y un dígito.
            string patronDUI = @"^\d{8}-\d{1}$";

            // Verifica si el DUI ingresado cumple con el formato usando una expresión regular
            if (System.Text.RegularExpressions.Regex.IsMatch(dui, patronDUI))
            {
                return true; // El formato es válido
            }
            else
            {
                MessageBox.Show("El DUI ingresado no es válido. Debe tener el formato 12345678-9.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // El formato es inválido
            }
        }
        private bool EsTelValido(string telefono)
        {
            if (!telefono.Contains("-"))
            {
                return false;
            }
            return true;
        }


        public Controlador_Registrar(RegistroClientes Vista, int p_accion, string DUI, string Nombre, string Apellido, string Correo_E, string Edad, char Genero, string Profeción, string Padecimientos, string Telefono)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;
            //Se guarda en la variable acción en vaor
            verificarAccion();
            Cargarvalores(DUI, Nombre, Apellido, Telefono, Genero, Edad, Correo_E, Profeción, Padecimientos);
            //Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizar.Click += new EventHandler(AcualizarRegistro);
            //ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);

        }
        public void Cargarvalores(string DUI, string Nombre, string Apellido, string Telefono, char Genero, string Edad, string Correo_E, string Profeción, string Padecimientos)
        {
            try
            {
                ObjVistaR.txtdui.Text = DUI;
                ObjVistaR.txtNombre.Text = Nombre;
                ObjVistaR.txtApellido.Text = Apellido;
                ObjVistaR.txtTelefono.Text = Telefono;
                ObjVistaR.txtGenero.Text = Genero.ToString();
                ObjVistaR.txtEdad.Text = Edad;
                ObjVistaR.txtcorreo_electronico.Text = Correo_E;
                ObjVistaR.txtprofecion.Text = Profeción;
                ObjVistaR.txtpadecimientos.Text = Padecimientos;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV010 - Error de excepción");
            }
        }
        public void AcualizarRegistro(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                {
                    DAORegistro DAOActualizar = new DAORegistro();
                    DAOActualizar.Edad = ObjVistaR.txtEdad.Text.Trim();
                    DAOActualizar.Telefono = ObjVistaR.txtTelefono.Text.Trim();
                    DAOActualizar.Genero = char.Parse(ObjVistaR.txtGenero.Text.Trim());
                    DAOActualizar.Nombre = ObjVistaR.txtNombre.Text.Trim();
                    DAOActualizar.Apellido = ObjVistaR.txtApellido.Text.Trim();
                    DAOActualizar.Padecimientos = ObjVistaR.txtpadecimientos.Text.Trim();
                    DAOActualizar.Profesion = ObjVistaR.txtprofecion.Text.Trim();
                    DAOActualizar.DUI = ObjVistaR.txtdui.Text.Trim();
                    DAOActualizar.Correo_E = ObjVistaR.txtcorreo_electronico.Text.Trim();
                    int valorRetornado = DAOActualizar.ActualizarCliente();
                    if (valorRetornado == 1)
                    {
                        MessageBox.Show("Los datos han sido actualizado exitosamente",
                                        "Proceso completado",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        ObjVistaR.Close();
                    }
                    else if (valorRetornado == 0)
                    {
                        MessageBox.Show("EPV002 - Los datos no pudieron ser actualizados correctamente",
                                        "Proceso interrumpido",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("EPV001 - Error inesperado",
                                        "Proceso interrumpido",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }


            }
        }

        public void verificarAccion()
        {
            if (accion == 1)
            {
                ObjVistaR.btnAgregarCliente.Enabled = true;
                ObjVistaR.btnActualizar.Enabled = false;
            }
            else if (accion == 2)
            {
                ObjVistaR.btnAgregarCliente.Enabled = false;
                ObjVistaR.btnActualizar.Enabled = true;
            }
        }
    }
}

