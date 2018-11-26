using EGuardian.Common.Resources;
using EGuardian.Models.Menu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EGuardian.ViewModels.Menu
{
    public class MenuVistaModelo : BaseViewModel
    {
        public ObservableCollection<MenuItem> Menus { get; set; }
        public MenuVistaModelo()
        {
            Title = Strings.MenuTitle;
            Menus = new ObservableCollection<MenuItem>();
            Menus.Add(new MenuItem
            {
                Title = Strings.Menu1,
                Icon = "iEventos.png",
                Id = MenuItemType.Eventos,
                SeparatorVisibility = true
                //MenuTipoSiguiente = MenuItemType.Incidencias
            });
            /*Menus.Add(new MenuItem
            {
                Title = Strings.Menu2,
                Icon = "iIncidencias.png",
                Id = MenuItemType.Incidencias,
                SeparatorVisibility = true
                //MenuTipoSiguiente = MenuItemType.Perfil
            });*/
            Menus.Add(new MenuItem
            {
                Title = Strings.Menu3,
                Icon = "iPerfil.png",
                SeparatorVisibility = true,
                Id = MenuItemType.Perfil,
                //MenuTipoSiguiente = MenuItemType.Reportes
            });
            /*Menus.Add(new MenuItem
            {
                Title = Strings.Menu4,
                Icon = "iReportes.png",
                SeparatorVisibility = true,
                Id = MenuItemType.Reportes,
                //MenuTipoSiguiente = MenuItemType.API
            });*/
            Menus.Add(new MenuItem
            {
                Title = Strings.Menu5,
                Icon = "iAPI.png",
                SeparatorVisibility = true,
                Id = MenuItemType.API,
                //MenuTipoSiguiente = MenuItemType.Ajustes
            });
            Menus.Add(new MenuItem
            {
                Title = Strings.Menu6,
                Icon = "iConfiguracion.png",
                SeparatorVisibility = true,
                Id = MenuItemType.Ajustes,
                //MenuTipoSiguiente = MenuItemType.Salir
            });
            Menus.Add(new MenuItem
            {
                Title = Strings.Menu7,
                Icon = "iSalir.png",
                SeparatorVisibility = false,
                Id = MenuItemType.Salir
            });
        }
    }
}