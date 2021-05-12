using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Archive;

namespace GadzhiDAL.Mappings.FilesConvert.Archive
{
    /// <summary>
    /// Структура в БД для конвертируемого файла
    /// </summary>
    public class FileDataArchiveMap : ClassMap<FileDataArchiveEntity>
    {
        public FileDataArchiveMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FilePath).Not.Nullable().Default("");
            Map(x => x.ColorPrintType).CustomType<ColorPrintType>().Not.Nullable().Default("0");
            Map(x => x.StatusProcessing).CustomType<StatusProcessing>().Not.Nullable().Default("7");
            HasMany(x => x.FileDataSourceServerEntities).Inverse().Cascade.All();
            HasMany(x => x.FileErrors).Component(x =>
                {
                    x.Map(e => e.ErrorConvertingType);
                    x.Map(e => e.ErrorDescription);
                });
            References(x => x.PackageDataArchiveEntity);
        }
    }
}
