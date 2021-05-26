using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;
// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Информация об отконвертированных файлах в базе данных
    /// </summary>
    public class FileDataSourceEntity
    {
        public FileDataSourceEntity()
        { }

        public FileDataSourceEntity(string fileName, FileExtensionType fileExtensionType, 
                                    string paperSize, string printerName, IList<byte> fileDataSource)
        {
            FileName = fileName;
            FileExtensionType = fileExtensionType;
            PaperSize = paperSize;
            PrinterName = printerName;
            FileDataSource = fileDataSource;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Имя и расширение файла
        /// </summary>       
        public virtual string FileName { get; protected set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual FileExtensionType FileExtensionType { get; protected set; }

        /// <summary>
        /// Формат печати
        /// </summary>
        public virtual string PaperSize { get; protected set; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public virtual string PrinterName { get; protected set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual FileDataEntity FileDataEntity { get; set; }
    }
}
