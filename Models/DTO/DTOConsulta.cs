using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOConsulta : dbContext
    {
        private string cli_DUI;
        private string vis_ID;
        private DateTime con_fecha;
        private DateTime con_hora;
        private string con_obser;
        private string emp_ID;
        private int con_ID;
        private bool est_ID;

        public string Cli_DUI { get => cli_DUI; set => cli_DUI = value; }
        public string Vis_ID { get => vis_ID; set => vis_ID = value; }
        public DateTime Con_fecha { get => con_fecha; set => con_fecha = value; }
        public DateTime Con_hora { get => con_hora; set => con_hora = value; }
        public string Con_obser { get => con_obser; set => con_obser = value; }
        public string Emp_ID { get => emp_ID; set => emp_ID = value; }
        public int Con_ID { get => con_ID; set => con_ID = value; }
        public bool Est_ID { get => est_ID; set => est_ID = value; }
    }
}
