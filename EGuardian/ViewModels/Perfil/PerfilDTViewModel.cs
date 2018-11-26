using System;
using Xamarin.Forms;

namespace EGuardian.ViewModels.Perfil
{
    public class PerfilDTViewModel : DataTemplateSelector
    {
        private readonly DataTemplate Usuario;
        private readonly DataTemplate Cuenta;
        private readonly DataTemplate Empleados;

        public PerfilDTViewModel()
        {
            this.Usuario = new DataTemplate(typeof(UsuarioDTViewModel));
            this.Cuenta = new DataTemplate(typeof(CuentaDTViewModel));
            this.Empleados = new DataTemplate(typeof(EmpleadosDTViewModel));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch ((int)item)
            {
                case 1:
                    return Usuario;
                case 2:
                    return Cuenta;
                case 3:
                    return Empleados;
                default:
                    return Usuario;
            }
        }
    }
}