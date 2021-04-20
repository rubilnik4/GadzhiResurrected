using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Errors;
using NHibernate.Type;

namespace GadzhiDAL.Mappings.FilesConvert.Errors
{
    /// <summary>
    /// Структура в БД для конвертируемого файла с ошибками
    /// </summary>
    public class FileDataErrorMap : ClassMap<FileDataErrorEntity>
    {
        public FileDataErrorMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FilePath).Not.Nullable().Default("");         
            Map(x => x.ColorPrintType).CustomType<ColorPrintType>().Not.Nullable();  
            Map(x => x.FileDataSourceError).CustomType<BinaryBlobType>().LazyLoad();
            HasMany(x => x.FileErrorsStore).Component(x => 
                                                 {
                                                     x.Map(e => e.ErrorConvertingType);
                                                     x.Map(e => e.ErrorDescription);
                                                 });
            References(x => x.PackageDataEntityError);
        }
    }
}
