using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.FilesConvert
{
    /// <summary>
    /// Структура в БД для конвертируемого файла
    /// </summary>
    public class FileDataSourceMap : ClassMap<FileDataSourceEntity>
    {
        public FileDataSourceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FileName).Not.Nullable().Default("");
            Map(x => x.FileExtensionType).CustomType<FileExtensionType>().Not.Nullable().Default("0");
            HasManyToMany(x => x.PaperSizes).Cascade.All();
            Map(x => x.PrinterName).Not.Nullable().Default("");
            Map(x => x.FileDataSource).CustomType<BinaryBlobType>().LazyLoad();
            References(x => x.FileDataEntity);
        }
    }
}
