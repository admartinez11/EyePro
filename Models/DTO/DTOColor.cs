using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOColor : dbContext
    {
        private int color_ID;
        private string color_nombre;
        private string color_descripcion;

        public int Color_ID { get => color_ID; set => color_ID = value; }
        public string Color_nombre { get => color_nombre; set => color_nombre = value; }
        public string Color_descripcion { get => color_descripcion; set => color_descripcion = value; }
    }
}
