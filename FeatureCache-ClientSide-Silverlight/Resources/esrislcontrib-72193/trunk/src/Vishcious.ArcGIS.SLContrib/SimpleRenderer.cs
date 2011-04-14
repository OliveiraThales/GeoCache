using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client;

namespace Vishcious.ArcGIS.SLContrib
{
    public class SimpleRenderer : IRenderer
    {
        // Fields
        private Color _color;
        private SimpleFillSymbol _fill;
        private SimpleLineSymbol _line;
        private SimpleMarkerSymbol _marker;

        // Methods
        public SimpleRenderer()
        {
            Color = Colors.Red;
        }

        public Symbol GetSymbol( Graphic graphic )
        {
            if( graphic != null )
            {
                if( ( graphic.Geometry is MapPoint ) || ( graphic.Geometry is MultiPoint ) )
                {
                    return _marker;
                }
                if( graphic.Geometry is ESRI.ArcGIS.Client.Geometry.Polyline )
                {
                    return _line;
                }
                if( ( graphic.Geometry is ESRI.ArcGIS.Client.Geometry.Polygon ) || ( graphic.Geometry is Envelope ) )
                {
                    return _fill;
                }
            }
            return null;
        }

        // Properties
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                if( _color != value )
                {
                    _color = value;

                    _fill = new SimpleFillSymbol();
                    _fill.Fill = new SolidColorBrush( _color );


                    _line = new SimpleLineSymbol();
                    _line.Color = new SolidColorBrush( this._color );

                    _marker = new SimpleMarkerSymbol();
                    _marker.Color = new SolidColorBrush( this._color );
                    _marker.Size = 15;
                }
            }
        }
    }


}
