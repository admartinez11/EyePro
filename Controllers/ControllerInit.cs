using OpticaMultivisual.Models.DAO;
using OpticaMultivisual.Views.FirstUse;
using OpticaMultivisual.Views.Login;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpticaMultivisual.Controllers.Helper;

namespace OpticaMultivisual.Controllers
{
    internal class ControllerInit
    {
        public static void DeterminarVistaInicial()
        {
            CommonClasses objCommon = new CommonClasses();
            objCommon.LeerArchivoXMLConexion();
            //Creando objetos de la clase DAOLogin y DAOFirstUser
            DAOLogin Objlogin = new DAOLogin();
            DAOFirstUse Objfirst = new DAOFirstUse();
            //Se obtiene el numero de empresas registradas, esto con el fin de evaluar si el numero retornado es cero, quiere decir, que si no hay empresas registradas se deberá mostrar el formulario para registrar a la primer empresa.
            int primerEmpresa = Objfirst.VerificarRegistroEmpresa();
            //Se obtiene el numero de usuarios registrados, esto con el fin de evaluar si el numero retornado es cero, quiere decir, que si no hay usuarios registrados se deberá mostrar el formulario para registrar al primer usuario.
            //En caso si existan empresas y usuarios, se deberá mostrar el formulario de Login
            int primerUsuario = Objlogin.ValidarPrimerUsoSistema();
            if (primerEmpresa == 0)
            {
                Application.Run(new ViewFirstUse());
            }
            else if (primerUsuario == 0)
            {
                Application.Run(new ViewCreateFirstUser());
            }
            else
            {
                Application.Run(new ViewLogin());
            }
        }
    }
}
