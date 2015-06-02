using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace CardView.Forms.Plugin.Abstractions
{
    /// <summary>
    /// CardContentView Interface
    /// </summary>
    public class CardContentView : ContentView
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<CardContentView,float> ( p => p.CornderRadius, 3.0F);   
        public static readonly BindableProperty IsSwipeableProperty = BindableProperty.Create<CardContentView,bool> ( p => p.IsSwipeable, true);   

        public static readonly BindableProperty CommandProperty = BindableProperty.Create<CardContentView,ICommand> ( p => p.Command, null, propertyChanged:CommandChanged);   
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<CardContentView,object> ( p => p.CommandParameter, null, propertyChanged:CommandParameterChanged);   

        private TapGestureRecognizer tapGesture;


        public float CornderRadius 
        {
            get { return (float)GetValue (CornerRadiusProperty); } 
            set { SetValue (CornerRadiusProperty, value); } 
        }

        public bool IsSwipeable
        {
            get { return (bool)GetValue (IsSwipeableProperty); } 
            set { SetValue (IsSwipeableProperty, value); } 
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue (CommandProperty); } 
            set { SetValue (CommandProperty, value); } 
        }

        public object CommandParameter
        {
            get { return (object)GetValue (CommandParameterProperty); } 
            set { SetValue (CommandParameterProperty, value); } 
        }

        protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
        {
            return base.OnSizeRequest(widthConstraint, heightConstraint);
        }

        private void InternalCommandChanged()
        {
            if (tapGesture != null)
            {
                this.GestureRecognizers.Remove(this.tapGesture);
            }

            tapGesture = null;
            tapGesture = new TapGestureRecognizer();
            tapGesture.Command = this.Command;
            tapGesture.CommandParameter = this.CommandParameter;

            this.GestureRecognizers.Add(tapGesture);
        }

        private static void CommandChanged(BindableObject bindable, ICommand oldValue, ICommand newValue)
        {
            (bindable as CardContentView).InternalCommandChanged();
        }

        private static void CommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CardContentView).InternalCommandChanged();
        }
    }
}
