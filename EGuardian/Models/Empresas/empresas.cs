using System;
using SQLite;

namespace EGuardian.Models.Empresas
{
    public class empresas
    {
        public empresas() { }
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int idEmpresa { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public int numeroColaboradores { get; set; }
        public string telefono { get; set; }
        public string logo { get; set; }
        public string descripcion { get; set; }
        public int status { get; set; }
    }
}