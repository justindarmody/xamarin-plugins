using System;
using Xamarin.Forms;

namespace FAB.Forms
{
    public class FloatingActionButton : View
    {
        public static readonly BindableProperty SizeProperty = BindableProperty.Create<FloatingActionButton, FabSize>(mn => mn.Size, FabSize.Normal);

        public static readonly BindableProperty NormalColorProperty = BindableProperty.Create<FloatingActionButton, Color>(mn => mn.NormalColor, Color.Blue);

        public static readonly BindableProperty PressedColorProperty = BindableProperty.Create<FloatingActionButton, Color>(mn => mn.PressedColor, Color.Blue.MultiplyAlpha(0.1));

        public static readonly BindableProperty DisabledColorProperty = BindableProperty.Create<FloatingActionButton, Color>(mn => mn.DisabledColor, Color.Gray);

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create<FloatingActionButton, bool>(mn => mn.HasShadow, true);

        public static readonly BindableProperty SourceProperty = BindableProperty.Create<FloatingActionButton, ImageSource>(mn => mn.Source, null);

        public FabSize Size
        { 
            get { return (FabSize)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public Color NormalColor {
            get { return (Color)this.GetValue(NormalColorProperty); }
            set { this.SetValue(NormalColorProperty, value); }
        }

        public Color PressedColor
        {
            get { return (Color)this.GetValue(PressedColorProperty); }
            set { this.SetValue(PressedColorProperty, value); }
        }

        public Color DisabledColor
        { 
            get { return (Color)this.GetValue(DisabledColorProperty); }
            set { this.SetValue(DisabledColorProperty, value); }
        }

        public bool HasShadow
        { 
            get { return (bool)this.GetValue(HasShadowProperty); }
            set { this.SetValue(HasShadowProperty, value); }
        }

        [TypeConverter(typeof(ImageSourceConverter))] 
        public ImageSource Source { 
            get { return (ImageSource)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public FloatingActionButton()
        {
        }
    }
}

