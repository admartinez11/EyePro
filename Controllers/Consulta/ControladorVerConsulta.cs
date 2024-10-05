using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views;
using OpticaMultivisual.Views.Consultas;

namespace OpticaMultivisual.Controllers.Consulta
{
    internal class ControladorVerConsulta
    {
        AñadirConsulta ObjAañadirConsulta;
        private int accion;


        public ControladorVerConsulta(AñadirConsulta objAañadirConsulta, int accion)
        {
            ObjAañadirConsulta = objAañadirConsulta;
            objAañadirConsulta.Load += new EventHandler(CargaInicio);
            this.accion = accion;
            verificarAccion();
            objAañadirConsulta.btnAgendar.Click += new EventHandler(NuevaConsulta);
        }
        void CargaInicio(object sender, EventArgs e)
        {
            LlenarComboDui();
            LlenarComboVisita();
            LlenarComboEmpleados();
            //AñadirConsulta_Load();
        }

        //private void AñadirConsulta_Load()
        //{
        //    // Configurar ComboBox para DUI con búsqueda y selección únicamente
        //    ObjAañadirConsulta.cmbDUI.DropDownStyle = ComboBoxStyle.DropDown;
        //    ObjAañadirConsulta.cmbDUI.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        //    ObjAañadirConsulta.cmbDUI.AutoCompleteSource = AutoCompleteSource.ListItems;

        //    // Configurar ComboBox para Visita con búsqueda y selección únicamente
        //    ObjAañadirConsulta.cmbVisita.DropDownStyle = ComboBoxStyle.DropDown;
        //    ObjAañadirConsulta.cmbVisita.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        //    ObjAañadirConsulta.cmbVisita.AutoCompleteSource = AutoCompleteSource.ListItems;

