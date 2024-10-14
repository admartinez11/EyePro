using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTORecetaBase : dbContext
    {
        private int DR_ID;
        private int con_ID;
        private string OD_esfera;
        private string OD_cilindro;
        private string OD_eje;
        private string OD_prisma;
        private string OD_adicion;
        private string OD_AO;
        private string OD_AP;
        private string OD_DP;
        private string OI_esfera;
        private string OI_cilindro;
        private string OI_eje;
        private string OI_prisma;
        private string OI_adicion;
        private string OI_AO;
        private string OI_AP;
        private string OI_DP;

        public int DR_ID1 { get => DR_ID; set => DR_ID = value; }
        public int con_ID1 { get => con_ID; set => con_ID = value; }
        public string OD_esfera1 { get => OD_esfera; set => OD_esfera = value; }
        public string OD_cilindro1 { get => OD_cilindro; set => OD_cilindro = value; }
        public string OD_eje1 { get => OD_eje; set => OD_eje = value; }
        public string OD_prisma1 { get => OD_prisma; set => OD_prisma = value; }
        public string OD_adicion1 { get => OD_adicion; set => OD_adicion = value; }
        public string OD_AO1 { get => OD_AO; set => OD_AO = value; }
        public string OD_AP1 { get => OD_AP; set => OD_AP = value; }
        public string OD_DP1 { get => OD_DP; set => OD_DP = value; }
        public string OI_esfera1 { get => OI_esfera; set => OI_esfera = value; }
        public string OI_cilindro1 { get => OI_cilindro; set => OI_cilindro = value; }
        public string OI_eje1 { get => OI_eje; set => OI_eje = value; }
        public string OI_prisma1 { get => OI_prisma; set => OI_prisma = value; }
        public string OI_adicion1 { get => OI_adicion; set => OI_adicion = value; }
        public string OI_AO1 { get => OI_AO; set => OI_AO = value; }
        public string OI_AP1 { get => OI_AP; set => OI_AP = value; }
        public string OI_DP1 { get => OI_DP; set => OI_DP = value; }
    }
}
