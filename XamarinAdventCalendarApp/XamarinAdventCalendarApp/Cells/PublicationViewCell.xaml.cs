using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinAdventCalendarApp.Interfaces;
using XamarinAdventCalendarApp.Models;

namespace XamarinAdventCalendarApp.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicationViewCell : ViewCell
    {
        SKColor _accentColor;
        SKColor _accentDarkColor;
        SKColor _accentExtraDarkColor;
        SKPaint _accentPaint;
        SKPaint _accentDarkPaint;
        SKPaint _accentExtraDarkPaint;

        IViewLocationFetcher viewLocationFetcher;

        public PublicationViewCell()
        {
            InitializeComponent();

            viewLocationFetcher = DependencyService.Get<IViewLocationFetcher>();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.BindingContext != null)
            {
                var colors = ((Publication)BindingContext).Colors;

                _accentColor = colors.Accent.ToSKColor();
                _accentDarkColor = colors.DarkAccent.ToSKColor();
                _accentExtraDarkColor = colors.ExtraDarkAccent.ToSKColor();
                _accentPaint = new SKPaint() { Color = _accentColor };
                _accentDarkPaint = new SKPaint() { Color = _accentDarkColor };
                _accentExtraDarkPaint = new SKPaint() { Color = _accentExtraDarkColor };
            }
        }

        private void CellBackgroundCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // work out where the cell actually is on the page
            var thisCellPosition = viewLocationFetcher.GetCoordinates(this.View);

            canvas.DrawRect(info.Rect, _accentPaint);

            // create path for light color
            using (SKPath path = new SKPath())
            {
                path.MoveTo(0, 0);
                path.LineTo((info.Width) - thisCellPosition.Y, 0);
                path.LineTo(0, info.Height * .8f);
                path.Close();
                canvas.DrawPath(path, _accentDarkPaint);
            }
            using (SKPath path = new SKPath())
            {
                path.MoveTo(0, 0);
                path.LineTo(info.Width - (thisCellPosition.Y * 2f), 0);
                path.LineTo(0, info.Height * .6f);
                path.Close();
                canvas.DrawPath(path, _accentExtraDarkPaint);
            }
        }

    }
}