using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOModelo : dbContext
    {
        private int mod_ID;
        private string mod_nombre;
        private int marca_ID;

        public int Mod_ID { get => mod_ID; set => mod_ID = value; }
        public string Mod_nombre { get => mod_nombre; set => mod_nombre = value; }
        public int Marca_ID { get => marca_ID; set => marca_ID = value; }
    }
}
