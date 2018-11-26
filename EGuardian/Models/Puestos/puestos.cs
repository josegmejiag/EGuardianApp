using System;
using SQLite;

namespace EGuardian.Models.Puestos
{
    public class puestos
    {
        public puestos(){}
        [PrimaryKey]
        public int id { get; set; }
        public String nombre { get; set; }
    }
}