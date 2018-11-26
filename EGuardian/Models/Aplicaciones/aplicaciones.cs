using System;
using EGuardian.Common.Resources;
using SQLite;
using Xamarin.Forms;

namespace EGuardian.Models.Aplicaciones
{
    public class aplicaciones
    {
        public aplicaciones() { }
        [PrimaryKey]
        public int idAplicacion { get; set; }
        public String nombre { get; set; }
        public String package { get; set; }
        public String Nombre
        {
            get
            {
                return nombre;
            }
        }
        public String Package
        {
            get
            {
                return package.ToUpper();
            }
        }
        public bool IsVisible
        {
            get
            {
                return true;
            }
        }
        public ImageSource icono 
        {
            get
            {
                switch(idAplicacion)
                {
                    case 1:
                        return Images.Facebook;
                    case 2:
                        return Images.Navegador;
                    case 3:
                        return Images.Twitter;
                    case 4:
                        return Images.WhatsApp;
                    case 5:
                        return Images.Youtube;
                    default:
                        return null;
                }
            } 
        }

        public bool isVisible
        {
            get
            {
                if (idAplicacion == 2)
                    return false;
                else
                    return true;
            }
        }

        public bool mostrarChrome
        {
            get
            {
                if (idAplicacion == 2)
                    return true;
                else
                    return false;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                switch (idAplicacion)
                {
                    case 1:
                        return Color.FromHex("26579A");
                    case 2:
                        return Color.FromHex("F7B819");
                    case 3:
                        return Color.FromHex("01ABF0");
                    case 4:
                        return Color.FromHex("05D058");
                    case 5:
                        return Color.FromHex("FF0D01");
                    default:
                        return Color.Transparent;
                }
            }
        }
    }
}
