using System;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Signatures
{
    /// <summary>
    /// Получение диапазона для вставки подписей
    /// </summary>
    public static class SignatureRange
    {
        /// <summary>
        /// Получить координаты и размеры поля для вставки подписей
        /// </summary>       
        public static IResultAppValue<RangeMicrostation> GetSignatureRange(IResultAppCollection<ILineElementMicrostation> linesStamp,
                                                                           ITextElementMicrostation previousField,
                                                                           ITextElementMicrostation nextField, bool isVertical)
        {
            ILineElementMicrostation leftLine = null;
            ILineElementMicrostation rightLine = null;
            ILineElementMicrostation topLine = null;
            ILineElementMicrostation bottomLine = null;

            foreach (var lineStamp in linesStamp.Value)
            {
                if (ValidateLeftRange(lineStamp, leftLine, previousField, isVertical))
                {
                    leftLine = lineStamp;
                }
                if (ValidateTopRange(lineStamp, topLine, nextField, isVertical))
                {
                    topLine = lineStamp;
                }
                if (ValidateRightRange(lineStamp, rightLine, nextField, isVertical))
                {
                    rightLine = lineStamp;
                }
                if (ValidateBottomRange(lineStamp, bottomLine, previousField, isVertical))
                {
                    bottomLine = lineStamp;
                }
            }

            return (leftLine != null && rightLine != null && topLine != null && bottomLine != null)
                    ? new RangeMicrostation(new PointMicrostation(leftLine.StartPoint.X, bottomLine.StartPoint.Y),
                                            new PointMicrostation(rightLine.StartPoint.X, topLine.StartPoint.Y)).
                        Map(range => new ResultAppValue<RangeMicrostation>(range))
                    : new ResultAppValue<RangeMicrostation>(new ErrorApplication(ErrorApplicationType.RangeNotValid,
                                                                                 "Диапазон подписи не найден"));
        }

        /// <summary>
        /// Проверить левую точку диапазона координат подписи
        /// </summary>
        private static bool ValidateLeftRange(ILineElementMicrostation lineStamp, ILineElementMicrostation leftLine,
                                              ITextElementMicrostation stampField, bool isVertical) =>
            PointMicrostation.CompareCoordinate(lineStamp.StartPoint.X, lineStamp.EndPoint.X) &&
            ValidateByMiddleY(lineStamp, stampField) &&
            (!isVertical
                ? stampField.Range.OriginCenter.X < lineStamp.StartPoint.X &&
                  (leftLine == null || leftLine.StartPoint.X > lineStamp.StartPoint.X)
                : stampField.Range.OriginCenter.X > lineStamp.StartPoint.X &&
                  (leftLine == null || leftLine.StartPoint.X < lineStamp.StartPoint.X));

        /// <summary>
        /// Проверить верхнюю точку диапазона координат подписи
        /// </summary>
        private static bool ValidateTopRange(ILineElementMicrostation lineStamp, ILineElementMicrostation topLine,
                                             ITextElementMicrostation stampField, bool isVertical) =>
            PointMicrostation.CompareCoordinate(lineStamp.StartPoint.Y, lineStamp.EndPoint.Y) &&
            ValidateByMiddleX(lineStamp, stampField) &&
            (!isVertical
                ? stampField.Range.OriginCenter.Y < lineStamp.StartPoint.Y &&
                  (topLine == null || topLine.StartPoint.Y > lineStamp.StartPoint.Y)
                : stampField.Range.OriginCenter.Y > lineStamp.StartPoint.Y &&
                  (topLine == null || topLine.StartPoint.Y < lineStamp.StartPoint.Y));

        /// <summary>
        /// Проверить правую точку диапазона координат подписи
        /// </summary>
        private static bool ValidateRightRange(ILineElementMicrostation lineStamp, ILineElementMicrostation rightLine,
                                               ITextElementMicrostation stampField, bool isVertical) =>
            PointMicrostation.CompareCoordinate(lineStamp.StartPoint.X, lineStamp.EndPoint.X) &&
            ValidateByMiddleY(lineStamp, stampField) &&
            (!isVertical
                ? stampField.Range.OriginCenter.X > lineStamp.StartPoint.X &&
                  (rightLine == null || rightLine.StartPoint.X < lineStamp.StartPoint.X)
                : stampField.Range.OriginCenter.X < lineStamp.StartPoint.X &&
                  (rightLine == null || rightLine.StartPoint.X > lineStamp.StartPoint.X));

        /// <summary>
        /// Проверить нижнюю точку диапазона координат подписи
        /// </summary>
        private static bool ValidateBottomRange(ILineElementMicrostation lineStamp, ILineElementMicrostation bottomLine,
                                                ITextElementMicrostation stampField, bool isVertical) =>
            PointMicrostation.CompareCoordinate(lineStamp.StartPoint.Y, lineStamp.EndPoint.Y) &&
            ValidateByMiddleX(lineStamp, stampField) &&
            (!isVertical
                ? stampField.Range.OriginCenter.Y > lineStamp.StartPoint.Y &&
                  (bottomLine == null || bottomLine.StartPoint.Y < lineStamp.StartPoint.Y)
                : stampField.Range.OriginCenter.Y < lineStamp.StartPoint.Y &&
                  (bottomLine == null || bottomLine.StartPoint.Y > lineStamp.StartPoint.Y));

        /// <summary>
        /// Проверить на пересечение по оси X
        /// </summary>
        private static bool ValidateByMiddleX(ILineElementMicrostation lineStamp, ITextElementMicrostation stampField) =>
            stampField.Range.OriginCenter.X > Math.Min(lineStamp.StartPoint.X, lineStamp.EndPoint.X) &&
            stampField.Range.OriginCenter.X < Math.Max(lineStamp.StartPoint.X, lineStamp.EndPoint.X);

        /// <summary>
        /// Проверить на пересечение по оси Y
        /// </summary>
        private static bool ValidateByMiddleY(ILineElementMicrostation lineStamp, ITextElementMicrostation stampField) =>
            stampField.Range.OriginCenter.Y > Math.Min(lineStamp.StartPoint.Y, lineStamp.EndPoint.Y) &&
            stampField.Range.OriginCenter.Y < Math.Max(lineStamp.StartPoint.Y, lineStamp.EndPoint.Y);
    }
}