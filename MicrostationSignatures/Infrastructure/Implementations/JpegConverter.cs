using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static void ToJpegFromEmf(string filePathEmf)
        {
            using var source = File.OpenRead(filePathEmf);
            using var destination = File.OpenWrite(GetFilePathJpeg(filePathEmf));
            SaveMetaFileAs(source, destination, format: ImageFormat.Jpeg, parameters: GetJpegEncoderParameters());
        }

        /// <summary>
        /// Сохранить метафайл в определенном формате
        /// </summary>
        public static void SaveMetaFileAs(Stream source, Stream destination, double scale = 4d, Color? backgroundColor = null,
                                        ImageFormat format = null, EncoderParameters parameters = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

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
            bitmap.Save(destination, GetEncoder(format), parameters);
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
        /// Путь для сохранение Jpeg изображения
        /// </summary>
        private static string GetFilePathJpeg(string filePathEmf) =>
             Path.ChangeExtension(filePathEmf, "jpg");

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