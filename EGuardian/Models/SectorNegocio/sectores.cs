using System;
using SQLite;

namespace EGuardian.Models.SectorNegocio
{
    public class sectores
    {
        public sectores(){}
        [PrimaryKey]
        public int id { get; set; }
        public String nombre { get; set; }
    }
}