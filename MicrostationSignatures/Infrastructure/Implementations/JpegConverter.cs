using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace MicrostationSignatures.Infrastructure.Implementations
{
    /// <summary>
    /// Конвертация изображения в Jpeg
    /// </summary>
    public static class JpegConverter
    {
        /// <summary>
        /// Конвертировать изображение в Jpeg из Emf
        /// </summary>
        public static byte[] ToJpegFromEmf(string filePathEmf)
        {
            using var source = File.OpenRead(filePathEmf);
            return MetaFileToByte(source, format: ImageFormat.Jpeg, parameters: GetJpegEncoderParameters());
        }

        /// <summary>
        /// Сохранить метафайл в определенном формате
        /// </summary>
        public static byte[] MetaFileToByte(Stream source, double scale = 1d, Color? backgroundColor = null,
                                        ImageFormat format = null, EncoderParameters parameters = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            format ??= ImageFormat.Png;
            backgroundColor ??= GetBackgroundColor(format);

            using var metaFile = new Metafile(source);

            var bitmapSize = GetBitmapSize(metaFile, scale);
            using var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(backgroundColor.Value);
                g.DrawImage(metaFile, 0, 0, bitmap.Width, bitmap.Height);
            }

            var resizedBitmap = ResizeImage(bitmap);

            using var stream = new MemoryStream();
            resizedBitmap.Save(stream, GetEncoder(format), parameters);
            return stream.ToArray();
        }

        private static Bitmap ResizeImage(Image image)
        {
            int resizeWidth = image.Width / 10;
            int resizeHeight = image.Height / 10;
            var destRect = new Rectangle(0, 0, resizeWidth, resizeHeight);
            var destImage = new Bitmap(resizeWidth, resizeHeight);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }

        /// <summary>
        /// Определить задний фон для изображения
        /// </summary>
        private static Color GetBackgroundColor(ImageFormat imageFormat)
        {
            var transparentFormats = new List<ImageFormat> { ImageFormat.Gif, ImageFormat.Png, ImageFormat.Wmf, ImageFormat.Emf };
            bool isTransparentFormat = transparentFormats.Contains(imageFormat);

            return isTransparentFormat
                ? Color.Transparent
                : Color.White;
        }

        /// <summary>
        /// Получить размеры изображения
        /// </summary>
        private static Size GetBitmapSize(Metafile metaFile, double scale)
        {
            var metaFileHeader = metaFile.GetMetafileHeader();

            var width = (int)Math.Round(scale * metaFile.Width / metaFileHeader.DpiX * 100, 0, MidpointRounding.ToEven);
            var height = (int)Math.Round(scale * metaFile.Height / metaFileHeader.DpiY * 100, 0, MidpointRounding.ToEven);

            return new Size(width, height);
        }

        /// <summary>
        /// Найти кодировщик по соответствующему типу
        /// </summary>
        private static ImageCodecInfo GetEncoder(ImageFormat format) =>
            ImageCodecInfo.GetImageDecoders().
            FirstOrDefault(codec => codec.FormatID == format.Guid)
            ?? ImageCodecInfo.GetImageDecoders().First();

        /// <summary>
        /// Параметры конвертации в Jpeg
        /// </summary>
        private static EncoderParameters GetJpegEncoderParameters()
        {
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(Encoder.Quality, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            return myEncoderParameters;
        }
    }
}