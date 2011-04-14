using System.Windows.Media;
using System.IO;
namespace Vishcious.ArcGIS.SLContrib
{
    public class RainDropPngRenderer
    {
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }
        public PngEncoder encoder
        {
            get;
            set;
        }
        public RainDropRenderer renderer
        {
            get;
            set;
        }

        public RainDropPngRenderer( int width, int height )
        {
            encoder = new PngEncoder( width, height );
            renderer = new RainDropRenderer( width, height );
        }

        public void Display( Color background )
        {
            renderer.Display( encoder, background );
        }

        public Stream GetImageStream()
        {
            return encoder.GetImageStream();
        }

        public void Splash( int cx, int cy, int rippleRadius )
        {
            renderer.Splash( cx, cy, rippleRadius );
        }

        public void Step()
        {
            renderer.Step();
        }
    }
}