using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOTipoArticulo : dbContext
    {
        private int tipoart_ID;
        private string tipoart_nombre;
        private string tipoart_descripcion;

        public int Tipoart_ID { get => tipoart_ID; set => tipoart_ID = value; }
        public string Tipoart_nombre { get => tipoart_nombre; set => tipoart_nombre = value; }
        public string Tipoart_descripcion { get => tipoart_descripcion; set => tipoart_descripcion = value; }
    }
}
