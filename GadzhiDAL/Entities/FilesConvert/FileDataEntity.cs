using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Components;
// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataEntity
    {
        public FileDataEntity()
        { }

        public FileDataEntity(string filePath, ColorPrintType colorPrintType, StatusProcessing statusProcessing,
                              IList<FileDataSourceEntity> fileDataSourceServerEntities, IList<ErrorComponent> fileErrors,
                              IList<byte> fileDataSource, string fileExtensionAdditional, IList<byte> fileDataSourceAdditional)
        {
            FilePath = filePath;
            ColorPrintType = colorPrintType;
            StatusProcessing = statusProcessing;
            FileDataSourceServerEntities = fileDataSourceServerEntities;
            FileErrors = fileErrors;
            FileDataSource = fileDataSource;
            FileExtensionAdditional = fileExtensionAdditional;
            FileDataSourceAdditional = fileDataSourceAdditional;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual string FilePath { get; protected set; }

        /// <summary>
        /// Цвет печати
        /// </summary>       
        public virtual ColorPrintType ColorPrintType { get; protected set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>     
        public virtual StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Файлы отконвертированных данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<FileDataSourceEntity> FileDataSourceServerEntities { get; set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<ErrorComponent> FileErrors { get; set; }

        /// <summary>
        /// Конвертируемый файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Расширение дополнительного файла
        /// </summary>       
        public virtual string FileExtensionAdditional { get; protected set; }

        /// <summary>
        /// Дополнительный файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSourceAdditional { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual PackageDataEntity PackageDataEntity { get; set; }
    }
}
