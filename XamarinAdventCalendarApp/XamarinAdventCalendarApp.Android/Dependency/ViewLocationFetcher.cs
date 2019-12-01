using System.Drawing;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using XamarinAdventCalendarApp.Interfaces;
using XamarinAdventCalendarApp.Droid.Dependency;

[assembly: Dependency(typeof(ViewLocationFetcher))]
namespace XamarinAdventCalendarApp.Droid.Dependency
{
    public class ViewLocationFetcher : IViewLocationFetcher
    {
        public PointF GetCoordinates(VisualElement view)
        {
            var renderer = Platform.GetRenderer(view);
            if (renderer == null)
                return new PointF();
            var nativeView = renderer.View;
            var location = new int[2];

            var density = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;

            nativeView.GetLocationOnScreen(location);
            return new PointF(location[0] / (float)density, location[1] / (float)density);
        }
    }
}