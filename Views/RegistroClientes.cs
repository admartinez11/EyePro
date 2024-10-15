using AdministrarClientes.Controlador;
using OpticaMultivisual.Controllers.ScheduleAppointment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministrarClientes.View.RegistroCliente
{
    public partial class RegistroClientes : Form
    {
        public RegistroClientes(int accion)
        {
            InitializeComponent();
            //Se invoca al controlador de la vista y se le envía el formulario y la acción
            Controlador_Registrar objAddUser = new Controlador_Registrar(this, accion);
        }
        public RegistroClientes(int accion, string DUI, string Nombre, string Apellido, string Telefono, string Genero, string Edad, string Correo_E, string Profesion, string Padecimientos, bool Menor)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            Controlador_Registrar objAddUser = new Controlador_Registrar(this, accion, DUI, Nombre, Apellido, Telefono, Edad, Genero, Correo_E, Profesion, Padecimientos, Menor);
        }
        public RegistroClientes(int accion, string Nombre, string Apellido, string Telefono, string Correo_E, string DUI)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            Controlador_Registrar objaddvisita = new Controlador_Registrar(this, accion, Nombre, Apellido, Telefono, Correo_E, DUI);
        }

    }

}