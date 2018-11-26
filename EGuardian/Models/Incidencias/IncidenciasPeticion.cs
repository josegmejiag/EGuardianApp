using System;
namespace EGuardian.Models.Incidencias
{
    public class RegistarIncidente
    {
        public int duracion { get; set; }
        public string fechaInicio { get; set; }
        public int idaccion { get; set; }
        public int idaplicacion { get; set; }
        public int idevento { get; set; }
    }

    public class RegistarIncidenteResponse
    {

    }
}
