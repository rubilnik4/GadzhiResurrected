using FluentNHibernate.Mapping;
using GadzhiDAL.Entities.ServerStates;

namespace GadzhiDAL.Mappings.ServerStates
{
    /// <summary>
    /// Структура в БД для доступа клиента
    /// </summary>
    public class ClientAccessMap : ClassMap<ClientAccessEntity>
    {
        public ClientAccessMap()
        {
            Id(x => x.Identity).Not.Nullable().Default("");
            Map(x => x.LastAccess).Not.Nullable();
        }
    }
}