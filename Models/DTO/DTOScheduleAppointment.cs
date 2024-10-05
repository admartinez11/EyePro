using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    public class DTOScheduleAppointment : dbContext
    {
        private int vis_ID;
        private DateTime vis_fcita;
        private string vis_dui;
        private string vis_obser;
        private string vis_nombre;
        private string vis_apellido;
        private string vis_tel;
        private string vis_correo;

        public int Vis_ID { get => vis_ID; set => vis_ID = value; }
        public DateTime Vis_fcita { get => vis_fcita; set => vis_fcita = value; }
        public string Vis_dui { get => vis_dui; set => vis_dui = value; }
        public string Vis_obser { get => vis_obser; set => vis_obser = value; }
        public string Vis_nombre { get => vis_nombre; set => vis_nombre = value; }
        public string Vis_apellido { get => vis_apellido; set => vis_apellido = value; }
        public string Vis_tel { get => vis_tel; set => vis_tel = value; }
        public string Vis_correo { get => vis_correo; set => vis_correo = value; }
    }
}
