using System;
using System.Linq;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CardView.Forms.Plugin.Abstractions
{
    public class CardsView : ScrollView//StackLayout//Layout<CardContentView>
    {
        internal event EventHandler LayoutChildrenRequested;

        internal new View Content
        {
            get { return base.Content; }
            set { base.Content = value; }
        }

        public new Color BackgroundColor 
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public CardsView()
        {
            this.Content = new StackLayout
            {
                    Padding = new Thickness(15),
                    Spacing = 15
            };
        }

        public void AddCard(CardContentView card)
        {
            (this.Content as StackLayout).Children.Add(card);
        }

        public void RemoveCard(CardContentView card)
        {
            (this.Content as StackLayout).Children.Remove(card);
        }

//        protected override void OnSizeAllocated(double width, double height)
//        {
//            base.OnSizeAllocated(width, height);
//        }
//
//        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
//        {
//            return base.OnSizeRequest(widthConstraint, heightConstraint);
//        }

//        protected override void LayoutChildren(double x, double y, double width, double height)
//        {
////            throw new NotImplementedException();
//        }
    }
}