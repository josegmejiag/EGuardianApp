using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EGuardian.Helpers.Eventos;
using EGuardian.Models.Eventos;
using Xamarin.Forms;
namespace EGuardian.ViewModels.Eventos
{
    public class EventosViewModel : BaseViewModel
    {
        public ObservableCollection<eventos> Eventos { get; set; }
        public ObservableCollection<EventosAgrupacion<string, eventos>> EventosAgrupados { get; set; }
        private List<eventos> EventosFiltrados = new List<eventos>();
        public EventosViewModel()
        {
            Title = "Eventos";
            IsInitialized = false;
            Eventos = EventoHelper.Eventos;
            EventosAgrupados = EventoHelper.EventosAgrupados;
            EventosFiltrados = Eventos.ToList();
        }
        public int ConteoCitas => Eventos.Count;

        public void FiltrarCitas(string text)
        {
            /*CitasAgrupadas.Clear();
            Citas.Clear();
            CitasFiltradas.Where(t => t.nombrePilaPaciente.ToLower().Contains(text.ToLower())).ToList().ForEach(t => Citas.Add(t));
            var sorted = from cita in Citas
                orderby cita.fechaCita
                            group cita by cita.fechaCita into citasAgrupadas
                            select new CitasAgrupacion<string, citas>(citasAgrupadas.Key, citasAgrupadas);

            CitasAgrupadas = new ObservableCollection<CitasAgrupacion<string, citas>>(sorted);*/
        }
    }
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}
