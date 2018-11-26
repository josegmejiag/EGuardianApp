using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Controls
{
    public class ExtendedLabel : Label
    {
        public static readonly BindableProperty IsStrikeThroughProperty = BindableProperty.Create(nameof(IsStrikeThrough), typeof(bool), typeof(ExtendedLabel), false);

        public bool IsStrikeThrough
        {
            get => (bool)GetValue(IsStrikeThroughProperty);
            set => SetValue(IsStrikeThroughProperty, value);
        }
    }
}