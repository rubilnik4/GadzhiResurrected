using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using GadzhiCommon.Models.Implementations.Images;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Операции в памяти
    /// </summary>
    public static class ImageOperations
    {
        /// <summary>
        /// Повернуть изображение
        /// </summary>
        public static byte[] RotateImageInByte(byte[] signatureImage, ImageRotation imageRotation, ImageFormatApplication imageFormat)
        {
            if (signatureImage == null) return null;

            using var streamIn = new MemoryStream(signatureImage, 0, signatureImage.Length);
            streamIn.Write(signatureImage, 0, signatureImage.Length);
            var image = Image.FromStream(streamIn, true);
            image.RotateFlip(ImageRotationToRotateFlipType(imageRotation));

            var streamOut =  new MemoryStream();
            image.Save(streamOut, ImageFormatToDrawing(imageFormat));
            return streamOut.ToArray();
        }

        /// <summary>
        /// Определить тип поворота
        /// </summary>
        private static RotateFlipType ImageRotationToRotateFlipType(ImageRotation imageRotation) =>
            imageRotation switch
            {
                ImageRotation.Rotate90 => RotateFlipType.Rotate90FlipNone,
                ImageRotation.Rotate180 => RotateFlipType.Rotate180FlipNone,
                ImageRotation.Rotate270 => RotateFlipType.Rotate270FlipNone,
                _ => throw new InvalidEnumArgumentException(nameof(imageRotation), (int)imageRotation, typeof(ImageRotation))
            };

        /// <summary>
        /// Определить тип формата изображения
        /// </summary>
        private static ImageFormat ImageFormatToDrawing(ImageFormatApplication imageFormat) =>
            imageFormat switch
            {
                ImageFormatApplication.Jpeg => ImageFormat.Jpeg,
                _ => throw new InvalidEnumArgumentException(nameof(imageFormat), (int)imageFormat, typeof(ImageFormatApplication))
            };
    }
}