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

namespace Vishcious.ArcGIS.SLContrib
{
    public class RainDropRenderer
    {
        int[,] _buffer1;
        int[,] _buffer2;
        int _width;
        int _height;
        double _damping = 0.94;

        public RainDropRenderer( int width, int height )
        {
            _width = width;
            _height = height;
            _buffer1 = new int[width, height];
            _buffer2 = new int[width, height];
        }

        /// <summary>
        /// executes a single step of ripple propagation
        /// </summary>
        public void Step()
        {
            for (int y = 1; y < _height - 1; y++)
            {
                for (int x = 1; x < _width - 1; x++)
                {
                    _buffer2[x, y] =
                        (_buffer1[x - 1, y] + _buffer1[x + 1, y] + _buffer1[x, y + 1] + _buffer1[x, y - 1]) / 2
                        - _buffer2[x, y];

                    _buffer2[x, y] = (int)((double)_buffer2[x, y] * _damping);
                    //if (_buffer2[x, y] < 0) _buffer2[x, y] = 0;

                }
            }

            for (int x = 0; x < _width; x++)
            {
                _buffer2[x, 0] = 0;
                _buffer2[x, _height-1] = 0;
            }

            for (int y = 0; y < _height; y++)
            {
                _buffer2[0, y] = 0;
                _buffer2[_width-1, y] = 0;
            }

            object o = _buffer1;
            _buffer1 = _buffer2;
            _buffer2 = (int[,])o;
        }

        public void Display( PngEncoder surface, Color baseColor )
        {
            for( int y = 1; y < _height - 1; y++ )
            {
                int rowStart = surface.GetRowStart( y );
                for( int x = 1; x < _width - 1; x++ )
                {
                    int xoffset = _buffer1[ x - 1, y ] - _buffer1[ x + 1, y ];
                    int yoffset = _buffer1[ x, y - 1 ] - _buffer1[ x, y + 1 ];
                    int shading = ( xoffset - yoffset ) / 2;

                    xoffset = ( int ) ( xoffset / 35.0 );
                    yoffset = ( int ) ( yoffset / 35.0 );

                    int xnew = x + xoffset;
                    if( xnew < 0 )
                        xnew = 0;
                    if( xnew >= _width )
                        xnew = _width - 1;

                    int ynew = y + yoffset;
                    if( ynew < 0 )
                        ynew = 0;
                    if( ynew >= _height )
                        ynew = _height - 1;

                    byte tred = baseColor.R;
                    byte tgreen = baseColor.G;
                    byte tblue = baseColor.B;

                    if( shading < 0 )
                        shading = 0;
                    if( shading > 255 )
                        shading = 255;

                    tred = Saturate( tred + shading );
                    tgreen = Saturate( tgreen + shading );
                    tblue = Saturate( tblue + shading );

                    surface.SetPixelAtRowStart( x, rowStart, tred, tgreen, tblue, Saturate( shading * 12) );
                }
            }
        }

        private byte Saturate(int value)
        {
            if (value > 255) return 255;
            return (byte) value;
        }

        public void Splash(int cx, int cy, int rippleRadius)
        {
            int sy = cy - rippleRadius;
            if (sy < 0) sy = 0;
            int ey = cy + rippleRadius;
            if (ey > _height) ey = _height;

            int sx = cx - rippleRadius;
            if (sx < 0) sx = 0;
            int ex = cx + rippleRadius;
            if (ex > _width) ex = _width;

            for (int y = sy; y < ey; y++)
            {
                for (int x = sx; x < ex; x++)
                {
                    int distM2 = (x - cx) * (x - cx) + (y - cy) * (y - cy);
                    if (distM2 < rippleRadius * rippleRadius)
                    {
                        _buffer1[x, y] += 255 - (int)(512 * 1 - distM2 / (rippleRadius * rippleRadius));
                    }
                }
            }

        }
    }
}
