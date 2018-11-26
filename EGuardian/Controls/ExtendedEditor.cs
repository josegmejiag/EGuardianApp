using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EGuardian.Controls
{
    public class ExtendedEditor : Editor
    {
        /// <summary>
        /// The font property
        /// </summary>
        public static readonly BindableProperty FontProperty =
            BindableProperty.Create("Font", typeof(Font), typeof(ExtendedEditor), new Font());

        /// <summary>
        /// The XAlign property
        /// </summary>
        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(ExtendedEditor),
            TextAlignment.Start);


        public ExtendedEditor() : base()
        {
        }

        public void InvalidateLayout()
        {
            this.InvalidateMeasure();
        }


        // <summary>
        /// Gets or sets the Font
        /// </summary>
        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the X alignment of the text
        /// </summary>
        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }
    }
}