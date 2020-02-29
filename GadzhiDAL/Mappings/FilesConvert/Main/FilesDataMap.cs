using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;

namespace GadzhiDAL.Mappings.FilesConvert.Main
{
    /// <summary>
    /// Структура в БД для конвертируемого пакета файлов
    /// </summary>
    public class FilesDataMap : ClassMap<FilesDataEntity>
    {
        public FilesDataMap()
        {
            Id(x => x.Id);
            Map(x => x.CreationDateTime).Not.Nullable();
            Map(x => x.StatusProcessingProject).CustomType<StatusProcessingProject>().Not.Nullable();
            Map(x => x.IdentityLocalName).Not.Nullable().Default("");
            Map(x => x.IdentityServerName).Not.Nullable().Default("");
            Map(x => x.AttemptingConvertCount).Not.Nullable();
            HasMany(x => x.FileDataEntities)
                    .Inverse()
                    .Cascade.All();

        }
    }
}
