using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    class DTOAdminEmp : dbContext
    {
        // **** EMPLEADOS ****
        private int emp_ID;             // Identificador único de la persona
        private string emp_nombre;      // Nombres del empleado
        private string emp_apellido;    // Apellidos del empleado
        private string emp_genero;      // Genero del empleado
        private DateTime emp_fnacimiento;      // Fecha de nacimiento del empleado
        private string emp_correo;      // Correo electrónico del empleado
        private string emp_DUI;         // Número de Documento Único de Identidad del empleado (en El Salvador)
        private string emp_direccion;   // Dirección de la persona
        private string emp_telefono;     // Número de teléfono de la persona
        // **** USUARIO ****
        private string username;        // Nombre de usuario
        private string password;        // Contraseña del usuario
        private bool userStatus;
        private int rol;               // Rol del usuario
        private string securityQuestion;
        private string securityAnswer;
        private string verificationCode;
        private int userAttempts;
        public string SecurityQuestion { get => securityQuestion; set => securityQuestion = value; }
        public string SecurityAnswer { get => securityAnswer; set => securityAnswer = value; }
        // Propiedades públicas para acceder a los campos privados
        //getter y setter
        public int Id { get => emp_ID; set => emp_ID = value; }
        public string Nombre { get => emp_nombre; set => emp_nombre = value; }
        public string Apellido { get => emp_apellido; set => emp_apellido = value; }
        public string Genero { get => emp_genero; set => emp_genero = value; }
        public DateTime Nacimiento { get => emp_fnacimiento; set => emp_fnacimiento = value; }
        public string Correo { get => emp_correo; set => emp_correo = value; }
        public string Dui { get => emp_DUI; set => emp_DUI = value; }
        public string Direccion { get => emp_direccion; set => emp_direccion = value; }
        public string Telefono { get => emp_telefono; set => emp_telefono = value; }
        public string User { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public bool UserStatus { get => userStatus; set => userStatus = value; }
        public int Rol { get => rol; set => rol = value; }
        public string VerificationCode { get => verificationCode; set => verificationCode = value; }
        public int UserAttempts { get => userAttempts; set => userAttempts = value; }
    }
}