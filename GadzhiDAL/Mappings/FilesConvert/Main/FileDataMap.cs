using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.FilesConvert.Main
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
            Map(x => x.ColorPrintType).CustomType<ColorPrintType>().Not.Nullable();
            Map(x => x.StatusProcessing).CustomType<StatusProcessing>().Not.Nullable();          
            Map(x => x.FileDataSource).CustomType<BinaryBlobType>().LazyLoad();
            Map(x => x.FileExtensionAdditional);
            Map(x => x.FileDataSourceAdditional).CustomType<BinaryBlobType>().LazyLoad();
            HasMany(x => x.FileErrors).Component(x => 
                                                 {
                                                     x.Map(e => e.ErrorConvertingType);
                                                     x.Map(e => e.ErrorDescription);
                                                 });
            HasMany(x => x.FileDataSourceServerEntities).Inverse().Cascade.All();
            References(x => x.PackageDataEntity);
        }
    }
}
