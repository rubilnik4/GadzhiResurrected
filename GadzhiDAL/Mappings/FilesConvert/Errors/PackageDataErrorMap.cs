using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.FilesConvert.Errors;

namespace GadzhiDAL.Mappings.FilesConvert.Errors
{
    /// <summary>
    /// Структура в БД для конвертируемого пакета файлов с ошибками
    /// </summary>
    public class PackageDataErrorMap : ClassMap<PackageDataErrorEntity>
    {
        public PackageDataErrorMap()
        {
            Id(x => x.Id).Not.Nullable();
            Map(x => x.CreationDateTime).Not.Nullable();
            Map(x => x.IdentityLocalName).Not.Nullable().Default("");
            Map(x => x.IdentityServerName).Not.Nullable().Default("");
            HasMany(x => x.FileDataErrorEntities)
                    .Inverse()
                    .Cascade.All();
        }
    }
}
