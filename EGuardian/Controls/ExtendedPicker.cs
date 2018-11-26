using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Controls
{
    public class ExtendedPicker : Picker
    {
        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedPicker), true);

        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(ExtendedEntry),
            TextAlignment.Start);
        public static readonly BindableProperty FontProperty =
           BindableProperty.Create("Font", typeof(Font), typeof(ExtendedEntry), new Font());

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }

        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }
    }
}