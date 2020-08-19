using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ADS.Common.Utilities
{
    public static class XImage
    {
        public static byte[] CreateThumbnail( byte[] image , int width , int height )
        {
            byte[] thumbnail = image;

            try
            {
                using ( var memeroyStream = new MemoryStream( image ) )
                {
                    using ( var imageBitmap = ( Bitmap ) Image.FromStream( memeroyStream ) )
                    {
                        Size size = GetCorrectSizeRelativeToAspectRatio( imageBitmap.Width , imageBitmap.Height , width , height );  // Maintain aspect ratio.
                        if ( size == Size.Empty ) return null;

                        using ( var thumbnailBitmap = new Bitmap( size.Width , size.Height ) )
                        {
                            using ( Graphics thumbnailGraphics = Graphics.FromImage( thumbnailBitmap ) )
                            {
                                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                thumbnailGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                var thumbnailRect = new Rectangle( 0 , 0 , size.Width , size.Height );
                                thumbnailGraphics.DrawImage( imageBitmap , thumbnailRect , 0 , 0 , imageBitmap.Width , imageBitmap.Height , GraphicsUnit.Pixel );
                                thumbnailGraphics.Dispose();

                                // copying the image properties to its thumbnail ...
                                foreach ( PropertyItem propertyItem in imageBitmap.PropertyItems )
                                    thumbnailBitmap.SetPropertyItem( propertyItem );

                                using ( var thumbnailMemoryStream = new MemoryStream() )
                                {
                                    thumbnailBitmap.Save( thumbnailMemoryStream , ImageFormat.Jpeg );
                                    thumbnail = thumbnailMemoryStream.ToArray();
                                }
                            }
                        }
                    }
                }

                return thumbnail;
            }
            catch ( Exception x )
            {
                XLogger.Error( "XUtilities.XImage.CreateThumbnail ... Exception: " + x );
                return null;
            }
        }

        #region Helpres

        private static Size GetCorrectSizeRelativeToAspectRatio( int width , int height , int requiredWidth , int requiredHeight )
        {
            var size = new Size( width , height );

            if ( ( width <= 0 ) || ( height <= 0 ) ) return size;
            if ( width <= requiredWidth && height <= requiredHeight ) return size;

            size.Width = ( requiredHeight * width ) / height;
            size.Height = ( requiredWidth * height ) / width;

            if ( size.Width > requiredWidth ) size.Width = requiredWidth;
            if ( size.Height > requiredHeight ) size.Height = requiredHeight;

            return size;
        }

        #endregion
    }
}
