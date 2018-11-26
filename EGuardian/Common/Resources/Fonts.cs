using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Common.Resources
{
    public static class Fonts
    {
        public static readonly string ButtonFont = Device.OnPlatform("Montserrat-Regular", "Montserrat-Regular", null);
        public static readonly string LabelFont = Device.OnPlatform("Montserrat-Bold", "Montserrat-Bold", null);
        public static readonly string Label2Font = Device.OnPlatform("Montserrat-Regular", "Montserrat-Regular", null);
        public static readonly string EntryFont = Device.OnPlatform("OpenSans", "OpenSans-Regular", null);

    }
}