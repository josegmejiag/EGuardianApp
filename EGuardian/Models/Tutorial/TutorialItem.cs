using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Models.Tutorial
{
    class TutorialItem
    {
        public int index { get; set; }
        public ImageSource imagen { get; set; }
        public string texto { get; set; }
        public Thickness Padding { get; set; }
    }
}