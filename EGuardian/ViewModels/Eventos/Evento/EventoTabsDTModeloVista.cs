using System;
using EGuardian.Models.Eventos;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Eventos.Evento
{
    public class EventoTabsDTModeloVista : DataTemplateSelector
    {
        private readonly DataTemplate EventoDatos;
        private readonly DataTemplate EventoAsistntes;

        public EventoTabsDTModeloVista()
        {
            this.EventoDatos = new DataTemplate(typeof(EventoDatosViewModel));
            this.EventoAsistntes = new DataTemplate(typeof(EventoAsistentesViewModel));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            eventoItemSource EventoItemSource = (eventoItemSource)item;
            switch (EventoItemSource.id)
            {
                case 1:
                    return EventoDatos;
                case 2:
                    return EventoAsistntes;
                default:
                    return EventoDatos;
            }
        }
    }
}