        //    // Configurar ComboBox para Empleado con búsqueda y selección únicamente
        //    ObjAañadirConsulta.cmbEmpleado.DropDownStyle = ComboBoxStyle.DropDown;
        //    ObjAañadirConsulta.cmbEmpleado.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        //    ObjAañadirConsulta.cmbEmpleado.AutoCompleteSource = AutoCompleteSource.ListItems;
        //}
        void LlenarComboDui()
        {
            DAOConsulta DaoDui = new DAOConsulta();
            DataSet dataSet = DaoDui.ObtenerDUI();
            ObjAañadirConsulta.cmbDUI.DataSource = dataSet.Tables["Cliente"];
            ObjAañadirConsulta.cmbDUI.DisplayMember = "cli_dui";
            ObjAañadirConsulta.cmbDUI.ValueMember = "cli_dui";

        }
        void LlenarComboVisita()
        {
            DAOConsulta DaoDui = new DAOConsulta();
            DataSet dataSet = DaoDui.ObtenerVisita();
            ObjAañadirConsulta.cmbVisita.DataSource = dataSet.Tables["Visita"];
            ObjAañadirConsulta.cmbVisita.DisplayMember = "vis_dui";
            ObjAañadirConsulta.cmbVisita.ValueMember = "vis_ID";
        }
        void LlenarComboEmpleados()
        {
            DAOConsulta DaoDui = new DAOConsulta();
            DataSet dataSet = DaoDui.ObtenerEmpleado();
            ObjAañadirConsulta.cmbEmpleado.DataSource = dataSet.Tables["Empleado"];
            ObjAañadirConsulta.cmbEmpleado.DisplayMember = "emp_nombre";
            ObjAañadirConsulta.cmbEmpleado.ValueMember = "emp_ID";
        }
        public void NuevaConsulta(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                // Crear una nueva instancia de DAOConsulta con los valores correctos
                DAOConsulta Consulta = new DAOConsulta
                {
                    Con_fecha = DateTime.Parse(ObjAañadirConsulta.DTPfechaconsulta.Text.Trim()),
                    Con_obser = ObjAañadirConsulta.txtObservaciones.Text.Trim(),
                    Cli_DUI = ObjAañadirConsulta.cmbDUI.SelectedValue.ToString().Trim(),
                    Vis_ID = ObjAañadirConsulta.cmbVisita.SelectedValue.ToString().Trim(),
                    Emp_ID = ObjAañadirConsulta.cmbEmpleado.SelectedValue.ToString().Trim(),
                    Con_hora = DateTime.Parse(ObjAañadirConsulta.DTPHoraConsulta.Text.Trim())
                };
                if (ObjAañadirConsulta.cmbEstado.Checked == true)
                {
                    Consulta.Est_ID = true;
                }
                else
                {
                    Consulta.Est_ID = false;
                }

                // Registrar la consulta en la base de datos
                int valorRetornado = Consulta.RegistrarCliente();

                // Verificar el resultado de la inserción
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAañadirConsulta.Close();
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

        public ControladorVerConsulta(AñadirConsulta Vista, int p_accion, string cli_DUI, string vis_ID, DateTime con_fecha, string con_obser, string emp_ID, int con_ID, DateTime con_hora, bool est_ID)
        {
            ObjAañadirConsulta = Vista;
            ObjAañadirConsulta.Load += new EventHandler(CargaInicio);
            this.accion = p_accion;
            verificarAccion();
            CargarValores(cli_DUI, vis_ID, con_fecha, con_obser, emp_ID, con_ID, con_hora, est_ID);
            ObjAañadirConsulta.btnActualizar.Click += new EventHandler(ActualizarRegistro);
        }
        public void CargarValores(string cli_DUI, string vis_ID, DateTime con_fecha, string con_obser, string emp_ID, int con_ID, DateTime con_hora, bool est_ID)
        {
            try
            {
                ObjAañadirConsulta.txtObservaciones.Text = con_obser;
                ObjAañadirConsulta.cmbDUI.Text = cli_DUI;
                ObjAañadirConsulta.cmbVisita.Text = vis_ID;
                ObjAañadirConsulta.cmbEmpleado.Text = emp_ID;
                ObjAañadirConsulta.DTPfechaconsulta.Value = con_fecha;
                ObjAañadirConsulta.txtConID.Text = con_ID.ToString();
                ObjAañadirConsulta.DTPHoraConsulta.Value = con_hora;
                ObjAañadirConsulta.cmbEstado.Text = est_ID.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV005 - No se pudieron cargar los datos",
                                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ActualizarRegistro(object sender, EventArgs e)
        {
            try
            {
                // Verificar que los campos estén correctamente llenos y validados
                if (ValidarCampos())
                {
                    DAOConsulta DAOActualizar = new DAOConsulta();

                    // Verificar si los elementos seleccionados en los ComboBox son válidos
                    if (ObjAañadirConsulta.cmbDUI.SelectedItem == null ||
                        ObjAañadirConsulta.cmbVisita.SelectedItem == null ||
                        ObjAañadirConsulta.cmbEmpleado.SelectedItem == null)
                    {
                        MessageBox.Show("Uno o más valores seleccionados no son válidos. Por favor, selecciona un valor existente.",
                                        "Error de selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Accediendo directamente a las propiedades de DataRowView para los ComboBox
                    DAOActualizar.Cli_DUI = ((DataRowView)ObjAañadirConsulta.cmbDUI.SelectedItem)["cli_DUI"].ToString().Trim();
                    DAOActualizar.Vis_ID = ((DataRowView)ObjAañadirConsulta.cmbVisita.SelectedItem)["vis_ID"].ToString().Trim();
                    DAOActualizar.Emp_ID = ((DataRowView)ObjAañadirConsulta.cmbEmpleado.SelectedItem)["emp_ID"].ToString().Trim();

                    if (ObjAañadirConsulta.cmbEstado.Checked == true)
                    {
                        DAOActualizar.Est_ID = true;
                    }
                    else
                    {
                        DAOActualizar.Est_ID = false;
                    }

                    // Solo lectura del TextBox para con_ID
                    ObjAañadirConsulta.txtConID.Enabled = false;

                    // Asignación de los demás valores con validación de fecha
                    if (!VerificarFecha())
                    {
                        return; // Si la fecha no es válida, salir del método
                    }

                    DAOActualizar.Con_fecha = DateTime.Parse(ObjAañadirConsulta.DTPfechaconsulta.Text.Trim());
                    DAOActualizar.Con_hora = DateTime.Parse(ObjAañadirConsulta.DTPHoraConsulta.Text.Trim());
                    DAOActualizar.Con_obser = ObjAañadirConsulta.txtObservaciones.Text.Trim();
                    DAOActualizar.Con_ID = int.Parse(ObjAañadirConsulta.txtConID.Text.Trim());

                    // Ejecutar la actualización en la base de datos
                    int valorRetornado = DAOActualizar.ActualizarConsulta();

                    if (valorRetornado == 1)
                    {
                        MessageBox.Show("Los datos han sido actualizados exitosamente",
                                        "Proceso completado",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        ObjAañadirConsulta.Close();
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
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Ha ocurrido un error al intentar actualizar la consulta. Asegúrese de que todos los campos sean válidos y no estén vacíos.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("EPV001 - Error inesperado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            // Primero, verifica que todos los campos obligatorios estén llenos
            if (!VerificacionCamposLlenos())
            {
                return false;
            }

            // Luego, verifica que la fecha sea válida
            if (!VerificarFecha())
            {
                return false;
            }


            // Finalmente, verifica que el texto en txtObservaciones sea válido
            if (!VerificarTexto(ObjAañadirConsulta.txtObservaciones.Text))
            {
                return false;
            }

            return true;
        }


        private bool VerificacionCamposLlenos()
        {
            // Validar que los campos obligatorios estén llenos
            if (string.IsNullOrWhiteSpace(ObjAañadirConsulta.cmbDUI.Text))
            {
                MessageBox.Show("Por favor, seleccione un DUI.", "Campo Obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAañadirConsulta.cmbVisita.Text))
            {
                MessageBox.Show("Por favor, seleccione una Visita.", "Campo Obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAañadirConsulta.cmbEmpleado.Text))
            {
                MessageBox.Show("Por favor, seleccione un Empleado.", "Campo Obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ObjAañadirConsulta.txtObservaciones.Text))
            {
                MessageBox.Show("Por favor, ingrese alguna Observación.", "Campo Obligatorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Si todas las validaciones pasan, se retorna true.
            return true;
        }

        private bool VerificarFecha()
        {
            DateTime fechaSeleccionada = ObjAañadirConsulta.DTPfechaconsulta.Value;
            DateTime fechaActual = DateTime.Today;

            // Compara si la fecha seleccionada es anterior a la fecha y hora actuales.
            if (fechaSeleccionada < fechaActual)
            {
                MessageBox.Show("La fecha y hora no pueden ser anteriores al momento actual.", "Fecha Incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Si la fecha seleccionada pasa ambas validaciones, se retorna true, indicando que la fecha es válida.
            return true;
        }
        private bool VerificarTexto(string texto)
        {
            // Define un conjunto de caracteres que no están permitidos en secuencia.
            char[] caracteresNoPermitidos = { ',', '.', ';', ':', '/', '\\', '-', '_', '!', '?', '@', '#', '$', '%', '^', '&', '*', '(', ')', '[', ']', '{', '}', '<', '>', '|', '`', '~' };

            // Recorre el texto para verificar si hay signos repetidos consecutivos.
            for (int i = 0; i < texto.Length - 1; i++)
            {
                // Compara el carácter actual con el siguiente carácter en la cadena.
                if (caracteresNoPermitidos.Contains(texto[i]) && caracteresNoPermitidos.Contains(texto[i + 1]))
                {
                    // Si se encuentran signos repetidos consecutivos, muestra un mensaje de advertencia
                    // y retorna false para indicar que la validación ha fallado.
                    MessageBox.Show("El texto no puede tener signos consecutivos.", "Texto Incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Verifica si la longitud del texto supera los 100 caracteres.
            if (texto.Length > 100)
            {
                // Si el texto es demasiado largo, muestra un mensaje de advertencia
                // y retorna false para indicar que la validación ha fallado.
                MessageBox.Show("El texto no puede tener más de 100 caracteres.", "Texto Demasiado Largo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Si el texto pasa todas las verificaciones, retorna true,
            // indicando que la validación ha sido exitosa.
            return true;
        }

        public void verificarAccion()
        {
            if (accion == 1) // Acción 1: Agregar
            {
                ObjAañadirConsulta.btnAgendar.Enabled = true;
                ObjAañadirConsulta.btnActualizar.Enabled = false;
            }
            else if (accion == 2) // Acción 2: Actualizar
            {
                ObjAañadirConsulta.btnAgendar.Enabled = false;
                ObjAañadirConsulta.btnActualizar.Enabled = true;
            }
        }
        public ControladorVerConsulta(AñadirConsulta Vista, int p_accion, string cli_DUI)
        {
            ObjAañadirConsulta = Vista;
            ObjAañadirConsulta.Load += new EventHandler(CargaInicio);
            this.accion = p_accion;
            verificarAccion();
            CargarValoresCon(cli_DUI);
            ObjAañadirConsulta.btnAgendar.Click += new EventHandler(ActualizarRegistroCon);
        }
        public void CargarValoresCon(string cli_DUI)
        {
            try
            {
                // Asignar el valor directamente al TextBox (txtDuiCon)
                ObjAañadirConsulta.cmbDUI.Text = cli_DUI;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: EPV005 - No se pudieron cargar los datos",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ActualizarRegistroCon(object sender, EventArgs e)
        {
            try
            {
                // Asegúrate de que el TextBox del DUI no esté vacío
                if (string.IsNullOrWhiteSpace(ObjAañadirConsulta.cmbDUI.Text.Trim()))
                {
                    MessageBox.Show("El campo DUI no puede estar vacío.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear una nueva instancia de DAOConsulta con los valores correctos
                DAOConsulta Consulta = new DAOConsulta
                {
                    Cli_DUI = ObjAañadirConsulta.cmbDUI.Text.Trim(),  // Asignar el DUI desde el TextBox
                    Con_fecha = DateTime.Parse(ObjAañadirConsulta.DTPfechaconsulta.Text.Trim()),
                    Con_obser = ObjAañadirConsulta.txtObservaciones.Text.Trim(),
                    Vis_ID = ObjAañadirConsulta.cmbVisita.SelectedValue.ToString().Trim(),
                    Emp_ID = ObjAañadirConsulta.cmbEmpleado.SelectedValue.ToString().Trim(),
                    Con_hora = DateTime.Parse(ObjAañadirConsulta.DTPHoraConsulta.Text.Trim())
                };

                Consulta.Est_ID = ObjAañadirConsulta.cmbEstado.Checked;

                // Registrar la consulta en la base de datos
                int valorRetornado = Consulta.RegistrarCliente();

                // Verificar el resultado de la inserción
                if (valorRetornado == 1)
                {
                    MessageBox.Show("Los datos han sido registrados exitosamente",
                                    "Proceso completado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    ObjAañadirConsulta.Close();
                }
                else
                {
                    MessageBox.Show("EPV006 - No se pudieron registrar los datos",
                                    "Proceso interrumpido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
