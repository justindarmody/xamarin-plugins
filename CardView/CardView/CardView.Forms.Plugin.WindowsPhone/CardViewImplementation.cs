using CardView.Forms.Plugin.Abstractions;
using System;
using Xamarin.Forms;
using CardView.Forms.Plugin.WindowsPhone;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(CardView.Forms.Plugin.Abstractions.CardViewControl), typeof(CardViewRenderer))]
namespace CardView.Forms.Plugin.WindowsPhone
{
    /// <summary>
    /// CardView Renderer
    /// </summary>
    public class CardViewRenderer //: TRender (replace with renderer type
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init() { }
    }
}
