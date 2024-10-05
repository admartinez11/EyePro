using AdministrarClientes.Controlador;
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
        public RegistroClientes(int accion, string DUI, string Nombre, string Apellido, string Telefono, char Genero, string Edad, string Correo_E, string Profesion, string Padecimientos)
        {
            InitializeComponent();
            // Se invoca al controlador de la vista y se le envía el formulario, la acción y los datos que recibió la vista.
            // La vista al recibir los datos de un controlador externo los reenvia a su propio controlador.
            Controlador_Registrar objAddUser = new Controlador_Registrar(this, accion, DUI, Nombre, Apellido, Telefono, Edad, Genero, Correo_E, Profesion, Padecimientos);
        }

    }

}