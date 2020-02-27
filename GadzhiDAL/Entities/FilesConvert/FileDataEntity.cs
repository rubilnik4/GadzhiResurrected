using GadzhiCommon.Enums.FilesConvert;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataEntity : EntityBase<int>
    {       
        public FileDataEntity()
        {
            StatusProcessing = StatusProcessing.InQueue;
            IsCompleted = false;
            FileConvertErrorType = new List<FileConvertErrorType>();
        }

        /// <summary>
        /// Идентефикатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>       
        public virtual ColorPrint ColorPrint { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>     
        public virtual StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        public virtual bool IsCompleted { get; set; }

        /// <summary>
        /// Конвертируемый Файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<FileDataSourceEntity> FileDataSourceEntity { get; protected set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<FileConvertErrorType> FileConvertErrorType { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual FilesDataEntity FilesDataEntity { get; set; }

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetConvertedFileDataEntity(IEnumerable<FileDataSourceEntity> fileDataSourceEntity)
        {
            if (fileDataSourceEntity != null)
            {
                foreach (var fileData in fileDataSourceEntity)
                {
                    fileData.FileDataEntity = this;
                }
            }
            FileDataSourceEntity = fileDataSourceEntity?.ToList();
        }
    }
}
