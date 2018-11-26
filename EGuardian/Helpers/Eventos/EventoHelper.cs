using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using EGuardian.Models.Eventos;

namespace EGuardian.Helpers.Eventos
{
    public static class EventoHelper
    {
        public static ObservableCollection<EventosAgrupacion<string, eventos>> EventosAgrupados { get; set; }
        public static ObservableCollection<eventos> Eventos { get; set; }

        static EventoHelper()
        {
            Eventos = new ObservableCollection<eventos>(App.Database.GetEventos(Convert.ToInt32(Settings.session_idUsuario)));
            var sorted = from evento in Eventos
                         orderby evento.fechaCita
                         group evento by evento.fechaCita into eventoAgrupadas
                         select new EventosAgrupacion<string, eventos>(eventoAgrupadas.Key, eventoAgrupadas);

            EventosAgrupados = new ObservableCollection<EventosAgrupacion<string, eventos>>(sorted);
        }
    }
}