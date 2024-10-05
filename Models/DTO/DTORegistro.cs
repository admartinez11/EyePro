using OpticaMultivisual.Models;
using OpticaMultivisual.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministrarClientes.Modelo.DTO
{
    internal class DTORegistro : dbContext
    {
        private string nombre;
        private string apellido;
        private string telefono;
        private char genero;
        private string edad;
        private string correo_E;
        private string profesion;
        private string dUI;
        private string padecimientos;


        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public char Genero { get => genero; set => genero = value; }
        public string Edad { get => edad; set => edad = value; }
        public string Correo_E { get => correo_E; set => correo_E = value; }
        public string Profesion { get => profesion; set => profesion = value; }
        public string DUI { get => dUI; set => dUI = value; }
        public string Padecimientos { get => padecimientos; set => padecimientos = value; }

    }
}

