using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOMaterialTipoArticulo : dbContext
    {
        private int materialtipoart_ID;
        private int tipoart_ID;
        private int material_ID;

        public int Materialtipoart_ID { get => materialtipoart_ID; set => materialtipoart_ID = value; }
        public int Tipoart_ID { get => tipoart_ID; set => tipoart_ID = value; }
        public int Material_ID { get => material_ID; set => material_ID = value; }
    }
}
