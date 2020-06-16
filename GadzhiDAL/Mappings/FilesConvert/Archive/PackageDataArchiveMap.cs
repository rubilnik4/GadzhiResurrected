using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.FilesConvert.Archive;

namespace GadzhiDAL.Mappings.FilesConvert.Archive
{
    /// <summary>
    /// Структура в БД для конвертируемого пакета файлов
    /// </summary>
    public class PackageDataArchiveMap : ClassMap<PackageDataArchiveEntity>
    {
        public PackageDataArchiveMap()
        {
            Id(x => x.Id);
            Map(x => x.CreationDateTime).Not.Nullable();        
            Map(x => x.IdentityLocalName).Not.Nullable().Default("");
            Map(x => x.IdentityServerName).Not.Nullable().Default("");           
            HasMany(x => x.FileDataArchiveEntities).
                    Inverse().Cascade.All();
        }
    }
}
