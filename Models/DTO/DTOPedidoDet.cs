using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticaMultivisual.Models.DTO
{
    internal class DTOPedidoDet : dbContext
    {
        private int pd_ID;
        private int con_ID;
        private DateTime pd_fpedido;
        private DateTime pd_fprogramada;
        private string art_codigo;
        private int art_cant;
        private string pd_obser;
        private int pd_recetalab;

        public int pd_ID1 { get => pd_ID; set => pd_ID = value; }
        public int con_ID1 { get => con_ID; set => con_ID = value; }
        public DateTime pd_fpedido1 { get => pd_fpedido; set => pd_fpedido = value; }
        public DateTime pd_fprogramada1 { get => pd_fprogramada; set => pd_fprogramada = value; }
        public string art_codigo1 { get => art_codigo; set => art_codigo = value; }
        public int art_cant1 { get => art_cant; set => art_cant = value; }
        public string pd_obser1 { get => pd_obser; set => pd_obser = value; }
        public int pd_recetalab1 { get => pd_recetalab; set => pd_recetalab = value; }
    }
}
