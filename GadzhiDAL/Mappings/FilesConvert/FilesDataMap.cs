using FluentNHibernate.Mapping;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;

namespace GadzhiDAL.Mappings.FilesConvert
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
            Map(x => x.IsCompleted).Not.Nullable();
            Map(x => x.StatusProcessingProject).CustomType<StatusProcessingProject>().Not.Nullable();
            Component(x => x.IdentityMachine);
            HasMany(x => x.FilesData)
                    .Inverse()
                    .Cascade.All();
        }
    }
}
