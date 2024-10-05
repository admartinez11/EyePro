using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpticaMultivisual.Views;
using AdministrarClientes.Controlador;
using AdministrarClientes.View.RegistroCliente;
using OpticaMultivisual.Views.ScheduleAppointment;
using OpticaMultivisual.Views.Login;
using OpticaMultivisual.Views.Dashboard.Optometrista;
using OpticaMultivisual.Views.Reports.RecetaBase;
using OpticaMultivisual.Views.Dashboard;
using OpticaMultivisual.Views.Consultas;
using OpticaMultivisual.Views.FirstUse;
using OpticaMultivisual.Views.Dashboard.PedidoDet;
using OpticaMultivisual.Controllers;

namespace OpticaMultivisual
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ControllerInit.DeterminarVistaInicial();
            //Application.Run(new VerConsulta());
        }
    }

}
