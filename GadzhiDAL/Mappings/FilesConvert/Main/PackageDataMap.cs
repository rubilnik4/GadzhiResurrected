using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;

namespace GadzhiDAL.Mappings.FilesConvert.Main
{
    /// <summary>
    /// Структура в БД для конвертируемого пакета файлов
    /// </summary>
    public class PackageDataMap : ClassMap<PackageDataEntity>
    {
        public PackageDataMap()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.CreationDateTime).Not.Nullable();
            Map(x => x.StatusProcessingProject).CustomType<StatusProcessingProject>().Not.Nullable().Default("10");
            Map(x => x.IdentityLocalName).Not.Nullable().Default("");
            Map(x => x.IdentityServerName).Not.Nullable().Default("");
            Map(x => x.AttemptingConvertCount).Not.Nullable();
            Component(x => x.ConvertingSettings, m =>
                {
                    m.Map(x => x.PersonId).Not.Nullable();
                    m.Map(x => x.PdfNamingType).CustomType<PdfNamingType>().Not.Nullable();
                    m.Map(x => x.ConvertingModeType).CustomType<ConvertingModeType>().Not.Nullable().Default("0");
                });
            HasMany(x => x.FileDataEntities)
                    .Inverse()
                    .Cascade.All();
        }
    }
}
