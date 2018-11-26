using System;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Ajustes
{
    public class AjustesDTViewModel : DataTemplateSelector
    {
        private readonly DataTemplate Aplicaciones;
        private readonly DataTemplate Acciones;
        private readonly DataTemplate Contrasenia;

        public AjustesDTViewModel()
        {
            this.Aplicaciones = new DataTemplate(typeof(AplicacionesDTViewModel));
            this.Acciones = new DataTemplate(typeof(AccionesDTViewModel));
            this.Contrasenia = new DataTemplate(typeof(ContraseniaDTViewModel));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch ((int)item)
            {
                case 1:
                    return Aplicaciones;
                case 2:
                    return Acciones;
                case 3:
                    return Contrasenia;
                default:
                    return Aplicaciones;
            }
        }
    }
}