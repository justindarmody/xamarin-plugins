using System;
using System.Linq;
using CardView.Forms.Plugin.Abstractions;
using Xamarin.Forms.Platform.Android;
using CardView.Forms.Plugin.Droid;
using Xamarin.Forms;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;
using System.ComponentModel;
using Android.App;
using Android.Content;
using System.Collections.ObjectModel;

[assembly: ExportRenderer(typeof(CardsView), typeof(CardsViewRenderer))]

namespace CardView.Forms.Plugin.Droid
{
    public class CardsViewRenderer  : ViewRenderer<CardsView, Android.Widget.ListView> //Android.Widget.ListView, IVisualElementRenderer //VisualElementRenderer<CardsView>
    {
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        bool init;
        ViewGroup packed;

        private CardViewAdapter adapter;

        private IEnumerable<CardContentView> DataSource 
        {
            get { return (this.Element as CardsView).Children.Cast<CardContentView>(); }
        }

        public CardsViewRenderer()
        {
            this.AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CardsView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if (this.adapter != null)
                {
                    this.adapter.Dispose();
                    this.adapter = (CardViewAdapter) null;
                }
            }
            if (e.NewElement == null) {
                return;
            }

            Android.Widget.ListView listView = this.Control;
            if (listView == null)
            {
                Activity activity = (Activity)this.Context;
                listView = new Android.Widget.ListView((Context) activity);

                this.SetNativeControl(listView);//, (ViewGroup) this.refresh);
            }
                
            listView.DividerHeight = 0;
            listView.Focusable = false;
            listView.DescendantFocusability = DescendantFocusability.AfterDescendants;

            listView.Adapter = this.adapter = new CardViewAdapter(this.DataSource);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        void ChildrenReordered(object sender, EventArgs e)
        {

        }

        void ChildRemoved(object sender, ElementEventArgs e)
        {

        }

        void ChildAdded(object sender, ElementEventArgs e)
        {

        }

        private class CardViewAdapter : BaseAdapter<CardContentJavaWrapper>
        {
            public IList<CardContentView> ItemSource
            {
                get;
                private set;
            }

            public List<CardViewRenderer> Renderers { get; private set; }

            public override int Count
            {
                get
                {
                    return this.ItemSource.Count;
                }
            }


            public CardViewAdapter(IEnumerable<CardContentView> data)  : base()
            {
                this.ItemSource = new List<CardContentView>(data);
                this.Renderers = new List<CardViewRenderer>(this.ItemSource.Count); //new List<CardsViewRenderer>(this.ItemSource.Count);
            }

            public override CardContentJavaWrapper this[int index]
            {
                get
                {
                    return this.GetItem(index) as CardContentJavaWrapper;
                }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                // could wrap a Contact in a Java.Lang.Object
                // to return it here if needed
                return new CardContentJavaWrapper
                {
                    Element = this.ItemSource[position]
                };
            }

            public override long GetItemId(int position)
            {
                return (long)position;
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
            {
                var view = convertView;// as CardViewRenderer;

                if (view == null)
                {
                    view = new Container(Xamarin.Forms.Forms.Context);
                    view.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                }

                var renderer = RendererFactory.GetRenderer(this.ItemSource[position]);

                (view as Container).Child = renderer;

//                var width = parent.Width;
//                var height = parent.Height;
//
//                cardElement.Layout(new Rectangle(0, 0, width, height));
//                view.ForceLayout();
//                try {
//                this.Renderers.RemoveAt(position);
//                }
//                catch(Exception) {
//                }
//
//                try { 
//                    this.Renderers.Insert(position, view);
//                }
//                catch(Exception) {
//                }

                return view;
            }

            public void Reset(IEnumerable<CardContentView> replaceWith)
            {
                this.ItemSource.Clear();

                foreach (var replace in replaceWith)
                {
                    this.ItemSource.Add(replace);
                }
                this.Renderers.Clear();
                this.Renderers = new List<CardViewRenderer>(this.ItemSource.Count);

                this.NotifyDataSetChanged();
            }
        }

        private class CardContentJavaWrapper : Java.Lang.Object
        {
            public CardContentView Element
            {
                get;
                set;
            }

            public CardContentJavaWrapper() : base()
            {
            }

            public CardContentJavaWrapper(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
                : base(handle, transfer)
            {
            }
        }
    
        internal class Container : ViewGroup
        {
            private IVisualElementRenderer child;

            public IVisualElementRenderer Child
            {
                set
                {
                    if (this.child != null)
                        this.RemoveView((Android.Views.View) this.child.ViewGroup);
                    this.child = value;
                    this.AddView((Android.Views.View) value.ViewGroup);
                }
            }

            public Container(Context context)
                : base(context)
            {
            }

            protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
            {
                if (this.child == null)
                {
                    this.SetMeasuredDimension(widthMeasureSpec, heightMeasureSpec);
                }
                else
                {
                    VisualElement element = this.child.Element;
                    Context context = this.Context;
                    int num = (int) ContextExtensions.FromPixels(context, (double) MeasureSpecFactory.GetSize(widthMeasureSpec));
                    SizeRequest sizeRequest = this.child.Element.GetSizeRequest((double) num, double.PositiveInfinity);
                    this.child.Element.Layout(new Rectangle(0.0, 0.0, (double) num, sizeRequest.Request.Height));
                    int measuredWidth = MeasureSpecFactory.MakeMeasureSpec((int) ContextExtensions.ToPixels(context, element.Width), MeasureSpecMode.Exactly);
                    int measuredHeight = MeasureSpecFactory.MakeMeasureSpec((int) ContextExtensions.ToPixels(context, element.Height), MeasureSpecMode.Exactly);
                    this.child.ViewGroup.Measure(widthMeasureSpec, heightMeasureSpec);
                    this.SetMeasuredDimension(measuredWidth, measuredHeight);
                }
            }

            protected override void OnLayout(bool changed, int l, int t, int r, int b)
            {
                if (this.child == null)
                    return;
                this.child.UpdateLayout();
            }
        }
    }
}