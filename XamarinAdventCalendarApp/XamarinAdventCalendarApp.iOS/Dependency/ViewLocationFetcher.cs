using System.Drawing;

using Xamarin.Forms;
using Xamarin.Essentials;

using XamarinAdventCalendarApp.Interfaces;
using XamarinAdventCalendarApp.iOS.Dependency;

[assembly: Dependency(typeof(ViewLocationFetcher))]
namespace XamarinAdventCalendarApp.iOS.Dependency
{
    public class ViewLocationFetcher : IViewLocationFetcher
    {
        public PointF GetCoordinates(VisualElement view)
        {
            var renderer = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(view);
            var nativeView = renderer.NativeView;

            var rect = nativeView.Superview.ConvertPointToView(nativeView.Frame.Location, null);
            return rect.ToSystemPointF();
        }
    }
}