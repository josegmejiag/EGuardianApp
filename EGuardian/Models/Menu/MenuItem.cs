using EGuardian.Common.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Models.Menu
{
    public enum MenuItemType
    {
        Eventos,
        Incidencias,
        Perfil,
        Reportes,
        API,
        Ajustes,
        Salir
    }
    public class MenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool isSelected { get; set; }
        public Color TextColor
        {
            get
            {
                return isSelected ? Color.FromHex("21538B") : Colors.MenuTitle;
            }
        }


        public string FontFamily
        {
            get
            {
                return isSelected ? Fonts.LabelFont : Fonts.Label2Font;
            }
        }

        public bool SeparatorVisibility { get; set; }
    }
}