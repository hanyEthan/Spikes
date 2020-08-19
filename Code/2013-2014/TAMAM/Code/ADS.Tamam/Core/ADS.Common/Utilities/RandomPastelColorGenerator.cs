using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace ADS.Common.Utilities
{
    /// <summary>
    /// Color Utility : provides a range of random pastel colors ...
    /// </summary>
    public class RandomPastelColorGenerator
    {
        #region props ...

        private readonly Random _random;
        private List<ColorContainer> _colors;

        private const int _max = 20;
        private const int DELTA_PERCENT = 10;

        #endregion
        #region cst ...

        public RandomPastelColorGenerator()
        {
            _random = new Random();
            _colors = new List<ColorContainer>();
        }

        #endregion
        #region models

        private struct ColorContainer
        {
            public Color Color { get; set; }
            public Guid Id { get; set; }
        }
        private struct ColorRGB
        {
            public byte R;
            public byte G;
            public byte B;
            public ColorRGB( Color value )
            {
                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }
            public static implicit operator Color( ColorRGB rgb )
            {
                Color c = Color.FromArgb( rgb.R , rgb.G , rgb.B );
                return c;
            }
            public static explicit operator ColorRGB( Color c )
            {
                return new ColorRGB( c );
            }
        }

        #endregion
        #region publics ...

        public Color GetNext()
        {
            // to create lighter colours:
            // take a random integer between 0 & 128 (rather than between 0 and 255)
            // and then add 127 to make the colour lighter
            byte[] colorBytes = new byte[3];
            colorBytes[0] = ( byte ) ( _random.Next( 128 ) + 127 );
            colorBytes[1] = ( byte ) ( _random.Next( 128 ) + 127 );
            colorBytes[2] = ( byte ) ( _random.Next( 128 ) + 127 );

            Color color = Color.FromArgb( 255 , colorBytes[0] , colorBytes[1] , colorBytes[2] );
            //if ( !ValidateColor ( color ) )
            //    color = GetNext ();
            return color;
        }
        public Color GetNext( Guid Id )
        {
            var color = _colors.Where( c => c.Id == Id ).FirstOrDefault();
            if ( color.Id != Guid.Empty )
                return color.Color;
            else
            {
                var newColor = GetNext();
                _colors.Add( new ColorContainer { Color = newColor , Id = Id } );
                return newColor;
            }
        }
        public static Color GetGradient( Color c , int factor = 52 )
        {
            //ColorRGB t = new ColorRGB ( c );
            //double h,s,l;
            //RGB2HSL ( t , out h , out s , out l );

            //ColorRGB rgb = HSL2RGB ( h+ 0.1 , s , l );
            return Color.FromArgb( c.R - factor < 0 ? 0 : c.R - factor , c.G - factor < 0 ? 0 : c.G - factor , c.B - factor < 0 ? 0 : c.B - factor );
        }

        #endregion
        #region helpers

        private List<Color> GetRandomColors()
        {

            List<Color> alreadyChoosenColors = new List<Color>();

            // initialize the random generator
            Random r = new Random();

            for ( int i = 0 ; i < _max ; i++ )
            {
                bool chooseAnotherColor = true;
                Color tmpColor = new Color();
                while ( chooseAnotherColor )
                {
                    // create a random color by generating three random channels
                    //
                    int redColor = r.Next( 0 , 128 );
                    int greenColor = r.Next( 0 , 128 );
                    int blueColor = r.Next( 0 , 128 );
                    tmpColor = Color.FromArgb( redColor , greenColor , blueColor );

                    // check if a similar color has already been created
                    //
                    chooseAnotherColor = false;
                    foreach ( Color c in alreadyChoosenColors )
                    {
                        int delta = c.R * DELTA_PERCENT / 100;
                        if ( c.R - delta <= tmpColor.R && tmpColor.R <= c.R + delta )
                        {
                            chooseAnotherColor = true;
                            break;
                        }

                        delta = c.G * DELTA_PERCENT / 100;
                        if ( c.G - delta <= tmpColor.G && tmpColor.G <= c.G + delta )
                        {
                            chooseAnotherColor = true;
                            break;
                        }

                        delta = c.B * DELTA_PERCENT / 100;
                        if ( c.B - delta <= tmpColor.B && tmpColor.B <= c.B + delta )
                        {
                            chooseAnotherColor = true;
                            break;
                        }
                    }
                }

                alreadyChoosenColors.Add( tmpColor );
                // you can safely use the tmpColor here
            }
            return alreadyChoosenColors;
        }
        private bool ValidateColor( Color color )
        {
            foreach ( var item in _colors.Select( c => c.Color ) )
            {
                int delta = item.R * DELTA_PERCENT / 100;
                if ( item.R - delta <= color.R && color.R <= item.R + delta )
                {
                    return false;
                }

                delta = item.G * DELTA_PERCENT / 100;
                if ( item.G - delta <= color.G && color.G <= item.G + delta )
                {
                    return false;
                }

                delta = item.B * DELTA_PERCENT / 100;
                if ( item.B - delta <= color.B && color.B <= item.B + delta )
                {
                    return false;
                }
            }
            return true;
        }
        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        private static ColorRGB HSL2RGB( double h , double sl , double l )
        {
            double v;
            double r , g , b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = ( l <= 0.5 ) ? ( l * ( 1.0 + sl ) ) : ( l + sl - l * sl );
            if ( v > 0 )
            {
                double m;
                double sv;
                int sextant;
                double fract , vsf , mid1 , mid2;

                m = l + l - v;
                sv = ( v - m ) / v;
                h *= 6.0;
                sextant = ( int ) h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch ( sextant )
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            ColorRGB rgb;
            rgb.R = Convert.ToByte( r * 255.0f );
            rgb.G = Convert.ToByte( g * 255.0f );
            rgb.B = Convert.ToByte( b * 255.0f );
            return rgb;
        }
        // Given a Color (RGB Struct) in range of 0-255
        // Return H,S,L in range of 0-1
        private static void RGB2HSL( ColorRGB rgb , out double h , out double s , out double l )
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;
            double v;
            double m;
            double vm;
            double r2 , g2 , b2;

            h = 0; // default to black
            s = 0;
            l = 0;
            v = Math.Max( r , g );
            v = Math.Max( v , b );
            m = Math.Min( r , g );
            m = Math.Min( m , b );
            l = ( m + v ) / 2.0;
            if ( l <= 0.0 )
            {
                return;
            }
            vm = v - m;
            s = vm;
            if ( s > 0.0 )
            {
                s /= ( l <= 0.5 ) ? ( v + m ) : ( 2.0 - v - m );
            }
            else
            {
                return;
            }
            r2 = ( v - r ) / vm;
            g2 = ( v - g ) / vm;
            b2 = ( v - b ) / vm;
            if ( r == v )
            {
                h = ( g == m ? 5.0 + b2 : 1.0 - g2 );
            }
            else if ( g == v )
            {
                h = ( b == m ? 1.0 + r2 : 3.0 - b2 );
            }
            else
            {
                h = ( r == m ? 3.0 + g2 : 5.0 - r2 );
            }
            h /= 6.0;
        }

        #endregion
    }
}
