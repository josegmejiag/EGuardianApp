using System;
using EGuardian.Common.Resources;
using SQLite;
using Xamarin.Forms;

namespace EGuardian.Models.Acciones
{
    public class acciones
    {
        public acciones() { }
        [PrimaryKey]
        public int idAccion { get; set; }
        public String nombre { get; set; }
        public String Nombre
        {
            get
            {
                return nombre;
            }
        }
        public bool IsVisible
        {
            get
            {
                return false;
            }
        }

        public bool isVisible
        {
            get
            {
                return true;
            }
        }

        public bool mostrarChrome
        {
            get
            {
                return false;
            }
        }
        public ImageSource icono
        {
            get
            {
                switch (idAccion)
                {
                    case 1:
                        return Images.Sonido;
                    case 2:
                        return Images.Correo;
                    case 3:
                        return Images.Vibracion;
                    case 4:
                        return Images.SMS;
                    case 5:
                        return Images.Avion;
                    default:
                        return null;
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                switch (idAccion)
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
