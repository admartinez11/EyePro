using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    public class DTORecoverPassword : dbContext
    {
        private string correo;

        public string Correo { get => correo; set => correo = value; }
    }
}
