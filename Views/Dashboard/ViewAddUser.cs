using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Controllers.Dashboard;

namespace OpticaMultivisual.Views.Dashboard
{
    // Clase ViewAddUser que hereda de Form y representa la vista para agregar usuarios
    public partial class ViewAddUser : Form
    {
        // Constructor para inserción de datos
        public ViewAddUser(int accion)
        {
            InitializeComponent();
            // Crea una instancia del controlador ControllerAdduser y pasa la vista actual y la acción especificada
            ControllerAddUser objAddUser = new ControllerAddUser(this, accion);
        }

        //Constructor para actualización de datos
        public ViewAddUser(int accion, int id, string nombre, string apellido, string genero, DateTime nacimiento, string email, string telefono, string dui, string direccion, string username, string role)
        {
            InitializeComponent();
            // Crea una instancia del controlador ControllerAdduser y pasa la vista actual, la acción y los datos del usuario a actualizar
            ControllerAddUser objAddUser = new ControllerAddUser(this, accion, id, nombre, apellido, genero, nacimiento, email, telefono, dui, direccion, username, role);
        }

        // Constructor por defecto
        public ViewAddUser()
        {
            InitializeComponent();
            int accion = 0; // Asigna un valor predeterminado a 'accion'
            // Crea una instancia del controlador ControllerAdduser y pasa la vista actual y la acción especificada
            ControllerAddUser objAddUser = new ControllerAddUser(this, accion);
        }
    }
}