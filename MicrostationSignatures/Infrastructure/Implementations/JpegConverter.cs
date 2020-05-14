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
        /// Конвертировать изображение в Jpeg
        /// </summary>
        public static void ToJpeg(string filePath)
        {
            var metaFile = new Metafile(filePath);

            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var myEncoder = Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);

            var myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            string jpegPath = Path.ChangeExtension(filePath, "jpg");
            metaFile.Save(jpegPath, jpgEncoder, myEncoderParameters);
            metaFile.Dispose();

            File.Delete(filePath);
        }

        /// <summary>
        /// Найти кодировщик по соответствующему типу
        /// </summary>
        private static ImageCodecInfo GetEncoder(ImageFormat format) =>
            ImageCodecInfo.GetImageDecoders().
            FirstOrDefault(codec => codec.FormatID == format.Guid);
    }
}