using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.FilesConvert;

namespace GadzhiDAL.Mappings.FilesConvert
{
    /// <summary>
    /// Структура в БД для идентефикации устройства
    /// </summary>
    public class IdentityMachineMap : ComponentMap<IdentityMachine>
    {
        public IdentityMachineMap()
        {
            Map(x => x.IdentityLocalName).Not.Nullable().Default("");
            Map(x => x.IdentityServerName).Not.Nullable().Default("");
            Map(x => x.AttemptingConvertCount).Not.Nullable();
        }
    }
}
