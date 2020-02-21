using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.FilesConvert
{
    /// <summary>
    /// Структура в БД для конвертируемого файла
    /// </summary>
    public class FileDataMap : ClassMap<FileDataEntity>
    {
        public FileDataMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FilePath).Not.Nullable().Default("");
            Map(x => x.IsCompleted).Not.Nullable();
            Map(x => x.ColorPrint).CustomType<ColorPrint>().Not.Nullable();
            Map(x => x.StatusProcessing).CustomType<StatusProcessing>().Not.Nullable();
            HasMany(x => x.FileConvertErrorType).Element("FileConvertErrorType");
            Map(x => x.FileDataSource).CustomType<BinaryBlobType>().LazyLoad();
            References(x => x.FilesDataEntity);
        }
    }
}
