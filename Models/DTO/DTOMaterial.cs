using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOMaterial : dbContext
    {
        private int material_ID;    
        private string material_nombre;
        private string material_descripcion;

        public int Material_ID { get => material_ID; set => material_ID = value; }
        public string Material_nombre { get => material_nombre; set => material_nombre = value; }
        public string Material_descripcion { get => material_descripcion; set => material_descripcion = value; }
    }
}
