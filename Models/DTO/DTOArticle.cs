using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOArticle : dbContext
    {
        private int art_codigo;
        private string art_nombre;
        private string art_descripcion;
        private int tipoart_ID;
        private int mod_ID;
        private string art_medidas;
        private int material_ID;
        private int color_ID;
        private string art_urlimagen;
        private string art_comentarios;
        private string art_punitario;

        public int Art_codigo { get => art_codigo; set => art_codigo = value; }
        public string Art_nombre { get => art_nombre; set => art_nombre = value; }
        public string Art_descripcion { get => art_descripcion; set => art_descripcion = value; }
        public int Tipoart_ID { get => tipoart_ID; set => tipoart_ID = value; }
        public int Mod_ID { get => mod_ID; set => mod_ID = value; }
        public string Art_medidas { get => art_medidas; set => art_medidas = value; }
        public int Material_ID { get => material_ID; set => material_ID = value; }
        public int Color_ID { get => color_ID; set => color_ID = value; }
        public string Art_urlimagen { get => art_urlimagen; set => art_urlimagen = value; }
        public string Art_comentarios { get => art_comentarios; set => art_comentarios = value; }
        public string Art_punitario { get => art_punitario; set => art_punitario = value; }
    }
}