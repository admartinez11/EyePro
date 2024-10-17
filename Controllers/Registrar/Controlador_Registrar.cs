using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Models.DTO;
using OpticaMultivisual.Views;
using OpticaMultivisual.Views.Consultas;
using OpticaMultivisual.Views.ScheduleAppointment;
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
                    Genero = ObjVistaR.cmbGenero.SelectedItem.ToString().Trim(),
                    Padecimientos = ObjVistaR.txtpadecimientos.Text.Trim(),
                    Profesion = ObjVistaR.txtprofecion.Text.Trim(),
                };

                

                if (ObjVistaR.checkmenor.Checked == true)
                {
                    DAOIngresarR.Menor = true;
                }
                else
                {
                    DAOIngresarR.Menor = false;
                }

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
            
            if (!ValidarEdadMenorDe18())
            {
                return false;
            }
            if (!ValidarGenero())
            {
                return false;
            }


            string telefono = ObjVistaR.txtTelefono.Text.Trim();
            if (!EsTelValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener un guion (-).", "Validación de Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            string DUI = ObjVistaR.txtdui.Text.Trim();
            if (!EsDUIValidoRegistrar(DUI)) // Usar la lógica existente sin crear un nuevo controlador
            {
                MessageBox.Show("El campo DUI solo puede contener números y un solo guion.", "Validación de DUI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Salir si el DUI no es válido.
            }
            string Dui = ObjVistaR.txtdui.Text.Trim(); // Obtén el DUI del TextBox
            if (!ValidarDUI(Dui))
            {
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
        private bool ValidarGenero()
        {
            string genero = ObjVistaR.cmbGenero.SelectedItem?.ToString().Trim();

            // Verificar que el valor no sea nulo o vacío
            if (string.IsNullOrEmpty(genero))
            {
                MessageBox.Show("Por favor, seleccione un género válido (Masculino o Femenino).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Verificar que el valor sea 'Masculino' o 'Femenino'
            if (genero == "Masculino" || genero == "Femenino")
            {
                
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un género válido (Masculino o Femenino).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool ValidarEdadMenorDe18()
        {
            // Verificar si el contenido del txtEdad es un número válido
            if (int.TryParse(ObjVistaR.txtEdad.Text, out int edad))
            {
                // Si la edad es menor de 18
                if (edad < 18)
                {
                    ObjVistaR.checkmenor.Checked = true;  // Marcar el CheckBox
                    return true;
                }
                else
                {
                    ObjVistaR.checkmenor.Checked = false; // Desmarcar el CheckBox si es mayor o igual a 18
                    return true;
                }
            }
            else
            {
                // Si no es un número válido, podrías mostrar un mensaje de error
                MessageBox.Show("Por favor, ingrese una edad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool ValidarCamposa()
        {
            string genero = ObjVistaR.cmbGenero.SelectedItem.ToString().Trim();

            // Verificar que el valor no sea nulo y sea 'M' o 'F'
            if (string.IsNullOrEmpty(genero) || (genero != "Masculino" && genero != "Femenino"))
            {
                return false;
            }
            ObjVistaR.cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!ValidarEdadMenorDe18())
            {
                return false;
            }
            string telefono = ObjVistaR.txtTelefono.Text.Trim();
            if (!EsTelValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener un guion (-).", "Validación de Teléfono", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private bool ValidarDUI(string dui)
        {
            // Expresión regular para validar el formato básico del DUI (sin sufijo)
            string patronDUI = @"^\d{8}-\d{1}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(dui, patronDUI))
            {
                MessageBox.Show("El DUI ingresado no es válido. Debe tener el formato 12345678-9.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            // Si el checkbox está marcado, se asigna sufijo
            if (ObjVistaR.checkmenor.Checked)
            {
                DAORegistro daoRegistro = new DAORegistro();
                string nuevoDUI = daoRegistro.AsignarSufijoDUI(dui);

                if (nuevoDUI != null)
                {
                    ObjVistaR.txtdui.Text = nuevoDUI; // Actualiza el DUI en el TextBox
                    MessageBox.Show($"Nuevo DUI asignado: {nuevoDUI}", "DUI Asignado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("No se pueden agregar más sufijos. Se alcanzó el límite de -99.", "Límite Alcanzado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Si no se marcó el checkbox, retorna el DUI tal como está
            return true;
        }

        private bool EsDUIValidoRegistrar(string dui)
        {
            // Expresión regular para validar el formato del DUI: 8 dígitos seguidos de un guion y un dígito, opcionalmente seguido de un guion y 2 dígitos.
            string patronDUI = @"^\d{8}-\d{1}(-\d{2})?$";

            // Verifica si el DUI ingresado cumple con el formato usando una expresión regular.
            if (!System.Text.RegularExpressions.Regex.IsMatch(dui, patronDUI))
            {
                MessageBox.Show("El DUI ingresado no es válido. Debe tener el formato 12345678-9 o 12345678-9-01.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // El formato es inválido.
            }

            return true; // El formato es válido.
        }
        private bool EsCorreoValido(string correo)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }
        private bool EsTelValido(string telefono)
        {
            if (!telefono.Contains("-"))
            {
                return false;
            }
            return true;
        }

        public Controlador_Registrar(RegistroClientes Vista, int p_accion, string DUI, string Nombre, string Apellido, string Correo_E, string Edad, string Genero, string Profeción, string Padecimientos, string Telefono, bool Menor)
        {
            //Acciones iniciales
            ObjVistaR = Vista;
            this.accion = p_accion;
            //Se guarda en la variable acción en vaor
            verificarAccion();
            Cargarvalores(DUI, Nombre, Apellido, Telefono, Genero, Edad, Correo_E, Profeción, Padecimientos, Menor);
            //Métodos que se ejecutan al ocurrir eventos
            ObjVistaR.btnActualizar.Click += new EventHandler(AcualizarRegistro);
            //ObjAddUser.btnFoto.Click += new EventHandler(ChargePhoto);

        }
        public void Cargarvalores(string DUI, string Nombre, string Apellido, string Telefono, string Genero, string Edad, string Correo_E, string Profeción, string Padecimientos, bool Menor)
        {
            try
            {
                ObjVistaR.txtdui.Text = DUI;
                ObjVistaR.txtNombre.Text = Nombre;
                ObjVistaR.txtApellido.Text = Apellido;
                ObjVistaR.txtTelefono.Text = Telefono;
                ObjVistaR.cmbGenero.SelectedItem = Genero;
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
//h
        public void AcualizarRegistro(object sender, EventArgs e)
        {
            if (ValidarCamposa())
            {
                {
                    DAORegistro DAOActualizar = new DAORegistro();
                    DAOActualizar.Edad = ObjVistaR.txtEdad.Text.Trim();
                    DAOActualizar.Telefono = ObjVistaR.txtTelefono.Text.Trim();
                    DAOActualizar.Genero = ObjVistaR.cmbGenero.SelectedItem.ToString().Trim();
                    DAOActualizar.Nombre = ObjVistaR.txtNombre.Text.Trim();
                    DAOActualizar.Apellido = ObjVistaR.txtApellido.Text.Trim();
                    DAOActualizar.Padecimientos = ObjVistaR.txtpadecimientos.Text.Trim();
                    DAOActualizar.Profesion = ObjVistaR.txtprofecion.Text.Trim();
                    DAOActualizar.DUI = ObjVistaR.txtdui.Text.Trim();
                    DAOActualizar.Correo_E = ObjVistaR.txtcorreo_electronico.Text.Trim();
                    if (ObjVistaR.checkmenor.Checked == true)
                    {
                        DAOActualizar.Menor = true;
                    }
                    else
                    {
                        DAOActualizar.Menor = false;
                    }
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
        public Controlador_Registrar(RegistroClientes Vista, int accion, string Nombre, string Apellido, string Telefono, string Correo_E, string DUI)
        {
            ObjVistaR = Vista;
            this.accion = accion;
            verificarAccion();
            CargarValoresCon(accion, Nombre, Apellido, Telefono, Correo_E, DUI);
            ObjVistaR.btnAgregarCliente.Click += new EventHandler(ActualizarRegistroCon);
        }
        public void CargarValoresCon(int accion, string Nombre, string Apellido, string Telefono, string Correo_E, string DUI)
        {
            try
            {
                // Asignar el valor directamente al TextBox (txtDuiCon)
                ObjVistaR.txtTelefono.Text = Telefono; // Campo para Teléfono
                ObjVistaR.txtdui.Text = DUI; // Campo para Observaciones
                ObjVistaR.txtNombre.Text = Nombre; // Campo para Nombre
                ObjVistaR.txtApellido.Text = Apellido; // Campo para Apellido
                ObjVistaR.txtcorreo_electronico.Text = Correo_E; // Campo para Correo
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV005 - No se pudieron cargar los datos",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ActualizarRegistroCon(object sender, EventArgs e)
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
                    Genero = ObjVistaR.cmbGenero.SelectedItem.ToString().Trim(),
                    Padecimientos = ObjVistaR.txtpadecimientos.Text.Trim(),
                    Profesion = ObjVistaR.txtprofecion.Text.Trim(),
                };

                string dui = DAOIngresarR.DUI.Trim();
                // Mostrar longitud del DUI
                

                if (ObjVistaR.checkmenor.Checked == true)
                {
                    DAOIngresarR.Menor = true;
                }
                else
                {
                    DAOIngresarR.Menor = false;
                }

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
    }
}

