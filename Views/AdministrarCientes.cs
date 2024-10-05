using AdministrarClientes.Controlador;
using OpticaMultivisual.Controllers;
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
    public partial class AdministrarCientes : Form
    {
        public AdministrarCientes()
        {
            InitializeComponent();
            Controlador_ListaRegistro Control = new Controlador_ListaRegistro(this);
        }

    }

}
